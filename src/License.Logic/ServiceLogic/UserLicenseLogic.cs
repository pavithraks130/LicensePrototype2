using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model.Model;

namespace License.Logic.ServiceLogic
{
    public class UserLicenseLogic : BaseLogic
    {
        public bool CreateUserLicense(UserLicense lic)
        {
            var obj = AutoMapper.Mapper.Map<UserLicense, Core.Model.UserLicense>(lic);
            obj = Work.UserLicenseRepository.Create(obj);
            Work.UserLicenseRepository.Save();
            return obj.Id > 0;
        }

        public bool RemoveUserLicense(UserLicense lic)
        {
            var obj = AutoMapper.Mapper.Map<UserLicense, Core.Model.UserLicense>(lic);
            var obj1 = Work.UserLicenseRepository.Delete(obj);
            return obj1 != null;
        }

        public int UserLicenseCount(int licenseId)
        {
            return Work.UserLicenseRepository.GetData(l => l.LicenseId == licenseId).Count();
        }
    }
}
