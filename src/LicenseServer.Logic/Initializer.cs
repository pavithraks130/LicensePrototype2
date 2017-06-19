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

            cfg.CreateMap<LicenseServer.Core.Model.SubscriptionCategory, LicenseServer.DataModel.SubscriptionCategory>();
            cfg.CreateMap<LicenseServer.DataModel.SubscriptionCategory, LicenseServer.Core.Model.SubscriptionCategory>();

            cfg.CreateMap<LicenseServer.Core.Model.Product, LicenseServer.DataModel.Product>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features))
                .ForMember(dest => dest.AdditionalOption, opt => opt.MapFrom(src => src.AdditionalOption));

            cfg.CreateMap<LicenseServer.DataModel.Product, LicenseServer.Core.Model.Product>()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Categories))
                .ForMember(dest => dest.Features, opt => opt.MapFrom(src => src.Features));

            cfg.CreateMap<LicenseServer.Core.Model.Subscription, LicenseServer.DataModel.Subscription>();
            cfg.CreateMap<LicenseServer.DataModel.Subscription, LicenseServer.Core.Model.Subscription>();

            cfg.CreateMap<LicenseServer.Core.Model.SubscriptionDetail, LicenseServer.DataModel.SubscriptionDetails>();
            cfg.CreateMap<LicenseServer.DataModel.SubscriptionDetails, LicenseServer.Core.Model.SubscriptionDetail>();

            cfg.CreateMap<LicenseServer.Core.Model.UserSubscription, LicenseServer.DataModel.UserSubscription>()
                .ForMember(dest => dest.Subtype, opt => opt.MapFrom(src => src.Subtype));
            cfg.CreateMap<LicenseServer.DataModel.UserSubscription, LicenseServer.Core.Model.UserSubscription>();

            cfg.CreateMap<LicenseServer.Core.Model.CartItem, LicenseServer.DataModel.CartItem>();
            cfg.CreateMap<LicenseServer.DataModel.CartItem, LicenseServer.Core.Model.CartItem>();

            cfg.CreateMap<LicenseServer.DataModel.Feature, LicenseServer.Core.Model.Feature>();
            cfg.CreateMap<LicenseServer.Core.Model.Feature, LicenseServer.DataModel.Feature>();

            cfg.CreateMap<LicenseServer.Core.Model.UserToken, DataModel.UserToken>();
            cfg.CreateMap<DataModel.UserToken, Core.Model.UserToken>();

            cfg.CreateMap<LicenseServer.Core.Model.PurchaseOrder, DataModel.PurchaseOrder>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            cfg.CreateMap<DataModel.PurchaseOrder, LicenseServer.Core.Model.PurchaseOrder>()
               .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User))
               .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));

            cfg.CreateMap<DataModel.PurchaseOrderItem, LicenseServer.Core.Model.PurchaseOrderItem>()
                .ForMember(dest => dest.Subscription, opt => opt.MapFrom(src => src.Subscription));

            cfg.CreateMap<LicenseServer.Core.Model.PurchaseOrderItem, DataModel.PurchaseOrderItem>()
                .ForMember(dest => dest.Subscription, opt => opt.MapFrom(src => src.Subscription));

            cfg.CreateMap<Core.Model.ProductAdditionalOption, DataModel.ProductAdditionalOption>();
            cfg.CreateMap<DataModel.ProductAdditionalOption, Core.Model.ProductAdditionalOption>();

            cfg.CreateMap<Core.Model.Notification, DataModel.Notification>();
            cfg.CreateMap<DataModel.Notification, Core.Model.Notification>();



        }


    }
}
