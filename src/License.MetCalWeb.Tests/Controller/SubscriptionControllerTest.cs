using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;
using System.Web.Mvc;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class SubscriptionControllerTest
    {
        #region Initialization
        SubscriptionController subscriptionController = null;
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            subscriptionController = new SubscriptionController();
        }

        #endregion Initialization

        #region Test Methods


        #endregion Test Methods

        #region Class CleanUp

        /// <summary>
        /// Use ClassCleanup to run code after all tests in a class have run
        /// </summary>
        [TestCleanup()]
        public void TestCleanup()
        {
            subscriptionController = null;
        }

        #endregion Class CleanUp
    }
}

