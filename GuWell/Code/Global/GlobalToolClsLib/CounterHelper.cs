using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace GlobalToolClsLib
{
    public class CounterHelper
    {
        private static readonly object _lockObj = new object();
        private static volatile CounterHelper _instance = LoadConfig();
        private static string _configPath = Path.Combine($@"{System.Environment.CurrentDirectory}\Config", "CounterConfiguration.xml");
        public static CounterHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new CounterHelper();
                        }
                    }
                }
                return _instance;
            }
        }
        private CounterHelper()
        {

        }

        private static CounterHelper LoadConfig()
        {
            CounterHelper ret = new CounterHelper();
            try
            {
                ret = XmlSerializeHelper.XmlDeserializeFromFile<CounterHelper>(_configPath, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                LogRecorder.RecordLog(EnumLogContentType.Error, "Load  CounterConfiguration Error.", ex);
            }
            return ret;

        }

        [XmlElement("TotalBondCounter")]
        public int TotalBondCounter { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [XmlElement("DispenserCounter")]
        public int DispenserCounter { get; set; }

        [XmlArray("RecipeCounterList"), XmlArrayItem(typeof(DefineRecipeCounter))]
        public List<DefineRecipeCounter> RecipeCounterList { get; set; }

        public void SaveConfig()
        {
            XmlSerializeHelper.XmlSerializeToFile(this, _configPath, Encoding.UTF8);
        }
    }
}
