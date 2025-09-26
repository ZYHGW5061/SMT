using GlobalDataDefineClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WestDragon.Framework.UtilityHelper;

namespace JobClsLib
{
    /// <summary>
    /// 模拟下的Job检测流程
    /// </summary>
    internal class SimulatedBondJob : ABondJob
    {



        /// <summary>
        /// 模拟初始化
        /// </summary>
        public SimulatedBondJob()
        {
            _statusChecker = new RunStatusController();
        }

        /// <summary>
        /// 终止任务 & 重置坐标系到校准前的状态
        /// </summary>
        public override void Abort()
        {
            //停止当前检测过程
            _statusChecker.Abort();
            AbortSubScheduler();
        }


        /// <summary>
        /// 终止子任务，数据采集、数据处理、数据分析子任务
        /// </summary>
        protected void AbortSubScheduler()
        {
        }



        /// <summary>
        /// 终止任务, 不重置坐标系到校准前的状态
        /// </summary>
        public override void AbortWithoutReset()
        {
            //停止当前检测过程
            _statusChecker.Abort(); AbortSubScheduler();
        }

        /// <summary>
        /// 测量前执行
        /// </summary>
        protected override void RunBeforeJob()
        {
        }
        /// <summary>
        /// 生成数据存储目录
        /// </summary>
        /// <returns></returns>
        protected override string GenerateDataSavingPath()
        {
            try
            {
                return "";
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 初始化WaferJob
        /// </summary>
        public override void InitialJob()
        {
            try
            {              
                //初始化完成
                IsInitialized = true;
            }
            catch (Exception ex)
            {
                IsInitialized = false;
            }
        }

        /// <summary>
        /// 运行WaferJob
        /// </summary>
        /// <param name="recipe"></param>
        public override void RunProcessJob(BondRecipe recipe, int index = 0, bool isTestRecipe = false)
        {
            //------流程开始---------------
            this._statusChecker.Start();
            this._isCompleted = false;

            Task.Factory.StartNew(new Action(() =>
            {
                try
                {
                    if (UIOperation != null)
                    {
                        UIOperation();
                    }

                    //更新当前Recipe
                    this._currentRecipe = recipe;
                    this.MaterialStripIndex = index;

                    //生成原始数据存储路径
                    this.RawDataSavingPath = GenerateDataSavingPath();


                }
                catch (Exception ex)
                {
                    this.Abort();
                    ReportJobRunningStatus(EnumBondJobStatus.ProcessError, "Failed to excute job.");
                }
            }));
        }
    }
}
