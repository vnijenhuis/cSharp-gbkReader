using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class CodingSequenceCollection : System.Collections.CollectionBase
    {
        public void AddCodingSequence(CodingSequence codingSequence)
        {
            List.Add(codingSequence);
        }
    }
}
