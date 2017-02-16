using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class CodingSequence
    {
        public String GeneID { get; private set; }
        public String LocusTag { get; private set; }
        public String TranslatedSequence { get; private set; }
        public String GeneProduct { get; private set; }
        public String ProteinID { get; private set; }
        public int StartCoordinate { get; private set; }
        public int EndCoordinate { get; private set; }
        public Boolean IsReverse { get; private set; }

        public CodingSequence(string locusTag, string geneId, string translatedSequence, string geneProduct, bool isReverse, int start, int end, string proteinId)
        {
            GeneID = geneId;
            LocusTag = locusTag;
            TranslatedSequence = translatedSequence;
            GeneProduct = geneProduct;
            ProteinID = proteinId;
            StartCoordinate = start;
            EndCoordinate = end;
            IsReverse = isReverse;
        }
    }
}
