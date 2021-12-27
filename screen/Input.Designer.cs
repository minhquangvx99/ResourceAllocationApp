
namespace ResourceAllocationApp.screen
{
    partial class Input
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelSolution = new System.Windows.Forms.Label();
            this.textSolution = new System.Windows.Forms.TextBox();
            this.textResult = new System.Windows.Forms.TextBox();
            this.btnImportData = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnOpenDataFile = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.panel1.BackgroundImage = global::ResourceAllocationApp.Properties.Resources.icon_RA;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.labelSolution);
            this.panel1.Controls.Add(this.textSolution);
            this.panel1.Controls.Add(this.textResult);
            this.panel1.Controls.Add(this.btnImportData);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.btnOpenDataFile);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 496);
            this.panel1.TabIndex = 4;
            // 
            // labelSolution
            // 
            this.labelSolution.AutoSize = true;
            this.labelSolution.Location = new System.Drawing.Point(176, 161);
            this.labelSolution.Name = "labelSolution";
            this.labelSolution.Size = new System.Drawing.Size(77, 20);
            this.labelSolution.TabIndex = 10;
            this.labelSolution.Text = "Solution";
            // 
            // textSolution
            // 
            this.textSolution.Location = new System.Drawing.Point(180, 184);
            this.textSolution.Multiline = true;
            this.textSolution.Name = "textSolution";
            this.textSolution.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textSolution.Size = new System.Drawing.Size(593, 284);
            this.textSolution.TabIndex = 7;
            // 
            // textResult
            // 
            this.textResult.Location = new System.Drawing.Point(180, 51);
            this.textResult.Multiline = true;
            this.textResult.Name = "textResult";
            this.textResult.Size = new System.Drawing.Size(593, 107);
            this.textResult.TabIndex = 6;
            // 
            // btnImportData
            // 
            this.btnImportData.BackColor = System.Drawing.Color.Blue;
            this.btnImportData.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImportData.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnImportData.Location = new System.Drawing.Point(11, 119);
            this.btnImportData.Name = "btnImportData";
            this.btnImportData.Size = new System.Drawing.Size(145, 50);
            this.btnImportData.TabIndex = 5;
            this.btnImportData.Text = "Import Data";
            this.btnImportData.UseVisualStyleBackColor = false;
            this.btnImportData.Click += new System.EventHandler(this.btnImportData_Click);
            // 
            // btnExit
            // 
            this.btnExit.Image = global::ResourceAllocationApp.Properties.Resources.icon_exit;
            this.btnExit.Location = new System.Drawing.Point(757, 3);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(38, 31);
            this.btnExit.TabIndex = 4;
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnOpenDataFile
            // 
            this.btnOpenDataFile.BackColor = System.Drawing.Color.Blue;
            this.btnOpenDataFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenDataFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnOpenDataFile.Location = new System.Drawing.Point(11, 51);
            this.btnOpenDataFile.Name = "btnOpenDataFile";
            this.btnOpenDataFile.Size = new System.Drawing.Size(145, 50);
            this.btnOpenDataFile.TabIndex = 3;
            this.btnOpenDataFile.Text = "Open DataFile";
            this.btnOpenDataFile.UseVisualStyleBackColor = false;
            this.btnOpenDataFile.Click += new System.EventHandler(this.btnOpenDataFile_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(29, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 20);
            this.label2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DodgerBlue;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(526, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data for problem : Optimize assignment and schedule";
            // 
            // Input
            // 
            this.ClientSize = new System.Drawing.Size(800, 496);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Input";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Input";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOpenDataFile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnImportData;
        private System.Windows.Forms.TextBox textResult;
        private System.Windows.Forms.TextBox textSolution;
        private System.Windows.Forms.Label labelSolution;
    }
}

