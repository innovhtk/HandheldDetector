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

namespace SDFUploader_Gabo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public string LocalFile { get; private set; }

        private void btSelectSDF_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string file = openFileDialog1.FileName;
                lbLocalFile.SetPropertyThreadSafe(() => lbLocalFile.Text, file);
            }

            if (lbLocalFile.Text != "No se ha seleccionado ningún archivo")
                btSubirLocal.SetPropertyThreadSafe(() => btSubirLocal.Enabled, true);
        }

        private void btSubirLocal_Click(object sender, EventArgs e)
        {
            btSubirLocal.SetPropertyThreadSafe(() => btSubirLocal.Enabled, false);

            LocalFile = lbLocalFile.Text;

            //backgroundWorker1.RunWorkerAsync();
            backgroundWorker1.RunWorkerAsync();
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            UpdateDatabase();
        }
        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Thread.Sleep(1000);
            progressBar1.Invoke((MethodInvoker)delegate
            {
                progressBar1.Visible = false;
            });

            btSubirLocal.SetPropertyThreadSafe(() => btSubirLocal.Enabled, true);
        }

        public void UpdateDatabase()
        {
            try
            {
                SDF sdf = new SDF(LocalFile);
                var data = sdf.Read();
                sdf.UpdateProgress += Sdf_UpdateProgress;
                sdf.Update(data, tbDB.Text, tbUser.Text, tbPwd.Text, tbHost.Text);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void Sdf_UpdateProgress(int percentage)
        {
            backgroundWorker1.ReportProgress(percentage);
        }
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Invoke((MethodInvoker)delegate
            {
                progressBar1.Value = e.ProgressPercentage;
            });
        }

       
    }
}
