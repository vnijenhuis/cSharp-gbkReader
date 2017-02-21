using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class SearchSiteCollection
    {
        public List<SearchSite> collection { get; set; } = new List<SearchSite>();
        public String searchSiteRegexMessage { get; set; } = "";
        public void AddSearchSite(SearchSite site)
        {
            collection.Add(site);
        }

        public void Sort()
        {
            collection.Sort(delegate (SearchSite site1, SearchSite site2) { return site1.GeneName.CompareTo(site2.GeneName); });
        }
    }
}
