
namespace RecipeEditPanelClsLib
{
    partial class RecipeStep_ComponentInfoSettings
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbAccuracyMethod = new System.Windows.Forms.ComboBox();
            this.RingLightlabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.seComponentThicknessMM = new DevExpress.XtraEditors.SpinEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbRelatedPPTool = new System.Windows.Forms.ComboBox();
            this.cmbRelatedESTool = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.seCarrierThicknessMM = new DevExpress.XtraEditors.SpinEdit();
            this.cmbComponentCarrierType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbVisionPositionMethod = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbAccuracyVisionMethod = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbVisionPosUsedCamera = new System.Windows.Forms.ComboBox();
            this.cmbMaterialBoxTool = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnManageMaterialBoxTool = new DevExpress.XtraEditors.SimpleButton();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            ((System.ComponentModel.ISupportInitialize)(this.seComponentThicknessMM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.seCarrierThicknessMM.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbAccuracyMethod
            // 
            this.cmbAccuracyMethod.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAccuracyMethod.FormattingEnabled = true;
            this.cmbAccuracyMethod.Location = new System.Drawing.Point(145, 266);
            this.cmbAccuracyMethod.Name = "cmbAccuracyMethod";
            this.cmbAccuracyMethod.Size = new System.Drawing.Size(121, 20);
            this.cmbAccuracyMethod.TabIndex = 2;
            // 
            // RingLightlabel
            // 
            this.RingLightlabel.AutoSize = true;
            this.RingLightlabel.Location = new System.Drawing.Point(50, 269);
            this.RingLightlabel.Name = "RingLightlabel";
            this.RingLightlabel.Size = new System.Drawing.Size(91, 14);
            this.RingLightlabel.TabIndex = 3;
            this.RingLightlabel.Text = "二次定位方式：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "芯片厚度/mm：";
            // 
            // seComponentThicknessMM
            // 
            this.seComponentThicknessMM.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seComponentThicknessMM.Location = new System.Drawing.Point(145, 42);
            this.seComponentThicknessMM.Name = "seComponentThicknessMM";
            this.seComponentThicknessMM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seComponentThicknessMM.Properties.DisplayFormat.FormatString = "0.000";
            this.seComponentThicknessMM.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seComponentThicknessMM.Properties.EditFormat.FormatString = "0.000";
            this.seComponentThicknessMM.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seComponentThicknessMM.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.seComponentThicknessMM.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.seComponentThicknessMM.Properties.MaskSettings.Set("mask", "f2");
            this.seComponentThicknessMM.Properties.MaxValue = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.seComponentThicknessMM.Size = new System.Drawing.Size(121, 20);
            this.seComponentThicknessMM.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(73, 312);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "吸嘴工具：";
            // 
            // cmbRelatedPPTool
            // 
            this.cmbRelatedPPTool.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbRelatedPPTool.FormattingEnabled = true;
            this.cmbRelatedPPTool.Location = new System.Drawing.Point(145, 310);
            this.cmbRelatedPPTool.Name = "cmbRelatedPPTool";
            this.cmbRelatedPPTool.Size = new System.Drawing.Size(121, 20);
            this.cmbRelatedPPTool.TabIndex = 2;
            // 
            // cmbRelatedESTool
            // 
            this.cmbRelatedESTool.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbRelatedESTool.FormattingEnabled = true;
            this.cmbRelatedESTool.Location = new System.Drawing.Point(145, 352);
            this.cmbRelatedESTool.Name = "cmbRelatedESTool";
            this.cmbRelatedESTool.Size = new System.Drawing.Size(121, 20);
            this.cmbRelatedESTool.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(73, 354);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "顶针工具：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(49, 75);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "芯片载体类型：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(24, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(116, 14);
            this.label5.TabIndex = 3;
            this.label5.Text = "芯片载体厚度/mm：";
            // 
            // seCarrierThicknessMM
            // 
            this.seCarrierThicknessMM.EditValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.seCarrierThicknessMM.Location = new System.Drawing.Point(145, 105);
            this.seCarrierThicknessMM.Name = "seCarrierThicknessMM";
            this.seCarrierThicknessMM.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.seCarrierThicknessMM.Properties.DisplayFormat.FormatString = "0.000";
            this.seCarrierThicknessMM.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seCarrierThicknessMM.Properties.EditFormat.FormatString = "0.000";
            this.seCarrierThicknessMM.Properties.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.seCarrierThicknessMM.Properties.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.seCarrierThicknessMM.Properties.Mask.UseMaskAsDisplayFormat = true;
            this.seCarrierThicknessMM.Properties.MaskSettings.Set("mask", "f2");
            this.seCarrierThicknessMM.Properties.MaxValue = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.seCarrierThicknessMM.Size = new System.Drawing.Size(121, 20);
            this.seCarrierThicknessMM.TabIndex = 10;
            // 
            // cmbComponentCarrierType
            // 
            this.cmbComponentCarrierType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbComponentCarrierType.FormattingEnabled = true;
            this.cmbComponentCarrierType.Location = new System.Drawing.Point(145, 74);
            this.cmbComponentCarrierType.Name = "cmbComponentCarrierType";
            this.cmbComponentCarrierType.Size = new System.Drawing.Size(121, 20);
            this.cmbComponentCarrierType.TabIndex = 2;
            this.cmbComponentCarrierType.SelectedIndexChanged += new System.EventHandler(this.cmbComponentCarrierType_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(283, 269);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 14);
            this.label6.TabIndex = 3;
            this.label6.Text = "二次定位识别方式：";
            // 
            // cmbVisionPositionMethod
            // 
            this.cmbVisionPositionMethod.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbVisionPositionMethod.FormattingEnabled = true;
            this.cmbVisionPositionMethod.Location = new System.Drawing.Point(400, 224);
            this.cmbVisionPositionMethod.Name = "cmbVisionPositionMethod";
            this.cmbVisionPositionMethod.Size = new System.Drawing.Size(121, 20);
            this.cmbVisionPositionMethod.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(330, 227);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 14);
            this.label7.TabIndex = 3;
            this.label7.Text = "定位方式：";
            // 
            // cmbAccuracyVisionMethod
            // 
            this.cmbAccuracyVisionMethod.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAccuracyVisionMethod.FormattingEnabled = true;
            this.cmbAccuracyVisionMethod.Location = new System.Drawing.Point(400, 266);
            this.cmbAccuracyVisionMethod.Name = "cmbAccuracyVisionMethod";
            this.cmbAccuracyVisionMethod.Size = new System.Drawing.Size(121, 20);
            this.cmbAccuracyVisionMethod.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(73, 223);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(67, 14);
            this.label8.TabIndex = 12;
            this.label8.Text = "定位相机：";
            // 
            // cmbVisionPosUsedCamera
            // 
            this.cmbVisionPosUsedCamera.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbVisionPosUsedCamera.FormattingEnabled = true;
            this.cmbVisionPosUsedCamera.Location = new System.Drawing.Point(145, 221);
            this.cmbVisionPosUsedCamera.Name = "cmbVisionPosUsedCamera";
            this.cmbVisionPosUsedCamera.Size = new System.Drawing.Size(121, 20);
            this.cmbVisionPosUsedCamera.TabIndex = 11;
            // 
            // cmbMaterialBoxTool
            // 
            this.cmbMaterialBoxTool.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbMaterialBoxTool.FormattingEnabled = true;
            this.cmbMaterialBoxTool.Location = new System.Drawing.Point(145, 399);
            this.cmbMaterialBoxTool.Name = "cmbMaterialBoxTool";
            this.cmbMaterialBoxTool.Size = new System.Drawing.Size(121, 20);
            this.cmbMaterialBoxTool.TabIndex = 2;
            this.cmbMaterialBoxTool.SelectedIndexChanged += new System.EventHandler(this.cmbMaterialBoxTool_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(73, 401);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 14);
            this.label9.TabIndex = 3;
            this.label9.Text = "抽匣工具：";
            // 
            // btnManageMaterialBoxTool
            // 
            this.btnManageMaterialBoxTool.AllowFocus = false;
            this.btnManageMaterialBoxTool.Location = new System.Drawing.Point(276, 397);
            this.btnManageMaterialBoxTool.Name = "btnManageMaterialBoxTool";
            this.btnManageMaterialBoxTool.Size = new System.Drawing.Size(66, 23);
            this.btnManageMaterialBoxTool.TabIndex = 44;
            this.btnManageMaterialBoxTool.Text = "管理...";
            this.btnManageMaterialBoxTool.Click += new System.EventHandler(this.btnManageMaterialBoxTool_Click);
            // 
            // panelControl2
            // 
            this.panelControl2.Location = new System.Drawing.Point(573, 29);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(247, 485);
            this.panelControl2.TabIndex = 45;
            // 
            // RecipeStep_ComponentInfoSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelControl2);
            this.Controls.Add(this.btnManageMaterialBoxTool);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmbVisionPosUsedCamera);
            this.Controls.Add(this.seCarrierThicknessMM);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.seComponentThicknessMM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.RingLightlabel);
            this.Controls.Add(this.cmbMaterialBoxTool);
            this.Controls.Add(this.cmbRelatedESTool);
            this.Controls.Add(this.cmbRelatedPPTool);
            this.Controls.Add(this.cmbComponentCarrierType);
            this.Controls.Add(this.cmbAccuracyVisionMethod);
            this.Controls.Add(this.cmbVisionPositionMethod);
            this.Controls.Add(this.cmbAccuracyMethod);
            this.Name = "RecipeStep_ComponentInfoSettings";
            this.Size = new System.Drawing.Size(970, 560);
            ((System.ComponentModel.ISupportInitialize)(this.seComponentThicknessMM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.seCarrierThicknessMM.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbAccuracyMethod;
        private System.Windows.Forms.Label RingLightlabel;
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.SpinEdit seComponentThicknessMM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbRelatedPPTool;
        private System.Windows.Forms.ComboBox cmbRelatedESTool;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraEditors.SpinEdit seCarrierThicknessMM;
        private System.Windows.Forms.ComboBox cmbComponentCarrierType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbVisionPositionMethod;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbAccuracyVisionMethod;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbVisionPosUsedCamera;
        private System.Windows.Forms.ComboBox cmbMaterialBoxTool;
        private System.Windows.Forms.Label label9;
        private DevExpress.XtraEditors.SimpleButton btnManageMaterialBoxTool;
        private DevExpress.XtraEditors.PanelControl panelControl2;
    }
}
