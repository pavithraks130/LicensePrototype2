using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.ServiceInvoke
{
    public enum ServiceType
    {
        OnPremiseWebApi,
        CentralizeWebApi,
        All
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
        UserToken,
        Role,
        Notification,
        VISMAData
    }

    public enum Functionality
    {
        All,
        Create,
        Update,
        ForgotPassword,
        Get,
        GetById,
        Delete,
        UpdateInvite,
        OfflinePayment,
        UpdateLogoutStatus,
        CreateSubscriptionAddToCart,
        OnlinePayment,
        RenewSubscription,
        GetByUser,
        GetCartItemsCount,
        GetByCategory,
        ProductDependency,
        GetCMMSProducts,
        GetProductsByAdminId,
        CheckProductUpdates,
        UpdateProducts,
        GetProductsWithUserMappedProduct,
        GetProducts,
        UpdataMuliplePO,
        OrderByUser,
        syncpo,
        UpdateSubscriptionRenewal,
        SyncSubscription,
        UpdateConcurentUser,
        GetTeamsByAdminId,
        GetTeamsByUserId,
        Assign,
        UpdateAdminAccess,
        Revoke,
        GetTeamLicenseByTeam,
        ChangePassword,
        ApproveReject,
        GetRequestByTeam,
        LicenseRequest,
        GetRequestStatus,
        GetUserLicenseByUser,
        ExpireSubscription,
        SubscriptionDetils,
        GetSubscriptioDtlsForLicenseMap,
        SynchronizeSubscription,
        ResetPassword,
        Register,
        GetCartItems,
        UploadFile,
        GetUserDetailsById,
        GetVISMADataByTestDevice
    }
}
