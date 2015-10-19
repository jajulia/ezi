using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ezi
{
    public class Knowledge
    {
        public List<Document> documents { get; private set; }
        public List<String> keywords { get; private set; }
        public int[][] bagofwords { get; private set; }
       
        public Knowledge()
        {
            this.documents = new List<Document>();
            this.keywords = new List<String>();
       
        }

        private String CleanTerm(String tempTerm)
        {
            tempTerm = tempTerm.ToLower();
            
            string[] toRemove = new string[]{",", ".", "/", "<", ">", "?", ";", "'", ":", "\"","[", "]", "{", "}", "\\", "|", "~", "`", "!", "@", "#", "$", "%", "^", "&", "*", "(", ")" };

            foreach (string tempChar in toRemove)
            {
                tempTerm.Replace(tempChar, "");
            }
            return tempTerm;
        }

        public void UpdateData(string documentsFileName, string keywordsFileName)
        {
            this.documents.Clear();
            this.keywords.Clear();
            bool first = true;

            //TO DO jeśli koneiczna będzie walidacja poprawności struktury plików to trzeba dopisać, bo narazie zakładamy poprawnosc domyslnie
            foreach (String part in File.ReadAllLines(keywordsFileName))
            {
                if (part.Length > 0)
                {
                    Stemmer s = new Stemmer();
                    for (int j = 0; j < part.Length; j++)
                    {
                        s.add(part[j]);
                    }
                    s.stem();
                    this.keywords.Add(s.ToString());
                }

            }
            foreach (String part in File.ReadAllLines(documentsFileName))
            {

                if (part.Length == 0)
                {
                    first = true;
                }
                else if (first)
                {
                    this.documents.Add(new Document(part));
                    first = false;
                }
                String part2 = EraseChar(part);
                String[] terms = part2.Split(new char[] { ' ' });
                for (int i = 0; i < terms.Length; i++)
                {
                    Stemmer stem = new Stemmer();
                    for (int j = 0; j < terms[i].Length; j++)
                    {
                        stem.add(terms[i][j]);
                    }
                    stem.stem();
                    if (keywords.Contains(stem.ToString()))
                    {
                        this.documents.Last().AddTerm(stem.ToString());
                    }
                }

            }

           
        }
        public void Calculate()
        {

            double[][] TF = new double[keywords.Count][];
            double[] IDF = new double[keywords.Count];
            double[][] TFIDF = new double[documents.Count][];
            int i=0;
            int j=0;
            foreach (String keyword in keywords)
            {
                j=0;
                foreach(Document docu in documents)
                {
                    int maxCount = docu.terms.Max(y => y.Count);
                    Term t = docu.terms.Find(x => x.Value == keyword);
                    if (t!= null)
                    {
                        TF[i][j] = t.Count / maxCount;
                    }
                    else
                    {
                        TF[i][j] = 0;
                    }
                        
                    j++;
                }
                i++;
            }
            for(j=0 ; j<keywords.Count;j++)
            {
                IDF[j] =TF[j].Sum();
            }
            for (i = 0; i < documents.Count; i++)
            {
                for(j=0;j<keywords.Count;j++)
                {
                    TFIDF[i][j] = TF[j][i]*IDF[j];
                    TFIDF[i][keywords.Count + 1] += Math.Pow(TF[j][i] * IDF[j], 2);
                }
                TFIDF[i][keywords.Count + 1] = Math.Pow(TFIDF[i][keywords.Count + 1], 1/2);
            }


        }
        public String EraseChar(String part)
        {
            part = part.ToLower();
            part = Regex.Replace(part, @"[^\ a-z0-9]", "");
            part = part.Replace("  "," ");
            part = part.Replace("   ", " ");
            return part;
        }

    }
}
