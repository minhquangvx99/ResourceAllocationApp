using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceAllocationApp.daos;
using ResourceAllocationApp.utils;
using ResourceAllocationApp.screen;
using System.Collections;

namespace ResourceAllocationApp.algorithm
{
    class nash_ga
    {
        const int INFINITY = 10000;
        objectives obj = new objectives();
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> run(parameter para, List<individual> pop_init, random_Q r)
        {
            var population_info = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
            int pop_size = pop_init.Count;
            double Pc = 0.9;
            double Pm = 0.1;
            for (int i = 0; i < pop_size; i++)
            {
                var tuple = new Tuple<individual, Tuple<List<double>, List<double>>>((pop_init[i]), (obj.objectives_constraints(pop_init[i], para)));
                population_info.Add(tuple);
            }
            List<int> S_h = new List<int>();
            List<int> S_m = new List<int>();
            double[,] min = new double[para.tasks, 2];
            for (int i = 0; i < para.tasks; i++)
            {
                S_h.Add(pop_init[0].t_human_assign[i]);
                S_m.Add(pop_init[0].t_machine_assign[i]);
            }
            individual child_ind_max = new individual();
            child_ind_max.set(S_h, S_m);
            var tuple_S_max = new Tuple<individual, Tuple<List<double>, List<double>>>((child_ind_max), (obj.objectives_constraints(child_ind_max, para)));
            List<Tuple<individual, Tuple<List<double>, List<double>>>> new_P;
            int step = 1;
            while (true)
            {
                int check = 1;
                new_P = findNash(population_info, para, ref S_h, ref S_m, ref check, ref tuple_S_max);
                population_info = make_new_pop(population_info, Pc, Pm, para, r);
                if (check == 1)
                {
                    break;
                }
                step++;
            }
            return new_P;
        }
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> findNash(List<Tuple<individual, Tuple<List<double>, List<double>>>> population_info,
            parameter para, ref List<int> S_h, ref List<int> S_m, ref int check, ref Tuple<individual, Tuple<List<double>, List<double>>> tuple_S_max)
        {
            List<individual> ind = new List<individual>();
            int pop_size = population_info.Count;
            for (int i = 0; i < pop_size; i++)
            {
                ind.Add(population_info[i].Item1);
            }
            List<Tuple<individual, Tuple<List<double>, List<double>>>> ans;
            int[] S_temp_h = new int[para.tasks];
            int[] S_temp_m = new int[para.tasks];
            for (int i = 0; i < para.tasks; i++)
            {
                S_temp_h[i] = -1;
                S_temp_m[i] = -1;
            }
            for (int i = 0; i < para.tasks; i++)
            {
                int j_save = -1;
                int temp_S_h = S_h[i];
                int temp_S_m = S_m[i];
                double[] res = new double[2];
                for (int j = 0; j < pop_size; j++)
                {
                    S_h[i] = ind[j].t_human_assign[i];
                    S_m[i] = ind[j].t_machine_assign[i];
                    individual child_ind_res = new individual();
                    child_ind_res.set(S_h, S_m);
                    var tuple_S_res = new Tuple<individual, Tuple<List<double>, List<double>>>((child_ind_res), (obj.objectives_constraints(child_ind_res, para)));
                    List<double> rs = tuple_S_res.Item2.Item1;
                    List<double> max = tuple_S_max.Item2.Item1;
                    if (rs[0] >= max[0] && rs[1] >= max[1] && rs[2] >= max[2])
                    {
                        if(!(rs[0] == max[0] && rs[1] == max[1] && rs[2] == max[2]))
                        {
                            j_save = j; ;
                            tuple_S_max = tuple_S_res;
                        }
                    }
                }
                if (j_save != -1)
                {
                    S_temp_h[i] = ind[j_save].t_human_assign[i];
                    S_temp_m[i] = ind[j_save].t_machine_assign[i];
                    S_h[i] = ind[j_save].t_human_assign[i];
                    S_m[i] = ind[j_save].t_machine_assign[i];
                }
                else
                {
                    S_h[i] = temp_S_h;
                    S_m[i] = temp_S_m;
                }
            }
            for (int i = 0; i < para.tasks; i++)
            {
                if (S_temp_h[i] != -1)
                {
                    check = 0;
                    break;
                }
                if (S_temp_m[i] != -1)
                {
                    check = 0;
                    break;
                }
            }
            ans = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
            ans.Add(tuple_S_max);
            return ans;
        }
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> make_new_pop(List<Tuple<individual, Tuple<List<double>, List<double>>>> population_info, double Pc, double Pm, parameter para, random_Q r)
        {
            //expect Pc + Pm <= 1
            String t_h_bit_string;
            String t_m_bit_string;

            Tuple<List<int>, List<int>> copyParent(individual p_data)
            {
                List<int> t_h_assign = new List<int>();
                List<int> t_m_assign = new List<int>();
                for (int i = 0; i < p_data.t_human_assign.Count; i++)
                {
                    t_h_assign.Add(p_data.t_human_assign[i]);
                    t_m_assign.Add(p_data.t_machine_assign[i]);
                }
                var tuple = new Tuple<List<int>, List<int>>(t_h_assign, t_m_assign);
                return tuple;
            }

            Tuple<int, int> find2DifferentRandomPos(int maxPos)
            {
                int pos1 = r.random_rd(0, maxPos);
                int pos2 = r.random_rd(1, maxPos);
                if (pos2 == pos1)
                {
                    pos2 = 0;
                }
                if (pos1 > pos2)
                {
                    int temp = pos1;
                    pos1 = pos2;
                    pos2 = temp;
                }
                var tuple = new Tuple<int, int>(pos1, pos2);
                return tuple;
            }

            var new_pop_info = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
            int pop_size = population_info.Count;
            List<int> indices = new List<int>();
            for (int i = 0; i < pop_size; i++)
            {
                indices.Add(i);
            }
            List<int> Shuffle(List<int> list)
            {
                int n = list.Count;
                while (n > 1)
                {
                    int k = (r.random_rd(0, n) % n);
                    n--;
                    int value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
                return list;
            }
            indices = Shuffle(indices);
            int numCross = (int)(pop_size * Pc / 2);
            int numMutate = (int)(pop_size * Pm);

            for (int i = 0; i < numCross; i++)
            {
                int ind1 = indices[(2 * i) % pop_size];
                int ind2 = indices[(2 * i + 1) % pop_size];
                individual data1 = population_info[ind1].Item1;
                individual data2 = population_info[ind2].Item1;
                //dont change data1 and data2, make the copy and cross it
                Tuple<List<int>, List<int>> child1 = copyParent(data1);
                List<int> t_h_assign1 = child1.Item1;
                List<int> t_m_assign1 = child1.Item2;
                Tuple<List<int>, List<int>> child2 = copyParent(data2);
                List<int> t_h_assign2 = child2.Item1;
                List<int> t_m_assign2 = child2.Item2;
                int n = t_h_assign1.Count;
                Tuple<int, int> pos1pos2 = find2DifferentRandomPos(n);
                int pos1 = pos1pos2.Item1;
                int pos2 = pos1pos2.Item2;
                for (int pos = pos1; pos < pos2; pos++)
                {
                    //switch t_assign
                    int temp = t_m_assign1[pos];
                    t_m_assign1[pos] = t_m_assign2[pos];
                    t_m_assign2[pos] = temp;
                    int temp2 = t_h_assign1[pos];
                    t_h_assign1[pos] = t_h_assign2[pos];
                    t_h_assign2[pos] = temp2;
                }
                individual child_ind1 = new individual();
                child_ind1.set(t_h_assign1, t_m_assign1);
                var tuple1 = new Tuple<individual, Tuple<List<double>, List<double>>>((child_ind1), (obj.objectives_constraints(child_ind1, para)));
                new_pop_info.Add(tuple1);
                individual child_ind2 = new individual();
                child_ind2.set(t_h_assign2, t_m_assign2);
                var tuple2 = new Tuple<individual, Tuple<List<double>, List<double>>>((child_ind2), (obj.objectives_constraints(child_ind2, para)));
                new_pop_info.Add(tuple2);
            }
            for (int i = 0; i < numMutate; i++)
            {
                int ind = indices[(2 * numCross + i) % pop_size];
                individual data = population_info[ind].Item1; ;
                Tuple<List<int>, List<int>> child = copyParent(data);
                List<int> t_h_assign = child.Item1;
                List<int> t_m_assign = child.Item2;
                int n = t_h_assign.Count;
                int pos = r.random_rd(0, n);
                t_h_assign[pos] = r.random_rd(1, (1 << para.humans));
                t_m_assign[pos] = r.random_rd(1, (1 << para.machines));
                List<int> t_h_bit_assign = new List<int>();
                for (int h = 0; h < para.humans; h++)
                {
                    t_h_bit_assign.Add(para.valid_human[i, h]);
                }
                t_h_bit_string = "";
                for (int j = 0; j < para.humans; j++)
                {
                    t_h_bit_string = t_h_bit_string + "0";
                }
                List<int> t_m_bit_assign = new List<int>();
                for (int m = 0; m < para.machines; m++)
                {
                    t_m_bit_assign.Add(para.valid_machine[i, m]);
                }
                t_m_bit_string = "";
                for (int j = 0; j < para.machines; j++)
                {
                    t_m_bit_string = t_m_bit_string + "0";
                }
                common cm = new common();
                List<string> t_h_bit_string_list = new List<string>();
                if (cm.rand_pos(t_h_bit_assign, r) != -1)
                {
                    for (int j = 0; j < t_h_bit_string.Length; j++)
                    {
                        t_h_bit_string_list.Add(t_h_bit_string[j].ToString());
                    }
                    t_h_bit_string_list[cm.rand_pos(t_h_bit_assign, r)] = "1";
                    t_h_bit_string = "";
                    for (int j = 0; j < t_h_bit_string_list.Count; j++)
                    {
                        t_h_bit_string += t_h_bit_string_list[j];
                    }
                }
                List<string> t_m_bit_string_list = new List<string>();
                if (cm.rand_pos(t_m_bit_assign, r) != -1)
                {
                    for (int j = 0; j < t_m_bit_string.Length; j++)
                    {
                        t_m_bit_string_list.Add(t_m_bit_string[j].ToString());
                    }
                    t_m_bit_string_list[cm.rand_pos(t_m_bit_assign, r)] = "1";
                    t_m_bit_string = "";
                    for (int j = 0; j < t_m_bit_string_list.Count; j++)
                    {
                        t_m_bit_string += t_m_bit_string_list[j];
                    }
                }
                int binaryToInt(string str)
                {
                    int decimalValue = 0;
                    int len = str.Length;
                    for (int k = 0; k < len; k++)
                    {
                        if (str[k] == '1')
                        {
                            decimalValue += (int)Math.Pow(2, len - k - 1);
                        }
                    }
                    return decimalValue;
                }
                t_h_assign[i] = binaryToInt(t_h_bit_string);
                t_m_assign[i] = binaryToInt(t_m_bit_string);
                individual child_ind = new individual();
                child_ind.set(t_h_assign, t_m_assign);
                var tuple = new Tuple<individual, Tuple<List<double>, List<double>>>((child_ind), (obj.objectives_constraints(child_ind, para)));
                new_pop_info.Add(tuple);
            }
            return new_pop_info;
        }
    }
}