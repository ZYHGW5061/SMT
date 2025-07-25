
namespace SystemGUILib.UserMangement
{
    partial class CtrlRightsManager
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.treeUserRights = new System.Windows.Forms.TreeView();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.cmbRightsType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Location = new System.Drawing.Point(117, 431);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 14, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(121, 46);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // treeUserRights
            // 
            this.treeUserRights.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeUserRights.CheckBoxes = true;
            this.treeUserRights.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeUserRights.Location = new System.Drawing.Point(117, 59);
            this.treeUserRights.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.treeUserRights.Name = "treeUserRights";
            this.treeUserRights.Size = new System.Drawing.Size(565, 359);
            this.treeUserRights.TabIndex = 16;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(15, 59);
            this.labelControl3.Margin = new System.Windows.Forms.Padding(3, 15, 3, 4);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(79, 18);
            this.labelControl3.TabIndex = 15;
            this.labelControl3.Text = "User Rights:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(15, 16);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(83, 18);
            this.labelControl2.TabIndex = 13;
            this.labelControl2.Text = "Rights Type:";
            // 
            // cmbRightsType
            // 
            this.cmbRightsType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRightsType.Font = new System.Drawing.Font("宋体", 12F);
            this.cmbRightsType.FormattingEnabled = true;
            this.cmbRightsType.Location = new System.Drawing.Point(117, 9);
            this.cmbRightsType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbRightsType.Name = "cmbRightsType";
            this.cmbRightsType.Size = new System.Drawing.Size(260, 28);
            this.cmbRightsType.TabIndex = 14;
            // 
            // CtrlRightsManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.treeUserRights);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbRightsType);
            this.Name = "CtrlRightsManager";
            this.Size = new System.Drawing.Size(697, 487);
            this.Load += new System.EventHandler(this.CtrlRightsManager_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnSave;
        private System.Windows.Forms.TreeView treeUserRights;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private System.Windows.Forms.ComboBox cmbRightsType;
    }
}
