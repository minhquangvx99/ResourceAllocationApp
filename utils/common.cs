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
            for(int i=0; i < para.tasks+1; i++)
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
            for (int i = 0; i < str.Count; i++) {
                if (str[i] == 1)
                {
                    pos_flag.Add(i);
                }
            }
            if(pos_flag.Count > 0)
            {
                int k = r.random_rd(0,pos_flag.Count);
                return pos_flag[k];
            }
            return -1;
        }

        public string printPop(List<Tuple<individual, Tuple<List<double>, List<double>>>> populationInfo, int numResourceHuman, int numResourceMachine)
        {
            string result;
            int x = populationInfo.Count;
            result = "\r\nPopulation include " + x.ToString() + " element";
            int ind = 0;
            int numObj = 3;
            List<double> s = new List<double>();
            for (int i=0;i< numObj; i++)
            {
                s.Add(0);
            }
            for (int i = 0; i < x; i++) {
                individual ele = populationInfo[i].Item1;
                List<double> ob_constr = populationInfo[i].Item2.Item1;
                ind += 1;
                if (ind > 0)
                {
                    result += "\r\nsolution " + ind.ToString();
                    result += "\r\nraw_m: [";
                    for (int m = 0; m < ele.t_machine_assign.Count; m++)
                    {
                        if (m == ele.t_machine_assign.Count - 1)
                        {
                            result += ele.t_machine_assign[m].ToString();
                        }
                        else
                        {
                            result += ele.t_machine_assign[m].ToString() + ", ";
                        }

                    }
                    result += "]";
                    result += "\r\nraw_h: [";
                    for (int h = 0; h < ele.t_human_assign.Count; h++)
                    {
                        if(h== ele.t_human_assign.Count - 1)
                        {
                            result += ele.t_human_assign[h].ToString() ;
                        }
                        else
                        {
                            result += ele.t_human_assign[h].ToString() + ", ";
                        }
                    }
                    result += "]";

                }
                for (int j = 0; j < numObj; j++)
                {
                    s[j] += ob_constr[j];
                }
            }
            result = "\r\n]" + result;
            for (int i = numObj-1; i >= 0; i--)
            {
                s[i] = s[i]/x;
                if (i == 0)
                {
                    result = "\r\n" + s[i].ToString() + result;
                }
                else
                {
                    result = "," + "\r\n" + s[i].ToString() + result;
                }
            }
            result = "Average values:[" + result;
            return result;
        }
    }
}
