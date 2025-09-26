
namespace DynamometerGUI
{
    partial class DynamometerDisplayPanel
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
            this.CurrentvalueText = new System.Windows.Forms.Label();
            this.CurrentvalueLable = new System.Windows.Forms.Label();
            this.CurrentvalueLable2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CurrentvalueText
            // 
            this.CurrentvalueText.AutoSize = true;
            this.CurrentvalueText.Location = new System.Drawing.Point(17, 17);
            this.CurrentvalueText.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentvalueText.Name = "CurrentvalueText";
            this.CurrentvalueText.Size = new System.Drawing.Size(95, 12);
            this.CurrentvalueText.TabIndex = 0;
            this.CurrentvalueText.Text = "轨道传感器压力:";
            // 
            // CurrentvalueLable
            // 
            this.CurrentvalueLable.AutoSize = true;
            this.CurrentvalueLable.Location = new System.Drawing.Point(154, 17);
            this.CurrentvalueLable.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentvalueLable.Name = "CurrentvalueLable";
            this.CurrentvalueLable.Size = new System.Drawing.Size(11, 12);
            this.CurrentvalueLable.TabIndex = 1;
            this.CurrentvalueLable.Text = "0";
            // 
            // CurrentvalueLable2
            // 
            this.CurrentvalueLable2.AutoSize = true;
            this.CurrentvalueLable2.Location = new System.Drawing.Point(154, 49);
            this.CurrentvalueLable2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentvalueLable2.Name = "CurrentvalueLable2";
            this.CurrentvalueLable2.Size = new System.Drawing.Size(11, 12);
            this.CurrentvalueLable2.TabIndex = 3;
            this.CurrentvalueLable2.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 49);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "吸嘴传感器压力:";
            // 
            // DynamometerDisplayPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CurrentvalueLable2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CurrentvalueLable);
            this.Controls.Add(this.CurrentvalueText);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "DynamometerDisplayPanel";
            this.Size = new System.Drawing.Size(202, 85);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label CurrentvalueText;
        private System.Windows.Forms.Label CurrentvalueLable;
        private System.Windows.Forms.Label CurrentvalueLable2;
        private System.Windows.Forms.Label label2;
    }
}
