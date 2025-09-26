using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.XtraSplashScreen;
using DevExpress.XtraWaitForm;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.LoggerManagerClsLib;
using WestDragon.Framework.UtilityHelper;
using ConfigurationClsLib;
using GlobalDataDefineClsLib;

namespace CommonPanelClsLib
{
    public partial class CtrlMaterialBox : UserControl
    {
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }

        private int _waferSlotCount;
        private bool _isInitializeEnd;
        public int WaferSlotCount
        {
            get { return _waferSlotCount; }
        }
        /// <summary>
        /// LOT ID
        /// </summary>
        public string _materialBoxID { get; set; }
        /// <summary>
        /// WaferID
        /// </summary>
        public string _stripID { get; set; }

        #region 界面复选框控件对应词典
        private Dictionary<string, WaferControl> _dicLoadPort1 = new Dictionary<string, WaferControl>();
        #endregion


        //public EnumLoadPort LoadPort { get; set; }
        //private EnumEFEMStationID CurrentStationID 
        //{
        //    get 
        //    {
        //        return EnumEFEMStationID.Unknow;
        //    }
        //}
        /// <summary>
        /// 是否多选
        /// </summary>
        public bool isMultipleSelection { get; set; }
        /// <summary>
        /// 获得选择集合
        /// </summary>
        /// <returns></returns>
        public List<MaterialWaferInfo> GetSelectedWafers()
        {
            List<MaterialWaferInfo> selected = new List<MaterialWaferInfo>();

            for (int i = 1; i <= _waferSlotCount; i++)
            {
                if (_dicLoadPort1[i.ToString("00")].GetIsSelced)
                {
                    selected.Add(new MaterialWaferInfo() { WaferID = _dicLoadPort1[i.ToString("00")].WaferID, SlotIndex = i.ToString("00"), WaferSize = EnumWaferSize.INCH8 });
                }
            }
            return selected;
        }

        public CtrlMaterialBox()
        {
            _isInitializeEnd = false;
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            InitializeComponent();
            _waferSlotCount = _systemConfig.WaferSlotCount;
        }


