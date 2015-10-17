using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezi
{
    public class Document
    {
        public string name { get; private set; }
        public List<Term> terms { get; private set; }
        public string termsInSTring { get { return string.Join(",", terms); } }

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
