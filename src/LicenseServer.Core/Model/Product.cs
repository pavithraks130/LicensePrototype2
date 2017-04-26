﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Core.Model
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ProductCode { get; set; }

        public string ImagePath { get; set; }

        public string CreatedDate { get; set; }

        public double Price { get; set; }

        public ICollection<ProductCategory> Categories { get; set; }

        public virtual ICollection<Feature> AssociatedFeatures { get; set; }

    }

    public class ProductCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
