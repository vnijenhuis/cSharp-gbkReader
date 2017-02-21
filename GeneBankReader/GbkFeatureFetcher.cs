using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class GbkFeatureFetcher
    {
        public GeneCollection FetchGeneData(GeneCollection geneCollection, string[] geneArray, string originSequence)
        {
            List<string> nonMatchedGenes = new List<string>();
            GeneCollection updatedGeneCollection = new GeneCollection();
            foreach (String givenGene in geneArray)
            {
                Boolean isMatch = false;
                foreach (Gene gene in geneCollection.collection)
                {
                    if (gene.ID.Contains(givenGene) || gene.LocusTag.Contains(givenGene))
                    {
                        int start = gene.StartCoordinate;
                        int distance = gene.EndCoordinate - gene.StartCoordinate;
                        string nucleotideSequence = originSequence.Substring(start, distance);
                        gene.Sequence = nucleotideSequence;
                        updatedGeneCollection.AddGene(gene);
                        isMatch = true;
                        break;
                    }
                }
                //If no match was found: add to nonMatchedGenes list.
                if (!isMatch)
                {
                    nonMatchedGenes.Add(givenGene);
                }
            }
            //If all or some geneId entries did not match:\
            if (nonMatchedGenes.Any())
            {
                //Display list of gene entries that did not match.
                String message = "";
                foreach (String gene in nonMatchedGenes)
                {
                    if (message.Length == 0)
                    {
                        message += "Some of the provided enrties could not be found. Here is the list of genes; ";
                        message += gene;
                    }
                    else
                    {
                        message += ", " + gene;
                    }
                }
                Console.WriteLine("\n" + message);
            }
            updatedGeneCollection.SortOnID();
            return updatedGeneCollection;
        }

        public CodingSequenceCollection FetchCodingSequenceData(CodingSequenceCollection codingSequenceCollection, string[] cdsArray)
        {
            List<string> nonMatchedCds = new List<string>();
            CodingSequenceCollection updatedCodingSequenceCollection = new CodingSequenceCollection();
            foreach (string givenCds in cdsArray)
            {
                bool isMatch = false;
                foreach (CodingSequence codingSequence in codingSequenceCollection.collection)
                {
                    if (codingSequence.GeneProduct.Equals(givenCds))
                    {
                        updatedCodingSequenceCollection.AddCodingSequence(codingSequence);
                        isMatch = true;
                    }
                }
                if (!isMatch)
                {
                    nonMatchedCds.Add(givenCds);
                }
            }
            //If all CDS entries did not match:
            if (!updatedCodingSequenceCollection.collection.Any())
            {
                Console.WriteLine("None of the provided entries could be found.");
            }
            else if (!nonMatchedCds.Any())
            {
                //Display list of CDS entries that did not match.
                String message = "";
                foreach (String cds in nonMatchedCds)
                {
                    if (message == "")
                    {
                        message += "Some of the provided CDS could not be found. Here is the list of CDS; ";
                        message += cds;
                    }
                    else
                    {
                        message += ", " + cds;
                    }
                }
                Console.WriteLine("\n" + message);
            }
            updatedCodingSequenceCollection.Sort();
            return updatedCodingSequenceCollection;
        }

        /**
         * Fetches data from GeneCollection and CodingSequenceCollection.
         * @returns siteFeatureCollection containing SiteFeature objects.
         */
        public SiteFeatureCollection FetchSiteFeatures(GeneCollection geneCollection, CodingSequenceCollection cdsCollection, string[] featureCoordinateArray)
        {
            SiteFeatureCollection siteFeatureCollection = new SiteFeatureCollection();
            List<string> nonMatchedFeatures = new List<string>();
            foreach (string coordinate in featureCoordinateArray)
            {
                bool isMatch = false;
                if (Regex.IsMatch(coordinate, "\\d*\\.\\.\\d*"))
                {
                    string[] split = Regex.Split(coordinate, "\\.\\.");
                    int startCoordinate = Int32.Parse(split[0]);
                    int endCoordinate = Int32.Parse(split[1]);
                    for (int i = 0; i < geneCollection.collection.Count; i++)
                    {
                        string geneId = "";
                        string type = "";
                        string orientation = "";
                        Gene gene = geneCollection.collection[i];
                        CodingSequence cds = cdsCollection.collection[i];
                        if (gene.StartCoordinate >= startCoordinate && gene.EndCoordinate <= endCoordinate)
                        {
                            if (gene.ID != "")
                            {
                                geneId = gene.ID;
                            }
                            else
                            {
                                geneId = gene.LocusTag;
                            }
                            type = "gene";
                            int geneStartCoordinate = gene.StartCoordinate;
                            int geneStopCoordinate = gene.EndCoordinate;
                            if (gene.IsReverse)
                            {
                                orientation = "R";
                            }
                            else
                            {
                                orientation = "F";
                            }
                            SiteFeature feature = new SiteFeature(geneId, type, geneStartCoordinate, geneStopCoordinate, orientation);
                            siteFeatureCollection.AddSiteFeature(feature);
                            //Change type and change geneId to geneProduct for a CDS entry. Other values are similar to Gene values.
                            string product = cds.GeneProduct;
                            type = "CDS";
                            feature = new SiteFeature(product, type, geneStartCoordinate, geneStopCoordinate, orientation);
                            siteFeatureCollection.AddSiteFeature(feature);
                            isMatch = true;
                        }
                    }
                    if (!isMatch)
                    {
                        nonMatchedFeatures.Add(coordinate);
                    }
                }
            }
            if (!siteFeatureCollection.collection.Any() || !nonMatchedFeatures.Any())
            {
                //Display list of site entries that did not match.
                String message = "";
                foreach (String feature in nonMatchedFeatures)
                {
                    if (message != "")
                    {
                        message += "Some of the provided enrties could not be found. Here is the list of sites; ";
                        message += feature;
                    }
                    else
                    {
                        message += ", " + feature;
                    }
                }
                Console.WriteLine("\n" + message);
            }
            siteFeatureCollection.Sort();
            return siteFeatureCollection;
        }

        public List<SearchSiteCollection> FetchSearchSiteData(GeneCollection geneCollection, string originSequence, string[] siteArray)
        {
            Dictionary<string, string> iupacTable = CreateIupacTable();
            List<SearchSiteCollection> finalSiteList = new List<SearchSiteCollection>();
            List<string> nonMatchedSites = new List<string>();
            string upperOriginSequence = originSequence.ToUpper();
            foreach (string site in siteArray)
            {
                SearchSiteCollection searchSiteCollection = new SearchSiteCollection();
                string[] nucleotideSites = site.Split();
                List<string> targetSequences = new List<string>();
                bool isMatch = false;
                foreach (char nucleotideSite in site)
                {
                    string nucSite = nucleotideSite.ToString();
                    if (iupacTable.ContainsKey(nucSite))
                    {
                        String newNucleotides = iupacTable[nucSite];
                        String regex = "[" + newNucleotides + "]";
                        foreach (char newNuc in newNucleotides)
                        {
                            string nuc = newNuc.ToString();
                            string targetSequence = site.Replace(nucSite, nuc);
                            targetSequences.Add(targetSequence);
                        }
                        searchSiteCollection.searchSiteRegexMessage = "site search: " + nucSite + " (regex: " + nucSite.Replace(nucSite, regex) + ")";
                    }
                }
                foreach (String targetSequence in targetSequences)
                {
                    int index = 0;
                    while (index >= 0 && index != upperOriginSequence.Length)
                    {
                        index++;
                        index = upperOriginSequence.IndexOf(targetSequence, index);
                        String strIndex = "" + (index + 1);
                        if (index != -1)
                        {
                            String geneName = "INTERGENIC";
                            foreach (Gene gene in geneCollection.collection)
                            {
                                if (index >= gene.StartCoordinate && index <= gene.EndCoordinate)
                                {
                                    //Index + 1 because coding index starts at zero and sequence index starts at 1.
                                    if (gene.ID != "")
                                    {
                                        geneName = gene.ID;
                                    }
                                    else
                                    {
                                        geneName = gene.LocusTag;
                                    }
                                    isMatch = true;
                                }
                            }
                            SearchSite searchSite = new SearchSite(strIndex, targetSequence, geneName);
                            if (searchSiteCollection.collection.Any())
                            {
                                bool newEntry = true;
                                foreach (SearchSite siteEntry in searchSiteCollection.collection)
                                {
                                    if (siteEntry.StartPosition.Equals(strIndex))
                                    {
                                        if (siteEntry.Site.Equals("INTERGENIC") && !geneName.Equals("INTERGENIC"))
                                        {
                                            siteEntry.GeneName = geneName;
                                            newEntry = false;
                                            break;
                                        }
                                    }
                                }
                                if (newEntry)
                                {
                                    searchSiteCollection.AddSearchSite(searchSite);
                                }
                            }
                            else
                            {
                                searchSiteCollection.AddSearchSite(searchSite);
                            }
                        }
                    }
                }
                finalSiteList.Add(searchSiteCollection);
                if (!isMatch)
                {
                    nonMatchedSites.Add(site);
                }
            }
            //If all site entries did not match:
            if (nonMatchedSites.Any())
            {
                //Display list of nucleotide site entries that did not match.
                String message = "";
                foreach (String site in nonMatchedSites)
                {
                    if (message == "")
                    {
                        message += "Some of the provided entries could not be found. Here is the list of nucleotide sites; ";
                        message += site;
                    }
                    else
                    {
                        message += ", " + site;
                    }
                }
                Console.WriteLine("\n" + message);
            }
            return finalSiteList;
        }

        /**
         * Creates a Dictionary that functions as a IUPAC code table.
         *
         * @return Dictionary with string as key and string as value.
         */
        public Dictionary<string, string> CreateIupacTable()
        {
            Dictionary<string, string> IupacTable = new Dictionary<string, string>();
            IupacTable["R"] = "AG";
            IupacTable["Y"] = "CT";
            IupacTable["S"] = "GC";
            IupacTable["W"] = "AT";
            IupacTable["K"] = "GT";
            IupacTable["M"] = "AC";
            IupacTable["B"] = "CGT";
            IupacTable["D"] = "AGT";
            IupacTable["H"] = "ACT";
            IupacTable["V"] = "ACG";
            IupacTable["N"] = "ACTG";
            IupacTable["."] = " ";
            IupacTable["-"] = " ";
            return IupacTable;
        }
    }
}
