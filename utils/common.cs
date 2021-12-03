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

        public void printPop(List<Tuple<individual, Tuple<List<double>, List<double>>>> populationInfo, int numResourceHuman, int numResourceMachine)
        {
            int x = populationInfo.Count;
            //print(populationInfo)
            //print("\n Done \n Population include {} element ".format(x))
            Console.Write(populationInfo);
            Console.Write("\n Done \n Population include ");
            Console.Write(x);
            Console.Write(" element");
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
                List<int> machine = new List<int>();
                List<int> human = new List<int>();
                if (ind > 0)
                {
                    //int temp;
                    //print("\n solution {}. \n".format(ind))
                    Console.Write("\nsolution ");
                    Console.Write(ind);

                    for (int j = 0; j < ele.t_human_assign.Count; j++)
                    {
                        machine.Add(ele.t_machine_assign[j]);
                        human.Add(ele.t_human_assign[j]);
                        /*
                        temp = ele.t_machine_assign[j];
                        for (int k = 0; temp > 0; k++)
                        {
                            int[] machine_j = (int[])machine[j];
                            machine_j[k] = (temp % 2);
                            machine[j] = machine_j;
                            temp = temp / 2;
                        }
                        //machine.append( "({})".format("{0:b}".format(ele.t_machine_assign[i]).zfill(numResourceMachine)))
                        temp = ele.t_human_assign[j];
                        for (int k = 0; temp > 0; k++)
                        {
                            int[] human_j = (int[])human[j];
                            human_j[k] = (temp % 2);
                            human[j] = human_j;
                            temp = temp / 2;
                        }
                        //human.append("({})".format("{0:b}".format(ele.t_human_assign[i]).zfill(numResourceHuman)))
                        */
                    }
                    /*
                    print("t_m_assign  : {}".format(machine))
    
                    print("t_h_assign  : {}".format(human))
    
                    print("raw_m  :  {}".format(ele.t_machine_assign))
    
                    print("raw_h  :  {}".format(ele.t_human_assign))
                    */
                    Console.Write("\nt_m_assign: ");
                    Console.Write(machine);
                    Console.Write("\nt_h_assign: ");
                    Console.Write(human);
                    Console.Write("\nraw_m: ");
                    Console.Write(ele.t_machine_assign);
                    Console.Write("\nraw_h: ");
                    Console.Write(ele.t_human_assign);

                }
                for (int j = 0; j < numObj; j++)
                {
                    s[j] += ob_constr[j];
                }
            }
            //print("\n Average values : [")
            Console.WriteLine("\n Average values:[");
            for (int i = 0; i < numObj; i++)
            {
                s[i] = s[i]/x;
                //print("{},".format(s[i]));
                Console.WriteLine(s[i]);
            }
            Console.WriteLine("]");
        }
    }
}
