using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;
using System.Web.Mvc;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class UserControllerTest
    {
        #region Initialization
        UserController userController = null;
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            userController = new UserController();
        }

        #endregion Initialization

        #region Test Methods
       
        [TestMethod]
        public void ChangePassword_Test()
        {
            var result = userController.ChangePassword() as ViewResult;
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsNotNull(result.Model);
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
            userController = null;
        }

        #endregion Class CleanUp
    }
}
