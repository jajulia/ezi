using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezi
{
    class Term
    {
        String value;
        int count { get; set; }


        public Term(String tempValue) 
        {
            this.value = tempValue;
        }

        public void Increase()
        {
            this.count = count + 1;
        }
    }
}
