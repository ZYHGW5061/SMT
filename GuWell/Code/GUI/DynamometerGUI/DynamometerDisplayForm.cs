using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamometerGUI
{
    public partial class DynamometerDisplayForm : Form
    {
        public DynamometerDisplayForm()
        {
            InitializeComponent();

            //StartCollect();
        }

        ~DynamometerDisplayForm()
        {

        }

        /// <summary>
        /// 开始采集
        /// </summary>
        public void StartCollect()
        {
            //dynamometerDisplayPanel1.StartCollect();
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopCollect()
        {
            //dynamometerDisplayPanel1.StopCollect();
        }

        public void ShowLocation(Point? location = null)
        {
            if (location.HasValue)
            {
                this.StartPosition = FormStartPosition.Manual;
                this.Location = location.Value;
            }
        }

        private void DynamometerDisplayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //StopCollect();
        }

        public double ReadValue()
        {
            try
            {
                double value = dynamometerDisplayPanel1.ReadValue();
                dynamometerDisplayPanel1.ValueShow(value);
                return value;
            }
            catch
            {
                return -1;
            }
        }

        public void ValueShow(double value)
        {
            dynamometerDisplayPanel1.ValueShow(value);
        }

    }
}
