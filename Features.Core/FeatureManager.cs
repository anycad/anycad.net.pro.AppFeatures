using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Features.Core
{
    public class FeatureManager
    {
        public List<Feature> FeatrueList { get; set; }

        FeatureManager()
        {
            this.FeatrueList = new List<Feature>();
        }

        static FeatureManager _Instance = null;
        public static FeatureManager Instance()
        {
            if (_Instance == null)
            {
                _Instance = new FeatureManager();
            }

            return _Instance;
        }

        public void Add(Feature feature)
        {
            FeatrueList.Add(feature);
        }
    }
}
