using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlPanelClsLib.Recipe
{
    public partial class SubmountPositionStep_PPHeight : SubmountPositionStepBasePage
    {
        public SubmountPositionStep_PPHeight()
        {
            InitializeComponent();
        }
        public override EnumDefineSetupRecipeSubmountPositionStep CurrentStep 
        { 
            get 
            { return EnumDefineSetupRecipeSubmountPositionStep.SetPPHeight; } 
        }
    }
}
