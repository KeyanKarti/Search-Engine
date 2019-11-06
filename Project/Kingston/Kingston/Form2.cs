using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;
using System.Diagnostics;


namespace Kingston
{
    public partial class Form2 : Form
    {
        public static string search_term;
        public static Boolean status;
        public static string documentID;
        private QueryParser queryParser;
        String[] words = new String[1000];
        Lucene.Net.Search.IndexSearcher searcher;
        static Form1 f1 = new Form1();
        Lucene.Net.Store.Directory luceneIndexDirectory= Lucene.Net.Store.FSDirectory.Open(f1.getIndexPath());
        Lucene.Net.QueryParsers.QueryParser Parser;
        static int count = 0;
        public Lucene.Net.Analysis.Analyzer analyzer;
        public static string save_results_path;
        static long index_time;


        static String[] url=new String[1000];
        static String[] Passage_Text=new String[1000];
        static TopDocs results;

        Program program = new Program();

        public Form2()
        {
           
            InitializeComponent();
        }

        public void CreateParser()
        {
            String[] fields = { "url", "Passage_Text"};
            Parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, analyzer);
        }

        

        public void CreateAnalyser()
        {
            //Hashtable hashtable = new Hashtable();
            //hashtable ={ "a", "an", "and", "are", "as", "at", "be", "but", "by", "for", "if", "in", "into", "is", "it", "no", "not", "of", "on", "or", "such", "that", "the", "their", "then", "there", "these", "they", "this", "to", "was", "will", "with"};
            // HashSet<string> hashTable = new HashSet<string>();
            //ISet<String s>;
            ISet<String> stopWords = new HashSet<String>{"a",
"able",
"about",
"above",
"according",
"accordingly",
"across",
"actually",
"after",
"afterwards",
"again",
"against",
"ain't",
"all",
"allow",
"allows",
"almost",
"alone",
"along",
"already",
"also",
"although",
"always",
"am",
"among",
"amongst",
"an",
"and",
"another",
"any",
"anybody",
"anyhow",
"anyone",
"anything",
"anyway",
"anyways",
"anywhere",
"apart",
"appear",
"appreciate",
"appropriate",
"are",
"aren't",
"around",
"as",
"a's",
"aside",
"ask",
"asking",
"associated",
"at",
"available",
"away",
"awfully",
"be",
"became",
"because",
"become",
"becomes",
"becoming",
"been",
"before",
"beforehand",
"behind",
"being",
"believe",
"below",
"beside",
"besides",
"best",
"better",
"between",
"beyond",
"both",
"brief",
"but",
"by",
"came",
"can",
"cannot",
"cant",
"can't",
"cause",
"causes",
"certain",
"certainly",
"changes",
"clearly",
"c'mon",
"co",
"com",
"come",
"comes",
"concerning",
"consequently",
"consider",
"considering",
"contain",
"containing",
"contains",
"corresponding",
"could",
"couldn't",
"course",
"c's",
"currently",
"definitely",
"described",
"despite",
"did",
"didn't",
"different",
"do",
"does",
"doesn't",
"doing",
"done",
"don't",
"down",
"downwards",
"during",
"each",
"edu",
"eg",
"eight",
"either",
"else",
"elsewhere",
"enough",
"entirely",
"especially",
"et",
"etc",
"even",
"ever",
"every",
"everybody",
"everyone",
"everything",
"everywhere",
"ex",
"exactly",
"example",
"except",
"far",
"few",
"fifth",
"first",
"five",
"followed",
"following",
"follows",
"for",
"former",
"formerly",
"forth",
"four",
"from",
"further",
"furthermore",
"get",
"gets",
"getting",
"given",
"gives",
"go",
"goes",
"going",
"gone",
"got",
"gotten",
"greetings",
"had",
"hadn't",
"happens",
"hardly",
"has",
"hasn't",
"have",
"haven't",
"having",
"he",
"he'd",
"he'll",
"hello",
"help",
"hence",
"her",
"here",
"hereafter",
"hereby",
"herein",
"here's",
"hereupon",
"hers",
"herself",
"he's",
"hi",
"him",
"himself",
"his",
"hither",
"hopefully",
"how",
"howbeit",
"however",
"how's",
"i",
"i'd",
"ie",
"if",
"ignored",
"i'll",
"i'm",
"immediate",
"in",
"inasmuch",
"inc",
"indeed",
"indicate",
"indicated",
"indicates",
"inner",
"insofar",
"instead",
"into",
"inward",
"is",
"isn't",
"it",
"it'd",
"it'll",
"its",
"it's",
"itself",
"i've",
"just",
"keep",
"keeps",
"kept",
"know",
"known",
"knows",
"last",
"lately",
"later",
"latter",
"latterly",
"least",
"less",
"lest",
"let",
"let's",
"like",
"liked",
"likely",
"little",
"look",
"looking",
"looks",
"ltd",
"mainly",
"many",
"may",
"maybe",
"me",
"mean",
"meanwhile",
"merely",
"might",
"more",
"moreover",
"most",
"mostly",
"much",
"must",
"mustn't",
"my",
"myself",
"name",
"namely",
"nd",
"near",
"nearly",
"necessary",
"need",
"needs",
"neither",
"never",
"nevertheless",
"new",
"next",
"nine",
"no",
"nobody",
"non",
"none",
"noone",
"nor",
"normally",
"not",
"nothing",
"novel",
"now",
"nowhere",
"obviously",
"of",
"off",
"often",
"oh",
"ok",
"okay",
"old",
"on",
"once",
"one",
"ones",
"only",
"onto",
"or",
"other",
"others",
"otherwise",
"ought",
"our",
"ours",
"ourselves",
"out",
"outside",
"over",
"overall",
"own",
"particular",
"particularly",
"per",
"perhaps",
"placed",
"please",
"plus",
"possible",
"presumably",
"probably",
"provides",
"que",
"quite",
"qv",
"rather",
"rd",
"re",
"really",
"reasonably",
"regarding",
"regardless",
"regards",
"relatively",
"respectively",
"right",
"said",
"same",
"saw",
"say",
"saying",
"says",
"second",
"secondly",
"see",
"seeing",
"seem",
"seemed",
"seeming",
"seems",
"seen",
"self",
"selves",
"sensible",
"sent",
"serious",
"seriously",
"seven",
"several",
"shall",
"shan't",
"she",
"she'd",
"she'll",
"she's",
"should",
"shouldn't",
"since",
"six",
"so",
"some",
"somebody",
"somehow",
"someone",
"something",
"sometime",
"sometimes",
"somewhat",
"somewhere",
"soon",
"sorry",
"specified",
"specify",
"specifying",
"still",
"sub",
"such",
"sup",
"sure",
"take",
"taken",
"tell",
"tends",
"th",
"than",
"thank",
"thanks",
"thanx",
"that",
"thats",
"that's",
"the",
"their",
"theirs",
"them",
"themselves",
"then",
"thence",
"there",
"thereafter",
"thereby",
"therefore",
"therein",
"theres",
"there's",
"thereupon",
"these",
"they",
"they'd",
"they'll",
"they're",
"they've",
"think",
"third",
"this",
"thorough",
"thoroughly",
"those",
"though",
"three",
"through",
"throughout",
"thru",
"thus",
"to",
"together",
"too",
"took",
"toward",
"towards",
"tried",
"tries",
"truly",
"try",
"trying",
"t's",
"twice",
"two",
"un",
"under",
"unfortunately",
"unless",
"unlikely",
"until",
"unto",
"up",
"upon",
"us",
"use",
"used",
"useful",
"uses",
"using",
"usually",
"value",
"various",
"very",
"via",
"viz",
"vs",
"want",
"wants",
"was",
"wasn't",
"way",
"we",
"we'd",
"welcome",
"well",
"we'll",
"went",
"were",
"we're",
"weren't",
"we've",
"what",
"whatever",
"what's",
"when",
"whence",
"whenever",
"when's",
"where",
"whereafter",
"whereas",
"whereby",
"wherein",
"where's",
"whereupon",
"wherever",
"whether",
"which",
"while",
"whither",
"who",
"whoever",
"whole",
"whom",
"who's",
"whose",
"why",
"why's",
"will",
"willing",
"wish",
"with",
"within",
"without",
"wonder",
"won't",
"would",
"wouldn't",
"yes",
"yet",
"you",
"you'd",
"you'll",
"your",
"you're",
"yours",
"yourself",
"yourselves",
"you've",
"zero"
};
            
            analyzer = new Lucene.Net.Analysis.Snowball.SnowballAnalyzer(Lucene.Net.Util.Version.LUCENE_30, "English", stopWords);
            

        }

