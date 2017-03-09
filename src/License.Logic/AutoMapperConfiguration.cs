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
            cfg.CreateMap<License.Core.Model.Role, License.Model.Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<License.Model.Role, License.Core.Model.Role>();

            cfg.CreateMap<License.Core.Model.AppUser, License.Model.User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<License.Model.User, License.Core.Model.AppUser>();

            cfg.CreateMap<License.Core.Model.TeamMembers, Model.TeamMembers>()
                .ForMember(dest => dest.AdminUser, opt => opt.MapFrom(src => src.AdminUser))
                .ForMember(dest => dest.InviteeUser, opt => opt.MapFrom(src => src.InviteeUser));

            cfg.CreateMap<Model.TeamMembers, License.Core.Model.TeamMembers>()
                .ForMember(dest => dest.AdminUser, opt => opt.MapFrom(src => src.AdminUser))
                .ForMember(dest => dest.InviteeUser, opt => opt.MapFrom(src => src.InviteeUser));

            //cfg.CreateMap<Model.Product, Core.Model.Product>();
            //cfg.CreateMap<Core.Model.Product, Model.Product>();

            //cfg.CreateMap<Model.ProductSubscriptionMapping, Core.Model.ProductSubscriptionMapping>();
            //cfg.CreateMap<Core.Model.ProductSubscriptionMapping, Model.ProductSubscriptionMapping>();

            //cfg.CreateMap<Model.Subscription, Core.Model.Subscription>();
            //cfg.CreateMap<Core.Model.Subscription, Model.Subscription>();

            cfg.CreateMap<License.Core.Model.UserSubscription, License.Model.UserSubscription>();
            cfg.CreateMap<License.Model.UserSubscription, License.Core.Model.UserSubscription>();

            cfg.CreateMap<License.Core.Model.LicenseData, License.Model.LicenseData>();
            cfg.CreateMap<License.Model.LicenseData, License.Core.Model.LicenseData>();

            cfg.CreateMap<License.Core.Model.UserLicense, License.Model.UserLicense>();
            cfg.CreateMap<License.Model.UserLicense, License.Core.Model.UserLicense>();
        }

    }
}
