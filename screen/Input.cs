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
        public Input()
        {
            InitializeComponent();

        }
        private void btnOpenDataFile_Click(object sender, EventArgs e)
        {
            string fullpath = "";
            OpenFileDialog dlg = new OpenFileDialog();
            textResult.Text = "";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                fullpath = dlg.FileName;
                parameter para = new parameter();
                para.process(fullpath);
                individual pop = new individual();
                nsga ng = new nsga();
                common cm = new common();
                random_Q r = new random_Q();
                pop.randomize(para, r);
                population pp = new population();
                List<individual> pop_init = pp.make_pop(para, r);
                List<Tuple<individual, Tuple<List<double>, List<double>>>> best_allocate = ng.run(para, pop_init, r);
                textResult.Text = cm.printPop(best_allocate, para.humans, para.machines);
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
