using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GlobalDataDefineClsLib;

namespace CommonPanelClsLib
{
    public partial class WaferControl : UserControl
    {
        public Action<EnumWaferType> WaferType;

        public Action<bool> IsSelected;

        public Action<bool> IsEnabled;

        public bool GetIsSelced { get; private set; }

        public string WaferID
        { 
            get { return this.LabWafer.Text; }
            set { this.LabWafer.Text = value; } 
        }

        private bool _isAutoFill;

        public void SetAutoFill(bool isAutoFill)
        {
            _isAutoFill = isAutoFill;
            if (!_isAutoFill)
            {
                LabWafer.Text = "";
            }
        }

       

        private Color _lableBackColor { get; set; }

        public WaferControl()
        {
            InitializeComponent();
            WaferType = CheckedStripType;
            IsSelected = CheckedSeleced;
            IsEnabled = ChekedEnabled;
            textWaferID.LostFocus += textWaferID_LostFocus;
            this.LostFocus += textWaferID_LostFocus;
            LabWafer.Click += LabWafer_Click;
            LabWafer.DoubleClick+=LabWafer_DoubleClick;
            textWaferID.KeyDown += textWaferID_KeyDown;            
        }

        void textWaferID_KeyDown(object sender, KeyEventArgs e)
        {           
            if (e.KeyValue==13)
            {
                LabWafer.Focus();
            }
           
        }

        void LabWafer_DoubleClick(object sender, EventArgs e)
        {
            if (_isAutoFill)
            {
                return;
            }
            LabWafer_Click(sender, e);
            CheckedUnit(this.Enabled);
        }

        void LabWafer_Click(object sender, EventArgs e)
        {           
            LabWafer.Focus();
            GetIsSelced = !GetIsSelced;
            CheckedSeleced(GetIsSelced);
        }

        void textWaferID_LostFocus(object sender, EventArgs e)
        {
            CheckedUnit(false);
        }

        private void ChekedEnabled(bool isEnabled)
        {
            this.Enabled = isEnabled;
        }

        private void CheckedSeleced(bool isSelected)
        {
            if (isSelected)
            {
                LabWafer.BackColor = Color.LightGreen;
            }
            else
            {
                LabWafer.BackColor = _lableBackColor;
            }
            GetIsSelced = isSelected;
        }

        private void CheckedUnit(bool isUnit)
        {
            if (isUnit)
            {
                tableMain.RowStyles[0].SizeType = SizeType.Percent;
                tableMain.RowStyles[0].Height = 100;
                tableMain.RowStyles[1].SizeType = SizeType.Absolute;
                tableMain.RowStyles[1].Height = 0;
                textWaferID.Text = LabWafer.Text;
                textWaferID.Focus();
                textWaferID.Height = tableMain.Height;
            }
            else
            {
                tableMain.RowStyles[1].SizeType = SizeType.Percent;
                tableMain.RowStyles[1].Height = 100;
                tableMain.RowStyles[0].SizeType = SizeType.Absolute;
                tableMain.RowStyles[0].Height = 0;
                LabWafer.Text = textWaferID.Text;
            }
        }

        private void CheckedStripType(EnumWaferType stripType)
        {
            switch (stripType)
            {
                case EnumWaferType.Closed:
                    LabWafer.BackColor = Color.DarkGray;
                    GetIsSelced = false;
                    break;
                case EnumWaferType.None:
                    LabWafer.BackColor = Color.White;
                    _lableBackColor = Color.White;
                    break;
                case EnumWaferType.Normal:
                    LabWafer.BackColor = Color.LightSteelBlue;
                    _lableBackColor = Color.LightSteelBlue;
                    break;
                case EnumWaferType.Thickness:
                    LabWafer.BackColor = Color.Red;
                    _lableBackColor = Color.Red;
                    break;
                default:
                    break;
            }
        }
    }
}
