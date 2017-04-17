using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using License.MetCalWeb.Controllers;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class AccountControllerTest
    {

        AccountController accountController = null;

        #region Initialization
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            accountController = new AccountController();
        }
        #endregion Initialization

        #region Test Methods

        /// <summary>
        /// Register Http get request from user
        /// </summary>
        [TestMethod]
        public void Register_Http_Get_Test()
        {
            var result = accountController.Register() as ViewResult;
            Assert.AreEqual(false,result.ViewData["SucessMessageDisplay"]);
        }

        /// <summary>
        /// LogIn user HttpGet Test
        /// </summary>
        [TestMethod]
        public void LogIn_HttpGet_Test()
        {
            var result = accountController.LogIn() as ViewResult;
            Assert.AreEqual(string.Empty,result.ViewName);
            Assert.IsNotNull(result);
        }

        #endregion Test Methods

        #region Class CleanUp
        /// <summary>
        /// class level object cleanup
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            accountController = null;
        }
        #endregion Class CleanUp
    }
}
