using ConfigurationClsLib;
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
using static GlobalToolClsLib.GlobalCommFunc;

namespace MainGUI.UserControls.Product
{
    public partial class ComponentDetail : UserControl
    {
        public ComponentDetail()
        {
            InitializeComponent();
            LoadExistESTool();
            LoadExistPPTool();
        }
        /// <summary>
        /// 系统配置
        /// </summary>
        private SystemConfiguration _systemConfig
        {
            get { return SystemConfiguration.Instance; }
        }
        public float CurrentChipPPPickPos
        {
            get
            {
                var ret = 0f;
                float.TryParse(teChipPPPickPos.Text, out ret);
                return ret;
            }
        }
        public float CurrentChipPPPlacePos
        {
            get
            {
                var ret = 0f;
                float.TryParse(teChipPPPlacePos.Text, out ret);
                return ret;
            }
        }
        public float CurrentChipPPPress
        {
            get
            {
                var ret = 0f;
                float.TryParse(teChipPPPress.Text, out ret);
                return ret;
            }
        }

        public float CurrentSubmountPPPickPos
        {
            get
            {
                var ret = 0f;
                //float.TryParse(teSubmountPPPickPos.Text, out ret);
                return ret;
            }
        }
        public float CurrentSubmountPPPlacePos
        {
            get
            {
                var ret = 0f;
                //float.TryParse(teSubmountPPPlacePos.Text, out ret);
                return ret;
            }
        }
        public float CurrentSubmountPPPress
        {
            get
            {
                var ret = 0f;
                //float.TryParse(teSubmountPPPress.Text, out ret);
                return ret;
            }
        }
        public float CurrentVaccumDelayMS
        {
            get
            {
                var ret = 0f;
                float.TryParse(teVaccumDelayMS.Text, out ret);
                return ret;
            }
        }
        public float CurrentNeedleUpHeight
        {
            get
            {
                var ret = 0f;
                float.TryParse(teNeedleUpHeight.Text, out ret);
                return ret;
            }
        }

        public float CurrentPlaceStress
        {
            get
            {
                var ret = 0f;
                float.TryParse(sePlaceStress.Text, out ret);
                return ret;
            }
        }
        public float CurrentDelayMSForPlace
        {
            get
            {
                var ret = 0f;
                float.TryParse(sePlaceDelayMs.Text, out ret);
                return ret;
            }
        }
        public float CurrentBreakVaccumTimespanMS
        {
            get
            {
                var ret = 0f;
                float.TryParse(seBreakVaccumTimespanMs.Text, out ret);
                return ret;
            }
        }
        public string CurrentRelatedPPTool
        {
            get
            {
                return cbPPName.Text;
            }
        }

