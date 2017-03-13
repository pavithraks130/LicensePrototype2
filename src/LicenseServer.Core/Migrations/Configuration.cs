namespace LicenseServer.Core.Migrations
{
    using Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<LicenseServer.Core.DbContext.AppDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LicenseServer.Core.DbContext.AppDbContext context)
        {
            var dbIntialize = Convert.ToBoolean(System.Configuration.ConfigurationSettings.AppSettings.Get("IsDbIntialize"));
            if (dbIntialize)
            {
                var featureList1 = new LicenseServer.Core.Model.Features()
                {
                    FeatureId = 1,
                    Name = "Feature List1",
                    Description = "Contains Premium Feature List",
                };
                context.Features.Add(featureList1);

                var featureList2 = new LicenseServer.Core.Model.Features()
                {
                    FeatureId = 2,
                    Name = "Feature List2",
                    Description = "Contains M1,M2,M3 and M4",
                };
                context.Features.Add(featureList1);

                var featureList3 = new LicenseServer.Core.Model.Features()
                {
                    FeatureId = 3,
                    Name = "Feature List3",
                    Description = "Contains M1,M2,M3,M4 and M5",
                };
                context.Features.Add(featureList3);

                var featureList4 = new LicenseServer.Core.Model.Features()
                {
                    FeatureId = 4,
                    Name = "Feature List4",
                    Description = "Contains M1,M2 ,M3 M4,M5 and M6 product",
                };
                context.Features.Add(featureList4);

                var featureList5 = new LicenseServer.Core.Model.Features()
                {
                    FeatureId = 5,
                    Name = "Feature List5",
                    Description = "Contains M1,M2 ,M3 M4,M5,M6,M7 and M8 product",
                };
                context.Features.Add(featureList4);

                var pro1 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product A",
                    Description = "Product A",
                    ImagePath = "P1.png",
                    ProductCode = "ProO1",
                    AssociatedFeatures = new List<Features> { featureList1, featureList2 }
                };
                context.Product.Add(pro1);


                var sub1 = new LicenseServer.Core.Model.SubscriptionType()
                {
                    Name = "5Pack",
                    ActiveDays = 365,
                    Price = 500,
                    ImagePath = "P1.png"
                };
                context.SubscriptionType.Add(sub1);

                var subdetails = new LicenseServer.Core.Model.SubscriptionDetail()
                {
                    Product = pro1,
                    SubscriptyType = sub1,
                    Quantity = 5
                };
                context.SubscriptionDetail.Add(subdetails);

                var pro2 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product B",
                    Description = "Product B",
                    ImagePath = "P2.png",
                    ProductCode = "ProO2",
                    AssociatedFeatures = new List<Features> { featureList1, featureList2, featureList3 }

                };
                context.Product.Add(pro2);


                var sub2 = new LicenseServer.Core.Model.SubscriptionType()
                {
                    Name = "10Pack",
                    ActiveDays = 365,
                    Price = 1000,
                    ImagePath = "P2.png"
                };
                context.SubscriptionType.Add(sub2);

                var subdetails2 = new LicenseServer.Core.Model.SubscriptionDetail()
                {
                    Product = pro2,
                    SubscriptyType = sub2,
                    Quantity = 10
                };
                context.SubscriptionDetail.Add(subdetails2);

                var pro3 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product C",
                    Description = "Product C",
                    ImagePath = "P3.png",
                    ProductCode = "ProO3",
                    AssociatedFeatures = new List<Features> { featureList1, featureList2, featureList3, featureList4 }
                };
                context.Product.Add(pro3);


                var sub3 = new LicenseServer.Core.Model.SubscriptionType()
                {
                    Name = "15Pack",
                    ActiveDays = 365,
                    Price = 1500,
                    ImagePath = "P3.png"
                };
                context.SubscriptionType.Add(sub3);

                var subdetails3 = new LicenseServer.Core.Model.SubscriptionDetail()
                {
                    Product = pro3,
                    SubscriptyType = sub3,
                    Quantity = 15
                };
                context.SubscriptionDetail.Add(subdetails3);

                var pro4 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product D",
                    Description = "Product D",
                    ImagePath = "P4.png",
                    ProductCode = "ProO4",
                    AssociatedFeatures = new List<Features> { featureList1, featureList2, featureList3, featureList4, featureList5 }
                };
                context.Product.Add(pro4);


                var sub4 = new LicenseServer.Core.Model.SubscriptionType()
                {
                    Name = "1Pack",
                    ActiveDays = 365,
                    Price = 100,
                    ImagePath = "P4.png"
                };
                context.SubscriptionType.Add(sub4);

                var subdetails4 = new LicenseServer.Core.Model.SubscriptionDetail()
                {
                    Product = pro4,
                    SubscriptyType = sub4,
                    Quantity = 1
                };
                context.SubscriptionDetail.Add(subdetails4);

                context.SaveChanges();
            }
        }
    }
}
