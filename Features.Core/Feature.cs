using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Features.Core
{
    public class Feature
    {
        public String Name { get; set; }
        public String Category { get; set; }
        public String Group { get; set; }
        public String Description { get; set; }

        public virtual bool Run(FeatureContext context)
        {
            return false;
        }

        public virtual void OnExit(FeatureContext context)
        {
            
        }
    }
}
