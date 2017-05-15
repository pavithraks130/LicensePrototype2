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

                #region Biomedical Feature List

                var biomedicalFeature_01 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Full Suite",
                    Description = "It Contains full suite functionalities",
                    Version = "v1.0"


                };
                context.Feature.Add(biomedicalFeature_01);

                var biomedicalFeature_02 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Electrical Safety",
                    Description = "It contains electrical safety functionalities",
                    Version = "v1.0"


                };
                context.Feature.Add(biomedicalFeature_02);

                var biomedicalFeature_04 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Patient Simulation",
                    Description = "It contains patient simulation functionalities",
                    Version = "v1.0"


                };
                context.Feature.Add(biomedicalFeature_04);

                var biomedicalFeature_08 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Infusion Pump Verification",
                    Description = "It contains infusion pump verification functionalities",
                    Version = "v1.0"


                };
                context.Feature.Add(biomedicalFeature_08);
                #endregion Biomedical Feature List

                #region Calibration feature List
                var calibrationFeature_01 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Pressure Calibration",
                    Description = "It contains pressure calibration functionalities",
                    Version = "v1.0"


                };
                context.Feature.Add(calibrationFeature_01);

                var calibrationFeature_02 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Temperature Calibration",
                    Description = "It contains temperature calibration functionalities",
                    Version = "v1.0"


                };
                context.Feature.Add(calibrationFeature_02);

                var calibrationFeature_04 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Electrical Calibration",
                    Description = "It contains electrical calibration functionalities",
                    Version = "v1.0"
                };
                context.Feature.Add(calibrationFeature_04);

                var calibrationFeature_08 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Fluke 2638A Bundle",
                    Description = "It contains fluke 2638A functionalities",
                    Version = "v1.0"
                };
                context.Feature.Add(calibrationFeature_08);
                #endregion Calibration feature List

                #region CMMS Solution feature list

                var CMMSSolutionFeature_01 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "API Plug-in",
                    Description = "It contains api plugin functionalities",
                    Version = "v1.0"
                };
                context.Feature.Add(CMMSSolutionFeature_01);

                var CMMSSolutionFeature_02 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "MET/TEAM",
                    Description = "It contains MET/TEAM functionalities",
                    Version = "v1.0"
                };
                context.Feature.Add(CMMSSolutionFeature_02);

                var CMMSSolutionFeature_04 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "EMaint Link",
                    Description = "It contains EMaint Link functionalities",
                    Version = "v1.0"
                };
                context.Feature.Add(CMMSSolutionFeature_04);

                #endregion CMMS Solution feature list

                context.SaveChanges();

                #region Biomedical Product
                var biomedicalProduct_01 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_01",
                    Description = "biomedicalProduct_01",
                    ImagePath = "P1.png",
                    Price = 50,
                    ProductCode = "Pro_O1",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_01 }
                };
                context.Product.Add(biomedicalProduct_01);

                var biomedicalProduct_02 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_02",
                    Description = "biomedicalProduct_02",
                    ImagePath = "P1.png",
                    Price = 100,
                    ProductCode = "Pro_O2",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_02 }
                };
                context.Product.Add(biomedicalProduct_02);

                var biomedicalProduct_03 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_03",
                    Description = "biomedicalProduct_03",
                    ImagePath = "P1.png",
                    Price = 150,
                    ProductCode = "Pro_O3",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_02, biomedicalFeature_01 }
                };
                context.Product.Add(biomedicalProduct_03);

                var biomedicalProduct_04 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_04",
                    Description = "biomedicalProduct_04",
                    ImagePath = "P1.png",
                    Price = 200,
                    ProductCode = "Pro_O4",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_04 }
                };
                context.Product.Add(biomedicalProduct_04);

                var biomedicalProduct_05 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_05",
                    Description = "biomedicalProduct_05",
                    ImagePath = "P1.png",
                    Price = 250,
                    ProductCode = "Pro_O5",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_04, biomedicalFeature_01 }
                };
                context.Product.Add(biomedicalProduct_05);

                var biomedicalProduct_06 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_06",
                    Description = "biomedicalProduct_06",
                    ImagePath = "P1.png",
                    Price = 300,
                    ProductCode = "Pro_O6",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_04, biomedicalFeature_02 }
                };
                context.Product.Add(biomedicalProduct_06);

                var biomedicalProduct_07 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_07",
                    Description = "biomedicalProduct_07",
                    ImagePath = "P1.png",
                    Price = 350,
                    ProductCode = "Pro_O7",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_04, biomedicalFeature_02, biomedicalFeature_01 }
                };
                context.Product.Add(biomedicalProduct_07);

                var biomedicalProduct_08 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_08",
                    Description = "biomedicalProduct_08",
                    ImagePath = "P1.png",
                    Price = 400,
                    ProductCode = "Pro_O8",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08 }
                };
                context.Product.Add(biomedicalProduct_08);

                var biomedicalProduct_09 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_09",
                    Description = "biomedicalProduct_09",
                    ImagePath = "P1.png",
                    Price = 450,
                    ProductCode = "Pro_O9",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_01 }
                };
                context.Product.Add(biomedicalProduct_09);

                var biomedicalProduct_10 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_10",
                    Description = "biomedicalProduct_10",
                    ImagePath = "P1.png",
                    Price = 500,
                    ProductCode = "Pro_10",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_02 }
                };
                context.Product.Add(biomedicalProduct_10);

                var biomedicalProduct_11 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_11",
                    Description = "biomedicalProduct_11",
                    ImagePath = "P1.png",
                    Price = 550,
                    ProductCode = "Pro_11",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_02, biomedicalFeature_01 }
                };
                context.Product.Add(biomedicalProduct_11);

                var biomedicalProduct_12 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_12",
                    Description = "biomedicalProduct_12",
                    ImagePath = "P1.png",
                    Price = 600,
                    ProductCode = "Pro_12",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_04 }
                };
                context.Product.Add(biomedicalProduct_12);

                var biomedicalProduct_13 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_13",
                    Description = "biomedicalProduct_13",
                    ImagePath = "P1.png",
                    Price = 650,
                    ProductCode = "Pro_13",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_04, biomedicalFeature_01 }
                };
                context.Product.Add(biomedicalProduct_13);

                var biomedicalProduct_14 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_14",
                    Description = "biomedicalProduct_14",
                    ImagePath = "P1.png",
                    Price = 700,
                    ProductCode = "Pro_14",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_04, biomedicalFeature_02 }
                };
                context.Product.Add(biomedicalProduct_14);

                var biomedicalProduct_15 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_15",
                    Description = "biomedicalProduct_15",
                    ImagePath = "P1.png",
                    Price = 750,
                    ProductCode = "Pro_15",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_04, biomedicalFeature_02, biomedicalFeature_01 }
                };
                context.Product.Add(biomedicalProduct_15);

                #endregion Biomedical Product

                #region Calibration Product

                var calibrationProduct_01 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_01",
                    Description = "calibrationProduct_01",
                    ImagePath = "P1.png",
                    Price = 50,
                    ProductCode = "Pro_O1",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_01 }
                };
                context.Product.Add(calibrationProduct_01);

                var calibrationProduct_02 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_02",
                    Description = "calibrationProduct_02",
                    ImagePath = "P1.png",
                    Price = 100,
                    ProductCode = "Pro_O2",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_02 }
                };
                context.Product.Add(calibrationProduct_02);

                var calibrationProduct_03 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_03",
                    Description = "calibrationProduct_03",
                    ImagePath = "P1.png",
                    Price = 150,
                    ProductCode = "Pro_O3",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_02, calibrationFeature_01 }
                };
                context.Product.Add(calibrationProduct_03);

                var calibrationProduct_04 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_04",
                    Description = "calibrationProduct_04",
                    ImagePath = "P1.png",
                    Price = 200,
                    ProductCode = "Pro_O4",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_04 }
                };
                context.Product.Add(calibrationProduct_04);

                var calibrationProduct_05 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_05",
                    Description = "calibrationProduct_05",
                    ImagePath = "P1.png",
                    Price = 250,
                    ProductCode = "Pro_O5",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_04, calibrationFeature_01 }
                };
                context.Product.Add(calibrationProduct_05);

                var calibrationProduct_06 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_06",
                    Description = "calibrationProduct_06",
                    ImagePath = "P1.png",
                    Price = 300,
                    ProductCode = "Pro_O6",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_04, calibrationFeature_02 }
                };
                context.Product.Add(calibrationProduct_06);

                var calibrationProduct_07 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_07",
                    Description = "calibrationProduct_07",
                    ImagePath = "P1.png",
                    Price = 350,
                    ProductCode = "Pro_O7",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_04, calibrationFeature_02, calibrationFeature_01 }
                };
                context.Product.Add(calibrationProduct_07);

                var calibrationProduct_08 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_08",
                    Description = "calibrationProduct_08",
                    ImagePath = "P1.png",
                    Price = 400,
                    ProductCode = "Pro_O8",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08 }
                };
                context.Product.Add(calibrationProduct_08);

                var calibrationProduct_09 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_09",
                    Description = "calibrationProduct_09",
                    ImagePath = "P1.png",
                    Price = 450,
                    ProductCode = "Pro_O9",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_01 }
                };
                context.Product.Add(calibrationProduct_09);

                var calibrationProduct_10 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_10",
                    Description = "calibrationProduct_10",
                    ImagePath = "P1.png",
                    Price = 500,
                    ProductCode = "Pro_10",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_02 }
                };
                context.Product.Add(calibrationProduct_10);

                var calibrationProduct_11 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_11",
                    Description = "calibrationProduct_11",
                    ImagePath = "P1.png",
                    Price = 550,
                    ProductCode = "Pro_11",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_02, calibrationFeature_01 }
                };
                context.Product.Add(calibrationProduct_11);

                var calibrationProduct_12 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_12",
                    Description = "calibrationProduct_12",
                    ImagePath = "P1.png",
                    Price = 600,
                    ProductCode = "Pro_12",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_04 }
                };
                context.Product.Add(calibrationProduct_12);

                var calibrationProduct_13 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_13",
                    Description = "calibrationProduct_13",
                    ImagePath = "P1.png",
                    Price = 650,
                    ProductCode = "Pro_13",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_04, calibrationFeature_01 }
                };
                context.Product.Add(calibrationProduct_13);

                var calibrationProduct_14 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_14",
                    Description = "calibrationProduct_14",
                    ImagePath = "P1.png",
                    Price = 700,
                    ProductCode = "Pro_14",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_04, calibrationFeature_02 }
                };
                context.Product.Add(calibrationProduct_14);

                var calibrationProduct_15 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_15",
                    Description = "calibrationProduct_15",
                    ImagePath = "P1.png",
                    Price = 750,
                    ProductCode = "Pro_15",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_04, calibrationFeature_02, calibrationFeature_01 }
                };
                context.Product.Add(calibrationProduct_15);

                #endregion Calibration Product

                #region CMMS Solution Product

                var CMMSSolutionProduct_01 = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionProduct_01",
                    Description = "CMMSSolutionProduct_01",
                    ImagePath = "P1.png",
                    Price = 50,
                    ProductCode = "Pro_01",
                    AssociatedFeatures = new List<Feature> {CMMSSolutionFeature_01}
                };
                context.Product.Add(CMMSSolutionProduct_01);

                var CMMSSolutionProduct_02 = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionProduct_02",
                    Description = "CMMSSolutionProduct_02",
                    ImagePath = "P1.png",
                    Price = 100,
                    ProductCode = "Pro_02",
                    AssociatedFeatures = new List<Feature> {CMMSSolutionFeature_02 }
                };
                context.Product.Add(CMMSSolutionProduct_02);

                var CMMSSolutionProduct_03 = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionProduct_03",
                    Description = "CMMSSolutionProduct_03",
                    ImagePath = "P1.png",
                    Price = 150,
                    ProductCode = "Pro_02",
                    AssociatedFeatures = new List<Feature> { CMMSSolutionFeature_02,CMMSSolutionFeature_01}
                };
                context.Product.Add(CMMSSolutionProduct_03);

                var CMMSSolutionProduct_04 = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionProduct_04",
                    Description = "CMMSSolutionProduct_04",
                    ImagePath = "P1.png",
                    Price = 200,
                    ProductCode = "Pro_04",
                    AssociatedFeatures = new List<Feature> {CMMSSolutionFeature_04 }
                };
                context.Product.Add(CMMSSolutionProduct_04);

                var CMMSSolutionProduct_05 = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionProduct_05",
                    Description = "CMMSSolutionProduct_05",
                    ImagePath = "P1.png",
                    Price = 250,
                    ProductCode = "Pro_05",
                    AssociatedFeatures = new List<Feature> {CMMSSolutionFeature_04,CMMSSolutionFeature_01 }
                };
                context.Product.Add(CMMSSolutionProduct_05);

                var CMMSSolutionProduct_06 = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionProduct_06",
                    Description = "CMMSSolutionProduct_06",
                    ImagePath = "P1.png",
                    Price = 300,
                    ProductCode = "Pro_06",
                    AssociatedFeatures = new List<Feature> {CMMSSolutionFeature_04,CMMSSolutionFeature_02 }
                };
                context.Product.Add(CMMSSolutionProduct_06);

                var CMMSSolutionProduct_07 = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionProduct_07",
                    Description = "CMMSSolutionProduct_07",
                    ImagePath = "P1.png",
                    Price = 350,
                    ProductCode = "Pro_07",
                    AssociatedFeatures = new List<Feature> {CMMSSolutionFeature_04,CMMSSolutionFeature_02,CMMSSolutionFeature_01 }
                };
                context.Product.Add(CMMSSolutionProduct_07);

                #endregion CMMS Solution Product

                context.SaveChanges();

                //var calibrationSubscription = new LicenseServer.Core.Model.SubscriptionType()
                //{
                //    Name = "Calibration",
                //    ActiveDays = 365,
                //    Price = 500,
                //    ImagePath = "P2.png"
                //};
                //context.SubscriptionType.Add(calibrationSubscription);

                //var subdetails = new LicenseServer.Core.Model.SubscriptionDetail()
                //{
                //    Product = pro1,
                //    SubscriptyType = sub1,
                //    Quantity = 5
                //};
                //context.SubscriptionDetail.Add(subdetails);

                //var subdetails11 = new LicenseServer.Core.Model.SubscriptionDetail()
                //{
                //    Product = pro11,
                //    SubscriptyType = sub1,
                //    Quantity = 5
                //};
                //context.SubscriptionDetail.Add(subdetails11);




                //var bioMedicalSubscription = new LicenseServer.Core.Model.SubscriptionType()
                //{
                //    Name = "Biomedical",
                //    ActiveDays = 365,
                //    Price = 1000,
                //    ImagePath = "P2.png"
                //};
                //context.SubscriptionType.Add(bioMedicalSubscription);

                //var subdetails2 = new LicenseServer.Core.Model.SubscriptionDetail()
                //{
                //    Product = pro2,
                //    SubscriptyType = sub2,
                //    Quantity = 10
                //};
                //context.SubscriptionDetail.Add(subdetails2);



                //var diagnosticXray = new LicenseServer.Core.Model.SubscriptionType()
                //{
                //    Name = "Diagnostic Xray",
                //    ActiveDays = 365,
                //    Price = 1500,
                //    ImagePath = "P3.png"
                //};
                //context.SubscriptionType.Add(diagnosticXray);

                //var subdetails3 = new LicenseServer.Core.Model.SubscriptionDetail()
                //{
                //    Product = pro3,
                //    SubscriptyType = sub3,
                //    Quantity = 15
                //};
                //context.SubscriptionDetail.Add(subdetails3);



                //var sub4 = new LicenseServer.Core.Model.SubscriptionType()
                //{
                //    Name = "1Pack",
                //    ActiveDays = 365,
                //    Price = 100,
                //    ImagePath = "P4.png"
                //};
                //context.SubscriptionType.Add(sub4);

                //var subdetails4 = new LicenseServer.Core.Model.SubscriptionDetail()
                //{
                //    Product = pro4,
                //    SubscriptyType = sub4,
                //    Quantity = 1
                //};
                //context.SubscriptionDetail.Add(subdetails4);

               // context.SaveChanges();
            }

        }
    }
}
