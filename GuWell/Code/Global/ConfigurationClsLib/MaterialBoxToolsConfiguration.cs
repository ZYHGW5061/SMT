using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace ConfigurationClsLib
{
    [Serializable]
    [XmlRoot("MaterialBoxToolsConfiguration")]
    public class MaterialBoxToolsConfiguration
    {
        private static string _configPath = Path.Combine($@"{System.Environment.CurrentDirectory}\Config", "MaterialBoxToolsConfiguration.xml");
        private static readonly object _lockObj = new object();
        private static volatile MaterialBoxToolsConfiguration _instance = LoadConfig();
        public static MaterialBoxToolsConfiguration Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new MaterialBoxToolsConfiguration();
                        }
                    }
                }
                return _instance;
            }
        }
        private MaterialBoxToolsConfiguration()
        {
            MaterialBoxTools = new List<MaterialBoxToolSettings>();
        }
        private static MaterialBoxToolsConfiguration LoadConfig()
        {
            MaterialBoxToolsConfiguration ret = new MaterialBoxToolsConfiguration();
            try
            {
                ret= XmlSerializeHelper.XmlDeserializeFromFile<MaterialBoxToolsConfiguration>(_configPath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Load  MaterialBoxToolsConfiguration Error.", ex);
            }
            return ret;

        }
        public void SaveConfig()
        {
            XmlSerializeHelper.XmlSerializeToFile(this, _configPath, Encoding.UTF8);
        }
        
        [XmlArray("MaterialBoxTools"), XmlArrayItem(typeof(MaterialBoxToolSettings))]
        public List<MaterialBoxToolSettings> MaterialBoxTools { get; set; }


    }
}