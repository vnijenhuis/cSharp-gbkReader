using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine; // imports command line parser
using CommandLine.Text; // if you want text formatting helper
using System.Text.RegularExpressions;
using System.IO;

namespace GeneBankReader
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (args.Length != 0)
            {
                Parser parser = new Parser();
                FileReader reader = new FileReader();
                try
                {
                    if (parser.ParseArguments(args, options))
                    {
                        //Match to regex. Checks if file ends with .gbk
                        string gbkFile = options.InputFile;
                        Console.WriteLine("MyFile: " + gbkFile);
                        if (File.Exists(gbkFile) && gbkFile.EndsWith(".gb") || gbkFile.EndsWith(".gbk"))
                        {
                            Console.WriteLine("HOORAY: PASSED!");
                            reader.ReadGenebankFile(gbkFile);
                            if (options.Summary)
                            {
                                // get summary from data fetcher
                            }
                            //if (options.fetchGenes)
                            //{
                            //    //give options.GenesToFetch to dataFetcher and return the required data.
                            //}
                            //if (options.fetchCDS)
                            //{
                            //    //give options.CDSsToFetch to dataFetcher and return the required data.
                            //}
                            //if (options.fetchFeatures)
                            //{
                            //    //give options.FeaturesToFetch to dataFetcher and return the required data.
                            //}
                            //if (options.fetchSites)
                            //{
                            //    //give options.SitesToFetch to dataFetcher and return the required data.
                            //}
                        } else
                        {
                            Console.WriteLine("Test4");
                            Console.ReadKey();
                        }
                    } else
                    {
                        Console.WriteLine("Test 3: ?");
                        Console.WriteLine("Parser encountered an exception?");
                        Console.WriteLine(parser.ParseArguments(args, options));
                        Console.WriteLine(options.InputFile);
                        Console.WriteLine(args[0] + " " + args[1]);
                        Console.ReadKey();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Test 2: e");
                    Console.WriteLine("Encounted an exception while parsing. Please check your cmd arguments and try again, or use the -help function. " + e.GetBaseException());
                    Console.WriteLine("Press any key to close the console.");
                    Console.WriteLine(args[0] + " : " + args[1] + " : " + args[2]);
                    Console.WriteLine();
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Test 1");
                Console.WriteLine("No arguments we're provided.");
                options.GetHelp();
                Console.ReadKey();
            }
        }
    }

    class Options
    {
        [HelpOption]
        public String GetHelp()
        {
            var helpLine = new StringBuilder();
            helpLine.AppendLine("GeneBankReader");
            helpLine.AppendLine("Tool that can read a .gb or .gbk extension file and allows different functions to be applied to the data.");
            helpLine.AppendLine("-s -summary           Shows the summary of the given genebank file.");
            helpLine.AppendLine("-i -infile         Requires a single .gb or .gbk extension file as input.");
            helpLine.AppendLine("-fetch_gene        Fetches one or more genes from the genebank file.");
            helpLine.AppendLine("-fetch_cds         Fetches one or more CDS entries from the genebank file.");
            helpLine.AppendLine("-fetch_features    Fetches one or more series of gene features from the genebank file.");
            helpLine.AppendLine("-find_sites        Fetches one or more specified gene sites from the genebank file.");
            return helpLine.ToString();
        }

        //The -infile option. Requires a .gb or .gbk file as input.
        [Option('i', "infile", Required = true, HelpText = "Provide a .gb or .gbk file to read.")]
        public String InputFile { get; set; }

        //Gives the summary of the genebank file.
        [Option('s', "summary", Required = false, HelpText = "Provides a summary of the genebank file.", DefaultValue = true)]
        public Boolean Summary { get; set; }

        //[OptionArray("fetch_gene", Required = false, HelpText = "Provide one or more gene names or locus tags to search for in the genebank file.")]
        //public string[] GenesToFetch { get; set; }
        //public Boolean fetchGenes = true;

        //[OptionArray("fetch_cds", Required = false, HelpText = "Provide one or more gene names or locus tags to search for in the genebank file.")]
        //public string[] CDSsToFetch { get; set; }
        //public Boolean fetchCDS = true;

        //[OptionArray("fetch_features", Required = false, HelpText = "Provide one or more gene names or locus tags to search for in the genebank file.")]
        //public string[] FeaturesToFetch { get; set; }
        //public Boolean fetchFeatures = true;

        //[OptionArray("fetch_sites", Required = false, HelpText = "Provide one or more gene names or locus tags to search for in the genebank file.")]
        //public string[] SitesToFetch { get; set; }
        //public Boolean fetchSites = true;
    }
}
