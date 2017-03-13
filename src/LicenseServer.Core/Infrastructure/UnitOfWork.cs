using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.Core.Model;
using LicenseServer.Core.DbContext;

namespace LicenseServer.Core.Infrastructure
{
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

        private GenericRepository<SubscriptionType> _subscriptionType = null;
        public GenericRepository<SubscriptionType> SubscriptionRepository
        {
            get
            {
                return _subscriptionType ?? (_subscriptionType = new GenericRepository<SubscriptionType>(_context));
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
        private GenericRepository<LicenseFeatures> _licenseFeaturesRepository;
        public GenericRepository<LicenseFeatures> LicenseFeaturesRepository
        {
            get
            {
                return _licenseFeaturesRepository ??(_licenseFeaturesRepository=new GenericRepository<LicenseFeatures>(_context));
            }

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
