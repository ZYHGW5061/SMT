using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispenserControlClsLib
{
    public interface IDispenserController
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
        /// 断开连接
        /// </summary>
        void Disconnect();

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
        //获取报警状态

    }
}
