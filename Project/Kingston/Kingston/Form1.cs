﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lucene.Net.Index;
using Lucene.Net.Analysis;
using Lucene.Net.Documents; // for Socument
using Lucene.Net.Store; //for Directory
using Lucene.Net.Search;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

namespace Kingston
{
    public partial class Form1 : Form
    {
        public static string index_path;
        public static string collection_path;
        public static string search_term;
        static long index_time;
        Program program = new Program();
        Lucene.Net.Store.Directory luceneIndexDirectory;
        public Lucene.Net.Analysis.Analyzer analyzer;
        Lucene.Net.Index.IndexWriter writer;
        Lucene.Net.Search.IndexSearcher searcher;
        Lucene.Net.QueryParsers.QueryParser Parser;
        static string[] indexing = { };

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void button1_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            BrowseLabel.Text = folderBrowserDialog1.SelectedPath;
            collection_path = BrowseLabel.Text;
        }

        public void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog2.ShowDialog();
            SaveLabel.Text = folderBrowserDialog2.SelectedPath;
            index_path = SaveLabel.Text;
        }

        public void button3_Click(object sender, EventArgs e)
        {
            OpenIndex(getIndexPath());
            CreateAnalyser();
            CreateParser();
            CreateWriter();
            string[] filePaths = System.IO.Directory.GetFiles(getCollectionPath());

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            for (int i = 0; i < filePaths.Length; i++)
            {
                string text = System.IO.File.ReadAllText(filePaths[i]);
                IndexText(text);
            }

            stopwatch.Stop();
            index_time = stopwatch.ElapsedMilliseconds;

            CleanUp();
            CreateSearcher();
            Form2 f2 = new Form2();
            f2.ShowDialog();
        }

        public void CleanUp()
        {
            writer.Optimize();
            writer.Flush(true, true, true);
            writer.Dispose();
        }

        public void CreateSearcher()
        {
            searcher = new Lucene.Net.Search.IndexSearcher(luceneIndexDirectory);
        }
        public void IndexText(string text)
        {
            //String[] delimiters = { "url", "passage_text"};
            //indexing = text.Split(delimiters,0, StringSplitOptions.RemoveEmptyEntries);
            //var str = analyzer.TokenStream(null, new System.IO.StringReader(indexing[5]));
            //var str1 = analyzer.TokenStream(null, new System.IO.StringReader(indexing[2]));
            //string str = token.Preprocessing(indexing[5]);
            //string str1 = token.Preprocessing(indexing[2]);
            var jo = JObject.Parse(text);
            var url = jo["passages"]["url"].ToString();
            var passage_text = jo["passages"]["passage_text"].ToString();
            Lucene.Net.Documents.Field field = new Field(url, indexing[1], Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
            Lucene.Net.Documents.Field field1 = new Field(passage_text, indexing[2], Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);
            //Lucene.Net.Documents.Field field5 = new Field("abstract", indexing[5], Field.Store.YES, Field.Index.ANALYZED, Field.TermVector.WITH_POSITIONS_OFFSETS);


            Lucene.Net.Documents.Document doc = new Document();
            doc.Add(field);
            doc.Add(field1);
            //doc.Add(field5);
            writer.AddDocument(doc);
        }

        public void CreateAnalyser()
        {
            
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

        public long GetIndexTime()
        {
            return index_time;
        }

        public void CreateWriter()
        {
            IndexWriter.MaxFieldLength mfl = new IndexWriter.MaxFieldLength(IndexWriter.DEFAULT_MAX_FIELD_LENGTH);
            writer = new Lucene.Net.Index.IndexWriter(luceneIndexDirectory, analyzer, true, mfl);
        }

        public void CreateParser()
        {
            String[] fields = { "url", "Passage_Text" };
            Parser = new Lucene.Net.QueryParsers.MultiFieldQueryParser(Lucene.Net.Util.Version.LUCENE_30, fields, analyzer);
        }

        public void OpenIndex(string indexPath)
        {
            luceneIndexDirectory = Lucene.Net.Store.FSDirectory.Open(indexPath);
        }

        public string getIndexPath()
        {
            return index_path;
        }

        public string getCollectionPath()
        {
            return collection_path;
        }

        public string getSearchTerm()
        {
            return search_term;
        }
    }
}
