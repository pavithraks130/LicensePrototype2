using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.Logic;
using LicenseServer.DataModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace License.MetCalWeb.Tests.LicenseServer.Logic
{
    [TestClass]
    public class UserTokenLogicTest
    {
        UserTokenLogic tokenLogic;

        public UserTokenLogicTest()
        {
            tokenLogic = new UserTokenLogic();
        }

        [TestMethod]
        public void CreateUserToke()
        {
            UserToken token = new UserToken();
            token.Email = "pavithraks2006@yahoo.com";
            LicenseKey.GenerateKey keyGen = new LicenseKey.GenerateKey();
            keyGen.LicenseTemplate = "xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";
            keyGen.UseBase10 = false;
            keyGen.UseBytes = false;
            keyGen.CreateKey();
            token.Token = keyGen.GetLicenseKey();
            var tokenObj = tokenLogic.CreateUserToken(token);
            Assert.IsTrue(tokenObj.Id > 0);
        }

       
    }
}
