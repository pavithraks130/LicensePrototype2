using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace License.Logic
{
    public class AutoMapperCoonfig
    {
        public static void InitializeConfiguration(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<License.Core.Model.Role, License.Model.Model.Role>();
            cfg.CreateMap<License.Model.Model.Role, License.Core.Model.Role>();

            cfg.CreateMap<License.Model.Model.Team, License.Core.Model.Team>();
            cfg.CreateMap<License.Core.Model.Team, License.Model.Model.Team>();

            cfg.CreateMap<License.Core.Model.AppUser, License.Model.Model.User>();
            cfg.CreateMap<License.Model.Model.User, License.Core.Model.AppUser>();
        }
    }
}
