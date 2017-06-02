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

                var category1 = new ProductCategory() { Name = "Calibration", Description = "Best in class calibration solution." };
                var category2 = new ProductCategory() { Name = "Bio Medical", Description = "Best in class biomedical automation solution." };

                context.ProductCategory.Add(category1);
                context.ProductCategory.Add(category2);

                context.SaveChanges();


                #region Biomedical Feature List

                var biomedicalFeature_01 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Full Suite",
                    Description = "It Contains full suite functionalities",
                    Version = "v1.0",
                    price = 30,
                    Caategory = category2,
                   
                   
                    
                };
                context.Feature.Add(biomedicalFeature_01);

                var biomedicalFeature_02 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Electrical Safety",
                    Description = "It contains electrical safety functionalities",
                    Version = "v1.0",
                    price=40,
                    Caategory = category2
                    

                };
                context.Feature.Add(biomedicalFeature_02);

                var biomedicalFeature_04 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Patient Simulation",
                    Description = "It contains patient simulation functionalities",
                    Version = "v1.0",
                    price = 50,
                    Caategory = category2


                };
                context.Feature.Add(biomedicalFeature_04);

                var biomedicalFeature_08 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Infusion Pump Verification",
                    Description = "It contains infusion pump verification functionalities",
                    Version = "v1.0",
                    price = 70,
                    Caategory = category2


                };
                context.Feature.Add(biomedicalFeature_08);
                #endregion Biomedical Feature List

                #region Calibration feature List
                var calibrationFeature_01 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Pressure Calibration",
                    Description = "It contains pressure calibration functionalities",
                    Version = "v1.0",
                    price = 30,
                    Caategory = category1


                };
                context.Feature.Add(calibrationFeature_01);

                var calibrationFeature_02 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Temperature Calibration",
                    Description = "It contains temperature calibration functionalities",
                    Version = "v1.0",
                    price = 40,
                    Caategory = category1


                };
                context.Feature.Add(calibrationFeature_02);

                var calibrationFeature_04 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Electrical Calibration",
                    Description = "It contains electrical calibration functionalities",
                    Version = "v1.0",
                    price = 50,
                    Caategory = category1
                };
                context.Feature.Add(calibrationFeature_04);

                var calibrationFeature_08 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "Fluke 2638A Bundle",
                    Description = "It contains fluke 2638A functionalities",
                    Version = "v1.0",
                    price = 70,
                    Caategory = category1
                };
                context.Feature.Add(calibrationFeature_08);
                #endregion Calibration feature List

                #region CMMS Solution feature list

                var CMMSSolutionFeature_01 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "CMMSSolutionFeature_01",
                    Description = "It contains CMMSSolution Feature_01 functionalities",
                    price = 30,
                    Version = "v1.0"
                };
                context.Feature.Add(CMMSSolutionFeature_01);

                var CMMSSolutionFeature_02 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "CMMSSolutionFeature_02",
                    Description = "It contains CMMSSolution Feature_02 functionalities",
                    price = 50,
                    Version = "v1.0"
                };
                context.Feature.Add(CMMSSolutionFeature_02);

                var CMMSSolutionFeature_04 = new LicenseServer.Core.Model.Feature()
                {
                    Name = "CMMSSolutionFeature_04",
                    Description = "It contains CMMSSolution Feature_04 functionalities",
                    price = 70,
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
                    ImagePath = "P_2.PNG",
                    Price = 50,
                    ProductCode = "Pro_O1",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_01.Categories = new List<ProductCategory>();
                biomedicalProduct_01.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_01);

                var biomedicalProduct_02 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_02",
                    Description = "biomedicalProduct_02",
                    ImagePath = "P_2.PNG",
                    Price = 100,
                    ProductCode = "Pro_O2",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_02.Categories = new List<ProductCategory>();
                biomedicalProduct_02.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_02);

                var biomedicalProduct_03 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_03",
                    Description = "biomedicalProduct_03",
                    ImagePath = "P_2.PNG",
                    Price = 150,
                    ProductCode = "Pro_O3",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_02, biomedicalFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_03.Categories = new List<ProductCategory>();
                biomedicalProduct_03.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_03);

                var biomedicalProduct_04 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_04",
                    Description = "biomedicalProduct_04",
                    ImagePath = "P_2.PNG",
                    Price = 200,
                    ProductCode = "Pro_O4",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_04 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_04.Categories = new List<ProductCategory>();
                biomedicalProduct_04.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_04);

                var biomedicalProduct_05 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_05",
                    Description = "biomedicalProduct_05",
                    ImagePath = "P_2.PNG",
                    Price = 250,
                    ProductCode = "Pro_O5",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_04, biomedicalFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_05.Categories = new List<ProductCategory>();
                biomedicalProduct_05.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_05);

                var biomedicalProduct_06 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_06",
                    Description = "biomedicalProduct_06",
                    ImagePath = "P_2.PNG",
                    Price = 300,
                    ProductCode = "Pro_O6",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_04, biomedicalFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_06.Categories = new List<ProductCategory>();
                biomedicalProduct_06.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_06);

                var biomedicalProduct_07 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_07",
                    Description = "biomedicalProduct_07",
                    ImagePath = "P_2.PNG",
                    Price = 350,
                    ProductCode = "Pro_O7",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_04, biomedicalFeature_02, biomedicalFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_07.Categories = new List<ProductCategory>();
                biomedicalProduct_07.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_07);

                var biomedicalProduct_08 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_08",
                    Description = "biomedicalProduct_08",
                    ImagePath = "P_2.PNG",
                    Price = 400,
                    ProductCode = "Pro_O8",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_08.Categories = new List<ProductCategory>();
                biomedicalProduct_08.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_08);

                var biomedicalProduct_09 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_09",
                    Description = "biomedicalProduct_09",
                    ImagePath = "P_2.PNG",
                    Price = 450,
                    ProductCode = "Pro_O9",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_09.Categories = new List<ProductCategory>();
                biomedicalProduct_09.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_09);

                var biomedicalProduct_10 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_10",
                    Description = "biomedicalProduct_10",
                    ImagePath = "P_2.PNG",
                    Price = 500,
                    ProductCode = "Pro_10",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_10.Categories = new List<ProductCategory>();
                biomedicalProduct_10.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_10);

                var biomedicalProduct_11 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_11",
                    Description = "biomedicalProduct_11",
                    ImagePath = "P_2.PNG",
                    Price = 550,
                    ProductCode = "Pro_11",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_02, biomedicalFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_11.Categories = new List<ProductCategory>();
                biomedicalProduct_11.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_11);

                var biomedicalProduct_12 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_12",
                    Description = "biomedicalProduct_12",
                    ImagePath = "P_2.PNG",
                    Price = 600,
                    ProductCode = "Pro_12",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_04 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_12.Categories = new List<ProductCategory>();
                biomedicalProduct_12.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_12);

                var biomedicalProduct_13 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_13",
                    Description = "biomedicalProduct_13",
                    ImagePath = "P_2.PNG",
                    Price = 650,
                    ProductCode = "Pro_13",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_04, biomedicalFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_13.Categories = new List<ProductCategory>();
                biomedicalProduct_13.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_13);

                var biomedicalProduct_14 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_14",
                    Description = "biomedicalProduct_14",
                    ImagePath = "P_2.PNG",
                    Price = 700,
                    ProductCode = "Pro_14",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_04, biomedicalFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_14.Categories = new List<ProductCategory>();
                biomedicalProduct_14.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_14);

                var biomedicalProduct_15 = new LicenseServer.Core.Model.Product()
                {
                    Name = "biomedicalProduct_15",
                    Description = "biomedicalProduct_15",
                    ImagePath = "P_2.PNG",
                    Price = 750,
                    ProductCode = "Pro_15",
                    AssociatedFeatures = new List<Feature> { biomedicalFeature_08, biomedicalFeature_04, biomedicalFeature_02, biomedicalFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                biomedicalProduct_15.Categories = new List<ProductCategory>();
                biomedicalProduct_15.Categories.Add(category2);
                context.Product.Add(biomedicalProduct_15);

                #endregion Biomedical Product

                #region Calibration Product

                var calibrationProduct_01 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_01",
                    Description = "calibrationProduct_01",
                    ImagePath = "P_2.PNG",
                    Price = 50,
                    ProductCode = "Pro_16",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_01.Categories = new List<ProductCategory>();
                calibrationProduct_01.Categories.Add(category1);
                context.Product.Add(calibrationProduct_01);

                var calibrationProduct_02 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_02",
                    Description = "calibrationProduct_02",
                    ImagePath = "P_2.PNG",
                    Price = 100,
                    ProductCode = "Pro_17",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_02.Categories = new List<ProductCategory>();
                calibrationProduct_02.Categories.Add(category1);
                context.Product.Add(calibrationProduct_02);

                var calibrationProduct_03 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_03",
                    Description = "calibrationProduct_03",
                    ImagePath = "P_2.PNG",
                    Price = 150,
                    ProductCode = "Pro_18",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_02, calibrationFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_03.Categories = new List<ProductCategory>();
                calibrationProduct_03.Categories.Add(category1);
                context.Product.Add(calibrationProduct_03);

                var calibrationProduct_04 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_04",
                    Description = "calibrationProduct_04",
                    ImagePath = "P_2.PNG",
                    Price = 200,
                    ProductCode = "Pro_19",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_04 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_04.Categories = new List<ProductCategory>();
                calibrationProduct_04.Categories.Add(category1);
                context.Product.Add(calibrationProduct_04);

                var calibrationProduct_05 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_05",
                    Description = "calibrationProduct_05",
                    ImagePath = "P_2.PNG",
                    Price = 250,
                    ProductCode = "Pro_20",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_04, calibrationFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_05.Categories = new List<ProductCategory>();
                calibrationProduct_05.Categories.Add(category1);
                context.Product.Add(calibrationProduct_05);

                var calibrationProduct_06 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_06",
                    Description = "calibrationProduct_06",
                    ImagePath = "P_2.PNG",
                    Price = 300,
                    ProductCode = "Pro_21",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_04, calibrationFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_06.Categories = new List<ProductCategory>();
                calibrationProduct_06.Categories.Add(category1);
                context.Product.Add(calibrationProduct_06);

                var calibrationProduct_07 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_07",
                    Description = "calibrationProduct_07",
                    ImagePath = "P_2.PNG",
                    Price = 350,
                    ProductCode = "Pro_22",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_04, calibrationFeature_02, calibrationFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_07.Categories = new List<ProductCategory>();
                calibrationProduct_07.Categories.Add(category1);
                context.Product.Add(calibrationProduct_07);

                var calibrationProduct_08 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_08",
                    Description = "calibrationProduct_08",
                    ImagePath = "P_2.PNG",
                    Price = 400,
                    ProductCode = "Pro_23",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_08.Categories = new List<ProductCategory>();
                calibrationProduct_08.Categories.Add(category1);
                context.Product.Add(calibrationProduct_08);

                var calibrationProduct_09 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_09",
                    Description = "calibrationProduct_09",
                    ImagePath = "P_2.PNG",
                    Price = 450,
                    ProductCode = "Pro_24",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_09.Categories = new List<ProductCategory>();
                calibrationProduct_09.Categories.Add(category1);
                context.Product.Add(calibrationProduct_09);

                var calibrationProduct_10 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_10",
                    Description = "calibrationProduct_10",
                    ImagePath = "P_2.PNG",
                    Price = 500,
                    ProductCode = "Pro_25",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_10.Categories = new List<ProductCategory>();
                calibrationProduct_10.Categories.Add(category1);
                context.Product.Add(calibrationProduct_10);

                var calibrationProduct_11 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_11",
                    Description = "calibrationProduct_11",
                    ImagePath = "P_2.PNG",
                    Price = 550,
                    ProductCode = "Pro_26",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_02, calibrationFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_11.Categories = new List<ProductCategory>();
                calibrationProduct_11.Categories.Add(category1);
                context.Product.Add(calibrationProduct_11);

                var calibrationProduct_12 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_12",
                    Description = "calibrationProduct_12",
                    ImagePath = "P_2.PNG",
                    Price = 600,
                    ProductCode = "Pro_27",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_04 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_12.Categories = new List<ProductCategory>();
                calibrationProduct_12.Categories.Add(category1);
                context.Product.Add(calibrationProduct_12);

                var calibrationProduct_13 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_13",
                    Description = "calibrationProduct_13",
                    ImagePath = "P_2.PNG",
                    Price = 650,
                    ProductCode = "Pro_28",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_04, calibrationFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_13.Categories = new List<ProductCategory>();
                calibrationProduct_13.Categories.Add(category1);
                context.Product.Add(calibrationProduct_13);

                var calibrationProduct_14 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_14",
                    Description = "calibrationProduct_14",
                    ImagePath = "P_2.PNG",
                    Price = 700,
                    ProductCode = "Pro_29",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_04, calibrationFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_14.Categories = new List<ProductCategory>();
                calibrationProduct_14.Categories.Add(category1);
                context.Product.Add(calibrationProduct_14);

                var calibrationProduct_15 = new LicenseServer.Core.Model.Product()
                {
                    Name = "calibrationProduct_15",
                    Description = "calibrationProduct_15",
                    ImagePath = "P_2.PNG",
                    Price = 750,
                    ProductCode = "Pro_30",
                    AssociatedFeatures = new List<Feature> { calibrationFeature_08, calibrationFeature_04, calibrationFeature_02, calibrationFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                calibrationProduct_15.Categories = new List<ProductCategory>();
                calibrationProduct_15.Categories.Add(category1);
                context.Product.Add(calibrationProduct_15);

                #endregion Calibration Product

                #region CMMS Solution Product

                var CMMSSolutionAPIPlugIn = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionAPI PlugIn",
                    Description = "CMMSSolution API PlugIn product",
                    ImagePath = "P_2.PNG",
                    Price = 50,
                    ProductCode = "Pro_31",
                    AssociatedFeatures = new List<Feature> { CMMSSolutionFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                context.Product.Add(CMMSSolutionAPIPlugIn);

                var CMMSSolutionMETorTEAM = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolution MET/TEAM",
                    Description = "CMMSSolution MET/TEAM product",
                    ImagePath = "P_2.PNG",
                    Price = 100,
                    ProductCode = "Pro_32",
                    AssociatedFeatures = new List<Feature> { CMMSSolutionFeature_02 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                context.Product.Add(CMMSSolutionMETorTEAM);

                var CMMSSolutionEMaintLink = new LicenseServer.Core.Model.Product()
                {
                    Name = "CMMSSolutionEMaintLink",
                    Description = "CMMSSolution EMaint Link Product",
                    ImagePath = "P_2.PNG",
                    Price = 150,
                    ProductCode = "Pro_33",
                    AssociatedFeatures = new List<Feature> { CMMSSolutionFeature_02, CMMSSolutionFeature_01 },
                    ModifiedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                context.Product.Add(CMMSSolutionEMaintLink);

                #endregion CMMS Solution Product

                context.SaveChanges();

               


            }

        }
    }
}
