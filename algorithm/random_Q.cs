using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceAllocationApp.algorithm
{
    class random_Q
    {
        Random r = new Random();
        public int random_rd(int a, int b)
        {
            return r.Next(a, b);
        }
    }
}
