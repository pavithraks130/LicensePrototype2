namespace LicenseServer.Core.Migrations
{
    using System;
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
                var pro1 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product A",
                    Description = "Product A",
                    ImagePath = "P1.png",
                    ProductCode = "ProO1"
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
                    ProductCode = "ProO2"
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
                    SubscriptyType=sub1,
                    Quantity = 10
                };
                context.SubscriptionDetail.Add(subdetails2);

                var pro3 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product C",
                    Description = "Product C",
                    ImagePath = "P3.png",
                    ProductCode = "ProO3"
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
                    ProductCode = "ProO4"
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
