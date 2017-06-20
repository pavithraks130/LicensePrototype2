using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.DBContext;
using License.Core.Model;

namespace License.Core.GenericRepository
{
    /// <summary>
    /// It is used as commmon gateway to connect DBContext
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();

        private LicenseRepository<TeamMember> _teamMemberRepository;
        public LicenseRepository<TeamMember> TeamMemberRepository
        {
            get
            {
                return _teamMemberRepository ??
                       (_teamMemberRepository = new LicenseRepository<TeamMember>(_dbContext));
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

        private LicenseRepository<ProductLicense> _productLicenseRepository;
        public LicenseRepository<ProductLicense> ProductLicenseRepository
        {
            get
            {
                return _productLicenseRepository ?? (_productLicenseRepository = new LicenseRepository<ProductLicense>(_dbContext));
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

        private LicenseRepository<TeamLicense> _teamLicenseRepository;
        public LicenseRepository<TeamLicense> TeamLicenseRepository
        {
            get
            {
                return _teamLicenseRepository ?? (_teamLicenseRepository = new LicenseRepository<TeamLicense>(_dbContext));
            }
        }

        private LicenseRepository<TeamAsset> _teamAssetRepository;
        public LicenseRepository<TeamAsset> TeamAssetRepository
        {
            get
            {
                return _teamAssetRepository ?? (_teamAssetRepository = new LicenseRepository<TeamAsset>(_dbContext));
            }
        }

        public LicenseRepository<UserLicenseRequest> _userLicenseRequestRepo;
        public LicenseRepository<UserLicenseRequest> UserLicenseRequestRepo
        {
            get { return _userLicenseRequestRepo ?? (_userLicenseRequestRepo = new LicenseRepository<UserLicenseRequest>(_dbContext)); }
        }

        private LicenseRepository<Team> _teamRepository;
        public LicenseRepository<Team> TeamRepository
        {
            get
            {
                return _teamRepository ?? (_teamRepository = new LicenseRepository<Team>(_dbContext));
            }
        }

        private LicenseRepository<VISMAData > _VISMADataRepository;
        public LicenseRepository<VISMAData > VISMADataRepository
        {
            get
            {
                return _VISMADataRepository ?? (_VISMADataRepository = new LicenseRepository<VISMAData >(_dbContext));
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

        //private LicenseRepository<Product> _productLicenseRepository;
        //public LicenseRepository<Product> ProductLicenseRepository
        //{
        //    get { return _productLicenseRepository ?? (_productLicenseRepository = new LicenseRepository<Product>(_dbContext)); }
        //}

        //private LicenseRepository<Subscription> _subscriptionRepository;
        //public LicenseRepository<Subscription> SubscriptionRepository
        //{
        //    get { return _subscriptionRepository ?? (_subscriptionRepository = new LicenseRepository<Subscription>(_dbContext)); }
        //}

        //private LicenseRepository<ProductSubscriptionMapping> _productSubscriptionMapping;
        //public LicenseRepository<ProductSubscriptionMapping> ProductSubscriptionMapping
        //{
        //    get { return _productSubscriptionMapping ?? ( _productSubscriptionMapping = new LicenseRepository<Model.ProductSubscriptionMapping>(_dbContext)); }
        //}

    }
}
