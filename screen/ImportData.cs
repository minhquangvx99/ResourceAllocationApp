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
            var filename = textFileName.Text.Trim();
            string projectDirectory = Directory.GetParent("ResourceAllocationApp").Parent.FullName;
            string path = projectDirectory.Remove(projectDirectory.Count() - 4, 4) +"/data";
            //string path = projectDirectory + "/ResourceAllocationApp_Nash-GA Setup/data";
            Console.WriteLine(path);
            var fullpath = Path.Combine(path, filename);
            random_Q r = new random_Q();
            if (filename == "") {
                //Chua nhap filename
                MessageBox.Show("Vui lòng nhập FileName. ");
            }
            else if (File.Exists(fullpath))
            {
                // File đã tồn tại
                MessageBox.Show("FileName đã tồn tại, vui lòng nhập FileName khác. ");
            }
            else
            {
                // tạo mới vì chưa tồn tại file
                string content;
                int num_ship = int.Parse(textNumberOfShip.Text);
                int num_tasks = num_ship * 19;
                int num_officers = int.Parse(textNumberOfOfficers.Text);
                int num_workers = int.Parse(textNumberOfWorkers.Text);
                int human_resource = num_officers + num_workers;
                string officers_salary = "", workers_salary = "", human_salary = "";
                for (int i = 0; i < num_officers; i++)
                {
                    officers_salary += textOfficersSalary.Text.ToString() + " ";
                }
                for (int i = 0; i < num_workers; i++)
                {
                    if(i== num_workers - 1)
                    {
                        workers_salary += textWorkersSalary.Text.ToString();
                    }
                    else
                    {
                        workers_salary += textWorkersSalary.Text.ToString() + " ";
                    }
                }
                human_salary = officers_salary + workers_salary;
                string task_durations = "1 1 2 5 2 5 2 2 1 5 2 2 2 1 5 2 5 2 5";
                string task_durations_str = task_durations;
                for (int i = 1; i < num_ship; i++)
                {
                    task_durations_str +=" " + task_durations;
                }
                int sizeD = 18 * num_ship;
                int num_skills = int.Parse(textNumberOfSkills.Text);
                //D = (t_i,t_j)
                string D = "";
                for (int i = 0; i < num_ship; i++)
                {
                    D += (1 + 19 * i).ToString() + " " + (2 + 19 * i).ToString() + "\n";
                    D += (2 + 19 * i).ToString() + " " + (3 + 19 * i).ToString() + "\n";
                    D += (3 + 19 * i).ToString() + " " + (4 + 19 * i).ToString() + "\n";
                    D += (4 + 19 * i).ToString() + " " + (5 + 19 * i).ToString() + "\n";
                    D += (5 + 19 * i).ToString() + " " + (6 + 19 * i).ToString() + "\n";
                    D += (5 + 19 * i).ToString() + " " + (7 + 19 * i).ToString() + "\n";
                    D += (7 + 19 * i).ToString() + " " + (8 + 19 * i).ToString() + "\n";
                    D += (8 + 19 * i).ToString() + " " + (9 + 19 * i).ToString() + "\n";
                    D += (6 + 19 * i).ToString() + " " + (10 + 19 * i).ToString() + "\n";
                    D += (10 + 19 * i).ToString() + " " + (11 + 19 * i).ToString() + "\n";
                    D += (11 + 19 * i).ToString() + " " + (12 + 19 * i).ToString() + "\n";
                    D += (12 + 19 * i).ToString() + " " + (13 + 19 * i).ToString() + "\n";
                    D += (13 + 19 * i).ToString() + " " + (14 + 19 * i).ToString() + "\n";
                    D += (14 + 19 * i).ToString() + " " + (15 + 19 * i).ToString() + "\n";
                    D += (15 + 19 * i).ToString() + " " + (16 + 19 * i).ToString() + "\n";
                    D += (16 + 19 * i).ToString() + " " + (17 + 19 * i).ToString() + "\n";
                    D += (16 + 19 * i).ToString() + " " + (18 + 19 * i).ToString() + "\n";
                    D += (17 + 19 * i).ToString() + " " + (19 + 19 * i).ToString() + "\n";
                }
                //TREQ matrix
                string matrixTREQ = "";
                string[] str_array1 = {"1 1 0 0 0", "1 1 0 0 0", "1 0 0 0 0", "0 0 1 0 0", "0 0 0 1 0", "0 0 0 0 1", "1 0 0 0 0", "1 0 0 0 0", "1 0 0 0 0",
                "0 0 1 0 0", "0 0 0 0 1", "1 0 0 0 0", "1 0 0 0 0", "1 1 0 0 0", "0 0 1 0 0", "0 0 1 0 0", "0 0 0 0 1", "1 0 0 0 0", "0 0 0 0 1" };
                for (int i = 0; i < num_ship; i++)
                {
                    foreach (string j in str_array1)
                    {
                        matrixTREQ += j + "\n";
                    }
                }
                //LEXP matrix
                string matrixLEXP="";
                string[] officer_array = {"0.97 0.86 0.00 0.00 0.00", "0.86 0.97 0.00 0.00 0.00"};
                string[] worker_array = { "0.00 0.00 0.5 0.52 0.9", "0.00 0.00 0.91 0.91 0.95" };
                for(int i = 0; i < num_officers; i++)
                {
                    int temp = r.random_rd(0, 2);
                    matrixLEXP += officer_array[temp] + "\n";
                }
                for (int i = 0; i < num_workers; i++)
                {
                    int temp = r.random_rd(0, 2);
                    matrixLEXP += worker_array[temp] + "\n";
                }
                //MREQ matrix
                string matrixMREQ = "";
                string[] str_array2 = { "1 1 0 0 0", "1 1 0 0 0", "1 0 0 0 0", "0 0 1 0 0", "0 0 0 1 0", "0 0 0 0 0", "1 0 0 0 0", "1 0 0 0 0", "1 0 0 0 0",
                "0 0 1 0 0", "0 0 0 0 0", "1 0 0 0 0", "1 0 0 0 0", "1 1 0 0 0", "0 0 1 0 0", "0 0 1 0 0", "0 0 0 0 0", "1 0 0 0 0", "0 0 0 0 1"};
                for (int i = 0; i < num_ship; i++)
                {
                    foreach (string j in str_array2)
                    {
                        matrixMREQ += j + "\n";
                    }
                }
                //MEXP matrix
                string matrixMEXP = "";
                for (int i = 0; i < num_officers; i++)
                {
                    int temp = r.random_rd(0, 2);
                    matrixMEXP += officer_array[temp] + "\n";
                }
                for (int i = 0; i < num_workers; i++)
                {
                    int temp = r.random_rd(0, 2);
                    matrixMEXP += worker_array[temp] + "\n";
                }
                content = "Data for problem : Optimize assignment and schedule\n-number of task:\n" + num_tasks.ToString()
                    + "\n-number of human resources:\n" + human_resource.ToString() + "\n-human salary:\n" + human_salary
                    + "\n-number of machine resources:\n" + textNumberOfMachines.Text + "\n-consuming of machine:\n" + textConsumingOfMachines.Text
                    + "\n-productivy of machine:\n" + textProductivyOfMachines.Text + "\n-number of skills:\n" + textNumberOfSkills.Text
                    + "\n-number of machine type\n" + textNumberOfMachineType.Text + "\n-task duration, 1D array len= "+ num_tasks.ToString()
                    +"\n" + task_durations_str + "\n-dependency relationship : D = (t_i,t_j)\n-size D:\n" + sizeD.ToString() +"\n" + D 
                    + "-TREQ matrix: size "+ num_tasks.ToString() +"x" +num_skills.ToString()+"\n" + matrixTREQ + "-LEXP matrix: size " 
                    + human_resource.ToString() + "x" + num_skills.ToString() + "\n" + matrixLEXP + "-MREQ matrix: size " + num_tasks.ToString() + "x" 
                    + textNumberOfMachines.Text + "\n" + matrixMREQ + "-MEXP matrix: size " + human_resource.ToString() + "x" 
                    + textNumberOfMachines.Text + "\n" + matrixMEXP;
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
