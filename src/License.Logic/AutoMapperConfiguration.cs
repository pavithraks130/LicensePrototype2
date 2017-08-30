using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using License.Models;

namespace License.Logic
{
    /// <summary>
    /// Buisines Logic Model and Data Logic Model property assigning 
    /// </summary>
    public class AutoMapperConfiguration
    {
        public static void InitializeAutoMapperConfiguration()
        {
            AutoMapper.Mapper.Initialize(AutoMapperConfiguration.InitializeConfiguration);
        }

        public static void InitializeConfiguration(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<License.Core.Model.Role, Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<Role, License.Core.Model.Role>();

            cfg.CreateMap<License.Core.Model.AppUser, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<User, License.Core.Model.AppUser>();

            cfg.CreateMap<License.Core.Model.TeamMember, TeamMember>()
                .ForMember(dest => dest.InviteeUser, opt => opt.MapFrom(src => src.InviteeUser));

            cfg.CreateMap<TeamMember, License.Core.Model.TeamMember>()
                .ForMember(dest => dest.InviteeUser, opt => opt.MapFrom(src => src.InviteeUser));

            cfg.CreateMap<License.Core.Model.UserSubscription, UserSubscription>();
            cfg.CreateMap<UserSubscription, License.Core.Model.UserSubscription>();

            cfg.CreateMap<License.Core.Model.ProductLicense, ProductLicense>();
            cfg.CreateMap<ProductLicense, License.Core.Model.ProductLicense>();

            cfg.CreateMap<License.Core.Model.UserLicense, UserLicense>();
            cfg.CreateMap<UserLicense, License.Core.Model.UserLicense>();

            cfg.CreateMap<License.Core.Model.TeamLicense, TeamLicense>();
            cfg.CreateMap<TeamLicense, License.Core.Model.TeamLicense>();


            cfg.CreateMap<License.Core.Model.UserLicenseRequest, UserLicenseRequest>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            cfg.CreateMap<UserLicenseRequest, License.Core.Model.UserLicenseRequest>()
               .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            cfg.CreateMap<TeamAsset, License.Core.Model.TeamAsset>();
            cfg.CreateMap<License.Core.Model.TeamAsset, TeamAsset>();

            cfg.CreateMap<VISMAData, License.Core.Model.VISMAData >();
            cfg.CreateMap<License.Core.Model.VISMAData , VISMAData>();

            cfg.CreateMap<Team, Core.Model.Team>();
            cfg.CreateMap<Core.Model.Team, Team>().MaxDepth(3)
                .ForMember(dest => dest.AdminUser, opt => opt.MapFrom(src => src.AdminUser));

            cfg.CreateMap<Core.Model.ClientAppVerificationSettings, ClientAppVerificationSettings>();
                //.ForMember(dest => dest.ApplicationCode, opt => opt.MapFrom(src => (ApplicationCodeType)Enum.Parse(typeof(ApplicationCodeType), src.ApplicationCode)));
            cfg.CreateMap<ClientAppVerificationSettings, Core.Model.ClientAppVerificationSettings>();
                //.ForMember(opt => opt.ApplicationCode, opt => opt.MapFrom(dest => dest.ApplicationCode.ToString()));

        }

    }
}
