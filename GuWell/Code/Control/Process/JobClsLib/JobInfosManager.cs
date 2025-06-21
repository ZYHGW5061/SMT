using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobClsLib
{
    /// <summary>
    /// 当前Job Wafer基础信息列表管理
    /// </summary>
    public class JobInfosManager
    {
        private static volatile JobInfosManager _instance;
        private static readonly object _lockObj = new object();
        private JobInfosManager()
        {
            JobMaterialStripSourceInfos = new Dictionary<string, MaterialStripSourceInfo>();
            ComponentMarkPointOffset = new XYZTCoordinateConfig();
            SubmountMarkPointOffset = new XYZTCoordinateConfig();
            CurrentComponentForProgramAccuracy = new XYZTCoordinateConfig();
        }
        public static JobInfosManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new JobInfosManager();
                        }
                    }
                }
                return _instance;
            }
        }

        public Dictionary<string,MaterialStripSourceInfo> JobMaterialStripSourceInfos { get; private set; }
        /// <summary>
        /// 当前最近的JobWaferSourceInfo
        /// </summary>
        public MaterialStripSourceInfo CurrentJobMaterialStripSourceInfo { get; set; }
        public XYZTCoordinateConfig ComponentMarkPointOffset { get; set; }
        public XYZTCoordinateConfig SubmountMarkPointOffset { get; set; }
        public XYZTCoordinateConfig CurrentComponentForProgramAccuracy { get; set; }
        public EnumJobRunStatus JobCurrentStatus { get; set; }
        public XYZTCoordinateConfig ComponentBasePosition { get; set; }
        public int CurrentBondPositionIndex { get; set; }
        public int CurrentComponentIndex { get; set; }
        public XYZTCoordinateConfig CurrentComponentMapPosition { get; set; }
    }

    public class MaterialStripSourceInfo
    {
        /// <summary>
        /// Lot 
        /// </summary>
        public string LotId { get; set; }

        /// <summary>
        /// WaferId
        /// </summary>
        public string MaterialStripId { get; set; }
        /// <summary>
        /// 源Loadport Slot Index
        /// </summary>
        public string SourceSlotIndex { get; set; }

    }
}
