using Microsoft.VisualStudio.TestTools.UnitTesting;
using License.Logic.ServiceLogic;
using License.Core.GenericRepository;
using Moq;
using License.Core.Manager;
using Microsoft.AspNet.Identity.EntityFramework;
using License.Core.Model;
using License.Core.DBContext;
using System.Collections.Generic;
using System.Linq;
using License.MetCalWeb.Tests;
using Microsoft.AspNet.Identity;

namespace License.Logic.Test
{
    [TestClass]
    public class UserLogicTest
    {

        UserLogic userLogic = null;
        Mock<UnitOfWork> unitOfWork = null;
        UserStore<AppUser> userStore = null;
        RoleStore<Role> roleStore = null;
        Mock<AppUserManager> userManager;
        Mock<AppRoleManager> rolemanager;
        List<AppUser> usersList = null;
        Mock<RoleLogic> roleLogic = null;


        #region Initialization
        /// <summary>
        /// Test Initializations
        /// </summary>
        [TestInitialize]
        public void Init()
        {
            InitializerClass.Initialize();
           
            BaseLogicSetup();

            userLogic = new UserLogic();
            roleLogic = new Mock<RoleLogic>();
        }
        #endregion Initialization

        #region Test Methods

        /// <summary>
        /// Get User count 
        /// </summary>
        [TestMethod]
        public void GetUser()
        {
            UserListSetup();
            userManager = new Moq.Mock<AppUserManager>(userStore);
            userManager.Setup(x => x.Users).Returns(usersList.AsQueryable());
            var users = userLogic.GetUsers();

            Assert.IsNotNull(users);
            Assert.AreEqual(2, users.Count);
        }

        //[TestMethod]
        //public void CreateUser()
        //{
        //    var reg = CreateUserSetup();
        //    roleLogic.Setup(x => x.CreateRole(It.IsAny<Model.Role>())).Returns(reg);
        //    var result = userLogic.CreateUser(reg);
        //    Assert.AreEqual("true", result.Succeeded);

        //}
        #endregion Test Methods

        #region Private Methods
        private void UserListSetup()
        {
            usersList = new List<AppUser>();
            AppUser user = new AppUser()
            {
                FirstName = "Charan",
                LastName = "Shetty",
                Email = "Charan.shetty@fluke.com",
                ManagerId = "2",

            };

            AppUser user2 = new AppUser()
            {
                // UserId ="1",
                FirstName = "Pavitra",
                LastName = "",
                Email = "Pavitra.aa@fluke.com",
                ManagerId = "2",

            };
            usersList.Add(user);
            usersList.Add(user2);
        }

        /// <summary>
        /// Base class setup
        /// </summary>
        private void BaseLogicSetup()
        {
            unitOfWork = new Mock<UnitOfWork>();

            //Not able mock this db instance . It will throw TypeInitializationException.
            // var applicationDBContext = new Mock<ApplicationDbContext>();
            var applicationDBContext = ApplicationDbContext.Create();
            userStore = new UserStore<AppUser>(applicationDBContext);
            roleStore = new RoleStore<Core.Model.Role>(applicationDBContext);
            rolemanager = new Moq.Mock<AppRoleManager>(roleStore);

            userLogic.UserManager = userManager.Object;
            userLogic.RoleManager = rolemanager.Object;
        }
        //private IdentityResult CreateUserSetup()
        //{
        //    IdentityResult identityResult = new IdentityResult();
        //    Model.Registration reg = new Model.Registration();
        //    reg.FirstName = "veeresh";
        //    reg.LastName = "S";
        //    reg.OrganizationName = "sidssol";
        //    reg.Email = "veereshrdrpp@gmail.com";
        //    reg.Password = "Test@1234";
        //    reg.PhoneNumber = "1234567890";
           
        //    return reg;
        //}

        #endregion Private Methods 

        #region Class CleanUp
        /// <summary>
        /// class level object cleanup
        /// </summary>
        [TestCleanup]
        public void CleanUp()
        {
            userLogic = null;
        }
        #endregion Class CleanUp
    }
}
