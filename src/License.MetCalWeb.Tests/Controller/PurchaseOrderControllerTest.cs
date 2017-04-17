using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.MetCalWeb.Controllers;

namespace License.MetCalWeb.Tests.Controller
{
    [TestClass]
    public class PurchaseOrderControllerTest
    {
        #region Initialization
        PurchaseOrderController purchaseOrderController = null;
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            purchaseOrderController = new PurchaseOrderController();
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
            purchaseOrderController = null;
        }

        #endregion Class CleanUp
    }
}
