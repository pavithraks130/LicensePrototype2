using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using License.DataModel;

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
            cfg.CreateMap<License.Core.Model.Role, License.DataModel.Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<License.DataModel.Role, License.Core.Model.Role>();

            cfg.CreateMap<License.Core.Model.AppUser, License.DataModel.User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<License.DataModel.User, License.Core.Model.AppUser>();

            cfg.CreateMap<License.Core.Model.TeamMember, TeamMember>()
                .ForMember(dest => dest.AdminUser, opt => opt.MapFrom(src => src.AdminUser))
                .ForMember(dest => dest.InviteeUser, opt => opt.MapFrom(src => src.InviteeUser));

            cfg.CreateMap<TeamMember, License.Core.Model.TeamMember>()
                .ForMember(dest => dest.AdminUser, opt => opt.MapFrom(src => src.AdminUser))
                .ForMember(dest => dest.InviteeUser, opt => opt.MapFrom(src => src.InviteeUser));

            //cfg.CreateMap<Model.Product, Core.Model.Product>();
            //cfg.CreateMap<Core.Model.Product, Model.Product>();

            //cfg.CreateMap<Model.ProductSubscriptionMapping, Core.Model.ProductSubscriptionMapping>();
            //cfg.CreateMap<Core.Model.ProductSubscriptionMapping, Model.ProductSubscriptionMapping>();

            //cfg.CreateMap<Model.Subscription, Core.Model.Subscription>();
            //cfg.CreateMap<Core.Model.Subscription, Model.Subscription>();

            cfg.CreateMap<License.Core.Model.UserSubscription, License.DataModel.UserSubscription>();
            cfg.CreateMap<License.DataModel.UserSubscription, License.Core.Model.UserSubscription>();

            cfg.CreateMap<License.Core.Model.LicenseData, License.DataModel.LicenseData>();
            cfg.CreateMap<License.DataModel.LicenseData, License.Core.Model.LicenseData>();

            cfg.CreateMap<License.Core.Model.UserLicense, License.DataModel.UserLicense>();
            cfg.CreateMap<License.DataModel.UserLicense, License.Core.Model.UserLicense>();


            cfg.CreateMap<License.Core.Model.UserLicenseRequest, License.DataModel.UserLicenseRequest>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.UserSubscripption, opt => opt.MapFrom(src => src.UserSubscripption));

            cfg.CreateMap<License.DataModel.UserLicenseRequest, License.Core.Model.UserLicenseRequest>()
               .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
               .ForMember(dest => dest.UserSubscripption, opt => opt.MapFrom(src => src.UserSubscripption));

            cfg.CreateMap<License.DataModel.TeamAsset, License.Core.Model.TeamAsset>();
            cfg.CreateMap<License.Core.Model.TeamAsset, License.DataModel.TeamAsset>();

        }

    }
}
