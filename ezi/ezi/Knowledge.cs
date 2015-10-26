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
        public Dictionary<String, double> keywords { get; private set; }
        public double alfa { get; set; }
        public double beta { get; set; }
        public double gamma { get; set; }


        public Knowledge()
        {
            this.documents = new List<Document>();
            this.keywords = new Dictionary<String, double>();

        }
        public void UpdateData(string documentsFileName, string keywordsFileName)
        {
            
            bool first = true;

            if (keywordsFileName.Length > 0)
            {
                this.keywords.Clear();
                //TO DO jeśli koneiczna będzie walidacja poprawności struktury plików to trzeba dopisać, bo narazie zakładamy poprawnosc domyslnie
                foreach (String part in File.ReadAllLines(keywordsFileName))
                {
                    if (part.Length > 0)
                    {
                        Stemmer s = new Stemmer();
                        s.add(part.ToCharArray(), part.Length);
                        s.stem();
                        if (!keywords.Keys.Contains(s.ToString()))
                        {
                            this.keywords.Add(s.ToString(), 0);
                        }
                        else //+++
                        {
                        }
                    }

                }
            }
            if (documentsFileName.Length > 0)
            {
                this.documents.Clear();
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
                        stem.add(terms[i].ToCharArray(), terms[i].Length);
                        stem.stem();
                        if (keywords.ContainsKey(stem.ToString()))
                        {
                            this.documents.Last().AddTerm(stem.ToString());
                        }
                    }

                }
            }


        }



        
        public void setParam(string alfaString, string betaString, string gammaString)
        {
            try
            {
                this.alfa = double.Parse(alfaString);
                this.beta = double.Parse(betaString);
                this.gamma = double.Parse(gammaString);
            }
            catch (Exception)
            {
                if (alfaString.Length == 0 && betaString.Length == 0 && gammaString.Length == 0)
                {
                    this.alfa = 1;
                    this.beta = 1;
                    this.gamma = 1;
                }
                else
                    throw new Exception("Niepoprawne wartości współczynników alfa, beta, gamma!");
            }

        }


        public void Calculate(string query)
        {


            int j = 0;
            query = EraseChar(query);
            String[] queryTerms = query.Split(new char[] { ' ' });
            for (j = 0; j < queryTerms.Length; j++)
            {
                Stemmer s = new Stemmer();
                s.add(queryTerms[j].ToCharArray(), queryTerms[j].Length);
                s.stem();
                //sprawdzanie czy jest to w zbiorze jesli tak to +1  //+++
                if (this.keywords.Keys.Contains(s.ToString()))
                {
                    this.keywords[s.ToString()] += 1;
                }
                else { } //+++
            }
            foreach (String keyword in keywords.Keys.ToList())
            {
                keywords[keyword] = rocchio(keyword);
            }              

            foreach(KeyValuePair<String, double> keyword in keywords)
            {
                
                //wykorzstanei result jako bufo
                foreach (Document docu in documents)
                {
                    //+++ czasem nie zawieralo
                    if (docu.terms.ContainsKey(keyword.Key)
                        //+++ dzielenie przez zero
                        && docu.terms.Values.Max() > 0 && keywords.Values.Max() > 0 && documents.Where(x => x.terms.ContainsKey(keyword.Key)).Count() > 0)
                    {

                        docu.result += docu.terms[keyword.Key] / (double) docu.terms.Values.Max()
                            * Math.Log10(documents.Count / (double) documents.Where(x => x.terms.ContainsKey(keyword.Key)).Count())
                            * keyword.Value / (double) keywords.Values.Max()
                            * Math.Log10(documents.Count / (double) documents.Where(x => x.terms.ContainsKey(keyword.Key)).Count());
                    }

                }

            }
            foreach (Document docu in documents)
            {
                
                foreach (KeyValuePair<String, double> keyword in keywords)
                {
                    //+++ czasem nie zawieralo
                    if (docu.terms.ContainsKey(keyword.Key)
                        //+++ dzielenie przez zero
                        && docu.terms.Values.Max() > 0 && documents.Where(x => x.terms.ContainsKey(keyword.Key)).Count() > 0)
                    {
                        docu.length += Math.Pow(docu.terms[keyword.Key] / (double) docu.terms.Values.Max()
                            * Math.Log10(documents.Count / (double) documents.Where(x => x.terms.ContainsKey(keyword.Key)).Count()), 2);
                    }
                }

                docu.length = Math.Sqrt(docu.length);
            }
            double len=0;
            foreach (KeyValuePair<String, double> keyword in keywords)
            {
                //+++
                if (keywords.Values.Max() > 0 && documents.Where(x => x.terms.ContainsKey(keyword.Key)).Count() > 0)
                {
                    len += Math.Pow(keyword.Value / (double) keywords.Values.Max()
                      * Math.Log10(documents.Count / (double) documents.Where(x => x.terms.ContainsKey(keyword.Key)).Count()), 2);
                }
            }
            len = Math.Sqrt(len);
            foreach (Document docu in documents)
            {
                if (docu.length > 0 && len > 0)
                    docu.result = docu.result / (double) (docu.length * len);
            }

        }
        public String EraseChar(String part)
        {
            part = part.ToLower();
            part = Regex.Replace(part, @"[^\ a-z0-9]", "");
            part = part.Replace("/  +/g", " ");
            //part = part.Replace("  ", " ");
            //part = part.Replace("   ", " ");//sklejanie spacji funkcja
            return part;
        }


        /*
         * zwraca wartosc q' dla termu w zapytaniu
         * */
        public double rocchio(string keyword)
        {
            double importantAvg;
            double unimportantAvg;
            List<Document> docs;

            //foreach (string keyword in this.keywords) { 
            if (keywords.ContainsKey(keyword))
            {
                docs = this.documents.Where(x => x.important == true && x.terms.ContainsKey(keyword)).ToList();
                if (docs.Count > 0)
                {
                    importantAvg = this.documents.Where(x => x.important == true && x.terms.ContainsKey(keyword)).ToList().Average(y => y.terms[keyword]);
                }
                else
                    importantAvg = 0;

                docs = this.documents.Where(x => x.important == false && x.terms.ContainsKey(keyword)).ToList();

                if (docs.Count > 0)
                {
                    unimportantAvg = this.documents.Where(x => x.important == false && x.terms.ContainsKey(keyword)).ToList().Average(y => y.terms[keyword]);
                }
                else
                    unimportantAvg = 0;

                //keywords[keyword] = 
                double tempResult = keywords[keyword] * this.alfa + importantAvg * this.beta + unimportantAvg * this.gamma;
                return tempResult;

            }

            return 0;
        }


    }
}