        /// <summary>
        /// 
        /// </summary>
        public void Initialize(int slotCount=0)
        {
            if(slotCount!=0)
            {
                _waferSlotCount = slotCount;
            }
            CreateWaitDialog("Opening...");
            _isInitializeEnd = true;
            InitializeDictionary();
            var wafermap = "";
            for (int i = 0; i < _waferSlotCount; i++)
            {
                wafermap = string.Join("", wafermap, "1");
            }
            UpdateUIMapInfo(wafermap);
            CloseWaitDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="all"></param>
        /// <returns></returns>
        private void UpdateUIMapInfo(string all)
        {
            if (all.Contains('2')||all.Contains('3')||all.Contains('4')||all.Contains('5')||all.Contains('6')||
                all.Contains('7')||all.Contains('8')||all.Contains('9'))
            {
            }
            for (int i = 0; i < all.Length; i++)
            {
                _dicLoadPort1[(i + 1).ToString("00")].IsSelected(false);
                switch (all[i])
                {                      
                    case '0':
                        _dicLoadPort1[(i + 1).ToString("00")].WaferType(EnumWaferType.None);
                        _dicLoadPort1[(i + 1).ToString("00")].IsEnabled(false);
                        _dicLoadPort1[(i + 1).ToString("00")].BackColor = Color.White;
                        _dicLoadPort1[(i + 1).ToString("00")].Tag = Color.White;
                        _dicLoadPort1[(i + 1).ToString("00")].Enabled = false;
                        _dicLoadPort1[(i + 1).ToString("00")].WaferID = "";
                        break;
                    case '1':                   
                        _dicLoadPort1[(i + 1).ToString("00")].WaferType(EnumWaferType.Normal);
                        _dicLoadPort1[(i + 1).ToString("00")].IsEnabled(true);
                        _dicLoadPort1[(i + 1).ToString("00")].BackColor = Color.LightSteelBlue;
                        _dicLoadPort1[(i + 1).ToString("00")].Tag = Color.LightSteelBlue;
                        _dicLoadPort1[(i + 1).ToString("00")].WaferID = "";
                        break;
                    case '2':
                        _dicLoadPort1[(i + 1).ToString("00")].WaferType(EnumWaferType.Thickness);
                        _dicLoadPort1[(i + 1).ToString("00")].IsEnabled(false);
                        _dicLoadPort1[(i + 1).ToString("00")].BackColor = Color.Red;
                        _dicLoadPort1[(i + 1).ToString("00")].Tag = Color.Red;
                        _dicLoadPort1[(i + 1).ToString("00")].Enabled = false;
                        _dicLoadPort1[(i + 1).ToString("00")].WaferID = "Thickness";
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void InitializeDictionary()
        {
            if (DesignMode)
            {                
                return;
            }
            _dicLoadPort1.Clear();
            tableWafer.Controls.Clear();
            int rowCount = _waferSlotCount;
            int rem =0;
            tableWafer.RowCount = rowCount;
            int rowHeight = Math.DivRem(tableWafer.Height - 25, rowCount, out rem)  ;
            for (int i = rowCount; i >= 1; i--)
            {
                WaferControl lotwafer = new WaferControl();
                Label labTitie = new Label();
                labTitie.Dock = DockStyle.Fill;
                labTitie.AutoSize = false;
                labTitie.TextAlign = ContentAlignment.MiddleCenter;
                labTitie.Text = string.Format("L{0}", (rowCount + 1 - i).ToString("00"));
                lotwafer.WaferType(EnumWaferType.Closed);
                lotwafer.IsEnabled(false);
                lotwafer.IsSelected(false);
                lotwafer.Dock = DockStyle.Fill;
                lotwafer.Margin = new System.Windows.Forms.Padding(0);
                if (rowHeight > 0)
                {
                    tableWafer.RowStyles[i - 1].Height = rem > 0 ? rowHeight + 1 : i == 1 ? rowHeight - 1 : rowHeight;
                }
                rem--;
                tableWafer.Controls.Add(labTitie, 0, i - 1);
                tableWafer.Controls.Add(lotwafer, 1, i - 1);
                lotwafer.SetAutoFill(false);
                _dicLoadPort1.Add((rowCount + 1 - i).ToString("00"), lotwafer);
            }
        }
        private void LoadPortControl_Load(object sender, EventArgs e)
        {

        }

        private void checkEdit1_CheckedChanged(object sender, EventArgs e)
        {
            CheckedAll(((CheckEdit)sender).Checked);
        }

        public void CheckedAll(bool che, bool isMarathon = false)
        {
            for (int i = 1; i <= _waferSlotCount; i++)
            {
                if (_dicLoadPort1[(i).ToString("00")].Enabled)
                {
                    _dicLoadPort1[(i).ToString("00")].IsSelected(che);          
                }
            }
        }
        public void CheckeSelectedWafers(List<MaterialWaferInfo> selWafers)
        {
            for (int i = 1; i <= selWafers.Count; i++)
            {
                if (_dicLoadPort1[(i).ToString("00")].Enabled)
                {
                    _dicLoadPort1[(i).ToString("00")].IsSelected(true);
                }
            }
        }
        /// <summary>
        ///  创建等待窗体
        /// </summary>
        private void CreateWaitDialog(string str)
        {
            if (SplashScreenManager.Default == null)
            {
                SplashScreenManager.ShowForm(this.FindForm(), typeof(DemoWaitForm), false, true);
            }
            //SplashScreenManager.Default.Properties.ParentForm = this.FindForm();
            SplashScreenManager.Default.SetWaitFormCaption("");
            SplashScreenManager.Default.SetWaitFormDescription(str);
        }
        /// <summary>
        /// 关闭等待窗体
        /// </summary>
        private void CloseWaitDialog()
        {
            if (SplashScreenManager.Default != null)
            {
                SplashScreenManager.CloseForm();
            }
        }
    }
}
