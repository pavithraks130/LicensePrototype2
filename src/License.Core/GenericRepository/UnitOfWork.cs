using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.DBContext;
using License.Core.Model;

namespace License.Core.GenericRepository
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        private LicenseRepository<Product> _productLicenseRepository;
        public LicenseRepository<Product> ProductLicenseRepository
        {
            get { return _productLicenseRepository ?? (_productLicenseRepository = new LicenseRepository<Product>(_dbContext)); }
        }
        
        private LicenseRepository<Subscription> _subscriptionRepository;
        public LicenseRepository<Subscription> SubscriptionRepository
        {
            get { return _subscriptionRepository ?? (_subscriptionRepository = new LicenseRepository<Subscription>(_dbContext)); }
        }

        private LicenseRepository<ProductSubscriptionMapping> _productSubscriptionMapping;
        public LicenseRepository<ProductSubscriptionMapping> ProductSubscriptionMapping
        {
            get { return _productSubscriptionMapping ?? ( _productSubscriptionMapping = new LicenseRepository<Model.ProductSubscriptionMapping>(_dbContext)); }
        }

        private LicenseRepository<TeamMembers> _userInviteLicenseRepository;
        public LicenseRepository<TeamMembers> UserInviteLicenseRepository
        {
            get
            {
                return _userInviteLicenseRepository ??
                       (_userInviteLicenseRepository = new LicenseRepository<TeamMembers>(_dbContext));
            }
        }


        private LicenseRepository<UserSubscription> _userSubscriptionRepository;
        public LicenseRepository<UserSubscription> UserSubscriptionRepository
        {
            get
            {
                return _userSubscriptionRepository ?? (_userSubscriptionRepository = new LicenseRepository<UserSubscription>(_dbContext));
            }
        }

        private LicenseRepository<LicenseData> _licenseDataRepository;
        public LicenseRepository<LicenseData> LicenseDataRepository
        {
            get
            {
                return _licenseDataRepository ?? (_licenseDataRepository = new LicenseRepository<LicenseData>(_dbContext));
            }
        }

        private LicenseRepository<UserLicense> _userLicenseRepository;
        public LicenseRepository<UserLicense> UserLicenseRepository
        {
            get
            {
                return _userLicenseRepository ?? (_userLicenseRepository = new LicenseRepository<UserLicense>(_dbContext));
            }
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
                if (disposing)
                    _dbContext.Dispose();
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
