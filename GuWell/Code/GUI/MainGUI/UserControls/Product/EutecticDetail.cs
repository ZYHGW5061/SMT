using GlobalDataDefineClsLib;
using System.Windows.Forms;

namespace MainGUI.UserControls.Product
{
    public partial class EuticticDetail : UserControl
    {
        public EuticticDetail()
        {
            InitializeComponent();
        }

        public void fillEutecticDetail(EutecticConfig eutecticConf)
        {
            if (eutecticConf == null)
            {
                this.teEutecticName.Text = null;
                return;
            }

            this.teEutecticName.Text = eutecticConf.ConfigName;
        }
    }
}
