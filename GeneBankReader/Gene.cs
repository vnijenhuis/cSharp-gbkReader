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
        public Boolean IsReverse { get; private set; }
        public Int32 StartCoordinate { get; private set; }
        public Int32 EndCoordinate { get; private set; }
        public String Sequence { get; set; } = "";
        public Gene(string locusTag, string geneId, bool isReverse, int start, int end)
        {
            LocusTag = locusTag;
            ID = geneId;
            IsReverse = isReverse;
            StartCoordinate = start;
            EndCoordinate = end;
        }
    }
}
