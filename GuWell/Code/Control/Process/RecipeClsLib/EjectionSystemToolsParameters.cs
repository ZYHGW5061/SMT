using ConfigurationClsLib;
using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WestDragon.Framework.UtilityHelper;

namespace RecipeClsLib
{
    [Serializable]
    [XmlRoot("EjectionSystemToolsParameters")]
    public class EjectionSystemToolsParameters
    {
        #region Recipe结构
        [XmlAttribute("MachineID")]
        public string MachineID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlElement("EjectZPosition")]
        public float EjectZPosition { get; set; }
        [XmlElement("NeedleZeroPosition")]
        public float NeedleZeroPosition { get; set; }
        [XmlElement("XYMeasuringPositon")]
        public XYZTCoordinateConfig XYMeasuringPositon { get; set; }
        #endregion
        /// <summary>
        /// 完整的存储路径路径
        /// </summary>
        private static string _recipeFullName = string.Empty;
        public EjectionSystemToolsParameters()
        {
            XYMeasuringPositon = new XYZTCoordinateConfig();
        }
        /// <summary>
        /// 加载Recipe
        /// </summary>
        /// <param name="recipeFullName"></param>
        /// <returns></returns>
        public static EjectionSystemToolsParameters LoadRecipe(string recipeFullName)
        {
            if (!File.Exists(recipeFullName))
            {
                throw new FileNotFoundException(string.Format("recipe {0} is not found.", recipeFullName));
            }
            EjectionSystemToolsParameters loadedRecipe = new EjectionSystemToolsParameters();
            try
            {
                loadedRecipe = LoadMainParameters(recipeFullName);
                _recipeFullName = recipeFullName;
            }
            catch (Exception ex)
            {
                loadedRecipe = null;
            }
            return loadedRecipe;
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
        private static EjectionSystemToolsParameters LoadMainParameters(string fullName)
        {
            return XmlSerializeHelper.XmlDeserializeFromFile<EjectionSystemToolsParameters>(fullName, Encoding.UTF8);
        }
        private void SaveMianParameters()
        {
            XmlSerializeHelper.XmlSerializeToFile(this, _recipeFullName, Encoding.UTF8);

        }
    }
}
