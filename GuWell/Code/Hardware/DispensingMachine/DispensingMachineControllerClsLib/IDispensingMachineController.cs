using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispensingMachineControllerClsLib
{
    public interface IDispensingMachineController
    {
        /// <summary>
        /// 链接状态
        /// </summary>
        bool IsConnect { get; }
        /// <summary>
        /// 建立连接
        /// </summary>
        void Connect();
        /// <summary>
        /// 建立连接
        /// </summary>
        void Connect(SerialPort serial);
        /// <summary>
        /// 断开连接
        /// </summary>
        void Disconnect();

        /// <summary>
        /// 
        /// </summary>
        void Write();


        bool Set(MUSASHICommandenum command, params string[] parameters);

        List<decimal> Get(MUSASHICommandenum command, params string[] parameters);

        /// <summary>
        /// 点胶
        /// </summary>
        void Shot();
        /// <summary>
        /// 获取配方列表
        /// </summary>
        /// <returns></returns>
        List<string> GetRecipeList();
        /// <summary>
        /// 指定配方
        /// </summary>
        /// <param name="recipeName"></param>
        void LoadRecipe(string recipeName);
        /// <summary>
        /// 根据名称获取配方信息
        /// </summary>
        /// <param name="recipeName"></param>
        /// <returns></returns>
        DispenseRecipeInfo GetRecipeInfo(string recipeName);
        /// <summary>
        /// 获取计数次数
        /// </summary>
        /// <returns></returns>
        int GetShotCounter();
        /// <summary>
        /// 获取余量
        /// </summary>
        /// <returns></returns>
        float GetRemainingQuantity();
        (decimal Time, decimal Pressure, decimal Vacuum) ReadDispensingParameters(int channel);

    }
}
