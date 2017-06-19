using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.Core.Model;
using LicenseServer.Core.DbContext;

namespace LicenseServer.Core.Infrastructure
{
    /// <summary>
    /// Unit of Work
    /// </summary>
    public class UnitOfWork
    {
        private AppDbContext _context = new AppDbContext();


        private GenericRepository<Organization> _organozationRepository;
        public GenericRepository<Organization> OrganizationRepository
        {
            get
            {
                return _organozationRepository ?? (_organozationRepository = new GenericRepository<Organization>(_context));
            }
        }

        private GenericRepository<Product> _productRepository = null;
        public GenericRepository<Product> ProductRepository
        {
            get
            {
                return _productRepository ?? (_productRepository = new GenericRepository<Product>(_context));
            }
        }

        private GenericRepository<Subscription> _subscriptionRepo = null;
        public GenericRepository<Subscription> SubscriptionRepository
        {
            get
            {
                return _subscriptionRepo ?? (_subscriptionRepo = new GenericRepository<Subscription>(_context));
            }
        }

        private GenericRepository<SubscriptionDetail> _subscriptionDetailResitory;
        public GenericRepository<SubscriptionDetail> SubscriptionDetailResitory
        {
            get
            {
                return _subscriptionDetailResitory ?? (_subscriptionDetailResitory = new GenericRepository<SubscriptionDetail>(_context));
            }
        }

        private GenericRepository<UserSubscription> _userSubscriptionRepository;
        public GenericRepository<UserSubscription> UserSubscriptionRepository
        {
            get
            {
                return _userSubscriptionRepository ?? (_userSubscriptionRepository = new GenericRepository<UserSubscription>(_context));
            }
        }

        private GenericRepository<CartItem> _cartItemRepository;
        public GenericRepository<CartItem> CartItemLicenseRepository
        {
            get
            {
                return _cartItemRepository ?? (_cartItemRepository = new GenericRepository<CartItem>(_context));
            }
        }
        private GenericRepository<Feature> _featuresRepository;
        public GenericRepository<Feature> FeaturesRepository
        {
            get
            {
                return _featuresRepository ?? (_featuresRepository = new GenericRepository<Feature>(_context));
            }

        }

        private GenericRepository<SubscriptionCategory> _subscriptionCategoryRepo;
        public GenericRepository<SubscriptionCategory> SubscriptionCategoryRepo
        {
            get
            {
                return _subscriptionCategoryRepo ?? (_subscriptionCategoryRepo = new GenericRepository<SubscriptionCategory>(_context));
            }
        }

        private GenericRepository<UserToken> _userTokenRepository;
        public GenericRepository<UserToken> UserTokenRepository
        {
            get
            {
                return _userTokenRepository ?? (_userTokenRepository = new GenericRepository<UserToken>(_context));
            }
        }


        private GenericRepository<PurchaseOrder> _purchaseOrderRepository;
        public GenericRepository<PurchaseOrder> PurchaseOrderRepository
        {
            get
            {
                return _purchaseOrderRepository ?? (_purchaseOrderRepository = new GenericRepository<PurchaseOrder>(_context));
            }
        }

        private GenericRepository<PurchaseOrderItem> _poItemRepository;
        public GenericRepository<PurchaseOrderItem> POItemRepository
        {
            get { return _poItemRepository ?? (_poItemRepository = new GenericRepository<PurchaseOrderItem>(_context)); }
        }

        public GenericRepository<ProductAdditionalOption> _prodAdditionalOptionRepoository;
        public GenericRepository<ProductAdditionalOption> ProdAdditionalOptionRepository
        {
            get { return _prodAdditionalOptionRepoository ?? (_prodAdditionalOptionRepoository = new GenericRepository<ProductAdditionalOption>(_context)); }
        }

        private GenericRepository<Notification> _notificationRepository;
        public GenericRepository<Notification> NotificationRepository
        {
            get { return _notificationRepository ?? (_notificationRepository = new GenericRepository<Notification>(_context)); }
        }
        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    _context.Dispose();
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
