using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace License.MetCalWeb.Tests.LicenseServerLogic
{
    [TestClass]
    public class ProductLogicTest
    {
        ProductLogic logic = null;
        public ProductLogicTest()
        {
            logic = new ProductLogic();
            InitializerClass.Initialize();
        }

        [TestMethod]
        public void createProduct()
        {
            var status = logic.CreateProduct(new Product() { Name = "Pro1", ProductCode = "pRO-01", CreatedDate = DateTime.Now.Date.ToString(), Description = "Product one", ImagePath = "P1.png" });
            Assert.IsTrue(status);
        }
    }
}