        public void label1_Click(object sender, EventArgs e)
        {

        }

        public void textBox1_TextChanged(object sender, EventArgs e)
        {
            search_term = textBox1.Text;
        }

        public string getSearchTerm()
        {
            return search_term;
        }

        public Boolean getStatus()
        {
            return status;
        }

        public void button1_Click(object sender, EventArgs e)
        {
            string need = getSearchTerm();
            status = getStatus();


            results = SearchIndex(need);
            DisplayResults(results);
            StringBuilder display = new StringBuilder();
            docID = GetDocID();
            title = GetTitle();
            author = GetAuthor();
            info = GetInfo();
            int rank = 0;
            if (title.Length >= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }
            else
            {
                for (int i = 0; i < title.Length; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }


            label5.Text = display.ToString();
            label4.Text = GetCount().ToString() + " results retrieved";

            CleanUpSearch();
        }

        public void CleanUpSearch()
        {
            searcher.Dispose();
        }

        public TopDocs SearchIndex(string querytext)
        {
            System.Console.WriteLine("\nSearch term: " + querytext);
            Stopwatch stopwatch = new Stopwatch();
            if (status)
            {
                stopwatch.Start();
                querytext = querytext.ToLower();
                String[] fields = { "url", "Passage_Text"};
                CreateAnalyser();
                Parser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, analyzer);
                Query searchQuery = Parser.Parse(querytext);
                searcher = new Lucene.Net.Search.IndexSearcher(luceneIndexDirectory);
                results = searcher.Search(searchQuery, 1000);
                count = results.TotalHits;
                stopwatch.Stop();
                index_time = stopwatch.ElapsedMilliseconds;
                label7.Text = index_time.ToString() + " milliseconds taken to search";
            }
            else
            {
                stopwatch.Start();
                SimpleAnalyzer simpleAnalyzer = new SimpleAnalyzer();
                String[] fields = { "url", "Passage_Text"};
                //Lucene.Net.Analysis.Standard.StandardAnalyzer analyzer2 = new Lucene.Net.Analysis.Standard.StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_30);
                queryParser = new MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, simpleAnalyzer);
                Query searchQuery = queryParser.Parse(querytext);
                searcher = new Lucene.Net.Search.IndexSearcher(luceneIndexDirectory);
                results = searcher.Search(searchQuery, 1000);
                count = results.TotalHits;
                stopwatch.Stop();
                index_time = stopwatch.ElapsedMilliseconds;
                label7.Text = index_time.ToString() + " milliseconds taken to search";
            }
                return results;
        }

        public void DisplayResults(TopDocs results)
        {
            int rank = 0;
            int j = 0;
            Console.WriteLine("\nTotal number of results: " + results.TotalHits);
            for (int i = 0; i < results.TotalHits; i++)
            {
                rank++;
                j++;


                Lucene.Net.Documents.Document doc = searcher.Doc(results.ScoreDocs[i].Doc);

                
                Console.WriteLine("Rank " + rank + "\tTitle:" + doc.Get("Title").ToString() + "Author:" + doc.Get("Author").ToString() + "Bibliographic information:" + doc.Get("info").ToString());
                url[i] = doc.Get("url").ToString();
                Passage_Text[i] = doc.Get("Passage_Text").ToString();
             
            }
        }


        public void button2_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            long time = f1.GetIndexTime();
            MessageBox.Show("Time taken for indexing: " + time + "milliseconds");
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                checkBox1.Text = "Pre-Process";
                status = true;
            }
            else
            {
                checkBox1.Text = "NO Pre-Process";
                status = false;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            documentID = textBox2.Text;

            String collectionPath = f1.getCollectionPath();
            string filePath = collectionPath + "/" + documentID + ".txt";
            string text = System.IO.File.ReadAllText(@filePath);
            char[] delimiters = { 'I', 'T', 'A', 'B', 'W' };
            words = text.Split(delimiters);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Abstarct: " + words[5]);
        }

        public string[] GetDocID()
        {
            return docID;
        }

        public string[] GetTitle()
        {
            return title;
        }

        public string[] GetAuthor()
        {
            return author;
        }

        public string[] GetInfo()
        {
            return info;
        }

        public int GetCount()
        {
            return count;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string save_path;
            folderBrowserDialog1.ShowDialog();
            label6.Text = folderBrowserDialog1.SelectedPath;
            string[] documentID= new String[1000];
            save_path = label6.Text;
            StringBuilder display = new StringBuilder();
            docID = GetDocID();
            title = GetTitle();
            author = GetAuthor();
            info = GetInfo();
            int rank = 0;
            for(int i=0;i<results.TotalHits;i++)
            {
                string[] splitStr= docID[i].Split('.');
                documentID[i] = splitStr[0];
                documentID[i] = documentID[i].Replace('\n', ' ');
            }
            
            string searchterm = getSearchTerm();
            for (int i = 0; i < results.TotalHits; i++)
            {
                Console.WriteLine(" \n " + documentID[i]);
                ScoreDoc scoredoc = results.ScoreDocs[i];
                display.Append("\n\n" + searchterm + " Q0 " + "  " + documentID[i]+" " + (++rank) +" " +scoredoc.Score+ "  Justice league");
            }


            File.AppendAllText(@save_path +"/"+"results.txt",display.ToString() + Environment.NewLine);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder display = new StringBuilder();
            docID = GetDocID();
            title = GetTitle();
            author = GetAuthor();
            info = GetInfo();
            int rank = 0;
            if (title.Length >= 10)
            {
                for (int i = 0; i < 10; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }
            else
            {
                for (int i = 0; i < title.Length; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }


            label5.Text = display.ToString();
            label4.Text = GetCount().ToString() + " results retrieved";
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder display = new StringBuilder();
            docID = GetDocID();
            title = GetTitle();
            author = GetAuthor();
            info = GetInfo();
            int rank = 10;
            if (title.Length >= 20)
            {
                for (int i = 10; i < 20; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }
            else
            {
                for (int i = 10; i < title.Length; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }


            label5.Text = display.ToString();
            label4.Text = GetCount().ToString() + " results retrieved";
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder display = new StringBuilder();
            docID = GetDocID();
            title = GetTitle();
            author = GetAuthor();
            info = GetInfo();
            int rank = 20;
            if (title.Length >= 30)
            {
                for (int i = 20; i < 30; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }
            else
            {
                for (int i = 20; i < title.Length; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }


            label5.Text = display.ToString();
            label4.Text = GetCount().ToString() + " results retrieved";
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder display = new StringBuilder();
            docID = GetDocID();
            title = GetTitle();
            author = GetAuthor();
            info = GetInfo();
            int rank = 30;
            if (title.Length >= 40)
            {
                for (int i = 30; i < 40; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }
            else
            {
                for (int i = 30; i < title.Length; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }


            label5.Text = display.ToString();
            label4.Text = GetCount().ToString() + " results retrieved";
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder display = new StringBuilder();
            docID = GetDocID();
            title = GetTitle();
            author = GetAuthor();
            info = GetInfo();
            int rank = 40;
            if (title.Length >= 50)
            {
                for (int i = 40; i < 50; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }
            else
            {
                for (int i = 40; i < title.Length; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }


            label5.Text = display.ToString();
            label4.Text = GetCount().ToString() + " results retrieved";
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            StringBuilder display = new StringBuilder();
            docID = GetDocID();
            title = GetTitle();
            author = GetAuthor();
            info = GetInfo();
            int rank = 50;
            if (title.Length >= 60)
            {
                for (int i = 50; i < 60; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }
            else
            {
                for (int i = 50; i < title.Length; i++)
                {
                    display.Append("\nRANK:" + (++rank) + "  DocID: " + docID[i] + "TITLE: " + title[i] + "\n AUTHOR: " + author[i] + "INFO: " + info[i]);
                }
            }


            label5.Text = display.ToString();
            label4.Text = GetCount().ToString() + " results retrieved";
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }
    }
}
