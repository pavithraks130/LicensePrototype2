using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Common
{
    public enum ServiceType
    {
        OnPremiseWebApi,
        CentralizeWebApi
    }

    public enum InviteStatus
    {
        Pending,
        Accepted,
        Declined
    }

    public enum Method
    {
        POST,
        GET,
        PUT,
        DELETE
    }

   
    public enum Modules
    {
        Asset,
        Cart,
        Feature,
        Product,
        PurchaseOrder,
        Subscription,
        SubscriptionCategory,
        Team,
        TeamMember,
        TeamLicense,
        User,
        UserLicense,
        UserSubscription,
        UserToken
    }

    public enum AssetFunctionality
    {
        All,
        Create,
        Update,
        Get,
        GetById,
        Delete
    }
    public enum CartFunctionality
    {
        Create,
        Get,
        Delete,
        OfflinePayment,
        Update,
        GetById,
        CreateSubscriptionAddToCart,
        OnlinePayment,
        RenewSubscription,
        GetByUser,
        GetCartItemsCount
    }
    public enum FeatureFunctionality
    {
        All,
        Create,
        Update,
        Get,
        GetById,
        Delete,
        GetByCategory
    }
    public enum ProductFunctionality
    {
        All,
        Create,
        Update,
        Get,
        GetById,
        Delete,
        ProductDependency,
        GetCMMSProducts,
        GetProductsByAdminId,
        CheckProductUpdates,
        UpdateProducts,
        GetProductsWithUserMappedProduct,
        GetProducts
    }
    public enum PurchaseOrderFunctionality
    {
        Create,
        Update,
        Get,
        GetById,
        Delete,
        All,
        UpdataMuliplePO,
        OrderByUser,
        syncpo
    }
    public enum SubscriptionFunctionality
    {
        All,
        Create,
        Update,
        Get,
        GetById,
        Delete,
        UpdateSubscriptionRenewal,
        SyncSubscription
    }
    public enum SubscriptionCategoryFunctionality
    {
        All,
        Create,
        Update,
        Get,
        GetById,
        Delete
    }
    public enum TeamFunctionality
    {
        Create,
        Update,
        Get,
        GetById,
        Delete,
        UpdateConcurentUser,
        GetTeamsByAdminId,
        GetTeamsByUserId
    }
    public enum TeamMemberFunctionality
    {
        Create,
        Assign,
        Update,
        Get,
        GetById,
        Delete,
        UpdateAdminAccess,
        Revoke
    }
    public enum TeamLicenseFunctionality
    {
        Create,
        Update,
        Get,
        GetById,
        Delete,
        Revoke,
        GetTeamLicenseByTeam
    }
    public enum UserFunctionality
    {
        All,
        Create,
        Update,
        Get,
        GetById,
        Delete,
        ChangePassword
    }
    public enum UserLicenseFunctionality
    {
        Create,
        Update,
        Get,
        GetById,
        Delete,
        ApproveReject,
        GetRequestByTeam,
        Revoke,
        LicenseRequest,
        GetRequestStatus,
        GetUserLicenseByUser
    }
    public enum UserSubscriptionFunctionality
    {
        Create,
        Update,
        Get,
        GetById,
        Delete,
        RenewSubscription,
        ExpireSubscription,
        SubscriptionDetils,
        GetSubscriptioDtlsForLicenseMap,
        SynchronizeSubscription,
        UpdateSubscriptionRenewel
    }
    public enum UserTokenFunctionality
    {
        All,
        Create,
        Update,
        Get,
        GetById,
        Delete
    }
}