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
        public Term terms { get; private set; }
        public string termsInSTring { get { return string.Join(",", terms); } }
        public double result { get; set; }
        public double length { get; set; }

        public Document(string tempName)
        {
            this.name = tempName;
            this.terms = new Term();
            this.result = 0;
            this.length = 0;

        }
        public void AddTerm(String value)
        {
              if (terms.ContainsKey(value))
            {
                terms[value] += 1;
            }
            else
            {
                terms.Add(value, 1);
            }
            
        }
    }
}
