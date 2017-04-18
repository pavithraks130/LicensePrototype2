using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;
using System.Web.Mvc;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class UserTokenControllerTest
    {
        #region Initialization
        UserTokenController userTokenController = null;
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            userTokenController = new UserTokenController();
        }

        #endregion Initialization

        #region Test Methods
       
        [TestMethod]
        public void CreateToken_Test()
        {
            var result = userTokenController.CreateToken() as ViewResult;
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
            userTokenController = null;
        }

        #endregion Class CleanUp
    }
}
