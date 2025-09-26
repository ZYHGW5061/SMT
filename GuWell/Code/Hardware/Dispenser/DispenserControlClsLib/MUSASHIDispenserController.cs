using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispenserControlClsLib
{
    public class MUSASHIDispenserController : IDispenserController
    {
        private bool _isConnected;
        public bool IsConnect { get { return _isConnected; } }

        public void Connect()
        {

        }

        public void Disconnect()
        {

        }

        public DispenseRecipeInfo GetRecipeInfo(string recipeName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetRecipeList()
        {
            throw new NotImplementedException();
        }

        public float GetRemainingQuantity()
        {
            throw new NotImplementedException();
        }

        public int GetShotCounter()
        {
            throw new NotImplementedException();
        }

        public void LoadRecipe(string recipeName)
        {
            throw new NotImplementedException();
        }

        public void Shot()
        {
            throw new NotImplementedException();
        }
    }
}
