﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezi
{
    public class Knowledge
    {
        List<Document> documents;
        List<String> keywords;

        public Knowledge() 
        {
            this.documents = new List<Document>();
            this.keywords = new List<String>();
        }


        public void UpdateData(string documentsFileName, string keywordsFileName)
        {
            this.documents.Clear();
            this.keywords.Clear();
            bool first = true;

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
                else
                {
                    String[] terms = part.Split(new char[] { ' ' });
                    for (int i = 0; i < terms.Length; i++)
                    {
                        this.documents.Last().AddTerm(terms[i]);
                        
                    }
                }

            }
        }
        
    }
}