using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Models;

namespace License.MetCalWeb.Models
{
    public class FeatureExtended : Feature
    {
        public bool Selected { get; set; }
        public string Type { get; set; }
    }
}