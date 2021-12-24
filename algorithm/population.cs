using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResourceAllocationApp.daos;
using System.Collections;

namespace ResourceAllocationApp.algorithm
{
    class population
    {
        public List<individual> make_pop(parameter para, random_Q r)
        {
            int pop_size = 30; 
            List<individual> pop = new List<individual>();
            for (int i = 0; i < pop_size; i++)
            {
                individual ind = new individual();
                ind.randomize(para, r);
                pop.Add(ind);
            }
            return pop;
        }
    }
}