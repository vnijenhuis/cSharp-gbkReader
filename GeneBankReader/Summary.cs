using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class Summary
    {
        public String FileName { get; private set; }
        public String Organism { get; private set; }
        public String AccessionId { get; private set; }
        public String SequenceLength { get; private set; }
        public String OriginSequence { get; private set; }
        public int GeneCount { get; private set; }
        public int CodingSequenceCount { get; private set; }
        public Double ForwardReverseBalance { get; private set; }

        public Summary(string fileName, string organism, string accessionId, string sequenceLength, int geneCount, double forwardReverseBalance, int codingSequenceCount, string originSequence)
        {
            FileName = fileName;
            Organism = organism;
            AccessionId = accessionId;
            SequenceLength = sequenceLength;
            GeneCount = geneCount;
            ForwardReverseBalance = forwardReverseBalance;
            CodingSequenceCount = codingSequenceCount;
            OriginSequence = originSequence;
        }
    }
}
