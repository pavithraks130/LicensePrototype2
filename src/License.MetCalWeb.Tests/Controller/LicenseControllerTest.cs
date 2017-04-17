using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;
using System.Web.Mvc;
using Moq;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class LicenseControllerTest
    {
        #region Initialization
        LicenseController licenseController = null;
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            licenseController = new LicenseController();
        }

        #endregion Initialization

        #region Test Methods

        [TestMethod]
        public void Index_Test()
        {
            var result = licenseController.Index() as ViewResult;
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsNotNull(result);
        }
        #endregion Test Methods

        #region Class CleanUp

        /// <summary>
        /// Use ClassCleanup to run code after all tests in a class have run
        /// </summary>
        [TestCleanup()]
        public void TestCleanup()
        {
            licenseController = null;
        }

        #endregion Class CleanUp
    }
}
