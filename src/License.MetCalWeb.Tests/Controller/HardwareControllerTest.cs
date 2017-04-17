using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;
using System.Web.Mvc;
using Moq;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class HardwareControllerTest
    {
        #region Initialization
        HardwareController hardwareController = null;
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            hardwareController = new HardwareController();
        }

        #endregion Initialization

        #region Test Methods
        [TestMethod]
        public void AddHardware_Test()
        {
            var result = hardwareController.AddHardware() as ViewResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
        }
        #endregion Test Methods

        #region Class CleanUp

        /// <summary>
        /// Use ClassCleanup to run code after all tests in a class have run
        /// </summary>
        [TestCleanup()]
        public void TestCleanup()
        {
            hardwareController = null;
        }

        #endregion Class CleanUp
    }
}
