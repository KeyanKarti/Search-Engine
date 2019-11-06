using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Kingston
{
    
class Tokenisation
{
        Program Program;
    Stemmer stemmer = new Stemmer();
        public string[] stopWords = { "a", "what", "an", "must", "when", "and", "are", "as", "at", "be", "but", "by", "for", "if", "in", "into", "is", "it", "no", "not", "of", "on", "or", "such", "that", "the", "their", "then", "there", "these", "they", "this", "to", "was", "will", "with", "a", "able", "about", "above", "according", "accordingly", "across", "actually", "after", "afterwards", "again", "against", "ain't", "all", "allow", "allows", "almost", "alone", "along", "already", "also", "although", "always", "am", "among", "amongst", "an", "and", "another", "any", "anybody", "anyhow", "anyone", "anything", "anyway", "anyways", "anywhere", "apart", "appear", "appreciate" };

 // for challange activity

    public string Preprocessing(string need)
    {
        char[] splitters = new char[] { ' ', '\t', '\'', '"', '-', '(', ')', ',', '’', '\n', ':', ';', '?', '.', '!' };
        string[] tokens= need.ToLower().Split(splitters, StringSplitOptions.RemoveEmptyEntries);
        // var str = Program.analyzer.TokenStream(null, new System.IO.StringReader(need));
          //  string[] tokens = str.

           int numTokens = tokens.Count();
        List<string> filteredTokens = new List<string>();

        for (int i = 0; i < numTokens; i++)
        {
            string token = tokens[i];
            if (!stopWords.Contains(token) && (token.Length > 2)) filteredTokens.Add(token);
        }

        string[] processedTokens=filteredTokens.ToArray<string>();
        string[] stems=StemTokens(processedTokens);
        int count = stems.Length;
        string query = "";

        for(int i=0;i<count;i++)
        {
            query = query +" "+ stems[i];
        }

        return query;
    }

    public string[] StemTokens(string[] tokens)
    {
        int numTokens = tokens.Count();
        string[] stems = new string[numTokens];

        for (int i = 0; i < numTokens; i++)
        {
            stems[i] = stemmer.stemTerm(tokens[i]);
        }

        return stems;
    }
}
}
