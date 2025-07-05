using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using WestDragon.Framework.UtilityHelper;

namespace RecipeClsLib
{


    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot("RecipeBody")]
    public class BondRecipe
    {
        #region Recipe结构
        [XmlAttribute("MachineID")]
        public string MachineID { get; set; }

        [XmlAttribute("RecipeName")]
        public string RecipeName { get; set; }

        [XmlIgnore]
        public List<ProgramSubstrateSettings> StepSubstrateList { get; set; }


        [XmlIgnore]
        public List<ProgramComponentSettings> StepComponentList { get; set; }

        [XmlIgnore]
        public ProgramComponentSettings SubmonutInfos { get; set; }

        [XmlElement("SubstrateInfos")]
        public ProgramSubstrateSettings SubstrateInfos { get; set; }
        [XmlElement("DispenserSettings")]
        public DispenserSettings DispenserSettings { get; set; }

        [XmlIgnore]
        public List<BondingPositionSettings> StepBondingPositionList { get; set; }
        [XmlIgnore]
        public List<EpoxyApplication> StepEpoxyApplicationList { get; set; }

        [XmlIgnore]
        public List<EutecticParameters> EutecticParameters = new List<EutecticParameters>();


        [XmlArray("ProductSteps"), XmlArrayItem(typeof(ProductStep))]
        public List<ProductStep> ProductSteps { get; set; }

        
        #endregion
        /// <summary>
        /// Recipe存放系统默认路径
        /// </summary>
        [XmlIgnore]
        private static string SystemDefaultDirectory = SystemConfiguration.Instance.SystemDefaultDirectory;
        [XmlIgnore]
        private static string _SubmonutSavePath = string.Format(@"{0}Recipes\Components\", SystemDefaultDirectory);
        [XmlIgnore]
        private static string _SubstrateSavePath = string.Format(@"{0}Recipes\Substrate\", SystemDefaultDirectory);
        [XmlIgnore]
        private static string _componentsSavePath = string.Format(@"{0}Recipes\Components\", SystemDefaultDirectory);
        [XmlIgnore]
        private static string _bondPositionSavePath = string.Format(@"{0}Recipes\BondPositions\", SystemDefaultDirectory);
        private static string _epoxyApplicationSavePath = string.Format(@"{0}Recipes\EpoxyApplication\", SystemDefaultDirectory);
        private static string _EutecticSavePath = string.Format(@"{0}Recipes\Eutectic\", SystemDefaultDirectory);
        /// <summary>
        /// Recipe xml完整的路径
        /// </summary>
        private static string _recipeFullName = string.Empty;

        /// <summary>
        /// Recip文件夹全路径
        /// </summary>
        public static string _recipeFolderFullName = string.Empty;

        /// <summary>
        /// Recipe文件夹全路径
        /// </summary>
        [XmlIgnore]
        public string RecipeFolderFullName
        {
            get { return _recipeFolderFullName; }
        }
        [XmlIgnore]
        public static EnumRecipeType RecipeType
        {
            get { return EnumRecipeType.Bonder; }
        }
        [XmlIgnore]
        public string CurrentBondPositionSettingsName { get; set; }
        [XmlIgnore]
        public string CurrentSubstrateInfosName { get; set; }
        [XmlIgnore]
        public string CurrentComponentInfosName { get; set; }
        [XmlIgnore]
        public string CurrentEpoxyApplicationName { get; set; }
        [XmlIgnore]
        public string CurrentEutecticName { get; set; }

        [XmlIgnore]
        public ProgramSubstrateSettings CurrentSubstrate
        {
            get
            {
                ProgramSubstrateSettings ret = null;
                if (!string.IsNullOrEmpty(CurrentSubstrateInfosName))
                {
                    ret = StepSubstrateList.FirstOrDefault(i => i.Name == CurrentSubstrateInfosName);
                }
                return ret;
            }
        }


        [XmlIgnore]
        public ProgramComponentSettings CurrentComponent
        {
            get
            {
                ProgramComponentSettings ret = null;
                if (!string.IsNullOrEmpty(CurrentComponentInfosName))
                {
                    ret = StepComponentList.FirstOrDefault(i => i.Name == CurrentComponentInfosName);
                }
                return ret;
            }
        }
        [XmlIgnore]
        public BondingPositionSettings CurrentBondPosition
        {
            get
            {
                BondingPositionSettings ret = null;
                if (!string.IsNullOrEmpty(CurrentBondPositionSettingsName))
                {
                    ret = StepBondingPositionList.FirstOrDefault(i => i.Name == CurrentBondPositionSettingsName);
                }
                return ret;
            }
        }
        [XmlIgnore]
        public EpoxyApplication CurrentEpoxyApplication
        {
            get
            {
                EpoxyApplication ret = null;
                if (!string.IsNullOrEmpty(CurrentEpoxyApplicationName))
                {
                    ret = StepEpoxyApplicationList.FirstOrDefault(i => i.Name == CurrentEpoxyApplicationName);
                }
                return ret;
            }
        }
        [XmlIgnore]
        public EutecticParameters CurrentEutecticParameter
        {
            get
            {
                EutecticParameters ret = null;
                if (!string.IsNullOrEmpty(CurrentEutecticName))
                {
                    ret = EutecticParameters.FirstOrDefault(i => i.Name == CurrentEutecticName);
                }
                return ret;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public BondRecipe()
        {
            RecipeName = string.Empty;
            StepSubstrateList = new List<ProgramSubstrateSettings>();
            StepComponentList = new List<ProgramComponentSettings>();
            SubmonutInfos = new ProgramComponentSettings();
            SubstrateInfos = new ProgramSubstrateSettings();
            SubstrateInfos.CarrierType = EnumCarrierType.WafflePack;
            StepBondingPositionList = new List<BondingPositionSettings>();
            EutecticParameters = new List<EutecticParameters>();
            ProductSteps = new List<ProductStep>();
            DispenserSettings = new DispenserSettings();
            StepEpoxyApplicationList = new List<EpoxyApplication>();
        }

        /// <summary>
        /// 删除Recipe
        /// </summary>
        public void Delete()
        {
            try
            {

            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 复制Recipe
        /// </summary>
        public bool Copy(string newRecipeName)
        {
            return false;
        }

        /// <summary>
        /// 验证recipe是否完整有效
        /// </summary>
        /// <param name="recipeName"></param>
        /// <param name="waferSize"></param>
        /// <returns></returns>
        public static bool Validate(string recipeName)
        {
            return false;
        }

        /// <summary>
        /// 加载Recipe
        /// </summary>
        /// <param name="recipeFullName"></param>
        /// <returns></returns>
        public static BondRecipe LoadRecipe(string recipeName)
        {
            var recipeDirectory = string.Format(SystemDefaultDirectory + @"Recipes\{0}\{1}", RecipeType.ToString(), recipeName);
            _recipeFullName = string.Format(recipeDirectory + @"\{0}.xml", recipeName);
            _recipeFolderFullName = _recipeFullName.Substring(0, _recipeFullName.LastIndexOf("\\"));
            if (!File.Exists(_recipeFullName))
            {
                throw new FileNotFoundException(string.Format("recipe {0} is not found.", _recipeFullName));
            }
            BondRecipe loadedRecipe = new BondRecipe();
            try
            {
                loadedRecipe = LoadMainParameters(_recipeFullName);
                //此处需根据ProcessList加载Component和BondPosition
                //loadedRecipe.ComponentInfos.ComponentMapInfos = LoadComponentMap();
                

                if (loadedRecipe.ProductSteps.Count > 0)
                {
                    loadedRecipe.SubmonutInfos = LoadComponents(loadedRecipe.ProductSteps)[0];
                    var substrateInfos = LoadSubstrate(loadedRecipe.ProductSteps);
                    if(substrateInfos != null && substrateInfos.Count>0)
                    {
                        loadedRecipe.SubstrateInfos = LoadSubstrate(loadedRecipe.ProductSteps)[0];
                    }
                    loadedRecipe.StepSubstrateList = LoadSubstrate(loadedRecipe.ProductSteps);
                    loadedRecipe.StepComponentList = LoadComponents(loadedRecipe.ProductSteps);
                    loadedRecipe.StepBondingPositionList = LoadBondPositions(loadedRecipe.ProductSteps);
                    loadedRecipe.StepEpoxyApplicationList = LoadEpoxyApplications(loadedRecipe.ProductSteps);
                }
                else
                {
                    loadedRecipe.SubmonutInfos = LoadComponents(loadedRecipe.RecipeName)[0];
                    var substrateInfos = LoadSubstrates(loadedRecipe.RecipeName);
                    if (substrateInfos != null && substrateInfos.Count > 0)
                    {
                        loadedRecipe.SubstrateInfos = LoadSubstrate(loadedRecipe.ProductSteps)[0];
                    }
                    loadedRecipe.StepSubstrateList = LoadSubstrates(loadedRecipe.RecipeName);
                    loadedRecipe.StepComponentList = LoadComponents(loadedRecipe.RecipeName);
                    loadedRecipe.StepBondingPositionList = LoadBondPositions(loadedRecipe.RecipeName);
                    loadedRecipe.StepEpoxyApplicationList = LoadEpoxyApplications(loadedRecipe.RecipeName);
                }
                //loadedRecipe.SubstrateInfos.SubstrateMapInfos = LoadSubstrateMap();
                //loadedRecipe.SubstrateInfos.ModuleMapInfos = LoadModuleMap();

                //foreach (var item in loadedRecipe.SubstrateInfos.ModuleMapInfos)
                //{
                //    for (int i = 0; i < item.Count; i++)
                //    {
                //        var newItem = new Tuple<MaterialMapInformation, List<BondingPositionSettings>>(item[i].Item1, loadedRecipe.StepBondingPositionList);
                //        item[i] = newItem;
                //    }
                //}

            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error,string.Format("Load Recipe {0} 信息异常", _recipeFullName), ex);
                loadedRecipe = null;
            }
            return loadedRecipe;
        }



        /// <summary>
        /// 保存recipe到xml文件
        /// </summary>
        //public void SaveRecipe()
        //{
        //}

        public void NewRecipe(string fullFileName,EnumRecipeStep recipeStep = EnumRecipeStep.None)
        {
            _recipeFullName = fullFileName;
            _recipeFolderFullName = fullFileName.Substring(0, fullFileName.LastIndexOf("\\"));
            SaveRecipe(recipeStep);
        }
        public void SaveRecipe(EnumRecipeStep recipeStep = EnumRecipeStep.None)
        {
            SaveMianParameters();
            switch (recipeStep)
            {
                case EnumRecipeStep.Create:
                    break;
                case EnumRecipeStep.Configuration:
                    break;
                case EnumRecipeStep.Substrate_InformationSettings:
                    SaveSubstrate2();
                    break;
                case EnumRecipeStep.Substrate_PositionSettings:
                    SaveSubstrate2();
                    break;
                case EnumRecipeStep.Substrate_MaterialMap:
                    SaveSubstrate2();
                    break;
                case EnumRecipeStep.Substrate_PPSettings:
                    SaveSubstrate2();
                    break;
                case EnumRecipeStep.Substrate_Accuracy:
                    SaveSubstrate2();
                    break;
                case EnumRecipeStep.BondPosition:
                    SaveBondPosition();
                    break;
                case EnumRecipeStep.Component_InformationSettings:
                    SaveComponent();
                    break;
                case EnumRecipeStep.Component_PositionSettings:
                    SaveComponent();
                    break;
                case EnumRecipeStep.Component_MaterialMap:
                    SaveComponent();
                    break;
                case EnumRecipeStep.Component_PPSettings:
                    SaveComponent();
                    break;
                case EnumRecipeStep.Component_Accuracy:
                    SaveComponent();
                    break;
                case EnumRecipeStep.EutecticSettings:
                    SaveEutectic();
                    break;
                case EnumRecipeStep.ProductStepList:
                    break;
                case EnumRecipeStep.BlankingSetting:
                    break;
                case EnumRecipeStep.EpoxyApplications:
                    SaveEpoxyApplication();
                    break;
                case EnumRecipeStep.Module_MaterialMap:
                    SaveSubstrateMap2();
                    break;
                case EnumRecipeStep.None:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 保存主要参数，保存其他如Map数据需单独调用其他方法
        /// </summary>
        private void SaveMianParameters()
        {
            XmlSerializeHelper.XmlSerializeToFile(this, _recipeFullName, Encoding.UTF8);

        }
        private static BondRecipe LoadMainParameters(string fullName)
        {
            var ret= XmlSerializeHelper.XmlDeserializeFromFile<BondRecipe>(fullName, Encoding.UTF8);

            return ret;
        }

        public static List<ProgramSubstrateSettings> LoadSubstrate(List<ProductStep> ProcessingList)
        {
            var ret = new List<ProgramSubstrateSettings>();
            try
            {
                foreach (var item in ProcessingList)
                {
                    var xmlFile = $@"{_SubstrateSavePath}{item.SubstrateName}\{item.SubstrateName}.xml";
                    var comp = XmlSerializeHelper.XmlDeserializeFromFile<ProgramSubstrateSettings>(xmlFile, Encoding.UTF8);
                    comp.SubstrateMapInfos = LoadSubstrateMap(item.SubstrateName);
                    comp.ModuleMapInfos = LoadModuleMap(item.SubstrateName);
                    if (!ret.Any(i => i.Name == comp.Name))
                    {
                        ret.Add(comp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadSubstrate，Error.", ex);
            }
            return ret;
        }

        public static List<ProgramComponentSettings> LoadComponents(List<ProductStep> ProcessingList)
        {
            var ret = new List<ProgramComponentSettings>();
            try
            {
                foreach (var item in ProcessingList)
                {
                    var xmlFile = $@"{_componentsSavePath}{item.ComponentName}\{item.ComponentName}.xml";
                    var comp = XmlSerializeHelper.XmlDeserializeFromFile<ProgramComponentSettings>(xmlFile, Encoding.UTF8);
                    comp.ComponentMapInfos = LoadComponentMap(item.ComponentName);
                    if (!ret.Any(i => i.Name == comp.Name))
                    {
                        ret.Add(comp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadComponents，Error.", ex);
            }
            return ret;
        }
        public static List<BondingPositionSettings> LoadBondPositions(List<ProductStep> ProcessingList)
        {
            var ret = new List<BondingPositionSettings>();
            try
            {
                foreach (var item in ProcessingList)
                {
                    var xmlFile = $@"{_bondPositionSavePath}{item.BondingPositionName}\{item.BondingPositionName}.xml";
                    var bp = XmlSerializeHelper.XmlDeserializeFromFile<BondingPositionSettings>(xmlFile, Encoding.UTF8);
                    if (!ret.Any(i => i.Name == bp.Name))
                    {
                        ret.Add(bp);
                    }
                }
            }
            catch(Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadBondPositions，Error.", ex);
            }
            return ret;
        }
        public static List<List<List<BondingPositionSettings>>> LoadBondPositionsInfos(List<List<MaterialMapInformation>> moduleMapInfos, List<ProductStep> ProcessingList)
        {
            var ret = new List<List<List<BondingPositionSettings>>>();
            try
            {
                foreach (var item in moduleMapInfos)
                {
                    foreach (var step in ProcessingList)
                    {
                        var xmlFile = $@"{_bondPositionSavePath}{step.BondingPositionName}\{step.BondingPositionName}.xml";
                        var bp = XmlSerializeHelper.XmlDeserializeFromFile<BondingPositionSettings>(xmlFile, Encoding.UTF8);
                        if(bp!=null)
                        {

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadBondPositionsInfos，Error.", ex);
            }
            return ret;
        }
        public static List<EpoxyApplication> LoadEpoxyApplications(List<ProductStep> ProcessingList)
        {
            var ret = new List<EpoxyApplication>();
            try
            {
                foreach (var item in ProcessingList)
                {
                    var xmlFile = $@"{_epoxyApplicationSavePath}{item.EpoxyApplicationName}\{item.EpoxyApplicationName}.xml";
                    var bp = XmlSerializeHelper.XmlDeserializeFromFile<EpoxyApplication>(xmlFile, Encoding.UTF8);
                    if (!ret.Any(i => i.Name == bp.Name))
                    {
                        ret.Add(bp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEpoxyApplications，Error.", ex);
            }
            return ret;
        }

        public static List<EutecticParameters> LoadEutectics(List<ProductStep> ProcessingList)
        {
            var ret = new List<EutecticParameters>();
            try
            {
                foreach (var item in ProcessingList)
                {
                    var xmlFile = $@"{_EutecticSavePath}{item.EutecticName}\{item.EutecticName}.xml";
                    var bp = XmlSerializeHelper.XmlDeserializeFromFile<EutecticParameters>(xmlFile, Encoding.UTF8);
                    if (!ret.Any(i => i.Name == bp.Name))
                    {
                        ret.Add(bp);
                    }
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, "LoadEpoxyApplications，Error.", ex);
            }
            return ret;
        }


        /// <summary>
        /// 根据物料名获取物料对象
        /// </summary>
        public static ProgramSubstrateSettings LoadSubstrateByName(string substrateName)
        {
            var ret = new ProgramSubstrateSettings();
            var xmlFile = $@"{_SubstrateSavePath}{substrateName}\{substrateName}.xml";
            ret = XmlSerializeHelper.XmlDeserializeFromFile<ProgramSubstrateSettings>(xmlFile, Encoding.UTF8);
            ret.SubstrateMapInfos = LoadSubstrateMap(substrateName);
            return ret;
        }
        public static List<ProgramSubstrateSettings> LoadSubstrates(string substrateName)
        {
            var ret = new List<ProgramSubstrateSettings>();
            var childFiles = Directory.GetDirectories(_SubstrateSavePath);
            for (int index = 0; index < childFiles.Length; index++)
            {
                var fileName = Path.GetFileName(childFiles[index]);
                if (fileName.EndsWith($"_{substrateName}"))
                {
                    var xmlFile = $@"{_SubstrateSavePath}\{fileName}\{fileName}.xml";
                    var comp = XmlSerializeHelper.XmlDeserializeFromFile<ProgramSubstrateSettings>(xmlFile, Encoding.UTF8);
                    comp.SubstrateMapInfos = LoadSubstrateMap(fileName);
                    ret.Add(comp);
                }
            }

            return ret;
        }

        /// <summary>
        /// 根据物料名获取物料对象
        /// </summary>
        public static ProgramComponentSettings LoadComponentByName(string CompName)
        {
            var ret = new ProgramComponentSettings();
            var xmlFile = $@"{_componentsSavePath}{CompName}\{CompName}.xml";
            ret = XmlSerializeHelper.XmlDeserializeFromFile<ProgramComponentSettings>(xmlFile, Encoding.UTF8);
            ret.ComponentMapInfos = LoadComponentMap(CompName);
            return ret;
        }
        public static List<ProgramComponentSettings> LoadComponents(string recipeName)
        {
            var ret = new List<ProgramComponentSettings>();
            var childFiles = Directory.GetDirectories(_componentsSavePath);
            for (int index = 0; index < childFiles.Length; index++)
            {
                var fileName = Path.GetFileName(childFiles[index]);
                if (fileName.EndsWith($"_{recipeName}"))
                {
                    var xmlFile = $@"{_componentsSavePath}\{fileName}\{fileName}.xml";
                    var comp = XmlSerializeHelper.XmlDeserializeFromFile<ProgramComponentSettings>(xmlFile, Encoding.UTF8);
                    comp.ComponentMapInfos = LoadComponentMap(fileName);
                    ret.Add(comp);
                }
            }

            return ret;
        }
        public static BondingPositionSettings LoadBondPositionByName(string name)
        {
            var ret = new BondingPositionSettings();
            var xmlFile = $@"{_bondPositionSavePath}{name}\{name}.xml";
            ret = XmlSerializeHelper.XmlDeserializeFromFile<BondingPositionSettings>(xmlFile, Encoding.UTF8);
            return ret;
        }
        public static List<BondingPositionSettings> LoadBondPositions(string recipeName)
        {
            var ret = new List<BondingPositionSettings>();
            var childDir = Directory.GetDirectories(_bondPositionSavePath);
            for (int index = 0; index < childDir.Length; index++)
            {
                var fileName = Path.GetFileName(childDir[index]);
                if (fileName.EndsWith($"_{recipeName}"))
                {

                    var xmlFile = $@"{_bondPositionSavePath}{fileName}\{fileName}.xml";
                    var bp = XmlSerializeHelper.XmlDeserializeFromFile<BondingPositionSettings>(xmlFile, Encoding.UTF8);
                    ret.Add(bp);
                }

            }

            return ret;
        }
        public static EpoxyApplication LoadEpoxyApplicationByName(string name)
        {
            var ret = new EpoxyApplication();
            var xmlFile = $@"{_epoxyApplicationSavePath}{name}\{name}.xml";
            ret = XmlSerializeHelper.XmlDeserializeFromFile<EpoxyApplication>(xmlFile, Encoding.UTF8);
            return ret;
        }
        public static List<EpoxyApplication> LoadEpoxyApplications(string recipeName)
        {
            var ret = new List<EpoxyApplication>();
            var childDir = Directory.GetDirectories(_epoxyApplicationSavePath);
            for (int index = 0; index < childDir.Length; index++)
            {
                var fileName = Path.GetFileName(childDir[index]);
                if (fileName.EndsWith($"_{recipeName}"))
                {

                    var xmlFile = $@"{_epoxyApplicationSavePath}{fileName}\{fileName}.xml";
                    var bp = XmlSerializeHelper.XmlDeserializeFromFile<EpoxyApplication>(xmlFile, Encoding.UTF8);
                    ret.Add(bp);
                }

            }

            return ret;
        }

        public static EutecticParameters LoadEutecticByName(string name)
        {
            var ret = new EutecticParameters();
            var xmlFile = $@"{_EutecticSavePath}{name}\{name}.xml";
            ret = XmlSerializeHelper.XmlDeserializeFromFile<EutecticParameters>(xmlFile, Encoding.UTF8);
            return ret;
        }
        public static List<EutecticParameters> LoadEutectics(string recipeName)
        {
            var ret = new List<EutecticParameters>();
            var childDir = Directory.GetDirectories(_EutecticSavePath);
            for (int index = 0; index < childDir.Length; index++)
            {
                var fileName = Path.GetFileName(childDir[index]);
                if (fileName.EndsWith($"_{recipeName}"))
                {

                    var xmlFile = $@"{_EutecticSavePath}{fileName}\{fileName}.xml";
                    var bp = XmlSerializeHelper.XmlDeserializeFromFile<EutecticParameters>(xmlFile, Encoding.UTF8);
                    ret.Add(bp);
                }

            }

            return ret;
        }

        private void SaveSubstrate()
        {
            if (SubstrateInfos != null)
            {
                var xmlFile = $@"{_SubstrateSavePath}{SubstrateInfos.Name}\{SubstrateInfos.Name}.xml";
                XmlSerializeHelper.XmlSerializeToFile(SubstrateInfos, xmlFile, Encoding.UTF8);
                SaveSubstrateMap();
            }
        }
        private void SaveSubstrate2()
        {
            if (CurrentSubstrate != null)
            {
                var xmlFile = $@"{_SubstrateSavePath}{CurrentSubstrate.Name}\{CurrentSubstrate.Name}.xml";
                XmlSerializeHelper.XmlSerializeToFile(CurrentSubstrate, xmlFile, Encoding.UTF8);
                SaveSubstrateMap2();
            }
        }

        private void SaveComponent()
        {
            if (CurrentComponent != null)
            {
                var xmlFile = $@"{_componentsSavePath}{CurrentComponent.Name}\{CurrentComponent.Name}.xml";
                XmlSerializeHelper.XmlSerializeToFile(CurrentComponent, xmlFile, Encoding.UTF8);
                SaveComponentMap();
            }
        }

        private void SaveBondPosition()
        {
            if (CurrentBondPosition != null)
            {
                var xmlFile = $@"{_bondPositionSavePath}{CurrentBondPosition.Name}\{CurrentBondPosition.Name}.xml";
                XmlSerializeHelper.XmlSerializeToFile(CurrentBondPosition, xmlFile, Encoding.UTF8);
            }
        }
        private void SaveEpoxyApplication()
        {
            if (CurrentEpoxyApplication != null)
            {
                var xmlFile = $@"{_epoxyApplicationSavePath}{CurrentEpoxyApplication.Name}\{CurrentEpoxyApplication.Name}.xml";
                XmlSerializeHelper.XmlSerializeToFile(CurrentEpoxyApplication, xmlFile, Encoding.UTF8);
            }
        }

        private void SaveEutectic()
        {
            if (CurrentEutecticParameter != null)
            {
                var xmlFile = $@"{_EutecticSavePath}{CurrentEutecticParameter.Name}\{CurrentEutecticParameter.Name}.xml";
                XmlSerializeHelper.XmlSerializeToFile(CurrentEutecticParameter, xmlFile, Encoding.UTF8);
            }
        }


        private void SaveComponentMap()
        {
            if (CurrentComponent != null)
            {
                var xmlFile = $@"{_componentsSavePath}{CurrentComponent.Name}\ComponentMap_{CurrentComponentInfosName}.xml";
                XmlSerializeHelper.XmlSerializeToFile(CurrentComponent.ComponentMapInfos, xmlFile, Encoding.UTF8);
            }
        }
        private static List<MaterialMapInformation> LoadComponentMap(string componentName)
        {
            var materialMapData = new List<MaterialMapInformation>();
            try
            {
                var xmlFile = $@"{_componentsSavePath}{componentName}\ComponentMap_{componentName}.xml";
                if (File.Exists(xmlFile))
                {
                    materialMapData = XmlSerializeHelper.XmlDeserializeFromFile<List<MaterialMapInformation>>(xmlFile, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, string.Format("Load Recipe {0} LoadComponentMap 信息异常", _recipeFullName), ex);
            }
            return materialMapData;
        }
        private void SaveSubstrateMap()
        {
            //var substrateXmlFile = $@"{_recipeFolderFullName}\SubstrateMap.xml";
            //XmlSerializeHelper.XmlSerializeToFile(this.SubstrateInfos.SubstrateMapInfos, substrateXmlFile, Encoding.UTF8);
            //var moduleXmlFile = $@"{_recipeFolderFullName}\ModuleMap.xml";
            //XmlSerializeHelper.XmlSerializeToFile(this.SubstrateInfos.ModuleMapInfos, moduleXmlFile, Encoding.UTF8);

            var substrateXmlFile = $@"{_SubstrateSavePath}{SubstrateInfos.Name}\SubstrateMap.xml";
            XmlSerializeHelper.XmlSerializeToFile(this.SubstrateInfos.SubstrateMapInfos, substrateXmlFile, Encoding.UTF8);
            var moduleXmlFile = $@"{_SubstrateSavePath}{SubstrateInfos.Name}\ModuleMap.xml";
            XmlSerializeHelper.XmlSerializeToFile(this.SubstrateInfos.ModuleMapInfos, moduleXmlFile, Encoding.UTF8);
        }
        private void SaveSubstrateMap2()
        {
            //var substrateXmlFile = $@"{_recipeFolderFullName}\SubstrateMap.xml";
            //XmlSerializeHelper.XmlSerializeToFile(this.SubstrateInfos.SubstrateMapInfos, substrateXmlFile, Encoding.UTF8);
            //var moduleXmlFile = $@"{_recipeFolderFullName}\ModuleMap.xml";
            //XmlSerializeHelper.XmlSerializeToFile(this.SubstrateInfos.ModuleMapInfos, moduleXmlFile, Encoding.UTF8);

            var substrateXmlFile = $@"{_SubstrateSavePath}{CurrentSubstrate.Name}\SubstrateMap.xml";
            XmlSerializeHelper.XmlSerializeToFile(this.CurrentSubstrate.SubstrateMapInfos, substrateXmlFile, Encoding.UTF8);
            var moduleXmlFile = $@"{_SubstrateSavePath}{CurrentSubstrate.Name}\ModuleMap.xml";
            XmlSerializeHelper.XmlSerializeToFile(this.CurrentSubstrate.ModuleMapInfos, moduleXmlFile, Encoding.UTF8);
        }

        private static List<MaterialMapInformation> LoadSubstrateMap()
        {
            var materialMapData = new List<MaterialMapInformation>();
            try
            {
                var xmlFile = $@"{_recipeFolderFullName}\SubstrateMap.xml";
                if (File.Exists(xmlFile))
                {
                    materialMapData = XmlSerializeHelper.XmlDeserializeFromFile<List<MaterialMapInformation>>(xmlFile, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, string.Format("Load Recipe {0} LoadSubstrateMap 信息异常", _recipeFullName), ex);
            }
            return materialMapData;
        }
        private static List<List<MaterialMapInformation>> LoadModuleMap()
        {
            var materialMapData = new List<List<MaterialMapInformation>>();
            try
            {
                var xmlFile = $@"{_recipeFolderFullName}\ModuleMap.xml";
                if (File.Exists(xmlFile))
                {
                    materialMapData = XmlSerializeHelper.XmlDeserializeFromFile<List<List<MaterialMapInformation>>>(xmlFile, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, string.Format("Load Recipe {0} LoadModuleMap 信息异常", _recipeFullName), ex);
            }
            return materialMapData;
        }

        private static List<MaterialMapInformation> LoadSubstrateMap(string substrateName)
        {
            var materialMapData = new List<MaterialMapInformation>();
            try
            {
                var xmlFile = $@"{_SubstrateSavePath}{substrateName}\SubstrateMap.xml";
                if (File.Exists(xmlFile))
                {
                    materialMapData = XmlSerializeHelper.XmlDeserializeFromFile<List<MaterialMapInformation>>(xmlFile, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, string.Format("Load Recipe {0} LoadSubstrateMap 信息异常", _recipeFullName), ex);
            }
            return materialMapData;
        }
        private static List<List<MaterialMapInformation>> LoadModuleMap(string substrateName)
        {
            var materialMapData = new List<List<MaterialMapInformation>>();
            try
            {
                var xmlFile = $@"{_SubstrateSavePath}{substrateName}\ModuleMap.xml";
                if (File.Exists(xmlFile))
                {
                    materialMapData = XmlSerializeHelper.XmlDeserializeFromFile<List<List<MaterialMapInformation>>>(xmlFile, Encoding.UTF8);
                }
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Error, string.Format("Load Recipe {0} LoadModuleMap 信息异常", _recipeFullName), ex);
            }
            return materialMapData;
        }


        public bool IsStepComplete_Substrate()
        {
            var ret = false;
            //if (IsStepComplete_SubmountInfo() && IsStepComplete_SubmountPosition() && IsStepComplete_SubmountMap()
            //        && IsStepComplete_SubmountPPSettings() && IsStepComplete_SubmountAccuracy())
            if (IsStepComplete_SubstrateInfo() 
                && IsStepComplete_SubstratePosition() 
                && IsStepComplete_SubstrateMap() 
                && IsStepComplete_ModulePosition()
                && IsStepComplete_ModuleMap())
            {
                ret = true;
            }
            return ret;
        }
        public bool IsStepComplete_SubstrateInfo()
        {
            var ret = false;
            ret = SubstrateInfos.IsMaterialInfoSettingsComplete;
            return ret;
        }
        public bool IsStepComplete_SubstratePosition()
        {
            var ret = false;
            ret = SubstrateInfos.IsMaterialPositionSettingsComplete;
            return ret;
        }
        public bool IsStepComplete_SubstrateMap()
        {
            var ret = false;
            ret = SubstrateInfos.IsMaterialMapSettingsComplete;
            return ret;
        }
        public bool IsStepComplete_ModulePosition()
        {
            var ret = false;
            ret = SubstrateInfos.IsModulePositionSettingsComplete;
            return ret;
        }
        public bool IsStepComplete_ModuleMap()
        {
            var ret = false;
            ret = SubstrateInfos.IsModuleMapSettingsComplete;
            return ret;
        }
        public bool IsStepComplete_SubmountPPSettings()
        {
            var ret = false;
            ret = SubstrateInfos.IsMaterialPPSettingsComplete;
            return ret;
        }
        public bool IsStepComplete_SubmountAccuracy()
        {
            var ret = false;
            ret = SubstrateInfos.IsMaterialAccuracySettingsComplete;
            return ret;
        }
        public bool IsStepComplete_Components()
        {
            var ret = true;
            foreach (var item in StepComponentList)
            {
                if(!IsStepComplete_Component(item.Name))
                {
                    ret = false;
                    break;
                }
            }
            if (StepComponentList.Count == 0)
            {
                ret = false;
            }
            return ret;
        }
        public bool IsStepComplete_Component(string componentName)
        {
            var ret = false;
            if(IsStepComplete_ComponentInfo(componentName)&& IsStepComplete_ComponentPosition(componentName) && IsStepComplete_ComponentMap(componentName) 
                && IsStepComplete_ComponentPPSettings(componentName) && IsStepComplete_ComponentAccuracy(componentName))
            {
                ret = true;
            }

            return ret;
        }
        public bool IsStepComplete_ComponentInfo(string componentName)
        {
            var ret = false;
            var material = StepComponentList.FirstOrDefault(i => i.Name == componentName);
            if(material != null)
            {
                ret = material.IsMaterialInfoSettingsComplete;
            }
            return ret;
        }
        public bool IsStepComplete_ComponentPosition(string componentName)
        {
            var ret = false;
            var material = StepComponentList.FirstOrDefault(i => i.Name == componentName);
            if (material != null)
            {
                ret = material.IsMaterialPositionSettingsComplete;
            }
            return ret;
        }
        public bool IsStepComplete_ComponentMap(string componentName)
        {
            var ret = false;
            var material = StepComponentList.FirstOrDefault(i => i.Name == componentName);
            if (material != null)
            {
                ret = material.IsMaterialMapSettingsComplete;
            }
            return ret;
        }
        public bool IsStepComplete_ComponentPPSettings(string componentName)
        {
            var ret = false;
            var material = StepComponentList.FirstOrDefault(i => i.Name == componentName);
            if (material != null)
            {
                ret = material.IsMaterialPPSettingsComplete;
            }
            return ret;
        }
        public bool IsStepComplete_ComponentAccuracy(string componentName)
        {
            var ret = false;
            var material = StepComponentList.FirstOrDefault(i => i.Name == componentName);
            if (material != null)
            {
                ret = material.IsMaterialAccuracySettingsComplete;
            }
            return ret;
        }
        public bool IsStepComplete_BondPositions()
        {
            var ret = true;
            foreach (var item in StepBondingPositionList)
            {
                if (!IsStepComplete_BondPosition(item.Name))
                {
                    ret = false;
                    break;
                }
            }
            if (StepBondingPositionList.Count == 0)
            {
                ret = false;
            }
            return ret;
        }
        public bool IsStepComplete_BondPosition(string name)
        {
            var ret = false;
            var material = StepBondingPositionList.FirstOrDefault(i => i.Name == name);
            if (material != null)
            {
                ret = material.IsComplete;
            }
            return ret;
        }
        public bool IsStepComplete_EutecticSettings()
        {
            var ret = false;
            //ret = EutecticParameters.IsCompleted;
            return ret;
        }
        public bool IsStepComplete_ProcessList()
        {
            var ret = false;
            ret = ProductSteps.Count > 0;
            return ret;
        }
        public bool IsStepComplete_BlankingSettings()
        {
            var ret = false;
            //ret = BlankingParameters.IsCompleted;
            return ret;
        }
        public bool IsStepComplete_DispenserSettings()
        {
            var ret = false;
            ret = DispenserSettings.IsCompleted;

            return ret;
        }

        public bool IsStepComplete_EpoxyApplication()
        {
            var ret = true;
            foreach (var item in StepEpoxyApplicationList)
            {
                if (!IsStepComplete_EpoxyApplication(item.Name))
                {
                    ret = false;
                    break;
                }
            }
            if (StepEpoxyApplicationList.Count == 0)
            {
                ret = false;
            }
            return ret;
        }
        public bool IsStepComplete_EpoxyApplication(string name)
        {
            var ret = false;
            var material = StepEpoxyApplicationList.FirstOrDefault(i => i.Name == name);
            if (material != null)
            {
                ret = material.IsCompleted;
            }
            return ret;
        }
    }
    

    [Serializable]
    public class ProcessingStep
    {
        [XmlElement("Name")]
        public bool Name { get; set; }
        [XmlElement("Index")]
        public int Index { get; set; }
        [XmlElement("BondingPositionSettingsName")]
        public string BondingPositionSettingsName { get; set; }
        [XmlElement("RelatedComponentName")]
        public string RelatedComponentName { get; set; }
        [XmlElement("RelatedSubmountName")]
        public string RelatedSubmountName { get; set; }

        [XmlElement("Source")]
        public string Source { get; set; }
        [XmlElement("Destination")]
        public string Destination { get; set; }

    }



}
