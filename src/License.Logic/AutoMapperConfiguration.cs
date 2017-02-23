using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace License.Logic
{
    public class AutoMapperConfiguration
    {
        public static void InitializeAutoMapperConfiguration()
        {
            AutoMapper.Mapper.Initialize(AutoMapperConfiguration.InitializeConfiguration);
        }

        public static void InitializeConfiguration(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<License.Core.Model.Role, License.Model.Model.Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<License.Model.Model.Role, License.Core.Model.Role>();

            cfg.CreateMap<License.Core.Model.Team, License.Model.Model.Team>();
            cfg.CreateMap<License.Model.Model.Team, License.Core.Model.Team>();

            cfg.CreateMap<License.Core.Model.AppUser, License.Model.Model.User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<License.Model.Model.User, License.Core.Model.AppUser>();

            cfg.CreateMap<License.Core.Model.TeamMembers, Model.Model.TeamMembers>()
                .ForMember(dest => dest.AdminUser, opt => opt.MapFrom(src => src.AdminUser))
                .ForMember(dest => dest.InviteeUser, opt => opt.MapFrom(src => src.InviteeUser));
            cfg.CreateMap<Model.Model.TeamMembers, License.Core.Model.TeamMembers>()
                .ForMember(dest => dest.AdminUser, opt => opt.MapFrom(src => src.AdminUser))
                .ForMember(dest => dest.InviteeUser, opt => opt.MapFrom(src => src.InviteeUser));

        }

    }
}
