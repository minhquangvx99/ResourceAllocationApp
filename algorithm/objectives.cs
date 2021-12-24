using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceAllocationApp.daos;
using ResourceAllocationApp.utils;
using System.Collections;

namespace ResourceAllocationApp.algorithm
{
    class objectives
    {
        common cm = new common();
        public double f_duration(individual ind, parameter para)
        {
            List<List<int>> prev_tasks = new List<List<int>>();
            prev_tasks = cm.get_prev_tasks(para);
            int[] t_start = new int[para.tasks];
            int[] t_finish = new int[para.tasks];
            int[] real_duration = new int[para.tasks];
            double project_finish = 0;
            for (int i = 0; i < para.tasks; i++)
            {
                t_start[i] = 0;
                double h_aff = 0;
                double m_aff = 1;
                double sum_prod = 0;
                int new_mreq = 0;
                for (int j = 0; j < para.skills; j++)
                {
                    double sum_exp = 0;
                    for (int k = 1; k < para.humans; k++)
                    {
                        double[] LEXP_k1 = (double[])para.LEXP[k - 1];
                        if ((ind.t_human_assign[i] & (1 << (para.humans - k))) != 0)
                        {
                            sum_exp += LEXP_k1[j];
                        }
                    }
                    int[] TREQ_i = (int[])para.TREQ[i];
                    if (sum_exp != 0)
                    {
                        h_aff = Math.Max(h_aff, TREQ_i[j] / sum_exp);
                    }
                }
                int[] MREQ_i = (int[])para.MREQ[i];
                for (int k = 0; k < para.machines; k++) {
                    if (MREQ_i[k] == 1)
                    {
                        new_mreq += 1;
                        if ((ind.t_machine_assign[i] & (1 << (para.machines - k))) != 0)
                        {
                            sum_prod += para.m_prod[k];
                        }
                    }
                }
                if (sum_prod != 0)
                {
                    m_aff = new_mreq / sum_prod;
                }
                real_duration[i] = (int)(m_aff * h_aff * para.t_duration[i]);
            }
            for (int i = 0; i < para.tasks; i++) {
                List<int> prev_tasks_i = prev_tasks[i];
                if (prev_tasks_i.Count == 0)
                {
                    t_finish[i] = t_start[i] + real_duration[i];
                }
                else
                {
                    foreach(int j in prev_tasks_i)
                    {
                        t_finish[i] = t_finish[j] + real_duration[i];
                    }
                }
                project_finish = Math.Max(project_finish, t_finish[i]);
            }
            return project_finish;
        }
        public double f_assignment(individual ind, parameter para)
        {
            List<List<int>> prev_tasks = new List<List<int>>();
            prev_tasks = cm.get_prev_tasks(para);
            int[] t_start = new int[para.tasks];
            int[] t_finish = new int[para.tasks];
            int[] real_duration = new int[para.tasks];
            for (int i = 0; i < para.tasks; i++)
            {
                t_start[i] = 0;
                double h_aff = 0;
                double m_aff = 1;
                double sum_prod = 0;
                double new_mreq = 0;
                for (int j = 0; j < para.skills; j++)
                {
                    double sum_exp = 0;
                    for (int k = 1; k < para.humans; k++)
                    {
                        double[] LEXP_k = (double[])para.LEXP[k - 1];
                        int mm = ind.t_human_assign[i] & (1 << (para.humans - k));
                        if ((ind.t_human_assign[i] & (1 << (para.humans - k))) != 0)
                        {
                            sum_exp += LEXP_k[j];
                        }
                    }
                    int[] TREQ_i = (int[])para.TREQ[i];
                    if (sum_exp != 0)
                    {
                        h_aff = Math.Max(h_aff, TREQ_i[j] / sum_exp);
                    }
                }
                int[] MREQ_i = (int[])para.MREQ[i];
                for (int k = 0; k < para.machines; k++)
                {
                    if (MREQ_i[k] == 1)
                    {
                        new_mreq += 1;
                        if ((ind.t_machine_assign[i] & (1 << (para.machines - k))) != 0)
                        {
                            sum_prod += para.m_prod[k];
                        }
                    }
                }
                if (sum_prod != 0)
                {
                    m_aff = new_mreq / sum_prod;
                }
                real_duration[i] = (int)(m_aff * h_aff * para.t_duration[i]);
            }
            for (int i = 0; i < para.tasks; i++)
            {
                List<int> prev_tasks_i = prev_tasks[i];
                if (prev_tasks_i.Count == 0)
                {
                    t_finish[i] = t_start[i] + real_duration[i];
                }
                else
                {
                    foreach (int j in prev_tasks_i)
                    {
                        t_finish[i] = t_finish[j] + real_duration[i];
                    }
                }
            }
            double[] h_conflict = new double[para.tasks];
            for(int i = 0; i < para.tasks; i++)
            {
                h_conflict[i] = 0;
            }
            double[] m_conflict = new double[para.tasks];
            for (int i = 0; i < para.tasks; i++)
            {
                m_conflict[i] = 0;
            }
            double total_h_conflict = 0;
            double total_m_conflict = 0;
            double[] h_working_time = new double[para.humans];
            for (int i = 0; i < para.humans; i++)
            {
                h_working_time[i] = 0;
            }
            double[] m_working_time = new double[para.machines];
            for (int i = 0; i < para.machines; i++)
            {
                m_working_time[i] = 0;
            }
            for(int u = 0; u < para.tasks; u++)
            {
                for(int v = u + 1; v < para.tasks; v++)
                {
                    int task_conflict = Math.Max(0, (Math.Min(t_finish[u], t_finish[v]) - Math.Max(t_start[u], t_start[v])));
                    int common_h_allocate = ind.t_human_assign[u] & ind.t_human_assign[v];
                    int common_m_allocate = ind.t_machine_assign[u] & ind.t_machine_assign[v];
                    for(int i = 1; i < para.humans + 1; i++)
                    {
                        if ((common_h_allocate & (1 << (para.humans - i))) != 0){
                            h_conflict[i - 1] += task_conflict;
                        }
                    }
                    for (int i = 1; i < para.machines + 1; i++)
                    {
                        if ((common_m_allocate & (1 << (para.machines - i))) != 0)
                        {
                            m_conflict[i - 1] += task_conflict;
                        }
                    }
                }
            }
            for (int u = 0; u < para.tasks; u++)
            {
                for (int i=1;i< para.humans + 1; i++)
                {
                    if ((ind.t_human_assign[u] & (1 << (para.humans - i))) != 0){
                        for (int h = 0; h < para.humans; h++)
                        {
                            h_working_time[h] += para.t_duration[u];
                        }
                    }
                }
                for (int i = 1; i < para.machines+ 1; i++)
                {
                    if ((ind.t_machine_assign[u] & (1 << (para.machines - i))) != 0)
                    {
                        for(int m=0;m< para.machines; m++)
                        {
                            m_working_time[m] += para.t_duration[u];
                        }
                    }
                }
            }
            for (int i = 0; i < para.humans; i++)
            {
                total_h_conflict += h_conflict[i] / h_working_time[i];
            }
            for (int i = 0; i < para.machines; i++)
            {
                total_m_conflict += m_conflict[i] / m_working_time[i];
            }
            return total_h_conflict / para.humans + total_m_conflict / para.machines;
        }
        public double f_cost(individual ind, parameter para)
        {
            double h_cost = 0;
            double m_cost = 0;
            double[] h_working_time = new double[para.humans];
            for (int i = 0; i < para.humans; i++)
            {
                h_working_time[i] = 0;
            }
            double[] m_working_time = new double[para.machines];
            for (int i = 0; i < para.machines; i++)
            {
                m_working_time[i] = 0;
            }
            for (int u = 0; u < para.tasks; u++)
            {
                for (int i = 1; i < para.humans + 1; i++)
                {
                    if ((ind.t_human_assign[u] & (1 << (para.humans - i))) != 0)
                    {
                        for (int h = 0; h < para.humans; h++)
                        {
                            h_working_time[h] += para.t_duration[u];
                        }
                    }
                }
                for (int i = 1; i < para.machines + 1; i++)
                {
                    if ((ind.t_machine_assign[u] & (1 << (para.machines - i))) != 0)
                    {
                        for (int m = 0; m < para.machines; m++)
                        {
                            m_working_time[m] += para.t_duration[u];
                        }
                    }
                }
            }

            for (int i = 0; i < para.humans; i++)
            {
                double wage = 0;
                double[] LEXP_i = (double[])para.LEXP[i];
                for (int k = 0; k < para.skills; k++)
                {
                    wage += LEXP_i[k];
                }
                h_cost += para.h_salary[i] * wage * h_working_time[i];
            }
            for(int i = 0; i < para.machines; i++)
            {
                m_cost += para.m_consuming[i] * m_working_time[i];
            }
            return h_cost / para.humans + m_cost / para.machines;
        }
        public Tuple<List<double>, List<double>> objectives_constraints(individual ind, parameter para)
        {
            List<double> obj = new List<double>();
            obj.Add(-f_duration(ind, para));
            obj.Add(-f_assignment(ind, para));
            obj.Add(-f_cost(ind, para));
            List<double> constraints = new List<double>();
            var tuple = new Tuple<List<double>, List<double>>(obj, constraints);
            return tuple;
        }
    }
}
