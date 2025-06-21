using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestDragon.Framework.UserControls;
using WestDragon.Framework.UtilityHelper;

namespace PositioningSystemClsLib
{

    /// <summary>
    /// 自动聚焦接口
    /// </summary>
    public interface IAutoFocus
    {
        Action<AutoFocusActionCallback> UserCustomAction { get; set; }

        AutoFocusParams Param { get; set; }

        CircleFigure FocusRegion { get; }

        PairData Result { get; set; }

        void DoAutoFocusSync();
        void DoAutoFocusSync_RelateiveMove();

    }

    /// <summary>
    /// 在自动聚焦过程中的自定义回调
    /// </summary>
    public class AutoFocusActionCallback : ACustomActionBaseClass
    {
        /// <summary>
        ///  拍照得到的图像
        /// </summary>
        public Bitmap ImageCaptured { get; set; }
    }

    /// <summary>
    /// 工厂类
    /// </summary>
    public static class AutoFocusProcessorFactory
    {
        public enum AutoFocusType { CognexAlgorithm1 = 0, CognexAlgorithm2 = 1, OpencvAlgorithm = 2 }

        public static IAutoFocus Create(AutoFocusType type)
        {
            IAutoFocus autofocusProcessor = null;

            switch (type)
            {
                case AutoFocusType.CognexAlgorithm1:
                    //autofocusProcessor = new VisionAutoFocusProcessorV2();
                    break;
                case AutoFocusType.CognexAlgorithm2:
                    //autofocusProcessor = new VisionAutoFocusProcessorV3();
                    break;
                case AutoFocusType.OpencvAlgorithm:
                    autofocusProcessor = new VisionAutoFocusProcessor();
                    break;
                default:
                    autofocusProcessor = new VisionAutoFocusProcessor();
                    break;
            }

            return autofocusProcessor;
        }
    }

    /// <summary>
    /// 聚焦原始数据--中间值
    /// </summary>
    internal class FocusRawValue
    {
        public Bitmap FocusImage { get; set; }
        public double FocusZ { get; set; }
        public double Sharpness { get; set; }
    }
    [Serializable]
    public class AutoFocusParams : ACloneable<AutoFocusParams>
    {
        /// <summary>
        /// 自动聚焦的方法
        /// </summary>
        public EnumAutoFocusMethod Method { get; set; }

        /// <summary>
        /// 开始Z轴位置
        /// </summary>
        public double StartZ { get; set; }

        /// <summary>
        /// 结束Z轴位置
        /// </summary>
        public double EndZ { get; set; }

        /// <summary>
        /// 自动聚焦Z步进值
        /// </summary>
        public double StepZ { get; set; }

        /// <summary>
        /// AutoCorrelation = 1，自相关
        /// AbsDiff = 2，绝对差异
        /// BandPass = 4, 带通
        /// GradientEnergy = 8，梯度能量
        /// </summary>
        public EnumImageSharpnessMode Mode { get; set; }

        /// <summary>
        /// BandPass 参数, 高频率(0~0.5)
        /// </summary>
        public double BandPassMaxFrequency { get; set; }

        /// <summary>
        /// BandPass 参数, 低频率(0~0.5)
        /// </summary>
        public double BandPassMinFrequency { get; set; }

        /// <summary>
        ///自相关 参数, 噪声级别
        /// </summary>
        public double AutoCorrelationNoiseLevel { get; set; }

        /// <summary>
        ///绝对差异 参数, X距离
        /// </summary>
        public int AbsDiffDistanceX { get; set; }

        /// <summary>
        ///绝对差异 参数, X距离
        /// </summary>
        public int AbsDiffDistanceY { get; set; }

        /// <summary>
        ///梯度能量 参数, 低通平滑, (0~2)
        /// </summary>
        public int GradientEnergyLowPassSmoothing { get; set; }

        /// <summary>
        /// 是否保存中间聚焦数据
        /// </summary>
        public bool SaveIntermediateFocusData { get; set; }

        /// <summary>
        /// 判断参数是否为空
        /// </summary>
        public bool IsEmpty
        {
            get { return StartZ == 0 && EndZ == 0 && StepZ == 0; }
        }

        /// <summary>
        /// 有效聚焦区域
        /// </summary>
        public CircleFigure FocusRegion { get; set; }

        /// <summary>
        /// 初始化参数
        /// </summary>
        /// <param name="start">聚焦起始Z方向位置</param>
        /// <param name="end">聚焦结束Z方向位置</param>
        /// <param name="step">步进值</param>
        /// <param name="focusRegion">聚焦区域</param>
        /// <param name="method">聚焦方式</param>
        public AutoFocusParams(double start, double end, double step, CircleFigure focusRegion, EnumAutoFocusMethod method)
        {
            StartZ = start;
            EndZ = end;
            StepZ = step;
            Method = method;
            FocusRegion = focusRegion;

            AbsDiffDistanceX = 1;
            AbsDiffDistanceY = 1;
            BandPassMaxFrequency = 0.45;
            BandPassMinFrequency = 0.25;
            GradientEnergyLowPassSmoothing = 0;
            AutoCorrelationNoiseLevel = 0;
            Mode = EnumImageSharpnessMode.MeanStdDev;
        }

        public new AutoFocusParams Clone()
        {
            return base.Clone() as AutoFocusParams;
        }
    }
}
