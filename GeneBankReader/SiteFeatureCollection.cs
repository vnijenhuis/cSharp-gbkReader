using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneBankReader
{
    class SiteFeatureCollection
    {
        public List<SiteFeature> collection { get; set; } = new List<SiteFeature>();
        public void AddSiteFeature(SiteFeature feature)
        {
            collection.Add(feature);
        }

        public void Sort()
        {
            collection.Sort(delegate (SiteFeature feature1, SiteFeature feature2) { return feature1.GeneID.CompareTo(feature2.GeneID); });
        }
    }
}
