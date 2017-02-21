using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class CodingSequenceCollection
    {
        public List<CodingSequence> collection { get; set; } = new List<CodingSequence>();
        public void AddCodingSequence(CodingSequence cds)
        {
            collection.Add(cds);
        }

        public void Sort()
        {
            collection.Sort(delegate (CodingSequence cds1, CodingSequence cds2) { return cds1.GeneID.CompareTo(cds2.GeneID); });
        }
    }
}
