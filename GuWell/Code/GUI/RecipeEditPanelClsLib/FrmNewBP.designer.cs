namespace RecipeEditPanelClsLib
{
    partial class FrmNewBP
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnOK = new DevExpress.XtraEditors.SimpleButton();
            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
            this.teNewName = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbFindBPMethod = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.teNewName.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(39, 118);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(117, 32);
            this.btnOK.TabIndex = 11;
            this.btnOK.Text = "确认";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(199, 118);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(117, 32);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // teNewName
            // 
            this.teNewName.Location = new System.Drawing.Point(154, 19);
            this.teNewName.Name = "teNewName";
            this.teNewName.Size = new System.Drawing.Size(162, 20);
            this.teNewName.TabIndex = 10;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(113, 22);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(36, 14);
            this.labelControl1.TabIndex = 13;
            this.labelControl1.Text = "名称：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(139, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = "确认贴装位置的方式法：";
            this.label6.Visible = false;
            // 
            // cmbFindBPMethod
            // 
            this.cmbFindBPMethod.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbFindBPMethod.FormattingEnabled = true;
            this.cmbFindBPMethod.Items.AddRange(new object[] {
            "精准模式",
            "非精准模式"});
            this.cmbFindBPMethod.Location = new System.Drawing.Point(153, 62);
            this.cmbFindBPMethod.Name = "cmbFindBPMethod";
            this.cmbFindBPMethod.Size = new System.Drawing.Size(163, 20);
            this.cmbFindBPMethod.TabIndex = 14;
            this.cmbFindBPMethod.Text = "精准模式";
            this.cmbFindBPMethod.Visible = false;
            // 
            // FrmNewBP
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(356, 184);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbFindBPMethod);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.teNewName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmNewBP";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "新建";
            ((System.ComponentModel.ISupportInitialize)(this.teNewName.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnOK;
        private DevExpress.XtraEditors.SimpleButton btnCancel;
        private DevExpress.XtraEditors.TextEdit teNewName;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbFindBPMethod;
    }
}