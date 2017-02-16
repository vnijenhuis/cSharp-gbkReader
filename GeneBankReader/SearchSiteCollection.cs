using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class SearchSiteCollection : System.Collections.CollectionBase
    {
        public void AddGene(SearchSite searchSite)
        {
            List.Add(searchSite);
        }
    }
}
