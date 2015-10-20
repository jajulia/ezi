using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezi
{
    public class Term : Dictionary<String, int>
    {

        public override string ToString()
        {
            return String.Format("{0} ({1})", this.Keys, this.Values.ToString());
        }
    }
}
