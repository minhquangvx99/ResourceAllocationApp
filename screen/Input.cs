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

namespace ResourceAllocationApp.screen
{
    public partial class Input : Form
    {
        int indexSolution;
        List<string[]> solution= new List<string[]>();
        parameter para;
        public Input()
        {
            InitializeComponent();
        }
        private void showSolution()
        {
            string solution_i="";
            string[] temp = solution[indexSolution];
            for (int i=0; i < temp.Length; i++)
            {
                if (i < para.machines)
                {
                    solution_i += "Machine " + (i + 1).ToString() + ": " + temp[i] + "\r\n";
                }
                else
                {
                    solution_i += "Labor " + (i - para.machines + 1).ToString() + ": " + temp[i] + "\r\n";
                }
            }
            textSolution.Text = solution_i;
        }
        private void btnOpenDataFile_Click(object sender, EventArgs e)
        {
            string fullpath = "";
            OpenFileDialog dlg = new OpenFileDialog();
            textResult.Text = "";
            textSolution.Text = "";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                indexSolution = 0;
                fullpath = dlg.FileName;
                para = new parameter();
                para.process(fullpath);
                individual pop = new individual();
                nash_ga ng = new nash_ga();
                common cm = new common();
                random_Q r = new random_Q();
                pop.randomize(para, r);
                population pp = new population();
                List<individual> pop_init = pp.make_pop(para, r);
                Tuple<individual, Tuple<List<double>, List<double>>> best_allocate = ng.run(para, pop_init, r);
                Tuple<string, List<string[]>> tuple = cm.printPop(best_allocate, para.humans, para.machines);
                textResult.Text = tuple.Item1;
                solution = tuple.Item2;
                showSolution();
            }
            else
            {
                this.Hide();
                Input f = new Input();
                f.ShowDialog();
            }
        }
        private void btnImportData_Click(object sender, EventArgs e)
        {
            this.Hide();
            ImportData f = new ImportData();
            f.ShowDialog();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
