﻿using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaserSensorControllerClsLib
{
    public class SimulatedLaserSensorController : ILaserSensorController
    {
        public bool IsConnect { get { return true; } }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Connect(SerialPort serial)
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
        }

        public double ReadDistance()
        {
            throw new NotImplementedException();
        }

        public void Write()
        {
            throw new NotImplementedException();
        }
    }
}
