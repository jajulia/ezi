using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezi
{
    public class Term
    {
        private String value;
        private int count;


        public Term(String tempValue) 
        {
            this.value = tempValue;
            this.count = 1;
        }

        public void Increase()
        {
            this.count = count + 1;
        }

        public override string ToString()
        {
            return String.Format("{0} ({1})", this.value, this.count.ToString());
        }
    }
}
