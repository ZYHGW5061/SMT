
namespace ControlPanelClsLib.Recipe
{
    partial class RecipeStep_Dispenser
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
            this.RingLightlabel = new System.Windows.Forms.Label();
            this.cmbComponentCarrierType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // RingLightlabel
            // 
            this.RingLightlabel.AutoSize = true;
            this.RingLightlabel.Location = new System.Drawing.Point(59, 126);
            this.RingLightlabel.Name = "RingLightlabel";
            this.RingLightlabel.Size = new System.Drawing.Size(82, 14);
            this.RingLightlabel.TabIndex = 3;
            this.RingLightlabel.Text = "XY对位基准：";
            // 
            // cmbComponentCarrierType
            // 
            this.cmbComponentCarrierType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbComponentCarrierType.FormattingEnabled = true;
            this.cmbComponentCarrierType.Items.AddRange(new object[] {
            "测力计"});
            this.cmbComponentCarrierType.Location = new System.Drawing.Point(145, 74);
            this.cmbComponentCarrierType.Name = "cmbComponentCarrierType";
            this.cmbComponentCarrierType.Size = new System.Drawing.Size(121, 20);
            this.cmbComponentCarrierType.TabIndex = 2;
            this.cmbComponentCarrierType.Text = "测力计";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(68, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "Z测高方式：";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "晶圆",
            "华夫盒"});
            this.comboBox1.Location = new System.Drawing.Point(145, 124);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 2;
            this.comboBox1.Text = "晶圆";
            // 
            // RecipeStep_Dispenser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.RingLightlabel);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.cmbComponentCarrierType);
            this.Name = "RecipeStep_Dispenser";
            this.Size = new System.Drawing.Size(883, 494);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label RingLightlabel;
        private System.Windows.Forms.ComboBox cmbComponentCarrierType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}
