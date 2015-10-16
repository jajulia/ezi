using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezi
{
    class Document
    {
        string name;
        List<Term> terms;

        public Document(string tempName)
        {
            this.name = tempName;
            this.terms = new List<Term>();
        }
        public void AddTerm(String value)
        {
            //if nie ma
            terms.Add(new Term(value));
        }
    }
}
