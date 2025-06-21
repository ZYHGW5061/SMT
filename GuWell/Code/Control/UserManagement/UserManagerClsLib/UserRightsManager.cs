using GlobalDataDefineClsLib;
using GWUserManager.Entity;
using GWUserManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GWUserManager.Entity.User;

namespace UserManagerClsLib
{
    public class UserRightsManager
    {
        MenuService menuService = new MenuService();
        private static volatile UserRightsManager _instance;
        private static readonly object _lockObj = new object();
        public static UserRightsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new UserRightsManager();
                        }
                    }
                }
                return _instance;
            }
        }
        public string CurrentUserName { get; set; }
        public int CurrentUserType { get; set; }
        private UserRightsManager()
        {
        }
        public List<KeyValuePair<int,string>> GetAccessMenusByUserlevel(int userLevel,int parentId)
        {
            var ret = new List<KeyValuePair<int, string>>();
            var menus = menuService.GetMenusByUserLevel((EnumUserType)userLevel);
            foreach (var item in menus.Where(m=>m.ParentId== parentId))
            {
                ret.Add(new KeyValuePair<int, string>(item.Id, item.MenuName));
            }
            return ret;
        }
        public List<RightsInfo> GetAllRights()
        {
            var temp = new List<RightsInfo>();
            foreach (var item in Enum.GetValues(typeof(User.EnumUserType)))
            {
                temp.Add(new RightsInfo { RightsID = (int)item, RightsType = item.ToString() });
            }
            return temp;
        }
        public List<FunctionInfo> GetFunctionInfoByParentID(int parentID = 0)
        {
            var allMenu = menuService.GetAllMenus();
            var selMenus = allMenu.Where(i => i.ParentId == parentID).ToList();
            List<FunctionInfo> listFunctionInfo = new List<FunctionInfo>();
            for (int i = 0; i < selMenus.Count; i++)
            {
                listFunctionInfo.Add(new FunctionInfo()
                {
                    FunctionID = selMenus[i].Id,
                    FunctionName = selMenus[i].MenuName,
                    ParentID = selMenus[i].ParentId
                });
            }
            return listFunctionInfo;
        }
        /// <summary>
        /// 根据userlevel获取权限信息
        /// </summary>
        /// <param name="rightsID">即userlevel</param>
        /// <returns></returns>
        public List<FunctionRightsInfo> GetFunctionRightsInfoByRightsID(int rightsID)
        {
            var rightsMenu = menuService.GetMenusByUserLevel((EnumUserType)rightsID);
            List<FunctionRightsInfo> listFunctionRightsInfo = new List<FunctionRightsInfo>();

                for (int i = 0; i < rightsMenu.Count; i++)
                {
                    listFunctionRightsInfo.Add(new FunctionRightsInfo()
                    {
                        FunctionInfoID = rightsMenu[i].Id,
                        RightsID = rightsMenu[i].Level,
                        Visible = true,
                        FunctionName = rightsMenu[i].MenuName
                    });
                }
            return listFunctionRightsInfo;

        }
        /// <summary>
        /// 根据权限类型ID和级别ID获得对应的功能权限
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="rightsId"></param>
        /// <returns></returns>
        //public List<FunctionRightsInfo> GetFunctionRightsInfo(int rightsId, int parentId = 0)
        //{
        //    string sqlstr = string.Format(@"select fr.FunctionInfoID,fr.RightsID,fr.Visible,f.FunctionName from  FunctionInfo f left join  FunctionRights  fr  
        //                                                    on f.id = fr.FunctionInfoID  where f.parentid = {0}  and fr.rightsid = {1}", parentId, rightsId);
        //    return GetFunctionRightsInfoByRightsIDTable(sqlstr);
        //}

        /// <summary>
        /// 更新一条记录        
        /// </summary>
        /// <param name="info"></param>
        public void UpdateUserLevelRights(int userLevel, List<int> menuIds, bool delOld = true)
        {
            menuService.AddUserLevelMenuRelated((User.EnumUserType)userLevel, menuIds, delOld);
            //string sqlstr;
            //sqlstr = string.Format("DELETE FROM   FunctionRights  WHERE  FunctionInfoID={0} and RightsID={1}", info.FunctionInfoID, info.RightsID);
            //_dataBaseManager.SingleSqlCommand(sqlstr);
            //sqlstr = string.Format("INSERT INTO FunctionRights   (FunctionInfoID  ,RightsID ,Visible)  VALUES    (  {0} , {1}  , {2})", info.FunctionInfoID, info.RightsID, Convert.ToInt32(info.Visible));
            //_dataBaseManager.SingleSqlCommand(sqlstr);
        }
        /// <summary>
        /// 根据权限类型ID获得对应的功能权限
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        //private List<FunctionRightsInfo> GetFunctionRightsInfoByRightsIDTable(string sqlstr)
        //{
        //    DataTable functionRightsInfo = _dataBaseManager.SelectSqlCommand(sqlstr);
        //    if (functionRightsInfo != null)
        //    {
        //        List<FunctionRightsInfo> listFunctionRightsInfo = new List<FunctionRightsInfo>();
        //        if (functionRightsInfo.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < functionRightsInfo.Rows.Count; i++)
        //            {
        //                listFunctionRightsInfo.Add(new FunctionRightsInfo()
        //                {
        //                    FunctionInfoID = (int)functionRightsInfo.Rows[i]["FunctionInfoID"],
        //                    RightsID = (int)functionRightsInfo.Rows[i]["RightsID"],
        //                    Visible = Convert.ToBoolean(functionRightsInfo.Rows[i]["Visible"]),
        //                    FunctionName = functionRightsInfo.Rows[i]["FunctionName"].ToString()
        //                });
        //            }
        //        }
        //        return listFunctionRightsInfo;
        //    }
        //    return null;
        //}
    }
}
