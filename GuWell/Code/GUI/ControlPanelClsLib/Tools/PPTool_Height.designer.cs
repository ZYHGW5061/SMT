
namespace ControlPanelClsLib
{
    partial class PPTool_Height
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
            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
            DevExpress.XtraCharts.Series series1 = new DevExpress.XtraCharts.Series();
            DevExpress.XtraCharts.LineSeriesView lineSeriesView1 = new DevExpress.XtraCharts.LineSeriesView();
            DevExpress.XtraCharts.ChartTitle chartTitle1 = new DevExpress.XtraCharts.ChartTitle();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelStepInfo = new DevExpress.XtraEditors.LabelControl();
            this.btnOK = new System.Windows.Forms.Button();
            this.chartControlHeatCurve = new DevExpress.XtraCharts.ChartControl();
            this.stageQuickMove1 = new StageCtrlPanelLib.StageQuickMove();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlHeatCurve)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.labelControl2);
            this.panelControl1.Controls.Add(this.labelControl1);
            this.panelControl1.Controls.Add(this.labelStepInfo);
            this.panelControl1.Location = new System.Drawing.Point(893, 29);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(267, 108);
            this.panelControl1.TabIndex = 39;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(14, 42);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(244, 19);
            this.labelControl2.TabIndex = 5;
            this.labelControl2.Text = "传感器上方5mm处，点击确认，";
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(14, 71);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(153, 19);
            this.labelControl1.TabIndex = 4;
            this.labelControl1.Text = "机器会自动测量高度";
            this.labelControl1.Click += new System.EventHandler(this.labelStepInfo_Click);
            // 
            // labelStepInfo
            // 
            this.labelStepInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStepInfo.Appearance.Options.UseFont = true;
            this.labelStepInfo.Location = new System.Drawing.Point(14, 11);
            this.labelStepInfo.Name = "labelStepInfo";
            this.labelStepInfo.Size = new System.Drawing.Size(221, 19);
            this.labelStepInfo.TabIndex = 4;
            this.labelStepInfo.Text = "步骤 1/1：将吸嘴移动到测力";
            this.labelStepInfo.Click += new System.EventHandler(this.labelStepInfo_Click);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(1041, 501);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(120, 30);
            this.btnOK.TabIndex = 40;
            this.btnOK.Text = "确认";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chartControlHeatCurve
            // 
            xyDiagram1.AxisX.GridLines.MinorVisible = true;
            xyDiagram1.AxisX.GridLines.Visible = true;
            xyDiagram1.AxisX.Title.Text = "Z高度/mm";
            xyDiagram1.AxisX.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisX.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisX.VisualRange.Auto = false;
            xyDiagram1.AxisX.VisualRange.AutoSideMargins = false;
            xyDiagram1.AxisX.VisualRange.EndSideMargin = 0D;
            xyDiagram1.AxisX.VisualRange.MaxValueSerializable = "5000";
            xyDiagram1.AxisX.VisualRange.MinValueSerializable = "0";
            xyDiagram1.AxisX.VisualRange.StartSideMargin = 0D;
            xyDiagram1.AxisX.WholeRange.Auto = false;
            xyDiagram1.AxisX.WholeRange.AutoSideMargins = false;
            xyDiagram1.AxisX.WholeRange.EndSideMargin = 0D;
            xyDiagram1.AxisX.WholeRange.MaxValueSerializable = "5000";
            xyDiagram1.AxisX.WholeRange.MinValueSerializable = "0";
            xyDiagram1.AxisX.WholeRange.StartSideMargin = 0D;
            xyDiagram1.AxisY.GridLines.MinorVisible = true;
            xyDiagram1.AxisY.Title.Text = "压力值";
            xyDiagram1.AxisY.Title.Visibility = DevExpress.Utils.DefaultBoolean.True;
            xyDiagram1.AxisY.VisibleInPanesSerializable = "-1";
            xyDiagram1.AxisY.WholeRange.Auto = false;
            xyDiagram1.AxisY.WholeRange.AutoSideMargins = false;
            xyDiagram1.AxisY.WholeRange.EndSideMargin = 0D;
            xyDiagram1.AxisY.WholeRange.MaxValueSerializable = "500";
            xyDiagram1.AxisY.WholeRange.MinValueSerializable = "0";
            xyDiagram1.AxisY.WholeRange.StartSideMargin = 0D;
            this.chartControlHeatCurve.Diagram = xyDiagram1;
            this.chartControlHeatCurve.Legend.Name = "Default Legend";
            this.chartControlHeatCurve.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
            this.chartControlHeatCurve.Location = new System.Drawing.Point(45, 29);
            this.chartControlHeatCurve.Name = "chartControlHeatCurve";
            series1.Name = "Series 1";
            series1.View = lineSeriesView1;
            this.chartControlHeatCurve.SeriesSerializable = new DevExpress.XtraCharts.Series[] {
        series1};
            this.chartControlHeatCurve.Size = new System.Drawing.Size(640, 451);
            this.chartControlHeatCurve.TabIndex = 47;
            chartTitle1.Text = "压力曲线";
            this.chartControlHeatCurve.Titles.AddRange(new DevExpress.XtraCharts.ChartTitle[] {
            chartTitle1});
            // 
            // stageQuickMove1
            // 
            this.stageQuickMove1.Location = new System.Drawing.Point(893, 163);
            this.stageQuickMove1.Name = "stageQuickMove1";
            this.stageQuickMove1.PositiveQucikMoveAct = null;
            this.stageQuickMove1.SelectedAxisSystem = GlobalDataDefineClsLib.EnumSystemAxis.XY;
            this.stageQuickMove1.SelectedStageSystem = GlobalDataDefineClsLib.EnumStageSystem.BondTable;
            this.stageQuickMove1.Size = new System.Drawing.Size(267, 317);
            this.stageQuickMove1.TabIndex = 4;
            // 
            // PPTool_Height
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chartControlHeatCurve);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.panelControl1);
            this.Controls.Add(this.stageQuickMove1);
            this.Name = "PPTool_Height";
            this.Size = new System.Drawing.Size(1179, 562);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            this.panelControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(xyDiagram1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(lineSeriesView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(series1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartControlHeatCurve)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private StageCtrlPanelLib.StageQuickMove stageQuickMove1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.LabelControl labelStepInfo;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Button btnOK;
        private DevExpress.XtraCharts.ChartControl chartControlHeatCurve;
        private DevExpress.XtraEditors.LabelControl labelControl2;
    }
}
