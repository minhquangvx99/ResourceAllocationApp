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
    class individual
    {
        public List<int> t_human_assign = new List<int>();
        public List<int> t_machine_assign = new List<int>();
        common cm = new common();
        public void randomize(parameter para, random_Q r)
        {
            for (int i = 0; i < para.tasks; i++)
            {
                t_human_assign.Add(0);
                t_machine_assign.Add(0);
            }
            for (int i = 0; i < para.tasks; i++)
            {
                t_human_assign[i] = r.random_rd(1, (1 << para.humans));
                t_machine_assign[i] = r.random_rd(1, (1 << para.machines));
                List<int> t_h_bit_assign = new List<int>();
                for (int h = 0; h < para.humans; h++)
                {
                    t_h_bit_assign.Add(para.valid_human[i, h]);
                }
                String t_h_bit_string = "";
                for (int j = 0; j < para.humans; j++)
                {
                    t_h_bit_string = t_h_bit_string + "0";
                }
                List<int> t_m_bit_assign = new List<int>();
                for (int m = 0; m < para.machines; m++)
                {
                    t_m_bit_assign.Add(para.valid_machine[i, m]);
                }
                String t_m_bit_string = "";
                for (int j = 0; j < para.machines; j++)
                {
                    t_m_bit_string = t_m_bit_string + "0";
                }
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
                        t_h_bit_string = t_h_bit_string + t_h_bit_string_list[j];
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
                        t_m_bit_string = t_m_bit_string + t_m_bit_string_list[j];
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
                t_human_assign[i] = binaryToInt(t_h_bit_string);
                t_machine_assign[i] = binaryToInt(t_m_bit_string);
            }
        }
        public void set(List<int> t_h, List<int> t_m)
        {
            t_human_assign = t_h;
            t_machine_assign = t_m;
        }
    }
}