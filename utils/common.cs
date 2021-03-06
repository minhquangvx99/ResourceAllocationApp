using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceAllocationApp.daos;
using ResourceAllocationApp.algorithm;
using System.Collections;

namespace ResourceAllocationApp.utils
{
    class common
    {
        public List<List<int>> get_prev_tasks(parameter para)
        {
            List<List<int>> prev_tasks = new List<List<int>>();
            for (int i = 0; i < para.tasks + 1; i++)
            {
                List<int> temp = new List<int>();
                prev_tasks.Add(temp);
            }
            foreach (List<int> item in para.D)
            {
                List<int> prev_tasks_item1 = prev_tasks[item[1]];
                prev_tasks_item1.Add(item[0]);
                prev_tasks[item[1]] = prev_tasks_item1;
            }
            return prev_tasks;
        }
        public string togger(random_Q r)
        {
            return r.random_rd(0, 2).ToString();
        }

        public int rand_pos(List<int> str, random_Q r)
        {
            List<int> pos_flag = new List<int>();
            for (int i = 0; i < str.Count; i++)
            {
                if (str[i] == 1)
                {
                    pos_flag.Add(i);
                }
            }
            if (pos_flag.Count > 0)
            {
                int k = r.random_rd(0, pos_flag.Count);
                return pos_flag[k];
            }
            return -1;
        }

        public Tuple<string, List<string[]>> printPop(Tuple<individual, Tuple<List<double>, List<double>>> populationInfo, int numResourceHuman, int numResourceMachine)
        {
            string result;
            int numObj = 3;
            List<double> s = new List<double>();
            for (int i = 0; i < numObj; i++)
            {
                s.Add(0);
            }
            List<string[]> solution = new List<string[]>();
            string[] str = new string[numResourceMachine + numResourceHuman];
            for (int q = 0; q < numResourceMachine + numResourceHuman; q++)
            {
                str[q] = "";
            }
            individual ele = populationInfo.Item1;
            List<double> ob_constr = populationInfo.Item2.Item1;
            for (int m = 0; m < ele.t_machine_assign.Count; m++)
            {
                string temp = Convert.ToString(ele.t_machine_assign[m], 2);
                for (int p = 0; p < temp.Length; p++)
                {
                    if (temp[p] == '1')
                    {
                        str[numResourceMachine - temp.Length] += (m + 1).ToString() + ", ";
                    }
                }
            }
            for (int h = 0; h < ele.t_human_assign.Count; h++)
            {
                string temp = Convert.ToString(ele.t_human_assign[h], 2);
                for (int p = 0; p < temp.Length; p++)
                {
                    if (temp[p] == '1')
                    {
                        str[numResourceHuman + numResourceMachine - temp.Length] += (h + 1).ToString() + ", ";
                    }
                }
            }
            solution.Add(str);
            for (int j = 0; j < numObj; j++)
            {
                s[j] += ob_constr[j];
            }
            result = "\r\n]";
            for (int i = numObj - 1; i >= 0; i--)
            {
                s[i] = s[i];
                if (i == 0)
                {
                    result = "\r\n" + s[i].ToString() + result;
                }
                else
                {
                    result = "," + "\r\n" + s[i].ToString() + result;
                }
            }
            result = "Values:[" + result;
            var tuple = new Tuple<string, List<string[]>>(result, solution);
            return tuple;
        }
    }
}
