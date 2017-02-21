using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class GeneBankDisplayFetcher
    {
        public string newLine = Environment.NewLine;
        public void FetchSummary(Summary summary)
        {
            string message = "Summary\n";
            message += "file                " + summary.FileName + newLine;
            message += "organism            " + summary.Organism + newLine;
            message += "accession           " + summary.AccessionId + newLine;
            message += "sequence length     " + summary.SequenceLength + newLine;
            message += "number of genes     " + summary.GeneCount + newLine;
            message += "gene F/R balance    " + summary.ForwardReverseBalance + newLine;
            message += "number of CDSs      " + summary.CodingSequenceCount + newLine;
            Console.WriteLine(message);
        }

        internal void FetchGeneDisplay(GeneCollection geneCollection, string[] geneArray, string originSequence)
        {
            GbkFeatureFetcher fetcher = new GbkFeatureFetcher();
            GeneCollection updatedGeneCollection = fetcher.FetchGeneData(geneCollection, geneArray, originSequence);
            foreach (Gene gene in updatedGeneCollection.collection)
            {
                String geneMessage = "";
                if (gene.ID != "")
                {
                    geneMessage += ">" + gene.ID + newLine;
                } else
                {
                    geneMessage += ">" + gene.LocusTag + newLine;
                }            //Print sequence in substrings of 80.
                String sequence = gene.Sequence;
                int length = sequence.Length;
                for (int start = 0; start < length;)
                {
                    int end = start + 80;
                    if (end < length)
                    {
                        geneMessage += sequence.Substring(start, 80) + newLine;
                    }
                    else
                    {
                        geneMessage += sequence.Substring(start, (length - start));
                    }
                    start += 80;
                }
                Console.WriteLine(geneMessage);
            }
            Console.WriteLine(newLine);
        }

        public void FetchCDSs(CodingSequenceCollection cdsCollection, string[] cdsArray)
        {
            GbkFeatureFetcher fetcher = new GbkFeatureFetcher();
            CodingSequenceCollection updatedCodingSequenceCollection = fetcher.FetchCodingSequenceData(cdsCollection, cdsArray);
            foreach (CodingSequence codingSequence in updatedCodingSequenceCollection.collection)
            {
                string cdsMesasge = "";
                if (codingSequence.GeneID != "")
                {
                    cdsMesasge += ">CDS " + codingSequence.GeneID + " sequence" + newLine;
                }
                else
                {
                    cdsMesasge += ">CDS " + codingSequence.LocusTag + " sequence" + newLine;
                }
                string sequence = codingSequence.TranslatedSequence;
                int length = sequence.Length;
                for (int start = 0; start < length;)
                {
                    int end = start + 80;
                    if (end < length)
                    {
                        cdsMesasge += sequence.Substring(start, 80) + newLine;
                    }
                    else
                    {
                        cdsMesasge += sequence.Substring(start, (length - start));
                    }
                    start += 80;
                }
                Console.WriteLine(cdsMesasge);
            }
            Console.WriteLine(newLine);
        }

        public void FetchFeatures(GeneCollection geneCollection, CodingSequenceCollection cdsCollection, string[] featureArray)
        {
            Console.Write("FEATURE;TYPE;START;STOP;ORIENTATION");
            GbkFeatureFetcher fetcher = new GbkFeatureFetcher();
            SiteFeatureCollection siteFeatureCollection = fetcher.FetchSiteFeatures(geneCollection, cdsCollection, featureArray);
            foreach (SiteFeature siteFeature in siteFeatureCollection.collection)
            {
                Console.WriteLine(siteFeature.GeneID + ";" + siteFeature.Type + ";" + siteFeature.StartCoordinate +";" + siteFeature.EndCoordinate + ";" + siteFeature.Orientation);
            }
            Console.WriteLine(newLine);
        }

        public void FetchSites(GeneCollection geneCollection, string originSequence, string[] siteArray)
        {
            Console.Write("POSITION;SEQUENCE;GENE");
            GbkFeatureFetcher fetcher = new GbkFeatureFetcher();
            List<SearchSiteCollection> list = fetcher.FetchSearchSiteData(geneCollection, originSequence, siteArray);
            foreach (SearchSiteCollection searchSiteCollection in list) {
                foreach (SearchSite searchSite in searchSiteCollection.collection)
                {
                    Console.WriteLine(searchSite.StartPosition + ";" + searchSite.Site + ";" + searchSite.GeneName);
                }
            }
            Console.WriteLine(newLine);
        }
    }
}
