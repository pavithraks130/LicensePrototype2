using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;
using Moq;
using System.Web.Mvc;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class CartControllerTest
    {
        #region Initialization
        CartController cartController = null;
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            cartController = new CartController();
        }

        #endregion Initialization

        #region Test Methods
        [TestMethod]
        public void PaymentGateway_Test()
        {
            var result = cartController.PaymentGateway(It.IsAny<string>())as ViewResult;
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsNotNull(result.ViewData);
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
            cartController = null;
        }

        #endregion Class CleanUp
    }
}
