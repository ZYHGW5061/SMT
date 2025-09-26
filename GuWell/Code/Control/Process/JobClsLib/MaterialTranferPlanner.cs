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
    public class MaterialTranferPlanner
    {
        #region 成员
        /// <summary>
        /// 
        /// </summary>
        private Queue<MaterialMapInformation> _selectedComponentsInfo { get; set; }
        private Queue<MaterialMapInformation> _selectedSubmountsInfo { get; set; }
        private HardwareConfiguration _hardwareConfig { get { return HardwareConfiguration.Instance; } }

        #endregion


        # region Plan transfer action


        /// <summary>
        /// 
        /// </summary>
        /// <param name="materials"></param>
        /// <returns></returns>
        public Queue<ComponentTransferInfo> PlanComponentTransferActionQueue(Queue<MaterialMapInformation> materials)
        {
            _selectedComponentsInfo = materials;
            Queue<ComponentTransferInfo> actionQueue = new Queue<ComponentTransferInfo>();
            //while (true)
            //{
            //    MaterialMapInformation nextComponent = this.GetNextComponentInfo();
            //    if (nextComponent == null)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        ComponentInfo nextComponentInfo = new ComponentInfo()
            //        {
            //            ID = nextComponent.MaterialNumber.ToString(),
            //            Index = nextComponent.MaterialNumber.ToString(),
            //            RowIndex = nextComponent.MaterialCoordIndex.X.ToString(),
            //            ColumnIndex = nextComponent.MaterialCoordIndex.Y.ToString(),
            //            coordinateX = nextComponent.MaterialLocation.X + (float)JobInfosManager.Instance.ComponentMarkPointOffset.X,
            //            coordinateY = nextComponent.MaterialLocation.Y + (float)JobInfosManager.Instance.ComponentMarkPointOffset.Y
            //        };
            //        actionQueue.Enqueue(new ComponentTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.ChipPP,
            //            TransferAction = EnumBonderAction.PositionComponent,
            //            Component = nextComponentInfo
            //        });
            //        actionQueue.Enqueue(new ComponentTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.ChipPP,
            //            TransferAction = EnumBonderAction.PickComponent,
            //            Component = nextComponentInfo
            //        });
            //        actionQueue.Enqueue(new ComponentTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.ChipPP,
            //            TransferAction = EnumBonderAction.AccuracyComponentPosition,
            //            Component = nextComponentInfo
            //        });
            //        actionQueue.Enqueue(new ComponentTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.ChipPP,
            //            TransferAction = EnumBonderAction.PickComponent,
            //            Component = nextComponentInfo
            //        });
            //    }
            //}

            return actionQueue;
        }
        public Queue<SubmountTransferInfo> PlanSubmountTransferActionQueue(Queue<MaterialMapInformation> materials)
        {
            _selectedSubmountsInfo = materials;
            Queue<SubmountTransferInfo> actionQueue = new Queue<SubmountTransferInfo>();
            //while (true)
            //{
            //    MaterialMapInformation nextSubmount = this.GetNextSubmountInfo();
            //    if (nextSubmount == null)
            //    {
            //        break;
            //    }
            //    else
            //    {
            //        SubmountInfo nextSubmountInfo = new SubmountInfo()
            //        {
            //            ID = nextSubmount.MaterialNumber.ToString(),
            //            Index = nextSubmount.MaterialNumber.ToString(),
            //            RowIndex = nextSubmount.MaterialCoordIndex.X.ToString(),
            //            ColumnIndex = nextSubmount.MaterialCoordIndex.Y.ToString(),
            //            coordinateX = nextSubmount.MaterialLocation.X + (float)JobInfosManager.Instance.ComponentMarkPointOffset.X,
            //            coordinateY = nextSubmount.MaterialLocation.Y + (float)JobInfosManager.Instance.ComponentMarkPointOffset.Y
            //        };
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.LoadSubstrate,
            //            Component = nextSubmountInfo
            //        });
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.PickSubmountFromSubstrate,
            //            Component = nextSubmountInfo
            //        });
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.AccuracySubmountPosition,
            //            Component = nextSubmountInfo
            //        });
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.PlaceSubmounttoChuck,
            //            Component = nextSubmountInfo
            //        });
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.PositionBeforeBond,
            //            Component = nextSubmountInfo
            //        });
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.PositionSubmountBeforePickFromChuck,
            //            Component = nextSubmountInfo
            //        });
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.PickSubmountFromChuck,
            //            Component = nextSubmountInfo
            //        });
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.AccuracySubmountPosition,
            //            Component = nextSubmountInfo
            //        });
            //        actionQueue.Enqueue(new SubmountTransferInfo()
            //        {
            //            UsedPP = EnumUsedPP.SubmountPP,
            //            TransferAction = EnumBonderAction.PlaceSubmounttoSubstrate,
            //            Component = nextSubmountInfo
            //        });
            //    }
            //}
            return actionQueue;
        }
        #endregion

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private MaterialMapInformation GetNextComponentInfo()
        {
            if (_selectedComponentsInfo.Count > 0)
            {
                return _selectedComponentsInfo.Dequeue();
            }
            else
            {
                return null;
            }
        }
        private MaterialMapInformation GetNextSubmountInfo()
        {
            if (_selectedSubmountsInfo.Count > 0)
            {
                return _selectedSubmountsInfo.Dequeue();
            }
            else
            {
                return null;
            }
        }
    }
}
