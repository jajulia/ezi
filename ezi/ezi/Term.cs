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

        public String Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        private int count;

        public int Count
        {
            get { return count; }
            set { count = value; }
        }
        
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
