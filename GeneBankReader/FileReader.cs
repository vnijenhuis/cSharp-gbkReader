using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace GeneBankReader
{
    class FileReader
    {
        public string ReadGenebankFile(string inputFile)
        {
            //Split path to get file name.
            GeneCollection geneCollection = new GeneCollection();
            CodingSequenceCollection codingSequenceCollection = new CodingSequenceCollection();
            string[] folders = inputFile.Split(Path.PathSeparator);
            string fileName = folders[folders.Length - 1];
            string organism = "";
            string accession = "";
            string length = "";
            string originSequence = "";

            //Booleans for if/else statements.
            bool isFirst = true;
            bool isOrigin = false;
            bool currentEntryIsCDS = false;
            bool currentEntryIsGene = false;
            //Both patterns check if both complement and non-complement entries are present.
            string genePattern = " *gene *(complement)?\\(?\\d*\\.\\.\\d*\\)?";
            string cdsPattern = " *CDS *(complement)?\\(?\\d*\\.\\.\\d*\\)?";

            string currentEntry = "";
            StreamReader reader = new StreamReader(inputFile);
            string gbkLine;
            while ((gbkLine = reader.ReadLine()) != null)
            {
                Console.WriteLine(gbkLine);
                //All comming lines contain nucleotide data which can be added to the origin sequence.
                if (isOrigin)
                {
                    originSequence += gbkLine.Replace(" ", "").Replace("\\d", "");
                }
                //Only occurs untill first entry is false.
                if (isFirst)
                {
                    if (gbkLine.StartsWith("LOCUS"))
                    {
                        length = GetSequenceLength(gbkLine);
                    }
                    if (gbkLine.Contains("  ORGANISM"))
                    {
                        organism = GetOrganism(gbkLine);
                    }
                    if (gbkLine.Contains("ACCESSION"))
                    {
                        accession = GetAccessionId(gbkLine);
                    }
                }
                //Check if 
                if (currentEntryIsCDS && !Regex.IsMatch(gbkLine, genePattern))
                {
                    currentEntry += gbkLine + "\n";
                }
                else if (currentEntryIsCDS && Regex.IsMatch(gbkLine, genePattern))
                {
                    currentEntryIsGene = true;
                    currentEntryIsCDS = false;
                    CodingSequence codingSequence = CreateCodingSequenceEntry(currentEntry);
                    codingSequenceCollection.AddCodingSequence(codingSequence);
                    currentEntry = gbkLine + "\n";
                }
                else if (currentEntryIsGene && !Regex.IsMatch(gbkLine, cdsPattern))
                {
                    currentEntry += gbkLine + "\n";
                }
                else if (currentEntryIsGene && Regex.IsMatch(gbkLine, cdsPattern))
                {
                    currentEntryIsGene = false;
                    currentEntryIsCDS = true;
                    Gene gene = CreateGeneEntry(currentEntry);
                    geneCollection.AddGene(gene);
                    currentEntry = gbkLine + "\n";
                }
                else if (isFirst && Regex.IsMatch(gbkLine, genePattern))
                {
                    currentEntryIsGene = true;
                    isFirst = false;
                    currentEntry += gbkLine + "\n";
                } else if (isFirst && Regex.IsMatch(gbkLine, cdsPattern))
                {
                    currentEntryIsCDS = true;
                    isFirst = false;
                    currentEntry += gbkLine + "\n";
                }
                if (gbkLine.StartsWith("ORIGIN"))
                {
                    //Set isOrigin to true: first if statement will be handled.
                    isOrigin = true;
                    originSequence += gbkLine.Replace(" ", "").Replace("\\d", "").Replace("ORIGIN", "");
                    if (currentEntryIsCDS)
                    {
                        currentEntryIsCDS = false;
                        CodingSequence codingSequence = CreateCodingSequenceEntry(currentEntry);
                        codingSequenceCollection.AddCodingSequence(codingSequence);
                    }
                    else if (currentEntryIsGene)
                    {
                        currentEntryIsGene = false;
                        Gene gene = CreateGeneEntry(currentEntry);
                        geneCollection.AddGene(gene);
                    }
                }
            }
            int geneCount = geneCollection.Count; //Size of gene collection
            int cdsCount = codingSequenceCollection.Count; //Size of coding sequence collection
            double totalGeneCounter = 0.0;
            double forwardGeneCounter = 0.0;
            //Forward/Reverse (FR) ratio calculation.
            double value = (forwardGeneCounter / totalGeneCounter);
            double forwardReverseBalance = Math.Round(value, 1);

            //For each gene: if gene isForward or !isReverse > +1 to total and foward
            //else +1 to total
            Summary summary = new Summary(fileName, organism, accession, length, geneCount, forwardReverseBalance, cdsCount, originSequence);
            return null;
        }

        public Gene CreateGeneEntry(string currentEntryData)
        {
            string locusTag = "";
            string geneId = "";
            string sequence = "";
            bool isReverse = true;
            int start = 0;
            int end = 0;
            string[] entryLines = currentEntryData.Split('\n');
            foreach (string item in entryLines)
            {
                string[] splitItem = item.Split(' ');
                string geneData = splitItem[splitItem.Length - 1];
                if (Regex.IsMatch(geneData, "\\d*\\.\\.\\d*"))
                {
                    string[] split = Regex.Split(geneData, @"\.\.");
                    start = Int32.Parse(split[0]);
                    end = Int32.Parse(split[1]);
                    isReverse = false;
                } else if (Regex.IsMatch(geneData, "complement\\(\\d*\\.\\.\\d*\\)"))
                {

                }
            }
            Gene gene = new Gene(locusTag, geneId, sequence, isReverse, start, end);
            return gene;
        }
        public CodingSequence CreateCodingSequenceEntry(string currentEntryData)
        {
            string locusTag = "";
            string geneId = "";
            string translatedSequence = "";
            string geneProduct = "";
            string proteinId = "";
            bool isReverse = true;
            bool foundTranslatedSequence = false;
            int start = 0;
            int end = 0;
            currentEntryData.Split('\n');
            string[] entryLines = currentEntryData.Split('\n');
            foreach (string item in entryLines)
            {
                //string[] geneData = entryLines.Where(line => !string.IsNullOrEmpty(line)).ToArray();
                string[] splitItem = item.Split(' ');
                string cdsData = splitItem[splitItem.Length - 1];
                if (Regex.IsMatch(cdsData, "\\d*\\.\\.\\d*"))
                {
                    //string[] coordinateSplit = data.Split(new[] { ".." }, StringSplitOptions.None);
                    string[] coordinates = Regex.Split(cdsData, @"\.\.");
                    start = Int32.Parse(coordinates[0]);
                    end = Int32.Parse(coordinates[1]);
                    isReverse = false;

                } else if (Regex.IsMatch(cdsData, "complement\\(\\d*\\.\\.\\d*\\)"))
                {
                    String substring = cdsData.Substring(11);
                    string replaceData = substring.Replace(")", "");
                    string[] coordinates = Regex.Split(replaceData, @"\.\.");
                    start = Int32.Parse(coordinates[0]);
                    end = Int32.Parse(coordinates[1]);
                    isReverse = true;

                } else if (item.Contains("/gene") )
                {
                    string[] split = cdsData.Split('=');
                    geneId = split[1].Replace("\"", "");
                }
                else if (item.Contains("/locus_tag"))
                {
                    string[] split = cdsData.Split('=');
                    locusTag = split[1].Replace("\"", "");
                }
                else if (item.Contains("/product"))
                {
                    string[] split = cdsData.Split('=');
                    geneProduct = split[1].Replace("\"", "");
                }
                else if (item.Contains("/protein_id"))
                {
                    string[] split = cdsData.Split('=');
                    proteinId = split[1].Replace("\"", "");
                }
                else if (item.Contains("/translation") && !foundTranslatedSequence)
                {
                    string[] split = cdsData.Split('=');
                    translatedSequence = split[1].Replace("\"", "");
                    foundTranslatedSequence = true;
                }
                else if (foundTranslatedSequence && !item.Contains("ORIGIN"))
                {
                    string sequence = splitItem[splitItem.Length - 1];
                    translatedSequence += sequence.Substring(0, sequence.Length);
                }
            }

            CodingSequence codingSequence = new CodingSequence(locusTag, geneId, translatedSequence, geneProduct, isReverse, start, end, proteinId);
            return codingSequence;
        }

        /**
         * Gets the full name of the organism.
         */
        public string GetOrganism(string gbkLine)
        {
            string[] splitLine = gbkLine.Split(' ');
            string organismName = "";
            foreach (string item in splitLine)
            {
                if (!item.Equals("") && !Regex.IsMatch(item, "ORGANISM"))
                {
                    if (organismName.Equals(""))
                    {
                        organismName += item;
                    }
                    else
                    {
                        organismName += " " + item;
                    }
                }
            }
            return organismName;
        }

        /**
         * Gets the sequence length from the given line.
         */
        public string GetSequenceLength(string gbkLine)
        {
            string[] splitLine = gbkLine.Split(' ');
            string sequenceLength = "";
            foreach (string item in splitLine)
            {
                if (!item.Equals("") && Regex.IsMatch(item, "\\d"))
                {
                    sequenceLength = item;
                }
            }
            return sequenceLength;
        }

        /**
         * Gets the accession id from the given line.
         */
        public string GetAccessionId(string gbkLine)
        {
            string[] splitLine = gbkLine.Split(' ');
            string accessionId = splitLine[splitLine.Length - 1];
            return accessionId;
        }
    }
}
