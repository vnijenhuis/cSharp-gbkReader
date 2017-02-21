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
                GeneBankDisplayFetcher displayFetcher = new GeneBankDisplayFetcher();
                try
                {
                    if (parser.ParseArguments(args, options))
                    {
                        //Match to regex. Checks if file ends with .gbk
                        string gbkFile = options.InputFile;
                        if (File.Exists(gbkFile) && gbkFile.EndsWith(".gb") || gbkFile.EndsWith(".gbk"))
                        {
                            CollectedGeneBankData geneBankData = reader.ReadGenebankFile(gbkFile);
                            GeneCollection geneCollection = geneBankData.geneCollection;
                            Summary summary = geneBankData.summary;
                            CodingSequenceCollection cdsCollection = geneBankData.codingSequenceCollection;
                            if (options.Summary)
                            {
                                // get summary from data fetcher
                                displayFetcher.FetchSummary(geneBankData.summary);
                            }

                            string[] geneArray = options.FetchGenes;
                            if (geneArray != null && geneArray.Length != 0)
                            {
                                Console.WriteLine("Fetching gene sequences...");
                                displayFetcher.FetchGeneDisplay(geneCollection, geneArray, summary.OriginSequence);
                                //give options.GenesToFetch to dataFetcher and return the required data.
                            }
                            if (options.FetchCDS != null && options.FetchCDS.Length != 0)
                            {
                                Console.WriteLine("Fetching cds product sequences...");
                                string[] cdsArray = options.FetchCDS;
                                displayFetcher.FetchCDSs(cdsCollection, cdsArray);
                                //give options.CDSsToFetch to dataFetcher and return the required data.
                            }
                            if (options.FetchFeatures != null && options.FetchFeatures.Length != 0)
                            {
                                Console.WriteLine("Fetching gene location features...");
                                string[] featureArray = options.FetchFeatures;
                                displayFetcher.FetchFeatures(geneCollection, cdsCollection, featureArray);
                                //give options.FeaturesToFetch to dataFetcher and return the required data.
                            }
                            if (options.FetchSites != null && options.FetchSites.Length != 0)
                            {
                                Console.WriteLine("Fetching given nucleotide sites...");
                                string[] siteArray = options.FetchSites;
                                displayFetcher.FetchSites(geneCollection, geneBankData.summary.OriginSequence, siteArray);
                                //give options.SitesToFetch to dataFetcher and return the required data.
                            }
                            Console.ReadKey();
                        } else
                        {
                            Console.WriteLine("Given file " + gbkFile + " seems to not exist on your computer. Please check your input.");
                            Console.WriteLine("Press any key to close the console.");
                            Console.ReadKey();
                        }
                    } else
                    {
                        Console.WriteLine("The commandline parser could not find any arguments. Please use the --help function for options.");
                        Console.WriteLine("Press any key to close the console.");
                        Console.WriteLine(parser.ParseArguments(args, options));
                        Console.WriteLine(options.InputFile);
                        Console.WriteLine(args[0] + " " + args[1]);
                        Console.ReadKey();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Encounted an exception while parsing." + e + "\nPlease check your cmd arguments and try again, or use the --help function.");
                    Console.WriteLine("Press any key to close the console.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("No arguments we're provided.");
                options.GetHelp();
                Console.WriteLine("Press any key to close the console.");
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
            helpLine.AppendLine("-s --summary        Shows the summary of the given genebank file.");
            helpLine.AppendLine("-i --infile         Requires a single .gb or .gbk extension file as input.");
            helpLine.AppendLine("--fetch_gene        Fetches one or more genes from the genebank file.");
            helpLine.AppendLine("--fetch_cds         Fetches one or more CDS entries from the genebank file.");
            helpLine.AppendLine("--fetch_features    Fetches one or more series of gene features from the genebank file.");
            helpLine.AppendLine("--find_sites        Fetches one or more specified gene sites from the genebank file.");
            return helpLine.ToString();
        }

        //The -infile option. Requires a .gb or .gbk file as input.
        [Option('i', "infile", Required = true, DefaultValue = "", HelpText = "Provide a .gb or .gbk file to read.")]
        public String InputFile { get; set; }

        //Gives the summary of the genebank file.
        [Option('s', "summary", Required = false, HelpText = "Provides a summary of the genebank file.", DefaultValue = false)]
        public Boolean Summary { get; set; }

        [OptionArray("fetch_gene", Required = false, DefaultValue = null, HelpText = "Provide one or more gene names or locus tags to search for in the genebank file.")]
        public String[] FetchGenes { get; set; }

        [OptionArray("fetch_cds", Required = false, DefaultValue = null, HelpText = "Provide one or more gene names or locus tags to search for in the genebank file.")]
        public String[] FetchCDS { get; set; }

        [OptionArray("fetch_features", Required = false, DefaultValue = null, HelpText = "Provide one or more gene names or locus tags to search for in the genebank file.")]
        public String[] FetchFeatures { get; set; }

        [OptionArray("fetch_sites", Required = false, DefaultValue = null, HelpText = "Provide one or more gene names or locus tags to search for in the genebank file.")]
        public String[] FetchSites { get; set; }
    }
}
