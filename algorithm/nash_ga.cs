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
            int pop_size = 100;
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
                int index = i % pop_size;
                S_h.Add(pop_init[index].t_human_assign[i]);
                S_m.Add(pop_init[index].t_machine_assign[i]);
                min[i, 0] = obj.countDuration(para, S_h[i], S_m[i], i);
                min[i, 1] = obj.countCost(para, S_h[i], S_m[i], i);
            }
            Tuple<int[,], int[,], int[]> hm = Selection(population_info, para, r, pop_size, min);
            var new_P = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
            int step = 0;
            while (true)
            {
                int check = 1;
                new_P = findNash(hm, para, ref S_h, ref S_m, ref check, ref min);
                population_info = make_new_pop(para, r, pop_size);
                hm = Selection(population_info, para, r, pop_size, min);
                if (check == 1)
                {
                    break;
                }
                step++;
            }
            return new_P;
        }
        public Tuple<int[,], int[,], int[]> Selection(List<Tuple<individual, Tuple<List<double>, List<double>>>> population_info, parameter para, random_Q r, int pop_size, double[,] min)
        {
            List<individual> ind = new List<individual>();
            pop_size = population_info.Count;
            for (int i = 0; i < pop_size; i++)
            {
                ind.Add(population_info[i].Item1);
            }
            int[,] h = new int[para.tasks, pop_size];
            int[,] m = new int[para.tasks, pop_size];
            int[] l = new int[para.tasks];
            for (int i = 0; i < para.tasks; i++)
            {
                l[i] = 0;
                for (int j = 0; j < pop_size; j++)
                {
                    double res_0 = obj.countDuration(para, ind[j].t_human_assign[i], ind[j].t_machine_assign[i], i);
                    double res_1 = obj.countCost(para, ind[j].t_human_assign[i], ind[j].t_machine_assign[i], i);
                    if ((res_1 < min[i, 1] && res_0 < min[i, 0]) || (res_1 == min[i, 1] && res_0 < min[i, 0]) || (res_1 < min[i, 1] && res_0 == min[i, 0]))
                    {
                        h[i,l[i]] = ind[j].t_human_assign[i];
                        m[i, l[i]] = ind[j].t_machine_assign[i];
                        l[i]++;
                    }
                }
            }
            var tuple = new Tuple<int[,], int[,], int[]>(h, m, l);
            return tuple;
        }
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> findNash(Tuple<int[,], int[,], int[]> hm,
            parameter para, ref List<int> S_h, ref List<int> S_m, ref int check, ref double[,] min)
        {
            List<Tuple<individual, Tuple<List<double>, List<double>>>> ans;
            int[] S_temp_h = new int[para.tasks];
            int[] S_temp_m = new int[para.tasks];
            for (int i = 0; i < para.tasks; i++)
            {
                S_temp_h[i] = -1;
                S_temp_m[i] = -1;
            }
            int[,] h = hm.Item1;
            int[,] m = hm.Item2;
            int[] l = hm.Item3;
            for (int i = 0; i < para.tasks; i++)
            {
                int j_save = -1;
                int temp_S_h = S_h[i];
                int temp_S_m = S_m[i];
                double[] res = new double[2];
                for (int j = 0; j < l[i]; j++)
                {
                    S_h[i] = h[i, j];
                    S_m[i] = m[i, j];
                    res[0] = obj.countDuration(para, S_h[i], S_m[i], i);
                    res[1] = obj.countCost(para, S_h[i], S_m[i], i);
                    if ((res[1] < min[i, 1] && res[0] < min[i, 0]) || (res[1] == min[i, 1] && res[0] < min[i, 0]) || (res[1] < min[i, 1] && res[0] == min[i, 0]))
                    {
                        j_save = j;
                        min[i, 0] = res[0];
                        min[i, 1] = res[1];
                    }
                }
                if (j_save != -1)
                {
                    S_temp_h[i] = h[i,j_save];
                    S_temp_m[i] = m[i, j_save];
                    S_h[i] = h[i,j_save];
                    S_m[i] = m[i, j_save];
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
            individual child_ind_temp = new individual();
            child_ind_temp.set(S_h, S_m);
            var tuple_S = new Tuple<individual, Tuple<List<double>, List<double>>>((child_ind_temp), (obj.objectives_constraints(child_ind_temp, para)));
            ans.Add(tuple_S);
            return ans;
        }
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> make_new_pop(parameter para, random_Q r, int pop_size)
        {
            var population_info = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
            population pp = new population();
            List<individual> pop_init = pp.make_pop(para, r);
            for (int i = 0; i < pop_size; i++)
            {
                var tuple = new Tuple<individual, Tuple<List<double>, List<double>>>((pop_init[i]), (obj.objectives_constraints(pop_init[i], para)));
                population_info.Add(tuple);
            }
            return population_info;
        }
    }
}