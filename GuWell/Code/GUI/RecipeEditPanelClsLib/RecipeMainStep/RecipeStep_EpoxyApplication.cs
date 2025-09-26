using BoardCardControllerClsLib;
using CommonPanelClsLib;
using ConfigurationClsLib;
using ControlPanelClsLib.Tools;
using DispensingMachineControllerClsLib;
using DispensingMachineManagerClsLib;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using LaserSensorManagerClsLib;
using PositioningSystemClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WestDragon.Framework.BaseLoggerClsLib;
using WestDragon.Framework.UtilityHelper;

namespace RecipeEditPanelClsLib
{
    public partial class RecipeStep_EpoxyApplication : RecipeStepBase
    {

        /// <summary>
        /// 定位系统
        /// </summary>
        private PositioningSystem _positioningSystem
        {
            get { return PositioningSystem.Instance; }
        }


        public RecipeStep_EpoxyApplication()
        {
            InitializeComponent();
            InitialControl();
        }
        private void InitialControl()
        {
            //cmbDispensePattern.Items.Clear();
            //foreach (var item in Enum.GetValues(typeof(EnumDispensePattern)))
            //{
            //    cmbDispensePattern.Items.Add(item);
            //}

            cmbDispensePattern.Items.Clear();
            cmbDispensePattern.BindToEnum<EnumDispensePattern>();

            //cmbDispensePattern.Items.Clear();
            //cmbDispensePattern.DataSource = EnumExtensions.GetEnumDataSource<EnumDispensePattern>();

            //cmbDispensePattern.DisplayMember = "Value";
            //cmbDispensePattern.ValueMember = "Key";

            cmbExistEpoxtTool.Items.Clear();

            foreach (var dispenser in _systemConfig.DispenserSettings)
            {
                cmbExistEpoxtTool.Items.Add(dispenser.Name);
            }

        }
        /// <summary>
        /// 点胶机控制器
        /// </summary>
        IDispensingMachineController _currentDispenseController
        {
            get
            {
                return DispensingMachineManager.Instance.GetCurrentHardware();
            }
        }
        public override void LoadEditedRecipe(BondRecipe recipe)
        {
            _editRecipe = recipe;
            cmbSelDispenserRecipe.Text = _editRecipe.CurrentEpoxyApplication.DispenserRecipeName;
            //cmbDispensePattern.Text = _editRecipe.CurrentEpoxyApplication.DispensePattern.ToString();
            //cmbDispensePattern.SelectedValue = _editRecipe.CurrentEpoxyApplication.DispensePattern;
            cmbDispensePattern.SetSelectedEnum(_editRecipe.CurrentEpoxyApplication.DispensePattern);

            seDispensePatternWidth.Text = _editRecipe.CurrentEpoxyApplication.DispensePatternWidthMM.ToString();
            seDispensePatternHeight.Text = _editRecipe.CurrentEpoxyApplication.DispensePatternHeightMM.ToString();
            seDispenserSystemPosZMM.Text = _editRecipe.CurrentEpoxyApplication.DispenserSystemPosZMM.ToString();
            seDispenseSpeed.Text = _editRecipe.CurrentEpoxyApplication.DispenserSpeed.ToString();
            //seCarrierThicknessMM.Text = _editRecipe.CurrentComponent.CarrierThicknessMM.ToString();
            //cmbVisionPositionMethod.Text= _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionMethod.ToString();
            //cmbAccuracyMethod.Text= _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod.ToString();
            //cmbAccuracyVisionMethod.Text= _editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod.ToString();

            //cmbVisionPosUsedCamera.Text = _editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera.ToString();

            //cmbRelatedPPTool.Text = _editRecipe.CurrentComponent.RelatedPPToolName;
            //cmbRelatedESTool.Text = _editRecipe.CurrentComponent.RelatedESToolName;
            var dispenseInfo=_currentDispenseController.ReadDispensingParameters(Int32.Parse(_editRecipe.CurrentEpoxyApplication.DispenserRecipeName));
            seDispensePressure.Text = dispenseInfo.Pressure.ToString();
            seDispenseVaccumPressure.Text = dispenseInfo.Vacuum.ToString();
            seDispenseTimeS.Text = dispenseInfo.Time.ToString();

            cmbExistEpoxtTool.Text = _editRecipe.CurrentEpoxyApplication.DispenserName;

        }

