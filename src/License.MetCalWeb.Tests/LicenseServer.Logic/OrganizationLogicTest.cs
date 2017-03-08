using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LicenseServer.Logic;

namespace License.MetCalWeb.Tests.LicenseServer.Logic
{
    [TestClass]
    public class OrganizationLogicTest
    {
        OrganizationLogic logic = new OrganizationLogic();
        [TestMethod]
        public void GetItems()
        {
            InitializerClass.Initialize();
            var orgList = logic.GetTeams();
            if (orgList.Count > 0)
            {
                Assert.IsNotNull(orgList);
            }
        }
    }
}
