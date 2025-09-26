using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using CameraControllerWrapperClsLib;
using WestDragon.Framework.UtilityHelper;
using GlobalDataDefineClsLib;
using WestDragon.Framework.BaseLoggerClsLib;
using GlobalToolClsLib;
using WestDragon.Framework.UserControls;
using CameraControllerClsLib;

namespace PositioningSystemClsLib
{

    #region 算法接口和数据定义
    //计算锐度模式
    //public enum EnumSharpnessMode
    //{
    //    GradientEnergy = 0,		  //梯度能量模式
    //    Bandpass = 1,		      //带通模式
    //    AbsDiff = 2,              //绝对差异模式
    //    AutoCorrelation = 3,	  //自相关模式
    //}
    public enum EnumSharpnessMode
    {
        MeanStdDev, 
        Laplance, 
        Sobel
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectregion
    {
        public int topx;
        public int topy;
        public int width;
        public int height;
    };
    //设置输入的计算锐度参数
    [StructLayout(LayoutKind.Sequential)]
    public struct SharpnessRunParam
    {
        //默认参数
        public EnumSharpnessMode Mode;
        public Rectregion region;                                    //图片计算锐度区域，都设置成-1，表示默认全图。
        public float BandPassMaxFrequency;                    //带通模式下的高频阈值，范围[0-0.5]
        public float BandPassMinFrequency;                    //带通模式下的低频阈值,范围[0-0.5],必须小于BandPassMaxFrequency
        public int AbsDiffDistanceX;                             //绝对差异模式下的横向距离，最小为0，不可超过图像的列数
        public int AbsDiffDistanceY;                            //绝对差异模式下的纵向距离，最小为0，不可超过图像的行数
        public int GradienEnergySmooting;                       //梯度能量模式下的平滑阈值，最小为0
    };
    #endregion
    /// <summary>
    /// 聚焦中间数据
    /// </summary>
     internal class FocusData 
     { 
         internal Bitmap Image { get; set; } 
         internal double Z { get; set; } 
     }
     internal class VisionAutoFocusProcessor : IAutoFocus
     {
         #region 实现接口
         /// <summary>
        /// 用户自定义动作回调
        /// </summary>
        public Action<AutoFocusActionCallback> UserCustomAction { get; set; }

       
        /// <summary>
        /// 聚焦参数
        /// </summary>
        public AutoFocusParams Param { get; set; }
        /// <summary>
        /// 聚焦区域
        /// </summary>
        public CircleFigure FocusRegion
        {
            get;
            protected set;
        }

        /// <summary>
        /// 聚焦结果
        /// </summary>
        public PairData Result { get; set; }
        private EnumCameraType _currentCamera
        {
            get
            {
                return CameraManager.Instance.CurrentCameraType;
            }
        }
        private EnumStageAxis _focusZAxis
        {
            get
            {
                var ret = EnumStageAxis.BondZ;
                if (_currentCamera == EnumCameraType.WaferCamera)
                {
                    ret = EnumStageAxis.WaferTableZ;
                }
                return ret;
            }
        }

