using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;

namespace License.MetCalWeb.Models
{
    public class FeatureExtended :Feature
    {
        //public Feature Feature { get; set; }
        //public int Id { get { return Feature.Id; } set { Feature.Id = value; } }
        //public string Name { get { return Feature.Name; } set { Feature.Name = value; } }
        //public string Description { get { return Feature.Description; } set { Feature.Description = value; } }
        //public string Version { get { return Feature.Version; } set { Feature.Version = value; } }
        //public bool IsEnabled { get { return Feature.IsEnabled; } set { Feature.IsEnabled = value; } }
        //public double Price { get { return Feature.Price; } set { Feature.Price = value; } }

        public bool Selected { get; set; }
        public string Type { get; set; }

        public FeatureExtended() { }
        public FeatureExtended(Feature f)
        {
            Id = f.Id;
            Name = f.Name;
            Description = f.Description;
            Version = f.Version;
            IsEnabled = f.IsEnabled;
            Price = f.Price;
        }
    }
}