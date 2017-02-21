using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class SearchSite
    {
        public String Site { get; private set; }
        public String StartPosition { get; private set; }
        public String GeneName { get; set; }
        
        public SearchSite(string site, string start, string geneName)
        {
            Site = site;
            StartPosition = start;
            GeneName = geneName;
        }
    }
}
