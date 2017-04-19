namespace LicenseServer.Core.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Collections;
    using System.Collections.Generic;
    using Microsoft.AspNet.Identity;
    using LicenseServer.Core.DbContext;
    using LicenseServer.Core.Manager;
    using Microsoft.AspNet.Identity.EntityFramework;
    using LicenseServer.Core.Model;
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
                LicUserManager usermanager = LicUserManager.Create(context);
                LicRoleManager roleManager = LicRoleManager.Create(context);

                Organization org = new Organization();
                org.Name = "Fluke";
                org = context.Organization.Add(org);
                context.SaveChanges();
                Appuser user = new Appuser();
                string roleName = "BackendAdmin";

                user.FirstName = "admin";
                user.Email = System.Configuration.ConfigurationSettings.AppSettings.Get("AdminUserName");
                user.UserName = user.Email;
                user.OrganizationId = org.Id;
                var result = usermanager.Create(user, System.Configuration.ConfigurationSettings.AppSettings.Get("AdminPassword"));

                if (roleManager.FindByName(roleName) == null)
                    roleManager.Create(new AppRole() { Name = roleName });

                user = usermanager.FindByEmail(user.Email);
                usermanager.AddToRole(user.UserId, roleName);

                var feature1 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Feature-I",
                    Description = "Contains basics features .",
                    Version = "V1.0"
                    
                };
                context.Feature.Add(feature1);

                var feature2 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Feature-II",
                    Description = "Contains M-1 and M-2 features.",
                    Version = "V1.0"
                };
                context.Feature.Add(feature2);

                var feature3 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Feature-III",
                    Description = "Contains M-3 and M-4 features.",
                    Version = "V1.0"
                };
                context.Feature.Add(feature3);

                var feature4 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Feature-IV",
                    Description = "Contains M-5 and M-6 features.",
                    Version = "V1.0"
                };
                context.Feature.Add(feature4);

                var feature5 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Feature-I",
                    Description = "Contains basics features .",
                    Version = "V1.0"

                };
                context.Feature.Add(feature5);

                var feature6 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Feature-II",
                    Description = "Contains M-1 and M-2 features.",
                    Version = "V1.0"
                };
                context.Feature.Add(feature6);

                var feature7 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Feature-III",
                    Description = "Contains M-3 and M-4 features.",
                    Version = "V1.0"
                };
                context.Feature.Add(feature7);

                var featureList8 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Feature-IV",
                    Description = "Contains M-5 and M-6 features.",
                    Version = "V1.0"
                };
                context.Feature.Add(featureList8);


                var pro1 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product A",
                    Description = "Product A",
                    ImagePath = "P1.png",
                    Price=50,
                    ProductCode = "ProO1",
                    AssociatedFeatures = new List<Feature> { feature1, feature2 }
                };
                context.Product.Add(pro1);

                var pro11 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product A1",
                    Description = "Product A1",
                    ImagePath = "P1.png",
                    Price = 100,
                    ProductCode = "Pro11",
                    AssociatedFeatures = new List<Feature> { feature5, feature6 }
                };
                context.Product.Add(pro11);


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

                var subdetails11 = new LicenseServer.Core.Model.SubscriptionDetail()
                {
                    Product = pro11,
                    SubscriptyType = sub1,
                    Quantity = 5
                };
                context.SubscriptionDetail.Add(subdetails11);

                var pro2 = new LicenseServer.Core.Model.Product()
                {
                    Name = "Product B",
                    Description = "Product B",
                    ImagePath = "P2.png",
                    Price = 150,
                    ProductCode = "ProO2",
                    AssociatedFeatures = new List<Feature> { feature1, feature2, feature3 }

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
                    Price = 200,
                    ProductCode = "ProO3",
                    AssociatedFeatures = new List<Feature> { feature1, feature2, feature3, feature4 }
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
                    Price = 250,
                    ProductCode = "ProO4",
                    AssociatedFeatures = new List<Feature> { feature1, feature2, feature3, feature4, feature5 }
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
