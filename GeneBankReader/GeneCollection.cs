using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class GeneCollection : System.Collections.CollectionBase
    {
        public void AddGene(Gene gene)
        {
            List.Add(gene);
        }
    }
}
