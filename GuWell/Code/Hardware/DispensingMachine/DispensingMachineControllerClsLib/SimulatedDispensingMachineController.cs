using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispensingMachineControllerClsLib
{
    public class SimulatedDispensingMachineController : IDispensingMachineController
    {
        public bool IsConnect { get { return true; } }

        public void Connect()
        {
            //throw new NotImplementedException();
        }

        public void Connect(SerialPort serial)
        {
            //throw new NotImplementedException();
        }

        public void Disconnect()
        {
            //throw new NotImplementedException();
        }

        public List<decimal> Get(MUSASHICommandenum command, params string[] parameters)
        {
            //throw new NotImplementedException();
            return new List<decimal> { 0 };
        }

        public DispenseRecipeInfo GetRecipeInfo(string recipeName)
        {
            return new DispenseRecipeInfo();
        }

        public List<string> GetRecipeList()
        {
            return new List<string> { "1" };
        }

        public float GetRemainingQuantity()
        {
            return 0f;
        }

        public int GetShotCounter()
        {
            return 1;
        }

        public void LoadRecipe(string recipeName)
        {
            //throw new NotImplementedException();
        }

        public (decimal Time, decimal Pressure, decimal Vacuum) ReadDispensingParameters(int channel)
        {
            throw new NotImplementedException();
        }

        public double ReadDistance()
        {
            return 0d;
        }

        public bool Set(MUSASHICommandenum command, params string[] parameters)
        {
            return true;
        }

        public void Shot()
        {
            //throw new NotImplementedException();
        }

        public void Write()
        {
            //throw new NotImplementedException();
        }

        //DispenseRecipeInfo IDispensingMachineController.GetRecipeInfo(string recipeName)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
