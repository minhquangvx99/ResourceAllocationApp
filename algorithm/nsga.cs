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
    class nsga
    {
        const int INFINITY = 10000;
        objectives obj = new objectives();
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> run(parameter para, List<individual> pop_init, random_Q r)
        {
            var population_info = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
            int pop_size = 100;
            double Pc = 0.9;
            double Pm = 0.1;
            int max_gen = 100;
            for(int i = 0; i < pop_size; i++){
                var tuple = new Tuple<individual, Tuple<List<double>, List<double>>>((pop_init[i]), (obj.objectives_constraints(pop_init[i], para)));
                population_info.Add(tuple);
            }
            var P = new List<List<Tuple<individual, Tuple<List<double>, List<double>>>>>();
            P.Add(population_info);
            var Q = new List<List<Tuple<individual, Tuple<List<double>, List<double>>>>>();
            for (int t = 0; t < max_gen; t++)
            {
                var Rt = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
                if (t < Q.Count)
                {
                    Rt = P[t].Concat(Q[t]).ToList();
                }
                else
                {
                    Rt = P[t];
                }
                var new_P = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
                //new_P = selection(Rt, pop_size);
                new_P = findNash(Rt);
                P.Add(new_P);
                Q.Add(make_new_pop(new_P, Pc, Pm, para,r));
            }
            return P[max_gen];
        }
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> findNash(List<Tuple<individual, Tuple<List<double>, List<double>>>> population_info)
        {
            var ans = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();
            ArrayList objects = new ArrayList();
            for (int i = 0; i < population_info.Count; i++)
            {
                objects.Add(population_info[i].Item2.Item1.ToArray());
            }
            double[] objects_0 = (double[])objects[0];
            int m = objects_0.Length;
            double[] min = objects_0;
            int min_i = 0;
            int pop_size = objects.Count;
            for (int i = 1; i < pop_size; i++)
            {
                double[] objects_i = (double[])objects[i];
                for (int t = 0; t < m; t++)
                {
                    if (objects_i[t] > min[t])
                    {
                        min = objects_i;
                        min_i = i;
                    }
                }
            }
            ans.Add(population_info[min_i]);
            return ans;
        }
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> selection(List<Tuple<individual, Tuple<List<double>, List<double>>>> population_info, int pop_size)
        {
            var obj_constrs = new List<Tuple<List<double>, List<double>>>();
            for (int i = 0; i < population_info.Count; i++)
            {
                obj_constrs.Add(population_info[i].Item2);
            }
            List<int[]> F=new List<int[]>();
            F = fast_nondominated_sort(obj_constrs);
            int n_front = F.Count;
            List<int> result = new List<int>();
            int current_len = result.Count;
            if (current_len < pop_size)
            {
                int[] F_i = (int[])F[0];
                result.AddRange(crowding_distance_selection(obj_constrs, F_i, pop_size - current_len));
            }
            var ans = new List<Tuple<individual, Tuple<List<double>, List<double>>>>();

            foreach (int index in result)
            {
                ans.Add(population_info[index]);
            }
            return ans;
        }

        public List<int[]> fast_nondominated_sort(List<Tuple<List<double>, List<double>>> obj_constr)
        {
            //obj_constr[i] contain all objective and constraint value of i-th element
            //return the indices after sorted by front
            //F[i-1] is the indices of i-th non-dominated front of population
            int size = obj_constr.Count;
            //n[i-1] : number of current element dominate the i-th element
            int[] n= new int[size];
            for(int i = 0; i < size; i++)
            {
                n[i] = 0;
            }
            List<int[]> S = new List<int[]>();
            List<int> first_front = new List<int>();
            for (int i = 0; i < size; i++)
            {
                List<int> S_i_list = new List<int>();
                for (int j = 0; j < size; j++)
                {
                    if(i == j)
                    {
                        continue;
                    }
                    int comp = get_dominate_state(obj_constr[i], obj_constr[j]);
                    if (comp == 1)
                    {
                        S_i_list.Add(j);
                    }
                    else if(comp == -1)
                    {
                        n[i] += 1;
                    }
                }
                S.Add(S_i_list.ToArray());
                if(n[i] == 0)
                {
                    first_front.Add(i);
                }
            }

            List<int[]> F = new List<int[]>();
            int ind = 0;
            List<int> next_front = first_front;
            while (next_front.Count > 0)
            {
                F.Add(next_front.ToArray());
                next_front = new List<int>();
                int[] F_ind = (int[])F[ind];
                foreach(int i in F_ind)
                {
                    int[] S_i = (int[])S[i];
                    foreach (int j in S_i)
                    {
                        n[j] -= 1;
                        if (n[j] == 0)
                        {
                            next_front.Add(j);
                        }
                    }
                }
                ind += 1;
            }
            return F;
        }
        public int get_dominate_state(Tuple<List<double>, List<double>> obj_constr1, Tuple<List<double>, List<double>> obj_constr2)
        {
            //1 dominate 2 : return 1
            //2 dominate 1 : return -1
            //ono-dominate : return 0
            List<double> obj1, obj2;
            obj1 = obj_constr1.Item1;
            obj2 = obj_constr2.Item1;
            bool first_dominate = false;
            bool second_dominate = false;
            for (int i = 0; i < obj1.Count; i++) {
                if (obj1[i] > obj2[i])
                {
                    first_dominate = true;
                }

                if(obj2[i] > obj1[i])
                {
                    second_dominate = true;
                }
            }
            if (first_dominate == true && second_dominate == false)
            {
                return 1;
            }
            if(second_dominate == true && first_dominate == false)
            {
                return -1;
            }
            return 0;
        }


        public int[] crowding_distance_selection(List<Tuple<List<double>, List<double>>> obj_constrs, int[] front_ind, int max_element)
        {
            //select max_element best element
            if (front_ind.Length <= max_element)
            {
                return front_ind;
            }
            //sort and select
            var obj_constrs_front = new List<Tuple<List<double>, List<double>>>();
            int cnt = 0;
            int[] mapping = new int[front_ind.Length];
            foreach (int index in front_ind)
            {
                obj_constrs_front.Add(obj_constrs[index]);
                mapping[cnt] = index;
                cnt += 1;
            }
            List<int> sorted_ind = sort_by_crowding_distance(obj_constrs_front);
            int[] ans = new int[max_element];
            for(int i=0;i< max_element; i++)
            {
                ans[i]=mapping[sorted_ind[i]];
            }
            return ans;
        }
        public List<int> sort_by_crowding_distance(List<Tuple<List<double>, List<double>>> obj_constr)
        {
            //obj_constr[i] contain all objective and constraint value of i-th element
            //return the indices after sorted by crowing distance
            //TODO : implement
            ArrayList objects = new ArrayList();
            ArrayList constraints = new ArrayList();
            List<int> sortedIndices = new List<int>();
            for (int i = 0; i < obj_constr.Count; i++)
            {
                objects.Add(obj_constr[i].Item1.ToArray());
                constraints.Add(obj_constr[i].Item2.ToArray());
            }
            double[] objects_0 = (double[])objects[0];
            int m = objects_0.Length;
            int pop_size = objects.Count;
            double[] distance = new double[pop_size];
            for (int i = 0; i < pop_size; i++)
            {
                distance[i] = 0;
            }
            for (int t = 0; t < m; t++)
            {
                //init array 0,1,2,..,pop_size
                for (int i = 0; i <pop_size; i++)
                {
                    sortedIndices.Add(i);
                }
                List<int> quicksort_increase(List<int> arr, int leftIndex, int rightIndex)
                {

                    int i = leftIndex;

                    int j = rightIndex;

                    int pivot = arr[(leftIndex + rightIndex) / 2];

                    while (i <= j)
                    {
                        double[] objects_arr = (double[])objects[arr[i]];
                        double[] objects_pivot = (double[])objects[pivot];
                        while (objects_arr[t] < objects_pivot[t])
                        {
                            i++;
                            objects_arr = (double[])objects[arr[i]];
                            objects_pivot = (double[])objects[pivot];
                        }

                        objects_arr = (double[])objects[arr[j]];
                        objects_pivot = (double[])objects[pivot];
                        while (objects_arr[t] > objects_pivot[t])
                        {
                            j--;
                            objects_arr = (double[])objects[arr[j]];
                            objects_pivot = (double[])objects[pivot];
                        }

                        if (i <= j)
                        {

                            int temp = arr[i];

                            arr[i] = arr[j];

                            arr[j] = temp;

                            i++; j--;

                        }

                        if (i < rightIndex) quicksort_increase(arr, i, rightIndex);

                        if (j > leftIndex) quicksort_increase(arr, leftIndex, j);

                    }
                    return arr;
                }
                sortedIndices = quicksort_increase(sortedIndices, 0, pop_size);

                distance[sortedIndices[0]] += INFINITY;
                distance[sortedIndices[pop_size - 1]] += INFINITY;
                for (int i = 1; i < pop_size - 1; i++)
                {
                    double[] objects_1 = (double[])objects[sortedIndices[i + 1]];
                    double[] objects_2 = (double[])objects[sortedIndices[i - 1]];

                    distance[sortedIndices[i]] += objects_1[t] + objects_2[t];
                }
            }
            for (int i = 0; i < pop_size; i++)
            {
                sortedIndices.Add(i);
            }
            List<int> quicksort_decrease(List<int> arr, int leftIndex, int rightIndex)
            {

                int i = leftIndex;

                int j = rightIndex;

                int pivot = arr[(leftIndex + rightIndex) / 2];

                while (i <= j)
                {
                    while (distance[i] > distance[pivot])
                    {
                        i++;
                    }
                    while (distance[j] < distance[pivot])
                    {
                        j--;
                    }
                    if (i <= j)
                    {

                        int temp = arr[i];

                        arr[i] = arr[j];

                        arr[j] = temp;

                        i++; j--;

                    }
                    if (i < rightIndex) quicksort_decrease(arr, i, rightIndex);
                    if (j > leftIndex) quicksort_decrease(arr, leftIndex, j);
                }
                return arr;
            }
            sortedIndices = quicksort_decrease(sortedIndices, 0, pop_size);
            return sortedIndices;
        }
        public List<Tuple<individual, Tuple<List<double>, List<double>>>> make_new_pop(List<Tuple<individual, Tuple<List<double>, List<double>>>> population_info, double Pc, double Pm, parameter para, random_Q r)
        {
            //expect Pc + Pm <= 1
            String t_h_bit_string;
            String t_m_bit_string;

            Tuple<List<int>, List<int>> copyParent(individual p_data) {
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

            for (int i = 0; i < numCross; i++) {
                int ind1 = indices[(2 * i) % pop_size];
                int ind2 = indices[(2 * i + 1) % pop_size];
                individual data1 = population_info[ind1].Item1;
                individual data2 = population_info[ind2].Item1;
                //dont change data1 and data2, make the copy and cross it
                Tuple<List<int>, List<int>> child1 = copyParent(data1);
                List<int>  t_h_assign1 = child1.Item1;
                List<int> t_m_assign1 = child1.Item2;
                Tuple<List<int>, List<int>> child2 = copyParent(data2);
                List<int> t_h_assign2 = child2.Item1;
                List<int> t_m_assign2 = child2.Item2;
                int n = t_h_assign1.Count;
                Tuple<int, int> pos1pos2 = find2DifferentRandomPos(n);
                int pos1 = pos1pos2.Item1;
                int pos2 = pos1pos2.Item2;
                for (int pos = pos1; pos < pos2; pos++) {
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
            for (int i = 0; i < numMutate; i++) {
                int ind = indices[(2 * numCross + i) % pop_size];
                individual data= population_info[ind].Item1;;
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
