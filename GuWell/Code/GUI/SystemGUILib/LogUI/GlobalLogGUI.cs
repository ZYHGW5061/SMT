using DevExpress.XtraEditors;
using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WestDragon.Framework.BaseLoggerClsLib;

namespace SystemGUILib.LogUI
{
    public partial class GlobalLogGUI : DevExpress.XtraEditors.XtraUserControl
    {
        public GlobalLogGUI()
        {
            InitializeComponent();

            LogRecorder.GlobalLogAct += new Action<EnumLogContentType, string>(OnGlobalLogActCallback);
        }

        protected override void OnFirstLoad()
        {
            base.OnFirstLoad();

            RefreshDataLog();
        }

        private void OnGlobalLogActCallback(EnumLogContentType logtype, string message)
        {
            //能刷新就刷新
            RefreshDataLog();
        }


        /// <summary>
        /// 刷新日志
        /// </summary>
        private void RefreshDataLog()
        {
            if (!gridControlForSystemLogger.IsHandleCreated)
            {
                return;
            }

            gridControlForSystemLogger.BeginInvoke(new Action(() =>
            {
                gridControlForSystemLogger.BeginUpdate();
                gridControlForSystemLogger.DataSource = LogRecorder.LastSystemLogList;
                gridControlForSystemLogger.EndUpdate();
            }));
        }

        void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (gridView1.GetRow(e.RowHandle) == null)
                return;
            else
            {
                if (gridView1.Columns["Level"] != null)
                {
                    string status = gridView1.GetRowCellDisplayText(e.RowHandle, "Level").ToString();
                    if (status == "Error")
                    {
                        e.Appearance.BackColor = Color.OrangeRed;
                        e.Appearance.BackColor2 = Color.OrangeRed;
                    }
                    else if (status == "Warn")
                    {
                        e.Appearance.BackColor = Color.LightYellow;
                        e.Appearance.BackColor2 = Color.LightYellow;
                    }
                }
            }
        }

    }
}
