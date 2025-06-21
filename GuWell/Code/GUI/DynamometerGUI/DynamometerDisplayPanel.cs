using DynamometerControllerClsLib;
using DynamometerManagerClsLib;
using GlobalToolClsLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamometerGUI
{
    public partial class DynamometerDisplayPanel : UserControl
    {
        private DynamometerManager _DynamometerManager
        {
            get { return DynamometerManager.Instance; }
        }
        private IDynamometerController _DynamometerController
        {
            get
            {
                return _DynamometerManager.GetCurrentHardware();
            }
        }

        private Thread ReadValueThd;
        private bool ReadValueable = true;
        private int Intervaltime = 200;

        private SynchronizationContext _syncContext;

        public DynamometerDisplayPanel()
        {
            InitializeComponent();

            DataModel.Instance.PropertyChanged += DataModel_PropertyChanged;
            _syncContext = SynchronizationContext.Current;

            ReadValueThd = new Thread(ReadValueFor);
        }

        private void DataModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (_syncContext == null)
            {
                return;
            }



            #region SerialPort

            
            if (e.PropertyName == nameof(DataModel.PressureValue1))
            {
                _syncContext.Post(_ => CurrentvalueLable.Text = ((decimal)DataModel.Instance.PressureValue1).ToString(), null);
            }
            if (e.PropertyName == nameof(DataModel.PressureValue2))
            {
                _syncContext.Post(_ => CurrentvalueLable2.Text = ((decimal)DataModel.Instance.PressureValue2).ToString(), null);
            }


            #endregion






        }


        private void ReadValueFor()
        {
            //while(ReadValueable)
            //{
            //    double Value = ReadValue();
            //    ValueShow(Value);
            //    Thread.Sleep(Intervaltime);
            //}
        }

        public double ReadValue()
        {
            try
            {
                if(_DynamometerController.IsConnect)
                {
                    return _DynamometerController.ReadValue();
                }
                else
                {
                    return -1;
                }
            }
            catch
            {
                return -1;
            }
        }

        public void ValueShow(double value)
        {
            if (this.InvokeRequired)
            {

                this.Invoke(new Action<double>(ValueShow), value);
                return;
            }
            if (CurrentvalueLable.InvokeRequired)
            {
                CurrentvalueLable.Invoke(new Action<double>(ValueShow), value);
            }
            else
            {
                CurrentvalueLable.Text = value.ToString("F3");
            }
        }

        /// <summary>
        /// 开始采集
        /// </summary>
        public void StartCollect()
        {
            if(ReadValueThd != null && ReadValueThd.IsAlive)
            {
                ReadValueable = false;
                ReadValueThd.Abort();
            }
            ReadValueThd = new Thread(ReadValueFor);
            ReadValueThd.Start();
            ReadValueable = true;
        }

        /// <summary>
        /// 停止采集
        /// </summary>
        public void StopCollect()
        {
            if (ReadValueThd != null && ReadValueThd.IsAlive)
            {
                ReadValueable = false;
                ReadValueThd.Abort();
            }
        }

    }
}
