using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class CollectedGeneBankData
    {
        public GeneCollection geneCollection { get; private set; }
        public CodingSequenceCollection codingSequenceCollection { get; private set; }
        public Summary summary { get; private set; }
        public CollectedGeneBankData(GeneCollection collectedGenes, CodingSequenceCollection cdsCollection, Summary sum)
        {
            geneCollection = collectedGenes;
            codingSequenceCollection = cdsCollection;
            summary = sum;
        }
    }
}
