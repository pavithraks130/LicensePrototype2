using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace LicenseServer.Logic
{
    public static class Initializer
    {
        public static void AutoMapperInitializer()
        {
            AutoMapper.Mapper.Initialize(InitializeConfiguration);
        }

        public static void InitializeConfiguration(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<LicenseServer.Core.Model.Organization, LicenseServer.DataModel.Organization>();
            cfg.CreateMap<LicenseServer.DataModel.Organization, LicenseServer.Core.Model.Organization>();

            cfg.CreateMap<LicenseServer.Core.Model.AppRole, LicenseServer.DataModel.Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<LicenseServer.DataModel.Role, LicenseServer.Core.Model.AppRole>();

            cfg.CreateMap<LicenseServer.Core.Model.Appuser, LicenseServer.DataModel.User>();
            cfg.CreateMap<LicenseServer.DataModel.User, LicenseServer.Core.Model.Appuser>();

            cfg.CreateMap<LicenseServer.Core.Model.Product, LicenseServer.DataModel.Product>();
            cfg.CreateMap<LicenseServer.DataModel.Product, LicenseServer.Core.Model.Product>();

            cfg.CreateMap<LicenseServer.Core.Model.SubscriptionType, LicenseServer.DataModel.SubscriptionType>()
                .ForMember(dest => dest.SubDetails, opt => opt.MapFrom(src => src.SubDetails));
            cfg.CreateMap<LicenseServer.DataModel.SubscriptionType, LicenseServer.Core.Model.SubscriptionType>();

            cfg.CreateMap<LicenseServer.Core.Model.SubscriptionDetail, LicenseServer.DataModel.SubscriptionDetails>();
            cfg.CreateMap<LicenseServer.DataModel.SubscriptionDetails, LicenseServer.Core.Model.SubscriptionDetail>();

            cfg.CreateMap<LicenseServer.Core.Model.UserSubscription, LicenseServer.DataModel.UserSubscription>();
            cfg.CreateMap<LicenseServer.DataModel.UserSubscription, LicenseServer.Core.Model.UserSubscription>();

            cfg.CreateMap<LicenseServer.Core.Model.CartItem, LicenseServer.DataModel.CartItem>();
            cfg.CreateMap<LicenseServer.DataModel.CartItem, LicenseServer.Core.Model.CartItem>();

            License.Logic.AutoMapperConfiguration.InitializeConfiguration(cfg);
        }


    }
}
