using GlobalDataDefineClsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobClsLib
{
    public class RecipeExecuter
    {
        private static readonly object _lockObj = new object();
        private static volatile RecipeExecuter _instance = null;
        public static RecipeExecuter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObj)
                    {
                        if (_instance == null)
                        {
                            _instance = new RecipeExecuter();
                        }
                    }
                }
                return _instance;
            }
        }
        private RecipeExecuter()
        {
        }
        public void ExecuteStep(EnumRecipeStep step, bool isSingleStepRun = false, Action beforeAct = null)
        {
        }
    }
}
