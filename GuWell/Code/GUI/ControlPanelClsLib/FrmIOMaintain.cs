using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Diagnostics;
using System.IO;
using GlobalDataDefineClsLib;
using WestDragon.Framework;
using WestDragon.Framework.BaseLoggerClsLib;
using RecipeClsLib;
using WestDragon.Framework.UtilityHelper;
using ConfigurationClsLib;
using CameraControllerClsLib;
using System.Drawing.Imaging;
using System.Threading;
using CommonPanelClsLib;
using JobClsLib;
using GlobalToolClsLib;

namespace ControlPanelClsLib
{
    public partial class FrmIOMaintain : BaseForm
    {

        
        public FrmIOMaintain()
        {
            InitializeComponent();

            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmIOMaintain_FormClosing);

        }

        private void FrmIOMaintain_FormClosing(object sender, EventArgs e)
        {
            ioMaintainPanel1._enablePollingIO2 = false;
            //e.Cancel = false;
        }

    }


}
