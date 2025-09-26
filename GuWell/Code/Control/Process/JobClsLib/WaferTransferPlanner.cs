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
    /// 传片流程规划器
    /// </summary>
    public class WaferTransferPlanner
    {
        #region 成员
        /// <summary>
        /// 
        /// </summary>
        private Queue<JobMaterialInfo> _selectedStripInfo { get; set; }
        private HardwareConfiguration _hardwareConfig { get { return HardwareConfiguration.Instance; } }

        #endregion

        #region 公有方法


        # region Plan transfer action


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strips"></param>
        /// <returns></returns>
        public Queue<WaferTransferInfo> PlanTransferActionQueue(Queue<JobMaterialInfo> strips)
        {
            _selectedStripInfo = strips;
            Queue<WaferTransferInfo> actionQueue = new Queue<WaferTransferInfo>();

            return actionQueue;
        }
        #endregion

        #endregion

        #region 私有方法
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private JobMaterialInfo GetNextWaferInfo()
        {
            if (_selectedStripInfo.Count > 0)
            {
                return _selectedStripInfo.Dequeue();
            }
            else
            {
                return null;
            }
        }


        #endregion
    }
}
