using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.Logic;
namespace License.MetCalWeb.Tests
{
    public static class InitializerClass
    {
        public static  void Initialize()
        {
            Initializer.AutoMapperInitializer();
        }
    }
}
