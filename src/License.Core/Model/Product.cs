﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace License.Core.Model
{
    public class Product
    {
      
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
       
        public string Description { get; set; }

        public string ImagePath { get; set; }
      
        public double? UnitPrice { get; set; }
    }
}