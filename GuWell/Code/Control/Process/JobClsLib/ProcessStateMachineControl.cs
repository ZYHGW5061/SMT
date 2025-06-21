using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using thinger.cn.DataConvertHelper;

namespace JobClsLib
{
    public static class ProcessStateMachineControl
    {

        #region 参数


        public static object GetParameterByCode(int code)
        {
            // 示例：根据 code 设置不同的参数  
            switch (code)
            {
                case 1:
                    return 0;
                default:
                    throw new InvalidOperationException($"Unsupported generic parameter code: {code}");
            }
        }

        public static object GetPositionByCode(int code)
        {

            // 示例：根据 code 设置不同的参数  
            switch (code)
            {
                case 1:
                    return 0;
                default:
                    return -1;
            }
        }



        #endregion


        #region 方法对应

        private static readonly Dictionary<int, Func<object, XktResult<string>>> methodFactory = new Dictionary<int, Func<object, XktResult<string>>>
        {
            { 0, param => SimulateAction(0,0) },
            { 1, param => InitAction(1,0) },
            { 29, param => InitDataAction(1,0) },
        };


        #endregion




        /// <summary>  
        /// 方法调用  
        /// </summary>  
        /// <param name="state"></param>  
        /// <returns></returns>  
        public static XktResult<string> ExecuteState(string state, object parameter = null)
        {
            if (state.Length != 9)
                throw new ArgumentException("State must be a 9-character string.");

            int variableCode = int.Parse(state.Substring(0, 3));
            int methodCode = int.Parse(state.Substring(3, 3));

            if (variableCode != 0)
            {
                parameter = GetParameterByCode(variableCode);
            }

            if (parameter == null)
            {
                //throw new ArgumentNullException(nameof(parameter), "Parameter cannot be null");
            }


            if (methodFactory.TryGetValue(methodCode, out var method))
            {
                XktResult<string> result = new XktResult<string>();
                //var runningType = SystemConfiguration.Instance.JobConfig.RunningType;
                //if (runningType == EnumRunningType.Actual)
                //{
                //    result = method(parameter);
                //}
                //else
                //{
                //    result = SimulateAction(methodCode, variableCode);
                //}
                result = method(parameter);

                return result;
            }
            else
            {
                throw new InvalidOperationException($"Method not found {methodCode}");
            }
        }





        #region 方法

        /// <summary>
        /// 模拟方法
        /// </summary>
        /// <param name="methodCode"></param>
        /// <param name="variableCode"></param>
        /// <returns></returns>
        private static XktResult<string> SimulateAction(int methodCode, int variableCode)
        {
            string variableCodestr = "000";
            variableCodestr = variableCode.ToString("D3");
            string methodCodestr = "000";
            methodCodestr = methodCode.ToString("D3");

            Thread.Sleep(1000);

            Task.Factory.StartNew(new Action(() =>
            {
                DataModel.Instance.JobLogText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " 状态机:" + variableCodestr + methodCodestr + "000" + " 完成";
            }));



            return new XktResult<string> { Content = variableCodestr + methodCodestr + "000", IsSuccess = true, Message = "完成" };
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        /// <returns></returns>
        private static XktResult<string> InitDataAction(int methodCode, int variableCode)
        {
            string variableCodestr = "000";
            variableCodestr = variableCode.ToString("D3");
            string methodCodestr = "000";
            methodCodestr = methodCode.ToString("D3");

            Thread.Sleep(5000);

            DataModel.Instance.JobLogText = "";

            DataModel.Instance.JobLogText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "初始化数据完成";
            return new XktResult<string> { Content = variableCodestr + methodCodestr + "000", IsSuccess = true, Message = "初始化数据完成" };

        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns></returns>
        private static XktResult<string> InitAction(int methodCode, int variableCode)
        {
            string variableCodestr = "000";
            variableCodestr = variableCode.ToString("D3");
            string methodCodestr = "000";
            methodCodestr = methodCode.ToString("D3");

            Console.WriteLine("Materialbox hooked to safe position.");
            DataModel.Instance.JobLogText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "初始化：料盒钩爪移动到空闲中";

            DataModel.Instance.JobLogText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "初始化完成";
            return new XktResult<string> { Content = variableCodestr + methodCodestr + "000", IsSuccess = true, Message = "初始化完成" };

        }


        #endregion



    }
}
