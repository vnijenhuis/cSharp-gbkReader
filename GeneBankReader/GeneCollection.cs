using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class GeneCollection
    {
        public List<Gene> collection { get; set; } = new List<Gene>();
        public void AddGene(Gene gene)
        {
            collection.Add(gene);
        }

        public void SortOnID()
        {
            collection.Sort(delegate (Gene g1, Gene g2) { return g1.ID.CompareTo(g2.ID); });
        }
    }
}
