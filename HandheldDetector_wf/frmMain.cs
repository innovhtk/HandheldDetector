using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Devices;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Linq.Expressions;
using System.Reflection;

namespace HandheldDetector_wf
{
    public partial class frmMain : Form
    {
        private RemoteDeviceManager mgr;
        private RemoteDevice Dev;
        private string Path;
        private string File;
        private string LocalFile;
        public frmMain()
        {
            InitializeComponent();

            try
            {
                mgr = new RemoteDeviceManager();
                mgr.DeviceConnected += mgr_DeviceConnected;
                mgr.DeviceDisconnected += mgr_DeviceDisconnected;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
            
        }

        private void mgr_DeviceConnected(object sender, RemoteDeviceConnectEventArgs e)
        {
            this.OnConnection(e.Device, true);
        }

        private void mgr_DeviceDisconnected(object sender, RemoteDeviceConnectEventArgs e)
        {
            this.OnConnection(e.Device, false);
        }

        private void OnConnection(RemoteDevice dev, bool connected)
        {
            if (connected)
            {
                lvFolders.Items.Clear();
                try
                {
                    Dev = mgr.Devices.FirstConnectedDevice;
                    //dev = mgr.Devices.FirstConnectedDevice;
                    statusLabel.Text = dev.Name + " está conectado";
                    var dirs = RemoteDirectory.GetDirectories(dev, dev.GetFolderPath(SpecialFolder.MyDocuments));

                    foreach (var dir in dirs)
                    {
                        lvFolders.Items.Add(dir);
                    }
                }
                catch(Exception)
                {
                    lvFolders.Items.Clear();
                }
                this.Visible = true;
            }
            else
            {
                statusLabel.Text = "No hay dispositivos conectados.";
                this.Visible = false;
                lvFiles.Items.Clear();
                lvFolders.Items.Clear();
            }
                
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            //RemoteDevice dev = mgr.Devices.FirstConnectedDevice;
            Dev = mgr.Devices.FirstConnectedDevice;
            OnConnection(Dev, Dev != null);
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            mgr.DeviceConnected -= mgr_DeviceConnected;
            mgr.DeviceDisconnected -= mgr_DeviceDisconnected;
        }

       
        private void lvFolders_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count < 1)
            {
                btCopy.Enabled = false;
                btUpload.Enabled = false;
            }
            try
            {
                //var dev = mgr.Devices.FirstConnectedDevice;
                //Dev = mgr.Devices.FirstConnectedDevice;
                string selected = lvFolders.SelectedItems[0].Text;
                Path = Dev.GetFolderPath(SpecialFolder.MyDocuments) + "\\" + selected;
                var files = RemoteDirectory.GetFiles(Dev, Path );
                lvFiles.Items.Clear();
                foreach (var file in files)
                {
                    RemoteFileInfo info = new RemoteFileInfo(Dev, Path + "\\" + file);
                    ListViewItem item = new ListViewItem(file);
                    item.SubItems.Add(info.LastWriteTime.ToString());
                    lvFiles.Items.Add(item);
                }
            }
            catch (Exception)
            {
                lvFiles.Items.Clear();
            }
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btCopy_Click(object sender, EventArgs e)
        {
            //var dev = mgr.Devices.FirstConnectedDevice;
            //string path = Dev.GetFolderPath(SpecialFolder.MyDocuments);
            //string file = Path + "\\" + lvFiles.SelectedItems[0].Text;
            try
            {
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog1.Filter = "Archivo SDF (*.sdf)|*.sdf|All files (*.*)|*.*";
                saveFileDialog1.FileName = "Catalogo_Activos_Etiq.sdf";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string name = saveFileDialog1.FileName;
                    Cursor.Current = Cursors.WaitCursor;
                    RemoteFile.CopyFileFromDevice(Dev, File, name, false);
                    Cursor.Current = Cursors.Default;
                }
            }
            catch { }
        }
        private string CopyToTemp()
        {
            string name = "";
            try
            {
                name = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".sdf";
                Cursor.Current = Cursors.WaitCursor;
                RemoteFile.CopyFileFromDevice(Dev, File, name, false);
                Cursor.Current = Cursors.Default;
            }
            catch(Exception ex) { MessageBox.Show(ex.Message); }
            return name;
        }
        private void btUpload_Click(object sender, EventArgs e)
        {
            btCopy.SetPropertyThreadSafe(() => btCopy.Enabled, false);
            btUpload.SetPropertyThreadSafe(() => btUpload.Enabled, false);
            
            LocalFile = CopyToTemp();

            Task taskUpdate = Task.Factory.StartNew(UpdateDatabase);

            

        }

        public void UpdateDatabase()
        {
            try
            {
                SDF sdf = new SDF(LocalFile);
                var data = sdf.Read();
                sdf.UpdateProgress += Sdf_UpdateProgress;
                sdf.Update(data, tbDB.Text, tbUser.Text, tbPwd.Text, tbHost.Text);

                Thread.Sleep(1000);
                progressBar1.Invoke((MethodInvoker)delegate
                {
                    progressBar1.Visible = false;
                });

                btCopy.SetPropertyThreadSafe(() => btCopy.Enabled, false);
                btUpload.SetPropertyThreadSafe(() => btUpload.Enabled, false);
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

        private void lvFiles_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if(lvFiles.SelectedItems.Count > 0)
            {
                btCopy.Enabled = true;
                btUpload.Enabled = true;
                File = Path + "\\" + lvFiles.SelectedItems[0].Text;
            }
            else
            {
                btCopy.Enabled = false;
                btUpload.Enabled = false;
            }
        }

        
    }
}
