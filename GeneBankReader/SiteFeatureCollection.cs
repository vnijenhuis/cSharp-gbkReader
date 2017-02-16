using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class SiteFeatureCollection : System.Collections.CollectionBase
    {
        public void AddGene(SiteFeature siteFeature)
        {
            List.Add(siteFeature);
        }
    }
}