        /// <summary>
        /// 验证单步定义是否完成
        /// </summary>
        public override void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep currengStep)
        {

            currengStep = EnumRecipeStep.EpoxyApplications;
            _editRecipe.CurrentEpoxyApplication.DispenserRecipeName = cmbSelDispenserRecipe.Text;

            //_editRecipe.CurrentEpoxyApplication.DispensePattern = (EnumDispensePattern)Enum.Parse(typeof(EnumDispensePattern), cmbDispensePattern.Text);
            //_editRecipe.CurrentEpoxyApplication.DispensePattern = (EnumDispensePattern)cmbDispensePattern.SelectedValue;
            _editRecipe.CurrentEpoxyApplication.DispensePattern = cmbDispensePattern.GetSelectedEnum<EnumDispensePattern>();

            _editRecipe.CurrentEpoxyApplication.DispenserName = cmbExistEpoxtTool.Text;

            _editRecipe.CurrentEpoxyApplication.DispensePatternWidthMM = float.Parse(seDispensePatternWidth.Text.Trim());
            _editRecipe.CurrentEpoxyApplication.DispensePatternHeightMM = float.Parse(seDispensePatternHeight.Text.Trim());
            _editRecipe.CurrentEpoxyApplication.DispenserSystemPosZMM = float.Parse(seDispenserSystemPosZMM.Text.Trim());
            _editRecipe.CurrentEpoxyApplication.DispenserSpeed = float.Parse(seDispenseSpeed.Text.Trim());
            //_editRecipe.CurrentComponent.ThicknessMM = float.Parse(seComponentThicknessMM.Text.Trim());
            //_editRecipe.CurrentComponent.CarrierType = (EnumCarrierType)Enum.Parse(typeof(EnumCarrierType), cmbComponentCarrierType.Text);
            //_editRecipe.CurrentComponent.CarrierThicknessMM = float.Parse(seCarrierThicknessMM.Text.Trim());
            //_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbVisionPositionMethod.Text);
            //_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyMethod = (EnumAccuracyMethod)Enum.Parse(typeof(EnumAccuracyMethod), cmbAccuracyMethod.Text);
            //_editRecipe.CurrentComponent.AccuracyComponentPositionVisionParameters.AccuracyVisionPositionMethod = (EnumVisionPositioningMethod)Enum.Parse(typeof(EnumVisionPositioningMethod), cmbAccuracyVisionMethod.Text);

            //_editRecipe.CurrentComponent.PositionComponentVisionParameters.VisionPositionUsedCamera = (EnumCameraType)Enum.Parse(typeof(EnumCameraType), cmbVisionPosUsedCamera.Text);

            //_editRecipe.CurrentComponent.RelatedPPToolName = cmbRelatedPPTool.Text;
            //_editRecipe.CurrentComponent.RelatedESToolName = cmbRelatedESTool.Text;

            _editRecipe.CurrentEpoxyApplication.IsCompleted = true;
            finished = true;
        }

        private void seCarrierThicknessMM_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void spinEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void spinEdit2_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void sePickupDelayMs_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void RingLightlabel_Click(object sender, EventArgs e)
        {

        }

