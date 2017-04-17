using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;
using System.Web.Mvc;
using Moq;

namespace License.MetCalWeb.Tests.Controller
{
    /// <summary>
    /// Team Controller unit Test
    /// </summary>
    [TestClass]
    public class TeamControllerTest
    {

        #region Initialization

        TeamController teamController = null;

        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            teamController = new TeamController();
        }

        #endregion Initialization

        #region Test Methods

        [TestMethod]
        public void CreateTeam_Test()
        {
            var result = teamController.CreateTeam() as ViewResult;
            Assert.AreEqual("Create", result.ViewName);
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
            teamController = null;
        }

        #endregion Class CleanUp

    }
}
