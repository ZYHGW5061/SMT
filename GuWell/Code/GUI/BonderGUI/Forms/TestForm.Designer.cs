
namespace BonderGUI.Forms
{
    partial class TestForm
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
            this.bonderHeadControl1 = new BonderGUI.UserControls.Manual.BonderHeadControl();
            this.SuspendLayout();
            // 
            // bonderHeadControl1
            // 
            this.bonderHeadControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bonderHeadControl1.Location = new System.Drawing.Point(0, 0);
            this.bonderHeadControl1.Name = "bonderHeadControl1";
            this.bonderHeadControl1.Size = new System.Drawing.Size(1096, 761);
            this.bonderHeadControl1.TabIndex = 0;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1096, 761);
            this.Controls.Add(this.bonderHeadControl1);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);

        }

        #endregion

        private UserControls.Manual.BonderHeadControl bonderHeadControl1;
    }
}