        public void DoAutoFocusSync()
        {

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            OutputResults("------------------------------------");
            OutputResults("AutoFous Start:" + DateTime.Now.ToString());
            if (Param == null)
            {
                return;
            }
            focusIndex = 0;
            SharpnessRunParam sharpnessParam = new SharpnessRunParam();
            switch (Param.Mode)
            { 
                case EnumImageSharpnessMode.MeanStdDev:
                    sharpnessParam.Mode = EnumSharpnessMode.MeanStdDev;
                    break;
                case EnumImageSharpnessMode.Laplance:
                    sharpnessParam.Mode = EnumSharpnessMode.Laplance;
                    break;
                case EnumImageSharpnessMode.Sobel:
                    sharpnessParam.Mode = EnumSharpnessMode.Sobel;
                    break;
            }

            sharpnessParam.AbsDiffDistanceX = Param.AbsDiffDistanceX;
            sharpnessParam.AbsDiffDistanceX = Param.AbsDiffDistanceY;
            sharpnessParam.BandPassMaxFrequency = (float)Param.BandPassMaxFrequency;
            sharpnessParam.BandPassMinFrequency = (float)Param.BandPassMinFrequency;
            sharpnessParam.GradienEnergySmooting = Param.GradientEnergyLowPassSmoothing;
            Rectregion rect = new Rectregion();
            rect.topx = (int)(Param.FocusRegion.CenterPoint.X - Param.FocusRegion.fRadius);
            rect.topy = (int)(Param.FocusRegion.CenterPoint.Y - Param.FocusRegion.fRadius);
            rect.width = (int)Param.FocusRegion.fRadius * 2;
            rect.height = (int)Param.FocusRegion.fRadius * 2;
            sharpnessParam.region = rect;
            //SkyverseShapnessInvoke.SharpnessSetRunParam(ref sharpnessParam);
            _rawDataResultList.Clear();
            try
            {
                
                CalculateOptimalFocusPositionUsingCurveFit(Param);
                while (_rawDataResultList.Count < AutofocusPointCount)
                {
                    Thread.Sleep(50);
                }
                _rawDataResultList.Sort();
                CurveFitEngine.RawData = _rawDataResultList;
                if (UsingCurveFitting)
                {
                    CurveFitEngine.FindCurveCoef();
                }
                Result = CalculateAutoFocusResult();
                //移动到指定位置并拍照
                _positionSystem.MoveAixsToStageCoord(_focusZAxis,Result.X, EnumCoordSetType.Absolute);
                _cameraEngine.CaptureImage();
                timer.Stop();
                OutputResults("AutoFous End:" + DateTime.Now.ToString());
                OutputResults(string.Format("AutoFous Algorithm: Best Focus z :{0},Shapness:{1}" , Result.X,Result.Y));
                OutputResults("AutoFous Algorithm: Cost:" + timer.ElapsedMilliseconds);
                OutputResults("------------------------------------");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Auto focus failed, {0}", ex.Message), ex);
            }
        }

        public void DoAutoFocusSync_RelateiveMove()
        {

            System.Diagnostics.Stopwatch timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            OutputResults("------------------------------------");
            OutputResults("AutoFous Start:" + DateTime.Now.ToString());
            if (Param == null)
            {
                return;
            }
            focusIndex = 0;
            SharpnessRunParam sharpnessParam = new SharpnessRunParam();
            switch (Param.Mode)
            {
                case EnumImageSharpnessMode.MeanStdDev:
                    sharpnessParam.Mode = EnumSharpnessMode.MeanStdDev;
                    break;
                case EnumImageSharpnessMode.Laplance:
                    sharpnessParam.Mode = EnumSharpnessMode.Laplance;
                    break;
                case EnumImageSharpnessMode.Sobel:
                    sharpnessParam.Mode = EnumSharpnessMode.Sobel;
                    break;
            }

            sharpnessParam.AbsDiffDistanceX = Param.AbsDiffDistanceX;
            sharpnessParam.AbsDiffDistanceX = Param.AbsDiffDistanceY;
            sharpnessParam.BandPassMaxFrequency = (float)Param.BandPassMaxFrequency;
            sharpnessParam.BandPassMinFrequency = (float)Param.BandPassMinFrequency;
            sharpnessParam.GradienEnergySmooting = Param.GradientEnergyLowPassSmoothing;
            Rectregion rect = new Rectregion();
            rect.topx = (int)(Param.FocusRegion.CenterPoint.X - Param.FocusRegion.fRadius);
            rect.topy = (int)(Param.FocusRegion.CenterPoint.Y - Param.FocusRegion.fRadius);
            rect.width = (int)Param.FocusRegion.fRadius * 2;
            rect.height = (int)Param.FocusRegion.fRadius * 2;
            sharpnessParam.region = rect;
            //SkyverseShapnessInvoke.SharpnessSetRunParam(ref sharpnessParam);
            _rawDataResultList.Clear();
            try
            {

                CalculateOptimalFocusPositionUsingCurveFit(Param);
                while (_rawDataResultList.Count < AutofocusPointCount)
                {
                    Thread.Sleep(50);
                }
                _rawDataResultList.Sort();
                CurveFitEngine.RawData = _rawDataResultList;
                if (UsingCurveFitting)
                {
                    CurveFitEngine.FindCurveCoef();
                }
                Result = CalculateAutoFocusResult();
                timer.Stop();
                OutputResults("AutoFous End:" + DateTime.Now.ToString());
                OutputResults(string.Format("AutoFous Algorithm: Best Focus z :{0},Shapness:{1}", Result.X, Result.Y));
                OutputResults("AutoFous Algorithm: Cost:" + timer.ElapsedMilliseconds);
                OutputResults("------------------------------------");
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Auto focus failed, {0}", ex.Message), ex);
            }
        }
        #endregion




