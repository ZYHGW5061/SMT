using DevExpress.Utils.Design;
using GlobalDataDefineClsLib;
using PositioningSystemClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VisionGUI;

namespace ControlPanelClsLib
{
    public partial class PPTool_Alignment : UserControl
    {
        public PPTool_Alignment()
        {
            InitializeComponent();
            if (!DesignTimeTools.IsDesignMode)
            {
                InitialCameraControl();
                LoadNextStepPage();
                UpdateStepSignStatus();
            }

            FirstCenterPosition = new XYZTCoordinateConfig();
            SecondCenterPosition = new XYZTCoordinateConfig();
        }
        private PPToolAlignmentStepBasePage currentStepPage;
        /// <summary>
        /// 特征的系统坐标系位置
        /// </summary>
        public XYZTCoordinateConfig FirstCenterPosition { get; set; }
        public XYZTCoordinateConfig SecondCenterPosition { get; set; }
        private void InitialCameraControl()
        {
            panelControlCameraAera.Controls.Clear();
            CameraWindowGUI.Instance.Dock = DockStyle.Fill;
            panelControlCameraAera.Controls.Add(CameraWindowGUI.Instance);
        }
        private void LoadPreviousStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefinePPAlignStep.None;
                //currentStepPage.NotifyStepFinished(out finished, out step);
                this.panelStepOperate.Controls.Clear();
                currentStepPage = GenerateStepPage(currentStepPage.CurrentStep - 1);
                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            else
            {
                this.panelStepOperate.Controls.Clear();
                currentStepPage = new PPToolAlignmentStep_PositionCenter();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            if (currentStepPage.CurrentStep == EnumDefinePPAlignStep.PositionCenterFirst)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefinePPAlignStep.PositionCenterSecond)
            {
                this.btnNext.Text = "下一步";
            }
            else
            {
                this.btnNext.Visible = true;
            }
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }
        private void LoadNextStepPage()
        {
            if (currentStepPage != null)
            {
                var finished = false;
                var step = EnumDefinePPAlignStep.None;
                currentStepPage.NotifyStepFinished(out finished, out step);
                SaveStepParametersWhenStepFinished(step);
                if (step == EnumDefinePPAlignStep.PositionCenterFirst)
                {
                    currentStepPage = GenerateStepPage(currentStepPage.CurrentStep + 1);
                    if (currentStepPage != null)
                    {
                        this.panelStepOperate.Controls.Clear();
                        currentStepPage.Dock = DockStyle.Fill;
                        this.panelStepOperate.Controls.Add(currentStepPage);
                        //ppt旋转180度
                        PositioningSystem.Instance.MoveAixsToStageCoord(EnumStageAxis.ChipPPT, 180, WestDragon.Framework.UtilityHelper.EnumCoordSetType.Relative);
                    }
                }
                else
                {
                    StepSignComplete();
                }
            }
            else
            {
                this.panelStepOperate.Controls.Clear();
                currentStepPage = new PPToolAlignmentStep_PositionCenter();

                currentStepPage.Dock = DockStyle.Fill;
                this.panelStepOperate.Controls.Add(currentStepPage);
            }
            if (currentStepPage.CurrentStep == EnumDefinePPAlignStep.PositionCenterFirst)
            {
                this.btnPrevious.Visible = false;
            }
            else
            {
                this.btnPrevious.Visible = true;
            }
            if (currentStepPage.CurrentStep == EnumDefinePPAlignStep.PositionCenterSecond)
            {
                this.btnNext.Text = "完成";
            }
            else
            {
                this.btnNext.Visible = true;
            }
            //this.labelStepInfo.Text = currentStepPage.StepDescription;
            //LoadStepParameters(currentTeachStepPage.CurrentStep);
        }

        private PPToolAlignmentStepBasePage GenerateStepPage(EnumDefinePPAlignStep step)
        {
            PPToolAlignmentStepBasePage ret = null;
            switch (step)
            {
                case EnumDefinePPAlignStep.None:
                    break;
                case EnumDefinePPAlignStep.PositionCenterFirst:
                    ret = new PPToolAlignmentStep_PositionCenter();
                    break;
                case EnumDefinePPAlignStep.PositionCenterSecond:
                    ret = new PPToolAlignmentStep_PositionCenterAfterR180();
                    break;
                default:
                    break;
            }
            return ret;
        }
        private void SaveStepParametersWhenStepFinished(EnumDefinePPAlignStep step)
        {
            switch (step)
            {
                case EnumDefinePPAlignStep.None:
                    break;
                case EnumDefinePPAlignStep.PositionCenterFirst:
                    FirstCenterPosition = currentStepPage.CenterPosition;
                    break;
                case EnumDefinePPAlignStep.PositionCenterSecond:
                    SecondCenterPosition = currentStepPage.CenterPosition;
                    break;
                default:
                    break;
            }
        }
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            LoadPreviousStepPage();
            UpdateStepSignStatus();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            LoadNextStepPage();
            UpdateStepSignStatus();

        }
        private void UpdateStepSignStatus()
        {
            switch (currentStepPage.CurrentStep)
            {
                case EnumDefinePPAlignStep.None:
                    break;
                case EnumDefinePPAlignStep.PositionCenterFirst:
                    step1Sign.Image = Properties.Resources.sucker_0;
                    step2Sign.Image = Properties.Resources.sucker_180_undo;
                    break;
                case EnumDefinePPAlignStep.PositionCenterSecond:
                    step1Sign.Image = Properties.Resources.sucker_0_done;
                    step2Sign.Image = Properties.Resources.sucker_180;
                    break;
                
                default:
                    break;
            }

        }
        private void StepSignComplete()
        {
            step1Sign.Image = Properties.Resources.sucker_0_done;
            step2Sign.Image = Properties.Resources.sucker_180_done;
        }
    }
}