        private void cmbComponentCarrierType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSetDispenseParam_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cmbSelDispenserRecipe.Text))
            {
                if (WarningBox.FormShow("即将设置点胶配方参数！", "是否设置点胶配方参数并保存？", "提示") == 1)
                {


                    _currentDispenseController.Set(DispensingMachineControllerClsLib.MUSASHICommandenum.吐出条件,
                                                      cmbSelDispenserRecipe.Text, "0", seDispenseTimeS.Text.Trim(), seDispensePressure.Text.Trim(),
                                                      seDispenseVaccumPressure.Text);
                    WarningBox.FormShow("成功！", "点胶配方参数保存完成", "提示");

                }
            }
            else
            {
                WarningBox.FormShow("错误！", "请先选择点胶配方！", "提示");
            }
        }

        private void cmbSelDispenserRecipe_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dispenseInfo = _currentDispenseController.ReadDispensingParameters(Int32.Parse(cmbSelDispenserRecipe.Text));
            seDispensePressure.Text = dispenseInfo.Pressure.ToString();
            seDispenseVaccumPressure.Text = dispenseInfo.Vacuum.ToString();
            seDispenseTimeS.Text = dispenseInfo.Time.ToString();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            FrmEpoxtTool2 form = (Application.OpenForms["FrmEpoxtTool2"]) as FrmEpoxtTool2;
            if (form == null)
            {
                form = new FrmEpoxtTool2();
                form.Location = this.PointToScreen(new Point(300, 300));
                form.Owner = this.FindForm();
                LogRecorder.RecordUserOperationLog($"打开点胶工具页面", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, "");
                form.Show(this);
            }
            else
            {
                LogRecorder.RecordUserOperationLog($"激活点胶工具页面", WestDragon.Framework.BaseLoggerClsLib.EnumLogContentType.Info, "");
                form.Activate();
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("胶座即将升起!", "确认榜头已到安全位置？", "提示") == 1)
            {
                IOUtilityClsLib.IOUtilityHelper.Instance.UpDispenserCylinder();
            }
        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("胶座即将下降!", "确认榜头已到安全位置？", "提示") == 1)
            {
                IOUtilityClsLib.IOUtilityHelper.Instance.DownDispenserCylinder();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            if (WarningBox.FormShow("即将记录基板划胶的高度位置！", "确认已定位到合适的划胶位置？", "提示") == 1)
            {
                float DispenserWorkHeight = (float)_positioningSystem.ReadCurrentSystemPosition(EnumStageAxis.BondZ);

                seDispenserSystemPosZMM.Text = DispenserWorkHeight.ToString();

                _editRecipe.CurrentEpoxyApplication.DispenserSystemPosZMM = float.Parse(seDispenserSystemPosZMM.Text.Trim());
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            var curDispenser = _systemConfig.DispenserSettings?.FirstOrDefault(tool => tool.Name == cmbExistEpoxtTool.Text);
            if(curDispenser == null)
            {
                WarningBox.FormShow("步骤错误！", "点胶器未空？", "提示");
                return;
            }
            if (WarningBox.FormShow("即将测量划胶的高度位置！", "确认相机已定位到合适的划胶位置？", "提示") == 1)
            {
                try
                {
                    CreateWaitDialog();
                    BoardCardManager.Instance.GetCurrentController().IO_WriteOutPut_2(11, (int)EnumBoardcardDefineOutputIO.EpoxtliftCylinder, 0);
                    //将激光测高仪移动到当前相机位置
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X,
                    EnumCoordSetType.Relative);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, _systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y,
                        EnumCoordSetType.Relative);
                    //读取激光测高仪读数
                    Thread.Sleep(500);
                    double distance = -1;
                    distance = (double)LaserSensorManager.Instance.GetCurrentHardware().ReadDistance();
                    if (distance >= 0)
                    {
                        DataModel.Instance.LaserValue = distance / 10000.0f;
                    }
                    else
                    {
                        DataModel.Instance.LaserValue = 0;
                    }
                    //var curLaserMeasureH = _laserSensor.ReadDistance() / 10000;
                    var curLaserMeasureH = DataModel.Instance.LaserValue;
                    //var curLaserMeasureH = _laserSensor.ReadDistance() / 1000;
                    //根据校准数据及当前的激光测高仪读数计算吸嘴工作高度
                    var curBondZ = _positioningSystem.ReadCurrentStagePosition(EnumStageAxis.BondZ);
                    //var offsetZ = curBondZ - _systemConfig.PositioningConfig.TrackLaserSensorOrigion.Z;
                    var offsetZ = curBondZ - (float)curDispenser.TrackBondCameraToEpoxtSpotCoordinate.Z;
                    var offsetMeasureZ = -(curLaserMeasureH - _systemConfig.PositioningConfig.TrackLaserSensorZ);
                    var componentZ = offsetMeasureZ - offsetZ;

                    float DispenserWorkHeight = (float)((double)curDispenser.TrackEpoxtSpotCoordinate.Z + offsetZ - offsetMeasureZ - _systemConfig.PositioningConfig.BondOrigion.Z);

                    seDispenserSystemPosZMM.Text = DispenserWorkHeight.ToString();

                    _editRecipe.CurrentEpoxyApplication.DispenserSystemPosZMM = float.Parse(seDispenserSystemPosZMM.Text.Trim());

                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondX, -_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.X,
                        EnumCoordSetType.Relative);
                    _positioningSystem.MoveAixsToStageCoord(EnumStageAxis.BondY, -_systemConfig.PositioningConfig.LaserSensorAndBondCameraOffset.Y,
                        EnumCoordSetType.Relative);
                    CloseWaitDialog();
                    WarningBox.FormShow("成功！", "测高完成！", "提示");
                }
                catch (Exception ex)
                {
                    LogRecorder.RecordLog(EnumLogContentType.Error, "BPHeightProgram-LaserMeasureHeight,Error.", ex);
                    WarningBox.FormShow("发生异常！", "测高失败！", "提示");
                }
                finally
                {
                    CloseWaitDialog();
                }

                //_editRecipe.CurrentEpoxyApplication.DispenserSystemPosZMM = float.Parse(DispenserSystemPosZMM.Text.Trim());
            }

        }
    }
}
