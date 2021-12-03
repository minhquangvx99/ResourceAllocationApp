using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ResourceAllocationApp.daos
{
    class parameter
    {
        public int tasks = 0;
        public int humans = 0;
        public int machines = 0;
        public int skills = 0;
        public int max_t_duration = 0;
        public int sizeD = 0;
        public int machines_type = 0;
        public List<int> h_salary = new List<int>();
        public List<double> m_consuming = new List<double>();
        public List<double> m_prod = new List<double>();
        public List<int> t_duration = new List<int>();
        public ArrayList D = new ArrayList();
        public List<int> t_dependency = new List<int>();
        public ArrayList TREQ = new ArrayList();
        public ArrayList LEXP = new ArrayList();
        public ArrayList MREQ = new ArrayList();
        public ArrayList MEXP = new ArrayList();
        public int[,] valid_human;
        public int[,] valid_machine;
        public void process(string filename)
        {
            StreamReader f = new StreamReader(filename);
            f.ReadLine();
            f.ReadLine();
            tasks = int.Parse(f.ReadLine());
            f.ReadLine();
            humans = int.Parse(f.ReadLine());
            f.ReadLine();
            h_salary = new List<int>(humans);
            string[] s_h_salary = f.ReadLine().Split();
            h_salary = Array.ConvertAll(s_h_salary, int.Parse).OfType<int>().ToList();
            f.ReadLine();
            machines = int.Parse(f.ReadLine());
            f.ReadLine();
            m_consuming = new List<double>(machines);
            string[] s_m_consuming = f.ReadLine().Split();
            m_consuming = Array.ConvertAll(s_m_consuming, double.Parse).OfType<double>().ToList();
            f.ReadLine();
            m_prod = new List<double>(machines);
            string[] s_m_prod = f.ReadLine().Split();
            m_prod = Array.ConvertAll(s_m_prod, double.Parse).OfType<double>().ToList();
            f.ReadLine();
            skills = int.Parse(f.ReadLine());
            f.ReadLine();
            machines_type = int.Parse(f.ReadLine());
            f.ReadLine();

            t_duration = new List<int>(tasks);
            string[] s_t_duration = f.ReadLine().Split();
            t_duration = Array.ConvertAll(s_t_duration, int.Parse).OfType<int>().ToList();
            for (int t = 0; t < t_duration.Count; t++)
            {
                max_t_duration = Math.Max(max_t_duration, t);
            }
            f.ReadLine();
            f.ReadLine();
            sizeD = int.Parse(f.ReadLine());

            t_dependency = new List<int>(2);
            for (int i = 0; i < sizeD; i++)
            {
                string[] s_t_dependency = f.ReadLine().Split();
                t_dependency = Array.ConvertAll(s_t_dependency, int.Parse).OfType<int>().ToList();
                D.Add(t_dependency);
            }
            f.ReadLine();
            for (int i = 0; i < tasks; i++)
            {
                string[] s_temp = f.ReadLine().Split();
                int[] temp = Array.ConvertAll(s_temp, int.Parse);
                TREQ.Add(temp);
            }
            f.ReadLine();
            for (int i = 0; i < humans; i++)
            {
                string[] s_temp = f.ReadLine().Split();
                double[] temp = Array.ConvertAll(s_temp, double.Parse);
                LEXP.Add(temp);
            }
            f.ReadLine();
            for (int i = 0; i < tasks; i++)
            {
                string[] s_temp = f.ReadLine().Split();
                int[] temp = Array.ConvertAll(s_temp, int.Parse);
                MREQ.Add(temp);
            }
            f.ReadLine();
            for (int i = 0; i < humans; i++)
            {
                string[] s_temp = f.ReadLine().Split();
                double[] temp = Array.ConvertAll(s_temp, double.Parse);
                MEXP.Add(temp);
            }
            valid_human = new int[tasks, humans];
            valid_machine = new int[tasks, machines];

            for(int i = 0; i < tasks; i++)
            {
                for (int j = 0; j < humans; j++)
                {
                    valid_human[i,j] = 0;
                }
            }

            for (int i = 0; i < tasks; i++)
            {
                for (int j = 0; j < machines; j++)
                {
                    valid_machine[i, j] = 0;
                }
            }

            for (int i = 0; i < tasks; i++)
            {
                int[] TREQ_i = (int[])TREQ[i];
                int[] MREQ_i = (int[])MREQ[i];
                for (int j = 0; j < humans; j++)
                {
                    double[] LEXP_j = (double[])LEXP[j];
                    double[] MEXP_j = (double[])MEXP[j];
                    for (int k = 0; k < skills; k++)
                    {
                        if (TREQ_i[k] == 1 && LEXP_j[k] > 0)
                        {
                            valid_human[i,j] = 1;
                            break;
                        }
                    }
                    for (int l = 0; l < machines; l++)
                    {
                        if (MREQ_i[l] == 1 && MEXP_j[l] > 0)
                        {
                            valid_machine[i,l]= 1;
                            break;
                        }
                    }
                }
            }
        }
    }
}
