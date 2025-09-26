using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisionClsLib
{
    public class VisionParam
    {


    }


    public class ScorePoint
    {
        public PointF Point;
        public int Score;
        public ScorePoint()
        {
            Point = new PointF();
        }

        public ScorePoint(PointF point, int Score)
        {
            this.Point = point;
            this.Score = Score;
        }
    }

    public class RectangleFA
    {
        public PointF Benchmark;
        public PointF Center;
        public float Width;
        public float Height;
        public float Angle;
        public PointF LeftTop;
        public PointF LeftBottom;
        public PointF RightTop;
        public PointF RightBottom;

        public RectangleFA()
        {

        }

        public RectangleFA(PointF Benchmark, PointF Center, float Width, float Height, float Angle)
        {
            this.Benchmark = Benchmark;
            this.Center = Center;
            this.Width = Width;
            this.Height = Height;
            this.Angle = Angle;

            double angleRadians = Angle * Math.PI / 180.0;

            float halfWidth = Width / 2;
            float halfHeight = Height / 2;

            PointF[] points = new PointF[4];
            points[0] = new PointF(Center.X - halfWidth, Center.Y - halfHeight); // 左上角
            points[1] = new PointF(Center.X + halfWidth, Center.Y - halfHeight); // 右上角
            points[2] = new PointF(Center.X + halfWidth, Center.Y + halfHeight); // 右下角
            points[3] = new PointF(Center.X - halfWidth, Center.Y + halfHeight); // 左下角

            PointF[] rotatedPoints = new PointF[4];
            for (int i = 0; i < 4; i++)
            {
                rotatedPoints[i] = new PointF(
                    Center.X + (float)((points[i].X - Center.X) * Math.Cos(angleRadians) - (points[i].Y - Center.Y) * Math.Sin(angleRadians)),
                    Center.Y + (float)((points[i].X - Center.X) * Math.Sin(angleRadians) + (points[i].Y - Center.Y) * Math.Cos(angleRadians)));
            }

            this.LeftTop = rotatedPoints[0];
            this.LeftBottom = rotatedPoints[3];
            this.RightTop = rotatedPoints[1];
            this.RightBottom = rotatedPoints[2];

        }

        public RectangleFA(PointF LeftTop, PointF LeftBottom, PointF RightTop, PointF RightBottom)
        {
            this.LeftTop = LeftTop;
            this.LeftBottom = LeftBottom;
            this.RightTop = RightTop;
            this.RightBottom = RightBottom;

            this.Center = new PointF((LeftTop.X + RightBottom.X + RightTop.X + LeftBottom.X) / 4, (LeftTop.Y + RightBottom.Y + RightTop.Y + LeftBottom.Y) / 4);

            float width1 = Distance(LeftTop, RightTop);
            float width2 = Distance(LeftBottom, RightBottom);
            this.Width = (width1 + width2) / 2;

            float height1 = Distance(LeftTop, LeftBottom);
            float height2 = Distance(RightTop, RightBottom);
            this.Height = (height1 + height2) / 2;

            float deltaX = RightTop.X - LeftTop.X;
            float deltaY = RightTop.Y - LeftTop.Y;

            float deltaX2 = RightBottom.X - LeftBottom.X;
            float deltaY2 = RightBottom.Y - LeftBottom.Y;

            float A1 = (float)(Math.Atan2(deltaY, deltaX) * (180.0 / Math.PI));
            float A2 = (float)(Math.Atan2(deltaY2, deltaX2) * (180.0 / Math.PI));
            this.Angle = (A1 + A2) / 2;
        }

        private float Distance(PointF p1, PointF p2)
        {
            return (float)Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2));
        }
    }

    public class ROI
    {
        public RectangleF Shape;
        public bool Sign;
        public ROI()
        {

        }
        public ROI(RectangleF Shape, bool Sign)
        {
            this.Shape = Shape;
            this.Sign = Sign;
        }
    }

    public class Line
    {
        public PointF point1;
        public PointF point2;

        public Line()
        {

        }
        public Line(PointF point1, PointF point2)
        {
            this.point1 = point1;
            this.point2 = point2;
        }

    }

    public class MatchTemplateResult
    {
        public PointF Center;
        public float Angle;
        public PointF Deviation;
        public List<ScorePoint> Points;

        public MatchTemplateResult()
        {
            //Points = new List<ScorePoint>();
        }
        public MatchTemplateResult(PointF Center, float Angle, PointF Deviation,
            List<ScorePoint> Points)
        {
            this.Center = Center;
            this.Angle = Angle;
            this.Deviation = Deviation;
            this.Points = Points;
        }
    }

    public class MatchResult
    {
        public RectangleFA MatchBox;
        public ROI SearchBox;
        public float Score;
        public float Scale;
        public float ScaleX;
        public float ScaleY;
        public List<ScorePoint> Points;
        public bool IsOk = false;

        public MatchResult()
        {

        }
        public MatchResult(RectangleFA MatchBox, ROI SearchBox,
            float Score, float Scale, float ScaleX, float ScaleY,
            List<ScorePoint> Points)
        {
            this.MatchBox = MatchBox;
            this.SearchBox = SearchBox;
            this.Score = Score;
            this.Scale = Scale;
            this.ScaleX = ScaleX;
            this.ScaleY = ScaleY;
            this.Points = Points;
        }

    }

    public enum MatchParas
    {
        /// <summary>
        /// 金字塔模板匹配最大匹配个数 int [1-1000] 1
        /// </summary>
        MaxMatchNum,
        /// <summary>
        /// 金字塔模板匹配匹配起始角度 int [-180-180] -180
        /// </summary>
        AngleStart,
        /// <summary>
        /// 金字塔模板匹配匹配终止角度 int [-180-180] 180
        /// </summary>
        AngleEnd,
        /// <summary>
        /// 金字塔模板匹配匹配噪点标记(是否考虑噪点) int [0-1] 0
        /// </summary>
        SpotterFlag,
        /// <summary>
        /// 金字塔模板匹配最大重叠率 int [1-100] 40
        /// </summary>
        MaxOverlap,
        /// <summary>
        /// 金字塔模板匹配匹配延拓阈值 int [0-90] 0
        /// </summary>
        MatchExtentRate,
        /// <summary>
        /// 金字塔模板匹配匹配速度阈值开关 int [0-1] 0
        /// </summary>
        RoughFlag,
        /// <summary>
        /// 金字塔模板匹配匹配速度阈值 int [0-100] 50
        /// </summary>
        RoughThreshold,
        /// <summary>
        /// 金字塔模板匹配超时控制 int [0-10000] 0
        /// </summary>
        TimeOut,
        /// <summary>
        /// 金字塔模板匹配边缘阈值 int [1-255] 40
        /// </summary>
        MatchThresholdHigh,
        /// <summary>
        /// 金字塔模板匹配重要点得分阈值 int [1-100] 50
        /// </summary>
        ImportantScoreThreshold,
        /// <summary>
        /// 金字塔模板匹配匹配极性(是否考虑极性) Enum ["No" "Yes"] "Yes"
        /// </summary>
        Polarity,
        /// <summary>
        /// 金字塔模板匹配排序类型 Enum ["None" "Score" "Angle" "X" "Y" "XY" "YX"] "Score"
        /// </summary>
        SortType,
        /// <summary>
        /// 金字塔模板匹配边缘阈值标志 Enum ["Auto" "Model" "Manual"] "Auto"
        /// </summary>
        MatchThresholdFlag,
        /// <summary>
        /// 金字塔模板匹配最小匹配分数 float [0.0-1.0] 0.5
        /// </summary>
        MinScore,
        /// <summary>
        /// 金字塔模板匹配匹配起始尺度 float [0.1-10.0] 1
        /// </summary>
        ScaleStart,
        /// <summary>
        /// 金字塔模板匹配匹配终止尺度 float [0.1-10.0] 1
        /// </summary>
        ScaleEnd,
        /// <summary>
        /// 金字塔模板匹配匹配起始X尺度 float [0.1-10.0] 1
        /// </summary>
        ScaleXStart,
        /// <summary>
        /// 金字塔模板匹配匹配终止X尺度 float [0.1-10.0] 1
        /// </summary>
        ScaleXEnd,

    }


    public class LineResult
    {
        public PointF Startpoint;
        public PointF Endpoint;
        public float Angle;
        public ROI SearchBox;
        public float Straightness;
        public int pointnumber;
        public List<ScorePoint> Points;
        public bool IsOk = false;

        public LineResult()
        {

        }
        public LineResult(PointF Startpoint, PointF Endpoint, ROI SearchBox,
            float Angle, float Straightness, int pointnumber, List<ScorePoint> Points)
        {
            this.Startpoint = Startpoint;
            this.Endpoint = Endpoint;
            this.SearchBox = SearchBox;
            this.Angle = Angle;
            this.Straightness = Straightness;
            this.pointnumber = pointnumber;
            this.Points = Points;
        }

    }


    public enum LineFindParas
    {
        /// <summary>
        /// 边缘阈值 int [1-255] 5
        /// </summary>
        EdgeStrength,
        /// <summary>
        /// 卡尺数量 int [2-1000] 30
        /// </summary>
        RayNum,
        /// <summary>
        /// 卡尺宽度(参数范围同时和能力集有关,即Min(AbilityWidth/Height, Value)) int [1-500] 5
        /// </summary>
        RegionWidth,
        /// <summary>
        /// 剔除距离 int [1-1000] 5
        /// </summary>
        RejectDist,
        /// <summary>
        /// 滤波核半宽 int [1-50] 2
        /// </summary>
        KernelSize,
        /// <summary>
        /// 忽略点数 int [0-998] 0
        /// </summary>
        RejectNum,
        /// <summary>
        /// 点边距离 int [0-100] 0
        /// </summary>
        P2BoxDist,
        /// <summary>
        /// 直线度 int [0-100] 0
        /// </summary>
        LineRate,
        /// <summary>
        /// 边缘极性 Enum ["BlackToWhite" "WhiteToBlack" "Both"] "Both"
        /// </summary>
        EdgePolarity,
        /// <summary>
        /// 查找模式 Enum ["Best" "First" "Last" "Manual"] "Best"
        /// </summary>
        LineFindMode,
        /// <summary>
        /// 搜索方向 Enum ["UpToDown" "LeftToRight"] "UpToDown"
        /// </summary>
        FindOrient,
        /// <summary>
        /// 拟合方式 Enum ["LS" "Huber" "Tukey" ] "Huber"
        /// </summary>
        FitFun,
        /// <summary>
        /// 初始拟合类型 Enum ["ALS" "LLS" ] "LLS"
        /// </summary>
        FitInitType,

    }


    public class CircleResult
    {
        public PointF CircleCenter;
        public float CircleRadius;
        public PointF ArcCenter;
        public float ArcOuterRadius;
        public float ArcStartAngle;
        public float ArcAngleRange;
        public ROI SearchBox;
        public float Circleness;

        public int pointnumber;
        public List<ScorePoint> Points;

        public bool IsOk = false;

        public CircleResult()
        {

        }
        public CircleResult(PointF CircleCenter, float CircleRadius, PointF ArcCenter,
            float ArcOuterRadius, float ArcStartAngle, float ArcAngleRange, float Circleness,
            ROI SearchBox, int pointnumber, List<ScorePoint> Points)
        {
            this.CircleCenter = CircleCenter;
            this.CircleRadius = CircleRadius;
            this.ArcCenter = ArcCenter;
            this.ArcOuterRadius = ArcOuterRadius;
            this.ArcStartAngle = ArcStartAngle;
            this.ArcAngleRange = ArcAngleRange;
            this.Circleness = Circleness;
            this.SearchBox = SearchBox;
            this.pointnumber = pointnumber;
            this.Points = Points;
        }

    }


    public enum CircleFindParas
    {
        /// <summary>
        /// 圆最小半径 int [1-10000] 10
        /// </summary>
        MinRadius,
        /// <summary>
        /// 圆最大半径 int [1-10000] 100
        /// </summary>
        MaxRadius,
        /// <summary>
        /// 边缘阈值 int [0-255] 5
        /// </summary>
        EdgeThresh,
        /// <summary>
        /// 卡尺数量 int [3-1000] 30
        /// </summary>
        RadNum,
        /// <summary>
        /// 圆环起始角度 int [-180-180] 0
        /// </summary>
        StartAngle,
        /// <summary>
        /// 圆环角度范围 int [1-360] 360
        /// </summary>
        AngleExtend,
        /// <summary>
        /// 下采样系数 int [1-8] 8
        /// </summary>
        CCDSampleScale,
        /// <summary>
        /// 圆定位敏感度 int [1-1000] 10
        /// </summary>
        CCDCircleThresh,
        /// <summary>
        /// 滤波核半宽 int [1-50] 2
        /// </summary>
        EdgeWidth,
        /// <summary>
        /// 卡尺宽度(参数范围同时和能力集有关,即Min(AbilityWidth/Height, Value)) int [1-500] 5
        /// </summary>
        ProLength,
        /// <summary>
        /// 忽略点数 int [0-998] 0
        /// </summary>
        RejectNum,
        /// <summary>
        /// 剔除距离 int [1-15000] 5
        /// </summary>
        RejectDist,
        /// <summary>
        /// 圆度 int [0-100] 0
        /// </summary>
        CircleRate,
        /// <summary>
        /// 圆面积度 int [0-100] 0
        /// </summary>
        AreaRate,
        /// <summary>
        /// 边缘极性 Enum ["BlackToWhite" "WhiteToBlack" "Both"] "BlackToWhite"
        /// </summary>
        EdgePolarity,
        /// <summary>
        /// 查找模式 Enum ["Best" "Largest" "Smallest" "Manual"] "Smallest"
        /// </summary>
        CircleFindMode,
        /// <summary>
        /// 边缘扫描方向 Enum ["InnerToOuter" "OuterToInner"] "InnerToOuter"
        /// </summary>
        EdgeScanOrient,
        /// <summary>
        /// 拟合方式 Enum ["LS" "Huber" "Tukey" ] "Huber"
        /// </summary>
        FitFun,
        /// <summary>
        /// 初始拟合类型 Enum ["ALS" "LLS" ] "LLS"
        /// </summary>
        InitType,
    }




}
