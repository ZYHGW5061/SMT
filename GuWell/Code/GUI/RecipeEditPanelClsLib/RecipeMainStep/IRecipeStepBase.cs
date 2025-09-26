using DevExpress.XtraEditors;
using GlobalDataDefineClsLib;
using GlobalToolClsLib;
using RecipeClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecipeEditPanelClsLib
{
    /// <summary>
    /// 定义ProcessRecipe的内容接口
    /// </summary>
    public interface IRecipeStepBase
    {
        /// <summary>
        /// 上一步的页面Id
        /// </summary>
        Type PrePageView
        {
            get;
            set;
        }
        /// <summary>
        /// 上一步页面描述
        /// </summary>
        string PrePageViewDescription
        {
            get;
            set;
        }

        /// <summary>
        /// 当前页面描述
        /// </summary>
        string NextPageViewDescription
        {
            get;
            set;
        }
        /// <summary>
        /// 下一步的页面Id
        /// </summary>
        Type NextPageView
        {
            get;
            set;
        }

        /// <summary>
        /// 当前步骤所属的root step
        /// </summary>
        EnumRecipeRootStep CurRecipeStepOwner { get; set; }

        /// <summary>
        /// 通知单步Recipe定义完成
        /// </summary>
        Action<BondRecipe, int[], int[]> NotifySingleStepDefineFinished
        {
            get;
            set;
        }

        /// <summary>
        /// 加载Recipe内容
        /// </summary>
        /// <param name="recipe"></param>
        void LoadEditedRecipe(BondRecipe recipe);

        /// <summary>
        /// 确认定义Recipe是否完成
        /// </summary>
        /// <param name="finished"></param>
        void VertifyAndNotifySingleStepDefineFinished(out bool finished, out EnumRecipeStep CurrentStep);

        /// <summary>
        /// 是否允许切换到前一页面
        /// </summary>
        /// <param name="enableGoto"></param>
        void GotoProviousStepPage(out bool enableGoto);
    }
    internal interface IUserControlResult
    {
        object CallbackResult { get; set; }
    }
    public class ChildFormArgs : EventArgs
    {
        public object Result { get; set; }
    }
    public static class ChildFormHelper
    {
        public static void ShowEditDialog(this Form parent, UserControl uc, EventHandler<ChildFormArgs> callback = null)
        {
            if (!(uc is IUserControlResult child))
            {
                throw new Exception("UserControl must inherit from IUserControlResult");
            }
            MaskChildForm dlg = new MaskChildForm(parent.Location, parent.Size);
            dlg.AddWithEdit(uc);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                callback?.Invoke(null, new ChildFormArgs
                {
                    Result = child.CallbackResult
                });
            }
            dlg.Dispose();
        }

        public static void ShowInfoDialog(this Form parent, UserControl uc)
        {
            MaskChildForm dlg = new MaskChildForm(parent.Location, parent.Size);
            dlg.AddWithInfo(uc);
            dlg.ShowDialog();
            dlg.Dispose();
        }
    }
	public class MaskChildForm : Form
	{
		private Color _color = Color.Transparent;

		private readonly Color _maskColor = Color.FromArgb(150, 211, 211, 211);

		private Point _parentLocation;

		private Size _parentSize;

		private bool _bStart = false;

		private bool[] _bVisible = null;

		private readonly int _operatorRegionHeight = 40;

		private IContainer components = null;

		private Panel pnlMask;

		private Panel pnlContent;

		private PanelControl pcEdit;

		private TableLayoutPanel tableLayoutPanel2;

		private SimpleButton btnConfirm;

		private SimpleButton btnCancel;

		private PanelControl pcChild;

		private PanelControl pcInfo;

		private TableLayoutPanel tableLayoutPanel1;

		private SimpleButton btnOK;

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= 33554432;
				return cp;
			}
		}

		public MaskChildForm(Point parentLocation, Size parentSize)
		{
			_parentLocation = parentLocation;
			_parentSize = parentSize;
			InitializeComponent();
			pcEdit.Height = _operatorRegionHeight;
			pcInfo.Height = _operatorRegionHeight;
		}

		public void AddWithEdit(UserControl uc)
		{
			pcChild.Controls.Clear();
			pcInfo.Visible = false;
			ShoImp(uc);
		}

		public void AddWithInfo(UserControl uc)
		{
			pcChild.Controls.Clear();
			pcEdit.Visible = false;
			ShoImp(uc);
		}

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == 274)
			{
				byte value = Win32ApiHelper.LOBYTE(m.WParam);
				if (value >= 1 && value <= 8)
				{
					return;
				}
			}
			base.WndProc(ref m);
		}

		private void SetBackgroundImageTransparent()
		{
			Point pt = PointToScreen(new Point(0, 0));
			Bitmap bmp = new Bitmap(base.Width, base.Height);
			using (Graphics g = Graphics.FromImage(bmp))
			{
				g.CopyFromScreen(pt, new Point(0, 0), new Size(base.Width, base.Height));
			}
			BackgroundImage = bmp;
		}

		private void BeginSet()
		{
			_color = base.TransparencyKey;
			_bStart = true;
		}

		private void Setting()
		{
			if (_bStart)
			{
				_bVisible = new bool[base.Controls.Count];
				for (int i = 0; i < base.Controls.Count; i++)
				{
					_bVisible[i] = base.Controls[i].Visible;
					base.Controls[i].Visible = false;
				}
				BackgroundImage = null;
				BackColor = Color.White;
				_bStart = false;
				base.TransparencyKey = Color.White;
			}
		}

		private void EndSet()
		{
			SetBackgroundImageTransparent();
			base.TransparencyKey = _color;
			for (int i = 0; i < base.Controls.Count; i++)
			{
				base.Controls[i].Visible = _bVisible[i];
			}
			_bStart = false;
		}

		private void MaskChildForm_Resize(object sender, EventArgs e)
		{
			Setting();
		}

		private void MaskChildForm_ResizeBegin(object sender, EventArgs e)
		{
			BeginSet();
		}

		private void MaskChildForm_ResizeEnd(object sender, EventArgs e)
		{
			EndSet();
		}

		private void MaskChildForm_Move(object sender, EventArgs e)
		{
			Setting();
		}

		private void LoadingPositonSize(UserControl uc)
		{
			int width = ((uc.Width > 350) ? uc.Width : 350);
			int height = ((uc.Height > 300) ? uc.Height : 300);
			if (uc.Width > _parentSize.Width || uc.Height > _parentSize.Height + _operatorRegionHeight)
			{
				pnlMask.Visible = false;
				BackgroundImage = null;
				base.Size = new Size(width, height + pcEdit.Height);
				base.StartPosition = FormStartPosition.CenterScreen;
				pnlContent.Dock = DockStyle.Fill;
				return;
			}
			pnlMask.Visible = true;
			pnlMask.BackColor = _maskColor;
			base.StartPosition = FormStartPosition.Manual;
			base.Location = _parentLocation;
			base.Size = _parentSize;
			pnlContent.Width = uc.Width;
			pnlContent.Height = uc.Height + _operatorRegionHeight;
			pnlContent.Location = new Point((_parentSize.Width - pnlContent.Width) / 2, (_parentSize.Height - pnlContent.Height) / 2);
			SetBackgroundImageTransparent();
			BeginSet();
		}

		private void ShoImp(UserControl uc)
		{
			LoadingPositonSize(uc);
			pcChild.Controls.Add(uc);
			pcChild.Dock = DockStyle.Fill;
			uc.Dock = DockStyle.Fill;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.pnlMask = new System.Windows.Forms.Panel();
			this.pnlContent = new System.Windows.Forms.Panel();
			this.pcEdit = new DevExpress.XtraEditors.PanelControl();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.btnConfirm = new DevExpress.XtraEditors.SimpleButton();
			this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
			this.pcChild = new DevExpress.XtraEditors.PanelControl();
			this.pcInfo = new DevExpress.XtraEditors.PanelControl();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.btnOK = new DevExpress.XtraEditors.SimpleButton();
			this.pnlContent.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pcEdit).BeginInit();
			this.pcEdit.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.pcChild).BeginInit();
			((System.ComponentModel.ISupportInitialize)this.pcInfo).BeginInit();
			this.pcInfo.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.pnlMask.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlMask.Location = new System.Drawing.Point(0, 0);
			this.pnlMask.Margin = new System.Windows.Forms.Padding(0);
			this.pnlMask.Name = "pnlMask";
			this.pnlMask.Size = new System.Drawing.Size(800, 450);
			this.pnlMask.TabIndex = 2;
			this.pnlContent.Controls.Add(this.pcChild);
			this.pnlContent.Controls.Add(this.pcInfo);
			this.pnlContent.Controls.Add(this.pcEdit);
			this.pnlContent.Location = new System.Drawing.Point(106, 63);
			this.pnlContent.Name = "pnlContent";
			this.pnlContent.Size = new System.Drawing.Size(584, 320);
			this.pnlContent.TabIndex = 0;
			this.pcEdit.Controls.Add(this.tableLayoutPanel2);
			this.pcEdit.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pcEdit.Location = new System.Drawing.Point(0, 280);
			this.pcEdit.Name = "pcEdit";
			this.pcEdit.Size = new System.Drawing.Size(584, 40);
			this.pcEdit.TabIndex = 4;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50f));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50f));
			this.tableLayoutPanel2.Controls.Add(this.btnConfirm, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.btnCancel, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(2, 2);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(580, 36);
			this.tableLayoutPanel2.TabIndex = 0;
			this.btnConfirm.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.btnConfirm.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnConfirm.Location = new System.Drawing.Point(187, 6);
			this.btnConfirm.Name = "btnConfirm";
			this.btnConfirm.Size = new System.Drawing.Size(75, 23);
			this.btnConfirm.TabIndex = 0;
			this.btnConfirm.Text = "OK";
			this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(318, 6);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.pcChild.Dock = System.Windows.Forms.DockStyle.Top;
			this.pcChild.Location = new System.Drawing.Point(0, 0);
			this.pcChild.Name = "pcChild";
			this.pcChild.Size = new System.Drawing.Size(584, 5);
			this.pcChild.TabIndex = 3;
			this.pcInfo.Controls.Add(this.tableLayoutPanel1);
			this.pcInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.pcInfo.Location = new System.Drawing.Point(0, 240);
			this.pcInfo.Name = "pcInfo";
			this.pcInfo.Size = new System.Drawing.Size(584, 40);
			this.pcInfo.TabIndex = 5;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.Controls.Add(this.btnOK, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100f));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20f));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(580, 36);
			this.tableLayoutPanel1.TabIndex = 0;
			this.btnOK.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOK.Location = new System.Drawing.Point(252, 6);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 0;
			this.btnOK.Text = "OK";
			base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 12f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new System.Drawing.Size(800, 450);
			base.ControlBox = false;
			base.Controls.Add(this.pnlContent);
			base.Controls.Add(this.pnlMask);
			this.Cursor = System.Windows.Forms.Cursors.Arrow;
			base.Name = "MaskChildForm";
			base.ResizeBegin += new System.EventHandler(MaskChildForm_ResizeBegin);
			base.ResizeEnd += new System.EventHandler(MaskChildForm_ResizeEnd);
			base.Move += new System.EventHandler(MaskChildForm_Move);
			base.Resize += new System.EventHandler(MaskChildForm_Resize);
			this.pnlContent.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.pcEdit).EndInit();
			this.pcEdit.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.pcChild).EndInit();
			((System.ComponentModel.ISupportInitialize)this.pcInfo).EndInit();
			this.pcInfo.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
