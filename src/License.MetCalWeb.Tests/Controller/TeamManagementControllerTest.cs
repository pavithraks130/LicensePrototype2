using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;
using System.Web.Mvc;
using Moq;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class TeamManagementControllerTest
    {
        #region Initialization
        TeamManagementController teamManagementController = null;
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            teamManagementController = new TeamManagementController();
        }

        #endregion Initialization

        #region Test Methods
       
        [TestMethod]
        public void Invite_Test()
        {
            var result = teamManagementController.Invite(It.IsAny<int>()) as ViewResult;
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.ViewBag);
        }
       
        #endregion Test Methods

        #region Class CleanUp

        /// <summary>
        /// Use ClassCleanup to run code after all tests in a class have run
        /// </summary>
        [TestCleanup()]
        public void TestCleanup()
        {
            teamManagementController = null;
        }

        #endregion Class CleanUp
    }
}
