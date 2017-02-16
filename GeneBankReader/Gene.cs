using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class Gene
    {
        public String LocusTag { get; private set; }
        public String ID { get; private set; }
        public String Sequence { get; private set; }
        public Boolean IsReverse { get; private set; }
        public int StartCoordinate { get; private set; }
        public int EndCoordinate { get; private set; }
        public Gene(string locusTag, string geneId, string sequence, bool isReverse, int start, int end)
        {
            LocusTag = locusTag;
            ID = geneId;
            Sequence = sequence;
            IsReverse = isReverse;
            StartCoordinate = start;
            EndCoordinate = end;
        }
    }
}
