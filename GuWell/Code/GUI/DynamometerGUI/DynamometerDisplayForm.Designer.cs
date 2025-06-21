
namespace DynamometerGUI
{
    partial class DynamometerDisplayForm
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
            this.dynamometerDisplayPanel1 = new DynamometerGUI.DynamometerDisplayPanel();
            this.SuspendLayout();
            // 
            // dynamometerDisplayPanel1
            // 
            this.dynamometerDisplayPanel1.Location = new System.Drawing.Point(10, 10);
            this.dynamometerDisplayPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.dynamometerDisplayPanel1.Name = "dynamometerDisplayPanel1";
            this.dynamometerDisplayPanel1.Size = new System.Drawing.Size(202, 88);
            this.dynamometerDisplayPanel1.TabIndex = 0;
            // 
            // DynamometerDisplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(224, 110);
            this.Controls.Add(this.dynamometerDisplayPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DynamometerDisplayForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "压力传感器";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DynamometerDisplayForm_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private DynamometerDisplayPanel dynamometerDisplayPanel1;
    }
}