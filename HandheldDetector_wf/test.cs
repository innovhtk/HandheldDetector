using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HandheldDetector_wf
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }

        private void test_Load(object sender, EventArgs e)
        {
            SDF sdf = new SDF(@"C:\Users\eleaz\Desktop\Catalogo_Activos_Etiq.sdf");
            var data = sdf.Read();
            sdf.Update(data, "autobuildHTK-aztecaUM2", "IosUser3", "IU2015!", "localhost");
            dataGridView1.DataSource = data;
        }
    }
}
