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
