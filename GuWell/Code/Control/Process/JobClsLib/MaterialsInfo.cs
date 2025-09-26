using GlobalDataDefineClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobClsLib
{
    public class JobMaterialInfo
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 料条信息
        /// </summary>
        public ComponentInfo Component { get; set; }
        public SubmountInfo Submount { get; set; }
        /// <summary>
        /// 检测的Recipe
        /// </summary>
        public BondRecipe ProcessRecipe { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string RecipeName { get; set; }

        public string TrayID { get; set; }

        public string WaferID { get; set; }

        /// <summary>
        /// 是否使用sinf
        /// </summary>
        public bool IsUseSinf { get; set; }

        /// <summary>
        /// sinf路径
        /// </summary>
        public string SinfPath { get; set; }
    }
    public class WaferTransferInfo
    {
        public EnumBonderAction TransferAction { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public ComponentInfo Component { get; set; }

    }
    public class WaferInfo
    {
        public string ID { get; set; }
        public string Index { get; set; }
        public string RowIndex { get; set; }
        public string ColumnIndex { get; set; }
        public float coordinateX { get; set; }
        public float coordinateY { get; set; }

    }
    public class ComponentTransferInfo
    {
        public EnumUsedPP UsedPP { get; set; }
        public EnumBonderAction TransferAction { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public ComponentInfo Component { get; set; }

    }
    public class SubmountTransferInfo
    {
        public EnumUsedPP UsedPP { get; set; }
        public EnumBonderAction TransferAction { get; set; }
        /// <summary>
        /// 信息
        /// </summary>
        public SubmountInfo Component { get; set; }

    }
    public class ComponentInfo
    {
        public string ID { get; set; }
        public string Index { get; set; }
        public string RowIndex { get; set; }
        public string ColumnIndex { get; set; }
        public float coordinateX { get; set; }
        public float coordinateY { get; set; }

    }
    public class SubmountInfo
    {
        public string ID { get; set; }
        public string Index { get; set; }
        public string RowIndex { get; set; }
        public string ColumnIndex { get; set; }
        public float coordinateX { get; set; }
        public float coordinateY { get; set; }

    }

    public class RunProcedureInfo
    {
        /// <summary>
        /// 料条ID
        /// </summary>
        public string MaterialStripID { get; set; }
        public string SourceSlotIndex { get; set; }

        /// <summary>
        ///  LotID
        /// </summary>
        public string LotID { get; set; }

        /// <summary>
        /// 是否全检
        /// </summary>
        public bool isSelectAll { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Queue<JobMaterialInfo> SelectMaterials { get; set; }

    }
}
