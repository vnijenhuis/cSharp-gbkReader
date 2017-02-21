using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class SiteFeature
    {
        public String GeneID { get; private set; }
        public String Type { get; private set; }
        public Int32 StartCoordinate { get; private set; }
        public Int32 EndCoordinate { get; private set; }
        public String Orientation { get; private set; }

        public SiteFeature(string geneId, string type, int start, int end, string orientation)
        {
            GeneID = geneId;
            Type = type;
            StartCoordinate = start;
            EndCoordinate = end;
            Orientation = orientation;
        }
    }
}