        public void FillComponentDetail(ProgramComponentSettings chipInfo, ProgramSubstrateSettings substrateInfo=null)
        {
            if (chipInfo != null)
            {
                ProgramComponentSettings material = chipInfo;

                teMaterialName.Text = material.Name;

                //EnumHelper.FillComboBoxWithEnumDesc(cbMaterialType, typeof(EnumMaterialType));
                //cbMaterialType.SelectedText = EnumHelper.GetEnumDescription(material.MaterialType);

                EnumHelper.FillComboBoxWithEnumDesc(cbCarrierType, typeof(EnumCarrierType));
                cbCarrierType.Text = EnumHelper.GetEnumDescription(material.CarrierType);



                teRowCount.Text = material.RowCount.ToString();
                teColumnCount.Text = material.ColumnCount.ToString();
                tePitchRowMM.Text = material.PitchRowMM.ToString();
                tePitchColumnMM.Text = material.PitchColumnMM.ToString();
                teCarrierThicknessMM.Text = material.CarrierThicknessMM.ToString();
                teWidthMM.Text = material.WidthMM.ToString();
                teHeightMM.Text = material.HeightMM.ToString();
                teThicknessMM.Text = material.ThicknessMM.ToString();
                teWaferThickness.Text = material.WaferThickness.ToString();
                //teFirstPosX.Text = material.FirstComponentLocation.X.ToString();
                //teFirstPosY.Text = material.FirstComponentLocation.Y.ToString();
                //teFirstPosZ.Text = material.FirstComponentLocation.Z.ToString();
                //teFirstPosTheta.Text = material.FirstComponentLocation.Theta.ToString();

                //EnumHelper.FillComboBoxWithEnumDesc(cbPPType, typeof(EnumUsedPP));
                //cbPPType.SelectedText = EnumHelper.GetEnumDescription(material.PPSettings.UsedPP);

                //后续需要去列表选吸嘴
                cbPPName.Text = material.RelatedPPToolName;

                cbPPName.Text = material.PPSettings.PPtoolName;

                teChipPPPress.Text = material.PPSettings.PickupStress.ToString();
                teChipPPPickPos.Text = material.ChipPPPickSystemPos.ToString();
                //teChipPPPlacePos.Text = material.ChipPPPlacePos.ToString();

                sePlaceDelayMs.Text = material.PPSettings.DelayMSForPlace.ToString();
                sePlaceStress.Text = material.PPSettings.PlaceStress.ToString();
                seBreakVaccumTimespanMs.Text= material.PPSettings.BreakVaccumTimespanMS.ToString();

                teSlowTravelBeforePickupMM.Text = material.PPSettings.SlowTravelBeforePickupMM.ToString();
                teSlowSpeedBeforePickup.Text = material.PPSettings.SlowSpeedBeforePickup.ToString();
                teSlowTravelAfterPickupMM.Text = material.PPSettings.SlowTravelAfterPickupMM.ToString();
                teSlowSpeedAfterPickup.Text = material.PPSettings.SlowSpeedAfterPickup.ToString();

                EnumHelper.FillComboBoxWithEnumDesc(cbIsUseNeedle, typeof(EnumYesOrNo));
                cbIsUseNeedle.Text = material.PPSettings.IsUseNeedle ? "是" : "否";

                cbNeedleName.Text = material.RelatedESToolName; 
                teNeedleUpHeight.Text = material.PPSettings.NeedleUpHeight.ToString();
                teNeedleSpeed.Text = material.PPSettings.NeedleSpeed.ToString();

                teVaccumDelayMS.Text = material.PPSettings.DelayMSForVaccum.ToString();
            }
            if(substrateInfo!=null)
            {

                //EnumHelper.FillComboBoxWithEnumDesc(cmbSMCarrierType, typeof(EnumCarrierType));
                //cmbSMCarrierType.Text = EnumHelper.GetEnumDescription(submountInfo.CarrierType);

                teSMRowCount.Text = substrateInfo.RowCount.ToString();
                teSMColumnCount.Text = substrateInfo.ColumnCount.ToString();
                teSMPitchRowMM.Text = substrateInfo.PitchRowMM.ToString();
                teSMPitchColumnMM.Text = substrateInfo.PitchColumnMM.ToString();
                teSMCarrierThicknessMM.Text = substrateInfo.CarrierThicknessMM.ToString();
                teSMWidthMM.Text = substrateInfo.WidthMM.ToString();
                teSMHeightMM.Text = substrateInfo.HeightMM.ToString();
                teSMThicknessMM.Text = substrateInfo.ThicknessMM.ToString();
                teSMWaferThickness.Text = substrateInfo.WaferThickness.ToString();

                //teSubmountPPPickPos.Text = submountInfo.SubmountPPPickPos.ToString();
                //teSubmountPPPlacePos.Text = submountInfo.SubmountPPPlacePos.ToString();
                //teSubmountPPPress.Text = submountInfo.PPSettings.PickupStress.ToString();
                //teSubmountPPPickPosForBlanking.Text = submountInfo.SubmountPPPlacePos.ToString();
                //teSubmountPPPlacePosForBlanking.Text = submountInfo.SubmountPPPickPos.ToString();
            }
        }

        private void ComponentDetail_Load(object sender, EventArgs e)
        {

        }
        private void LoadExistESTool()
        {
            cbNeedleName.Items.Clear();
            foreach (var item in _systemConfig.ESToolSettings)
            {
                cbNeedleName.Items.Add(item.Name);
            }
        }
        private void LoadExistPPTool()
        {
            cbPPName.Items.Clear();
            foreach (var item in _systemConfig.PPToolSettings)
            {
                cbPPName.Items.Add(item.Name);
            }
        }
    }
}
