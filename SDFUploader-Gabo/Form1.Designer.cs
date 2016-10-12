namespace SDFUploader_Gabo
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbLocalFile = new System.Windows.Forms.TextBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btSubirLocal = new System.Windows.Forms.Button();
            this.btSelectSDF = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.tbUser = new System.Windows.Forms.TextBox();
            this.tbDB = new System.Windows.Forms.TextBox();
            this.tbHost = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbLocalFile
            // 
            this.lbLocalFile.Location = new System.Drawing.Point(5, 18);
            this.lbLocalFile.Name = "lbLocalFile";
            this.lbLocalFile.Size = new System.Drawing.Size(397, 20);
            this.lbLocalFile.TabIndex = 9;
            this.lbLocalFile.Text = "No se ha seleccionado ningún archivo";
            this.lbLocalFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(5, 117);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(310, 23);
            this.progressBar1.TabIndex = 8;
            // 
            // btSubirLocal
            // 
            this.btSubirLocal.Enabled = false;
            this.btSubirLocal.Location = new System.Drawing.Point(320, 117);
            this.btSubirLocal.Name = "btSubirLocal";
            this.btSubirLocal.Size = new System.Drawing.Size(82, 25);
            this.btSubirLocal.TabIndex = 7;
            this.btSubirLocal.Text = "Subir";
            this.btSubirLocal.UseVisualStyleBackColor = true;
            this.btSubirLocal.Click += new System.EventHandler(this.btSubirLocal_Click);
            // 
            // btSelectSDF
            // 
            this.btSelectSDF.Location = new System.Drawing.Point(97, 55);
            this.btSelectSDF.Name = "btSelectSDF";
            this.btSelectSDF.Size = new System.Drawing.Size(204, 43);
            this.btSelectSDF.TabIndex = 6;
            this.btSelectSDF.Text = "Selecionar SDF";
            this.btSelectSDF.UseVisualStyleBackColor = true;
            this.btSelectSDF.Click += new System.EventHandler(this.btSelectSDF_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker1_RunWorkerCompleted);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(421, 179);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lbLocalFile);
            this.tabPage1.Controls.Add(this.btSelectSDF);
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.btSubirLocal);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(413, 153);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SDF";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.tbPwd);
            this.tabPage2.Controls.Add(this.tbUser);
            this.tabPage2.Controls.Add(this.tbDB);
            this.tabPage2.Controls.Add(this.tbHost);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(413, 153);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Configuración";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(48, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Contraseña:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(66, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Usuario:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(34, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Base de datos:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(80, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Host:";
            // 
            // tbPwd
            // 
            this.tbPwd.Location = new System.Drawing.Point(127, 107);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.Size = new System.Drawing.Size(252, 20);
            this.tbPwd.TabIndex = 12;
            this.tbPwd.Text = "IU2015!";
            // 
            // tbUser
            // 
            this.tbUser.Location = new System.Drawing.Point(127, 80);
            this.tbUser.Name = "tbUser";
            this.tbUser.Size = new System.Drawing.Size(252, 20);
            this.tbUser.TabIndex = 11;
            this.tbUser.Text = "IosUser3";
            // 
            // tbDB
            // 
            this.tbDB.Location = new System.Drawing.Point(127, 53);
            this.tbDB.Name = "tbDB";
            this.tbDB.Size = new System.Drawing.Size(252, 20);
            this.tbDB.TabIndex = 10;
            this.tbDB.Text = "autobuildHTK-doihiFZ";
            // 
            // tbHost
            // 
            this.tbHost.Location = new System.Drawing.Point(127, 26);
            this.tbHost.Name = "tbHost";
            this.tbHost.Size = new System.Drawing.Size(252, 20);
            this.tbHost.TabIndex = 9;
            this.tbHost.Text = "192.168.25.123";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(421, 179);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "SDF Uploader";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox lbLocalFile;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btSubirLocal;
        private System.Windows.Forms.Button btSelectSDF;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.TextBox tbUser;
        private System.Windows.Forms.TextBox tbDB;
        private System.Windows.Forms.TextBox tbHost;
    }
}

