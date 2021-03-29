using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CamadaBLL;

namespace APP
{
    public partial class frmImportarDados : Form
    {
        public frmImportarDados()
        {
            InitializeComponent();
        }
         
        private void button1_Click(object sender, EventArgs e)
        {
            BLLImportarDados objImportarDados = null;

            try
            {
                objImportarDados = new BLLImportarDados();
                objImportarDados.Processar();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
