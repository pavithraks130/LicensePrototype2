using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LicenseServer.Logic;

namespace License.MetCalWeb.Tests.LicenseServerLogic
{
    [TestClass]
    public class OrganizationLogicTest
    {
        OrganizationLogic logic = new OrganizationLogic();
        [TestMethod]
        public void GetTeams()
        {
            InitializerClass.Initialize();
            var orgList = logic.GetTeams();
            Assert.IsTrue(orgList.Count > 0);
        }
    }
}
