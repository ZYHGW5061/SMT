using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BonderGUI.UserControls.Programs
{
    public partial class PressureChart : UserControl
    {
        private Queue<double> dataQueue = new Queue<double>(100);

        public PressureChart()
        {
            InitializeComponent();
            initQueue();
            initChart();
        }

        public void updateQueueValue()
        {
            if (dataQueue.Count > 100)
            {
                dataQueue.Dequeue();
            }

            Random r = new Random();
            dataQueue.Enqueue(r.Next(50, 70));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.updateQueueValue();
            this.chart1.Series[0].Points.Clear();
            for(int i=0; i<dataQueue.Count; i++)
            {
                this.chart1.Series[0].Points.AddXY(i + 1, dataQueue.ElementAt(i));
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void initQueue()
        {
            for(int i=0; i<100; i++)
            {
                dataQueue.Enqueue(0);
            }
        }

        private void initChart()
        {
            this.chart1.Series[0].Points.Clear();
            for (int i = 0; i < dataQueue.Count; i++)
            {
                this.chart1.Series[0].Points.AddXY(i + 1, dataQueue.ElementAt(i));
            }
        }
    }
}
