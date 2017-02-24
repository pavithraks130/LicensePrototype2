using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Core.DBContext;
using License.Core.Model;

namespace License.Core.GenericRepository
{
    public class UnitOfWork :IDisposable
    {
        private ApplicationDbContext _dbContext = new ApplicationDbContext();
        private LicenseRepository<Organization> _teamLicenseRepository = null;

        public LicenseRepository<Organization> TeamLicenseRepository
        {
            get { return _teamLicenseRepository ?? (_teamLicenseRepository = new LicenseRepository<Organization>(_dbContext)); }
        }

        private LicenseRepository<TeamMembers> _userInviteLicenseRepository;
        public LicenseRepository<TeamMembers> UserInviteLicenseRepository
        {
            get {
                return _userInviteLicenseRepository ??
                       (_userInviteLicenseRepository = new LicenseRepository<TeamMembers>(_dbContext));
            }
        }
        public void Save()
        {
            _dbContext.SaveChanges();
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