        /// <summary>
        /// 抛物线拟合器
        /// </summary>
        public QuadraticCurveFitting CurveFitEngine { get; protected set; }
        /// <summary>
        /// 是否使用曲线拟合
        /// </summary>
        public bool UsingCurveFitting { get; set; }
        /// <summary>
        /// 保存聚焦过程中的计算出的原始中间值
        /// </summary>
        protected List<PairData> _rawDataResultList;

        /// <summary>
        /// 缓存队列机制,负责接收采集到的图片并进行处理
        /// </summary>
        public BufferQueueMechanismOperation<FocusData> _dataBufferQueue;
        /// <summary>
        /// 自动聚焦点数
        /// </summary>
        private int AutofocusPointCount
        {
            get;set;
        }

        /// <summary>
        /// 聚焦计数的索引
        /// </summary>
        protected int focusIndex = 0;
        /// <summary>
        /// 相机.
        /// </summary>
        protected CameraControllerWrapper _cameraEngine
        {
            get
            {
                return CameraControllerWrapper.Instance;
            }
        }

        /// <summary>
        /// 定位系统.
        /// </summary>
        protected PositioningSystem _positionSystem
        {
            get
            {
                return PositioningSystem.Instance;
            }
        }

        public VisionAutoFocusProcessor() 
        {
            _rawDataResultList = new List<PairData>();
            UsingCurveFitting = true;
            CurveFitEngine = new QuadraticCurveFitting();
            Result = new PairData();
            FocusRegion = new CircleFigure(new PointF(0,0),1);

            _dataBufferQueue = new BufferQueueMechanismOperation<FocusData>();
            _dataBufferQueue.ProcessAction = (data) =>
            {
                try
                {
                    float sharpness = GetSharpness(data.Image);
                    _rawDataResultList.Add(new PairData(data.Z, sharpness));

                    focusIndex++;
                    OutputResults(string.Format("AutoFous Algorithm:{0} Find Z:{1} Sharpness:{2}", focusIndex, data.Z,sharpness));

                    if (Param.SaveIntermediateFocusData)
                    {
                        if (data.Image != null)
                        {
                            data.Image.Save(string.Format("C:\\Temp\\{0}_{1}_{2}.bmp", focusIndex, DateTime.Now.ToString("yyyyMMddHHmmssfff"), data.Z));
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, "AutoFocusForCurrentPoint Start Step2.",ex);
                }
            };
            _dataBufferQueue.Start();
        }
        /// <summary>
        /// 获取锐度
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        private float GetSharpness(Bitmap image)
        {
            float functionReturnValue = 0;
            //计算锐度
            //functionReturnValue = (float)GWSharpness.GetSharpness(image,0);
            if (UserCustomAction != null)
            {
                UserCustomAction(new AutoFocusActionCallback()
                {
                    //ImageCaptured = CognexImageFormatter.GetHandler().ConvertToCogImage(image)
                });
            }
            return functionReturnValue;
        }
        /// <summary>
        /// 自动聚焦方法二, 曲线拟合方式
        /// </summary>
        private void CalculateOptimalFocusPositionUsingCurveFit(AutoFocusParams paramSetting)
        {
            double start = paramSetting.StartZ;
            double end = paramSetting.EndZ;
            var curStagePos = _positionSystem.ReadCurrentStagePosition(_focusZAxis);
            AutofocusPointCount = (int)Math.Ceiling((paramSetting.EndZ - paramSetting.StartZ) / paramSetting.StepZ);
            for (int i = 0; i < AutofocusPointCount; i++)
            {
                GetImageOnSpecifiedZ_RelativeMove(start + paramSetting.StepZ * i);
            }
        }
        /// <summary>
        /// 获取指定Z轴绝对位置的图片数据
        /// </summary>
        /// <param name="z"></param>
        /// <returns></returns>
        private void GetImageOnSpecifiedZ(double z)
        {
            _positionSystem.MoveAixsToStageCoord(_focusZAxis,z, EnumCoordSetType.Absolute);
            Bitmap capturedImage = new Bitmap(_cameraEngine.CaptureImage());
            if (capturedImage == null)
            {
                LogRecorder.RecordLog(EnumLogContentType.Info, "Captured image is null when get sharpness for auto focus!");
                return;
            }

            _dataBufferQueue.EnqueueNewData(new FocusData() { Image = capturedImage, Z = z });
        }
        /// <summary>
        /// 通过相对移动模式移动的目标位Z
        /// </summary>
        /// <param name="z">目标位</param>
        private void GetImageOnSpecifiedZ_RelativeMove(double z)
        {
            var curPos=_positionSystem.ReadCurrentStagePosition(_focusZAxis);
            var distance = z - curPos;
            _positionSystem.MoveAixsToStageCoord(_focusZAxis,distance, EnumCoordSetType.Relative);
            Bitmap capturedImage = new Bitmap(_cameraEngine.CaptureImage());
            if (capturedImage == null)
            {
                LogRecorder.RecordLog(EnumLogContentType.Info, "Captured image is null when get sharpness for auto focus!");
                return;
            }

            _dataBufferQueue.EnqueueNewData(new FocusData() { Image = capturedImage, Z = z });
        }
        /// <summary>
        /// 获取自动聚焦结果值
        /// </summary>
        /// <returns></returns>
        private PairData CalculateAutoFocusResult()
        {
            double maxValue = 0;
            int maxValueIndex = 0;
            double[] position = new double[_rawDataResultList.Count];
            double[] data = new double[_rawDataResultList.Count];
            //获取的清晰度中，舍去第一个点
            for (int i = 1; i < _rawDataResultList.Count; i++)
            {
                position[i] = _rawDataResultList[i].X;
                if (position[i] == 0.0)
                {
                    position[i] += 1e-5;
                }
                data[i] = _rawDataResultList[i].Y;
                if (data[i] > maxValue)
                {
                    maxValue = data[i];
                    maxValueIndex = i;
                }
            }

            PairData result = new PairData(position[maxValueIndex], maxValue);
            if (UsingCurveFitting)
            {
                CurveFitEngine.CalculateExtremumValue();
                result = CurveFitEngine.MaxValue;
                if (result != null)
                {
                    double newValue = GetSharpnessOnSpecifiedZ1_RelativeMove(result.X);
                    if (newValue < maxValue)
                    {
                        result.X = position[maxValueIndex];
                        result.Y = maxValue;
                    }
                }
            }

            return result;
        }
        private double GetSharpnessOnSpecifiedZ1_RelativeMove(double z)
        {
            var curPos = _positionSystem.ReadCurrentStagePosition(_focusZAxis);
            var distance = z - curPos;
            _positionSystem.MoveAixsToStageCoord(_focusZAxis, distance, EnumCoordSetType.Relative);
            _cameraEngine.CaptureImage();
            Bitmap capturedImage = _cameraEngine.CaptureImage();


            float sharpness = GetSharpness(capturedImage);

            return sharpness;
        }
        /// <summary>
        /// 输出结果值
        /// </summary>
        public void OutputResults(string info)
        {
            if(!Directory.Exists("C:\\Temp"))
            {
                Directory.CreateDirectory("C:\\Temp");
            }

            using (FileStream fsInfo = new FileStream(@"C:\temp\AutoFocusResults.txt", FileMode.Append, FileAccess.Write))
            {
                StreamWriter swInfo = new StreamWriter(fsInfo);
                swInfo.WriteLine(info);
                swInfo.Flush();
                fsInfo.Flush();
            }

            Console.WriteLine(info);
        }
    }
}
