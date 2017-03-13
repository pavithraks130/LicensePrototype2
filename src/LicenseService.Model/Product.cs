﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LicenseServer.DataModel
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductCode { get; set; }
        public string ImagePath { get; set; }
        public string CreatedDate { get; set; }
        public ICollection<LicenseFeatures> AssociatedFeatures { get; set; }

    }
}
