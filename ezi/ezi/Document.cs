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
            if (terms.Exists(x => x.Value == value))
            {
                Term t = terms.Find(x => x.Value == value);
                t.Count++;
            }
            else
            {
                terms.Add(new Term(value));
            }
            
        }
    }
}
