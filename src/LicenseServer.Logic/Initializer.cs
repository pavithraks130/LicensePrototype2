using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using License.Models;

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
            cfg.CreateMap<LicenseServer.Core.Model.Organization, Organization>();
            cfg.CreateMap<Organization, Core.Model.Organization>();

            cfg.CreateMap<LicenseServer.Core.Model.AppRole, Role>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.Id));
            cfg.CreateMap<Role, LicenseServer.Core.Model.AppRole>();

            cfg.CreateMap<LicenseServer.Core.Model.Appuser, User>();
            cfg.CreateMap<User, LicenseServer.Core.Model.Appuser>();

            cfg.CreateMap<LicenseServer.Core.Model.SubscriptionCategory, SubscriptionCategory>();
            cfg.CreateMap<SubscriptionCategory, LicenseServer.Core.Model.SubscriptionCategory>();

            cfg.CreateMap<LicenseServer.Core.Model.Product, Product>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features))
                .ForMember(dest => dest.AdditionalOption, opt => opt.MapFrom(src => src.AdditionalOption));

            cfg.CreateMap<Product, LicenseServer.Core.Model.Product>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features));

            cfg.CreateMap<LicenseServer.Core.Model.Subscription, Subscription>();
            cfg.CreateMap<Subscription, LicenseServer.Core.Model.Subscription>();

            cfg.CreateMap<LicenseServer.Core.Model.SubscriptionDetail, SubscriptionDetails>();
            cfg.CreateMap<SubscriptionDetails, LicenseServer.Core.Model.SubscriptionDetail>();

            cfg.CreateMap<LicenseServer.Core.Model.UserSubscription, UserSubscription>()
                .ForMember(dest => dest.Subtype, opt => opt.MapFrom(src => src.Subtype));
            cfg.CreateMap<UserSubscription, LicenseServer.Core.Model.UserSubscription>();

            cfg.CreateMap<LicenseServer.Core.Model.CartItem, CartItem>();
            cfg.CreateMap<CartItem, LicenseServer.Core.Model.CartItem>();

            cfg.CreateMap<Feature, LicenseServer.Core.Model.Feature>();
            cfg.CreateMap<LicenseServer.Core.Model.Feature, Feature>();

            cfg.CreateMap<LicenseServer.Core.Model.UserToken, UserToken>();
            cfg.CreateMap<UserToken, Core.Model.UserToken>();

            cfg.CreateMap<LicenseServer.Core.Model.PurchaseOrder, PurchaseOrder>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            cfg.CreateMap<PurchaseOrder, LicenseServer.Core.Model.PurchaseOrder>()
               .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
               .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            cfg.CreateMap<PurchaseOrderItem, LicenseServer.Core.Model.PurchaseOrderItem>()
                .ForMember(dest => dest.Subscription, opt => opt.MapFrom(src => src.Subscription));

            cfg.CreateMap<LicenseServer.Core.Model.PurchaseOrderItem, PurchaseOrderItem>()
                .ForMember(dest => dest.Subscription, opt => opt.MapFrom(src => src.Subscription));

            cfg.CreateMap<Core.Model.ProductAdditionalOption, ProductAdditionalOption>();
            cfg.CreateMap<ProductAdditionalOption, Core.Model.ProductAdditionalOption>();

            cfg.CreateMap<Core.Model.Notification, Notification>();
            cfg.CreateMap<Notification, Core.Model.Notification>();

            cfg.CreateMap<Core.Model.ClientAppVerificationSettings, ClientAppVerificationSettings>();
               // .ForMember(dest => dest.ApplicationCode, opt => opt.MapFrom(src => (ApplicationCodeType)Enum.Parse(typeof(ApplicationCodeType), src.ApplicationCode)));
            cfg.CreateMap<ClientAppVerificationSettings, Core.Model.ClientAppVerificationSettings>();
                //.ForMember(opt => opt.ApplicationCode, opt => opt.MapFrom(dest => dest.ApplicationCode.ToString()));



        }


    }
}
