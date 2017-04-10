﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LicenseServer.DataModel
{
    public class SubscriptionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int ActiveDays { get; set; }
        public double Price { get; set; }
        public string CreatedBy { get; set; }

        public IEnumerable<Product> Products { get; set; }
        
    }

    public class CustomSubscriptionType
    {
        public SubscriptionType SubscriptionType { get; set; }
        
        public bool AddSubscriptionToCart { get; set; }

        public string UserId { get; set; }
    }
}
