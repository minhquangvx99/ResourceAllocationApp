using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ResourceAllocationApp.daos;
using ResourceAllocationApp.algorithm;
using ResourceAllocationApp.utils;
using System.Collections;
using System.IO;

namespace ResourceAllocationApp.screen
{
    public partial class ImportData : Form
    {
        public ImportData()
        {
            InitializeComponent();
        }

        private void btnSaveDataFile_Click(object sender, EventArgs e)
        {
            var filename = textFileName.Text;
            var directory_mydoc = "D:/Tài liệu môn học kì 20211/ĐATN/ResourceAllocationApp/data";
            var fullpath = Path.Combine(directory_mydoc, filename);

            if (File.Exists(fullpath))
            {
                // File đã tồn tại
                MessageBox.Show("FileName đã tồn tại, vui lòng nhập FileName khác. ");
            }
            else
            {
                // tạo mới vì chưa tồn tại file
                string content;
                content = "Data for problem : Optimize assignment and schedule\n-number of task:\n" + textNumberOfTask.Text
                    + "\n-number of human resources:\n" + textNumberOfHuman.Text + "\n-human salary:\n" + textHumanSalary.Text
                    + "\n-number of machine resources:\n" + textNumberOfMachine.Text + "\n-consuming of machine:\n" + textConsumingOfMachine.Text
                    + "\n-productivy of machine:\n" + textProductivyOfMachine.Text + "\n-number of skills:\n" + textNumberOfSkills.Text
                    + "\n-number of machine type\n" + textNumberOfMachineType.Text + "\n-task duration, 1D array\n" + textTaskDuration.Text
                    + "\n-dependency relationship : D = (t_i,t_j)\n-size D:\n" + textD.Text + "\n-TREQ matrix :\n" + textTREQ.Text
                    + "\n-LEXP matrix:\n" + textLEXP.Text + "\n-MREQ matrix:\n" + textMREQ.Text + "\n-MEXP matrix:\n" +textMEXP.Text;
                File.WriteAllText(fullpath, content);
                this.Hide();
                Input f = new Input();
                f.ShowDialog();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Hide();
            Input f = new Input();
            f.ShowDialog();
        }
    }
}
