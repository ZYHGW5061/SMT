
namespace SystemGUILib.UserMangement
{
    partial class FrmUsers
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labName = new DevExpress.XtraEditors.LabelControl();
            this.labID = new DevExpress.XtraEditors.LabelControl();
            this.labPassWord = new DevExpress.XtraEditors.LabelControl();
            this.labType = new DevExpress.XtraEditors.LabelControl();
            this.txtID = new DevExpress.XtraEditors.TextEdit();
            this.txtName = new DevExpress.XtraEditors.TextEdit();
            this.txtPassWord = new DevExpress.XtraEditors.TextEdit();
            this.txtDescription = new DevExpress.XtraEditors.TextEdit();
            this.labDiscription = new DevExpress.XtraEditors.LabelControl();
            this.cbxType = new System.Windows.Forms.ComboBox();
            this.btn_Add = new DevExpress.XtraEditors.SimpleButton();
            this.btn_Edit = new DevExpress.XtraEditors.SimpleButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassWord.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.8843F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.1157F));
            this.tableLayoutPanel1.Controls.Add(this.labName, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.labID, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.labPassWord, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labType, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtID, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtPassWord, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtDescription, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.labDiscription, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.cbxType, 1, 3);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(13, 13);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(356, 264);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // labName
            // 
            this.labName.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labName.Location = new System.Drawing.Point(72, 66);
            this.labName.Margin = new System.Windows.Forms.Padding(4);
            this.labName.Name = "labName";
            this.labName.Size = new System.Drawing.Size(44, 18);
            this.labName.TabIndex = 0;
            this.labName.Text = "Name:";
            // 
            // labID
            // 
            this.labID.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labID.Location = new System.Drawing.Point(95, 16);
            this.labID.Margin = new System.Windows.Forms.Padding(4);
            this.labID.Name = "labID";
            this.labID.Size = new System.Drawing.Size(21, 18);
            this.labID.TabIndex = 0;
            this.labID.Text = "ID:";
            // 
            // labPassWord
            // 
            this.labPassWord.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labPassWord.Location = new System.Drawing.Point(46, 116);
            this.labPassWord.Margin = new System.Windows.Forms.Padding(4);
            this.labPassWord.Name = "labPassWord";
            this.labPassWord.Size = new System.Drawing.Size(70, 18);
            this.labPassWord.TabIndex = 0;
            this.labPassWord.Text = "PassWord:";
            // 
            // labType
            // 
            this.labType.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labType.Location = new System.Drawing.Point(77, 166);
            this.labType.Margin = new System.Windows.Forms.Padding(4);
            this.labType.Name = "labType";
            this.labType.Size = new System.Drawing.Size(39, 18);
            this.labType.TabIndex = 0;
            this.labType.Text = "Type:";
            // 
            // txtID
            // 
            this.txtID.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtID.Location = new System.Drawing.Point(124, 9);
            this.txtID.Margin = new System.Windows.Forms.Padding(4);
            this.txtID.Name = "txtID";
            this.txtID.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtID.Properties.Appearance.Options.UseBackColor = true;
            this.txtID.Properties.AutoHeight = false;
            this.txtID.Properties.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(228, 31);
            this.txtID.TabIndex = 1;
            // 
            // txtName
            // 
            this.txtName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtName.Location = new System.Drawing.Point(124, 59);
            this.txtName.Margin = new System.Windows.Forms.Padding(4);
            this.txtName.Name = "txtName";
            this.txtName.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtName.Properties.Appearance.Options.UseBackColor = true;
            this.txtName.Properties.AutoHeight = false;
            this.txtName.Size = new System.Drawing.Size(228, 31);
            this.txtName.TabIndex = 1;
            // 
            // txtPassWord
            // 
            this.txtPassWord.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtPassWord.Location = new System.Drawing.Point(124, 109);
            this.txtPassWord.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtPassWord.Properties.Appearance.Options.UseBackColor = true;
            this.txtPassWord.Properties.AutoHeight = false;
            this.txtPassWord.Properties.PasswordChar = '*';
            this.txtPassWord.Size = new System.Drawing.Size(228, 31);
            this.txtPassWord.TabIndex = 1;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtDescription.Location = new System.Drawing.Point(124, 216);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(4);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Properties.Appearance.BackColor = System.Drawing.Color.White;
            this.txtDescription.Properties.Appearance.Options.UseBackColor = true;
            this.txtDescription.Properties.AutoHeight = false;
            this.txtDescription.Size = new System.Drawing.Size(228, 31);
            this.txtDescription.TabIndex = 1;
            // 
            // labDiscription
            // 
            this.labDiscription.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.labDiscription.Location = new System.Drawing.Point(47, 223);
            this.labDiscription.Margin = new System.Windows.Forms.Padding(4);
            this.labDiscription.Name = "labDiscription";
            this.labDiscription.Size = new System.Drawing.Size(69, 18);
            this.labDiscription.TabIndex = 0;
            this.labDiscription.Text = "Discription:";
            // 
            // cbxType
            // 
            this.cbxType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.cbxType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxType.FormattingEnabled = true;
            this.cbxType.Location = new System.Drawing.Point(124, 163);
            this.cbxType.Margin = new System.Windows.Forms.Padding(4);
            this.cbxType.Name = "cbxType";
            this.cbxType.Size = new System.Drawing.Size(228, 26);
            this.cbxType.TabIndex = 2;
            // 
            // btn_Add
            // 
            this.btn_Add.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_Add.Appearance.BackColor = System.Drawing.Color.Silver;
            this.btn_Add.Appearance.Options.UseBackColor = true;
            this.btn_Add.Location = new System.Drawing.Point(-218, -49);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 36);
            this.btn_Add.TabIndex = 3;
            this.btn_Add.Text = "Ok";
            // 
            // btn_Edit
            // 
            this.btn_Edit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn_Edit.Appearance.BackColor = System.Drawing.Color.Silver;
            this.btn_Edit.Appearance.Options.UseBackColor = true;
            this.btn_Edit.Location = new System.Drawing.Point(-218, -49);
            this.btn_Edit.Name = "btn_Edit";
            this.btn_Edit.Size = new System.Drawing.Size(75, 36);
            this.btn_Edit.TabIndex = 4;
            this.btn_Edit.Text = "OK";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 286);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(356, 61);
            this.tableLayoutPanel2.TabIndex = 5;
            // 
            // FrmUsers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 367);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btn_Add);
            this.Controls.Add(this.btn_Edit);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "FrmUsers";
            this.Text = "FrmUsers";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtID.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPassWord.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtDescription.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private DevExpress.XtraEditors.LabelControl labName;
        private DevExpress.XtraEditors.LabelControl labID;
        private DevExpress.XtraEditors.LabelControl labPassWord;
        private DevExpress.XtraEditors.LabelControl labType;
        private DevExpress.XtraEditors.TextEdit txtID;
        private DevExpress.XtraEditors.TextEdit txtName;
        private DevExpress.XtraEditors.TextEdit txtPassWord;
        private DevExpress.XtraEditors.TextEdit txtDescription;
        private DevExpress.XtraEditors.LabelControl labDiscription;
        private System.Windows.Forms.ComboBox cbxType;
        private DevExpress.XtraEditors.SimpleButton btn_Add;
        private DevExpress.XtraEditors.SimpleButton btn_Edit;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    }
}