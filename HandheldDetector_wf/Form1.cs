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

namespace HandheldDetector_wf
{
    public partial class Form1 : Form
    {
        private RemoteDeviceManager mgr;

        public Form1()
        {
            InitializeComponent();
            mgr = new RemoteDeviceManager();
            mgr.DeviceConnected += mgr_DeviceConnected;
            mgr.DeviceDisconnected += mgr_DeviceDisconnected;
            
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
                    dev = mgr.Devices.FirstConnectedDevice;
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

        private void Form1_Load(object sender, EventArgs e)
        {
            RemoteDevice dev = mgr.Devices.FirstConnectedDevice;
            OnConnection(dev, dev != null);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            mgr.DeviceConnected -= mgr_DeviceConnected;
            mgr.DeviceDisconnected -= mgr_DeviceDisconnected;
        }

       
        private void lvFolders_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                var dev = mgr.Devices.FirstConnectedDevice;
                string selected = lvFolders.SelectedItems[0].Text;
                string path = dev.GetFolderPath(SpecialFolder.MyDocuments) + "\\" + selected;
                var files = RemoteDirectory.GetFiles(dev, path);
                lvFiles.Items.Clear();
                foreach (var file in files)
                {
                    RemoteFileInfo info = new RemoteFileInfo(dev, path + "\\" + file);
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
            var dev = mgr.Devices.FirstConnectedDevice;
            string path = dev.GetFolderPath(SpecialFolder.MyDocuments);
            string file = path + "\\" + lvFolders.SelectedItems[0].Text + "\\" + lvFiles.SelectedItems[0].Text;
            try
            {
                saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                saveFileDialog1.Filter = "Archivo SDF (*.sdf)|*.sdf|All files (*.*)|*.*";
                saveFileDialog1.FileName = "Catalogo_Activos_Etiq.sdf";

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string name = saveFileDialog1.FileName;
                    Cursor.Current = Cursors.WaitCursor;
                    RemoteFile.CopyFileFromDevice(dev, file, name, false);
                    Cursor.Current = Cursors.Default;
                }
            }
            catch { }
        }
    }
}
