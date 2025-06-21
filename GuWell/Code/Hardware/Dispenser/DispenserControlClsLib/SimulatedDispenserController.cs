using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DispenserControlClsLib
{
    public class SimulatedDispenserController : IDispenserController
    {
        public bool IsConnect => throw new NotImplementedException();

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public void Disconnect()
        {
            throw new NotImplementedException();
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
