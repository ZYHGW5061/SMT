using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlarmManagementClsLib.DB
{
    /// <summary>
    /// 
    /// </summary>
    public class AlarmEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DefineId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AlarmType EnumType { get { return (AlarmType)Type; } }
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int State { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public AlarmState EnumState { get { return (AlarmState)State; } }
        /// <summary>
        /// 
        /// </summary>
        public int Source { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime OccurrenceTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime AcknowledgeTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime RecoveryTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ClearTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ResetTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int RecoveryOption { get; set; }
        public RecoveryOptions EnumRecoveryOption { get { return (RecoveryOptions)RecoveryOption; } }
        /// <summary>
        /// 
        /// </summary>
        public bool IsRecoverable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string RecoveryFailText { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Cause { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }
    }
}
