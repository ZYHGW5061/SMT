using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace VisionClsLib
{
    public class VisualAlgorithms
    {
        #region Private file

        HIKAlgorithms visual = new HIKAlgorithms();

        #endregion

        #region Private Method

        static (double m, double b) CalculateLineEquation(PointF p1, PointF p2)
        {
            double deltaX = p2.X - p1.X;
            double deltaY = p2.Y - p1.Y;

            if (deltaX == 0)
            {
                throw new InvalidOperationException("Line is vertical, cannot use y = mx + b form");
            }

            double m = deltaY / deltaX;
            double b = p1.Y - m * p1.X;

            return (m, b);
        }

        static PointF FindIntersection((double m1, double b1) line1, (double m2, double b2) line2)
        {
            if (line1.m1 == line2.m2)
            {
                throw new InvalidOperationException("Lines are parallel or coincide");
            }

            double x = (line2.b2 - line1.b1) / (line1.m1 - line2.m2);
            double y = line1.m1 * x + line1.b1;

            return new PointF((float)x, (float)y);
        }

        #endregion

        #region 其他算法


        public static Bitmap DeepClone(Bitmap bitmap)
        {
            Bitmap dstBitmap = null;
            using (MemoryStream mStream = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(mStream, bitmap);
                mStream.Seek(0, SeekOrigin.Begin);
                dstBitmap = (Bitmap)bf.Deserialize(mStream);
                mStream.Close();
            }
            return dstBitmap;
        }

        public static double CalculateImageSharpness(Bitmap bitmap,Rectangle rectangle)
        {
            int flag = 2;

            //bitmap.Save("Transfer.bmp");

            Bitmap bitmap1 = DeepClone(bitmap);

            

            var image = BitmapConverter.ToMat(bitmap1);


            Rect roi = new Rect(rectangle.X, rectangle.Y, rectangle.Width, rectangle.Height);
            Mat croppedImage = new Mat(image, roi);

            if (image.Empty())
            {
                throw new ArgumentException("The image path is invalid or the image cannot be loaded.");
            }

            Mat imgLaplance = new Mat();
            Mat imageSobel = new Mat();
            Mat meanValueImage = new Mat();
            Mat meanImage = new Mat();
            Mat BlurMat = new Mat();
            Mat gray = new Mat();
            Cv2.CvtColor(croppedImage, gray, ColorConversionCodes.BGR2GRAY);
            //中值模糊 降低图像噪音
            //Cv2.MedianBlur(gray, BlurMat, 5);
            Cv2.BoxFilter(gray, BlurMat, MatType.CV_8UC3, new OpenCvSharp.Size(11, 11));
            string method = string.Empty;
            double meanValue = 0;
            if (flag == 1)
            {
                //方差法
                Cv2.MeanStdDev(BlurMat, meanValueImage, meanImage);
                meanValue = meanImage.At<double>(0, 0);
                method = "MeanStdDev";
            }
            else if (flag == 0)
            {
                //拉普拉斯
                Cv2.Laplacian(BlurMat, imgLaplance, MatType.CV_16S, 5, 1);
                Cv2.ConvertScaleAbs(imgLaplance, imgLaplance);
                //结果放大两倍,拉开差距
                meanValue = Cv2.Mean(imgLaplance)[0] * 2;
                method = "Laplacian";
            }
            else
            {
                //索贝尔
                Cv2.Sobel(BlurMat, imageSobel, MatType.CV_16S, 3, 3, 5);
                Cv2.ConvertScaleAbs(imageSobel, imageSobel);
                //结果放大两倍,拉开差距
                meanValue = Cv2.Mean(imageSobel)[0] * 2;
                method = "Sobel";

            }

            return meanValue;
        }


        #endregion


        #region Method

        /// <summary>
        /// 算法初始化
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            bool Done1 = visual.MatchInit();
            bool Done2 = visual.LineFindInit();
            bool Done3 = visual.CircleFindInit();

            return (Done1 && Done2 && Done3);
        }


        #region 高精度模板匹配

        /// <summary>
        /// 模板训练
        /// </summary>
        /// <param name="TrainImage">训练图像</param>
        /// <param name="Result">训练结果 点图</param>
        /// <param name="Benchmark">基准点</param>
        /// <param name="ROI">ROI区域</param>
        /// <param name="Sign">true 训练区域 false 掩膜</param>
        /// <returns></returns>
        public bool MatchTrain(Bitmap TrainImage, ref MatchTemplateResult Result, PointF Benchmark = new PointF(), RectangleF ROI = new RectangleF(), RectangleF MaskROI = new RectangleF())
        {
            return visual.MatchTrain(TrainImage, ref Result, Benchmark, ROI, MaskROI);
        }

        /// <summary>
        /// 模板训练
        /// </summary>
        /// <param name="TrainImage">训练图像</param>
        /// <param name="Result">训练结果 点图</param>
        /// <param name="Benchmark">基准点</param>
        /// <param name="ROI">ROI区域</param>
        /// <param name="Sign">true 训练区域 false 掩膜</param>
        /// <returns></returns>
        public bool MatchTrain(Bitmap TrainImage, ref MatchTemplateResult Result, RectangleF ROI = new RectangleF(), bool Sign = true)
        {
            return visual.MatchTrain(TrainImage, ref Result, ROI, Sign);
        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="TrainXmlFilepath">模板文件完整路径 后缀.contourmxml</param>
        /// <returns></returns>
        public bool MatchSaveTrain(string TrainXmlFilepath)
        {
            return visual.MatchSaveTrain(TrainXmlFilepath);
        }

        /// <summary>
        /// 读取模板
        /// </summary>
        /// <param name="TrainXmlFilepath">模板文件完整路径 后缀.contourmxml</param>
        /// <returns></returns>
        public bool MatchLoadTrain(string TrainXmlFilepath)
        {
            return visual.MatchLoadTrain(TrainXmlFilepath);
        }

        /// <summary>
        /// 加载训练参数
        /// </summary>
        /// <param name="fileDlg">识别参数.xml路径</param>
        /// <returns></returns>
        public bool TrainLoadPara(string fileDlg)
        {
            return visual.TrainLoadPara(fileDlg);
        }

        /// <summary>
        /// 保存训练参数
        /// </summary>
        /// <param name="fileDlg">识别参数.xml路径</param>
        /// <returns></returns>
        public bool TrainSavePara(string fileDlg)
        {
            return visual.TrainSavePara(fileDlg);
        }

        /// <summary>
        /// 设置训练参数
        /// </summary>
        /// <param name="ScaleMode"></param>
        /// <param name="ScaleLevel"></param>
        /// <param name="ScaleRLevel"></param>
        /// <param name="ThresMode"></param>
        /// <param name="ThresValue"></param>
        /// <param name="WeightFlag"></param>
        /// <param name="ChainFlag"></param>
        /// <param name="MinChain"></param>
        /// <returns></returns>
        public bool MatchSetTrainPara(ContourPatCreateScaleMode ScaleMode, float ScaleLevel, uint ScaleRLevel,
            ContourPatCreateThresholdMode ThresMode, uint ThresValue, ContourPatCreateWeightFlag WeightFlag,
            ContourPatCreateChainFlag ChainFlag, int MinChain)
        {
            return visual.MatchSetTrainPara(ScaleMode, ScaleLevel, ScaleRLevel, ThresMode, ThresValue, WeightFlag, ChainFlag, MinChain);

        }

        /// <summary>
        /// 获取训练参数
        /// </summary>
        /// <param name="ScaleMode"></param>
        /// <param name="ScaleLevel"></param>
        /// <param name="ScaleRLevel"></param>
        /// <param name="ThresMode"></param>
        /// <param name="ThresValue"></param>
        /// <param name="WeightFlag"></param>
        /// <param name="ChainFlag"></param>
        /// <param name="MinChain"></param>
        /// <returns></returns>
        public bool MatchGetTrainPara(ref ContourPatCreateScaleMode ScaleMode, ref float ScaleLevel, ref uint ScaleRLevel,
            ref ContourPatCreateThresholdMode ThresMode, ref uint ThresValue, ref ContourPatCreateWeightFlag WeightFlag,
            ref ContourPatCreateChainFlag ChainFlag, ref int MinChain)
        {
            return visual.MatchGetTrainPara(ref ScaleMode, ref ScaleLevel, ref ScaleRLevel, ref ThresMode, ref ThresValue, ref WeightFlag, ref ChainFlag, ref MinChain);
        }

        /// <summary>
        /// 设置识别参数
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="para">参数名</param>
        /// <param name="Val">值</param>
        /// <returns></returns>
        public bool MatchSetRunPara<T>(MatchParas para, T Val)
        {
            return visual.MatchSetRunPara<T>(para, Val);
        }

        /// <summary>
        /// 获取识别参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="para"></param>
        /// <param name="Val"></param>
        /// <returns></returns>
        public bool MatchGetRunPara<T>(MatchParas para, ref T Val)
        {
            return visual.MatchGetRunPara<T>(para, ref Val);
        }

        /// <summary>
        /// 加载识别参数
        /// </summary>
        /// <param name="fileDlg">识别参数.xml路径</param>
        /// <returns></returns>
        public bool MatchLoadRunPara(string fileDlg)
        {
            return visual.MatchLoadRunPara(fileDlg);
        }

        /// <summary>
        /// 保存识别参数
        /// </summary>
        /// <param name="fileDlg">识别参数.xml路径</param>
        /// <returns></returns>
        public bool MatchSaveRunPara(string fileDlg)
        {
            return visual.MatchSaveRunPara(fileDlg);
        }

        public bool MatchAddMask(RectangleF ROI)
        {
            try
            {
                //match.RunClearMask();
                visual.AddMask(ROI);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool MatchClearMask()
        {
            try
            {
                visual.ClearMask();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 模板识别
        /// </summary>
        /// <param name="RunImage">识别图像</param>
        /// <param name="Result">识别结果</param>
        /// <param name="ROI">识别区域</param>
        /// <returns></returns>
        public bool MatchRun(Bitmap RunImage,float Score, ref List<MatchResult> Result, RectangleF ROI = new RectangleF())
        {
            return visual.MatchRun(RunImage,Score, ref Result, ROI);
        }

        #endregion

        #region 边缘查找

        /// <summary>
        /// 设置边缘查找参数
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="para">参数名</param>
        /// <param name="Val">值</param>
        /// <returns></returns>
        public bool LineFindSetRunPara<T>(LineFindParas para, T Val)
        {
            return visual.LineFindSetRunPara<T>(para, Val);
        }

        /// <summary>
        /// 获取边缘查找参数
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="para">参数名</param>
        /// <param name="Val">返回值</param>
        /// <returns></returns>
        public bool LineFindGetRunPara<T>(LineFindParas para, ref T Val)
        {
            return visual.LineFindGetRunPara<T>(para, ref Val);
        }

        /// <summary>
        /// 加载边缘查找参数
        /// </summary>
        /// <param name="fileDlg">边缘查找参数.xml路径</param>
        /// <returns></returns>
        public bool LineFindLoadRunPara(string fileDlg)
        {
            return visual.LineFindLoadRunPara(fileDlg);
        }

        /// <summary>
        /// 保存边缘查找参数
        /// </summary>
        /// <param name="fileDlg">边缘查找参数.xml路径</param>
        /// <returns></returns>
        public bool LineFindSaveRunPara(string fileDlg)
        {
            return visual.LineFindSaveRunPara(fileDlg);
        }

        /// <summary>
        /// 边缘查找识别
        /// </summary>
        /// <param name="RunImage">识别图像</param>
        /// <param name="Result">识别结果</param>
        /// <param name="ROI">识别区域</param>
        /// <param name="ScanDirection">搜索方向 false 从左到右 true 从上到下</param>
        /// <returns></returns>
        public bool LineFindRun(Bitmap RunImage, int Score, ref LineResult Result, RectangleF ROI = new RectangleF(), bool ScanDirection = false)
        {
            return visual.LineFindRun(RunImage, Score, ref Result, ROI, ScanDirection);
        }

        public RectangleFA RectEdgeCalculate(LineResult Upline, LineResult Downline, LineResult Leftline, LineResult Rightline)
        {
            var line1 = CalculateLineEquation(Upline.Startpoint, Upline.Endpoint);
            var line2 = CalculateLineEquation(Downline.Startpoint, Downline.Endpoint);
            var line3 = CalculateLineEquation(Leftline.Startpoint, Leftline.Endpoint);
            var line4 = CalculateLineEquation(Rightline.Startpoint, Rightline.Endpoint);

            // 计算交点
            var UL_intersection = FindIntersection(line1, line3);//左上交点
            var DL_intersection = FindIntersection(line2, line3);//左下交点
            var UR_intersection = FindIntersection(line1, line4);//右上交点
            var DR_intersection = FindIntersection(line2, line4);//右下交点

            RectangleFA Rect = new RectangleFA(UL_intersection, DL_intersection, UR_intersection, DR_intersection);

            return Rect;
        }

        #endregion

        #region 圆查找


        /// <summary>
        /// 设置圆查找参数
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="para">参数名</param>
        /// <param name="Val">值</param>
        /// <returns></returns>
        public bool CircleFindSetRunPara<T>(CircleFindParas para, T Val)
        {
            return visual.CircleFindSetRunPara<T>(para, Val);
        }

        /// <summary>
        /// 获取圆查找参数
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="para">参数名</param>
        /// <param name="Val">返回值</param>
        /// <returns></returns>
        public bool CircleFindGetRunPara<T>(CircleFindParas para, ref T Val)
        {
            return visual.CircleFindGetRunPara<T>(para, ref Val);
        }

        /// <summary>
        /// 加载圆查找参数
        /// </summary>
        /// <param name="fileDlg">圆查找参数.xml路径</param>
        /// <returns></returns>
        public bool CircleFindLoadRunPara(string fileDlg)
        {
            return visual.CircleFindLoadRunPara(fileDlg);
        }

        /// <summary>
        /// 保存圆查找参数
        /// </summary>
        /// <param name="fileDlg">圆查找参数.xml路径</param>
        /// <returns></returns>
        public bool CircleFindSaveRunPara(string fileDlg)
        {
            return visual.CircleFindSaveRunPara(fileDlg);
        }

        /// <summary>
        /// 圆查找识别
        /// </summary>
        /// <param name="RunImage">识别图像</param>
        /// <param name="Result">识别结果</param>
        /// <param name="ROI">识别区域</param>
        /// <param name="MinRadius">最小半径</param>
        /// <param name="MaxRadius">最大半径</param>
        /// <returns></returns>
        public bool CircleFindRun(Bitmap RunImage, int Score, ref CircleResult Result, RectangleF ROI = new RectangleF(), int MinRadius = -1, int MaxRadius = -1)
        {
            return visual.CircleFindRun(RunImage, Score, ref Result, ROI, MinRadius, MaxRadius);
        }

        #endregion


        public void CroppingAndSaveImage(Bitmap rawImage, string saveFileFullName, RectangleF ROI = new RectangleF())
        {
            try
            {
                using (Bitmap bitmap = rawImage)
                {
                    var target = new Bitmap((int)ROI.Width, (int)ROI.Height, PixelFormat.Format32bppArgb);
                    Graphics g = Graphics.FromImage(target);
                    g.DrawImage(bitmap, new Rectangle(0, 0, (int)ROI.Width, (int)ROI.Height)
                        , new Rectangle((int)ROI.X, (int)ROI.Y, (int)ROI.Width, (int)ROI.Height), GraphicsUnit.Pixel);
                    Mat srcImg = BitmapConverter.ToMat(target);
                    Mat grayImgTemplate = new Mat();
                    Cv2.CvtColor(srcImg, grayImgTemplate, ColorConversionCodes.RGB2GRAY);
                    Cv2.ImWrite(saveFileFullName, grayImgTemplate);
                    srcImg.Dispose();
                    grayImgTemplate.Dispose();
                }
            }
            catch (Exception)
            {
            }

        }

        #endregion


    }
}
