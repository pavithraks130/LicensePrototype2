using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;

namespace License.Logic.ServiceLogic
{
    public class UserLicenseLogic : BaseLogic
    {
        public bool CreateUserLicense(UserLicense lic)
        {
            var obj = AutoMapper.Mapper.Map<UserLicense, Core.Model.UserLicense>(lic);
            obj = Work.UserLicenseRepository.Create(obj);
            return obj.Id > 0;
        }

        public bool CreateUserLicense(List<UserLicense> licList)
        {
            LicenseLogic licLogic = new LicenseLogic();
            int i = 0;
            foreach (var lic in licList)
            {
                var data = Work.LicenseDataRepository.GetData(l => l.ProductId == lic.License.ProductId && l.UserSubscriptionId == lic.License.UserSubscriptionId).Select(l => l.Id);
                var obj = Work.UserLicenseRepository.GetData(ul => data.Contains(ul.LicenseId) && ul.UserId == lic.UserId);
                if (obj.Count() == 0)
                {
                    i++;
                    UserLicense ul = new UserLicense();
                    ul.UserId = lic.UserId;
                    ul.LicenseId = licLogic.GetUnassignedLicense(lic.License.UserSubscriptionId, lic.License.ProductId).Id;
                    CreateUserLicense(ul);
                }
            }
            if (i > 0)
                Work.UserLicenseRepository.Save();
            return true;
        }

        public bool RemoveUserLicense(UserLicense lic)
        {
            var obj = Work.UserLicenseRepository.GetData(r => r.LicenseId == lic.LicenseId && r.UserId == lic.UserId).FirstOrDefault();
            if (obj == null)
                return false;
            var obj1 = Work.UserLicenseRepository.Delete(obj);
            return obj1 != null;
        }

        public bool RevokeUserLicense(List<UserLicense> licList, string userId)
        {
            int i = 0;
            LicenseLogic licLogic = new LicenseLogic();
            var licdata = GetUserLicense(userId);
            foreach (var lic in licList)
            {
                var obj = licdata.FirstOrDefault(l => l.License.ProductId == lic.License.ProductId && l.License.UserSubscriptionId == lic.License.UserSubscriptionId);
                RemoveUserLicense(obj);
            }
            if (i > 0)
                Work.UserLicenseRepository.Save();
            return true;
        }

        public bool RemoveById(int id)
        {
            return Work.UserLicenseRepository.Delete(id);
        }

        public int GetUserLicenseCount(int licenseId)
        {
            return Work.UserLicenseRepository.GetData(l => l.LicenseId == licenseId).Count();
        }

        public UserLicense GetUserLicenseById(int id)
        {
            var obj = Work.UserLicenseRepository.GetById(id);
            return AutoMapper.Mapper.Map<Core.Model.UserLicense, UserLicense>(obj);
        }

        public int GetUserLicenseCount(int userSubscriptionId, int productId)
        {
            var licenseIdList = Work.LicenseDataRepository.GetData(l => l.UserSubscriptionId == userSubscriptionId && l.ProductId == productId).Select(l => l.Id).ToList();
            return Work.UserLicenseRepository.GetData(ul => licenseIdList.Contains(ul.Id)).Count();
        }

        public List<UserLicense> GetUserLicense(string userId)
        {
            List<UserLicense> licenses = new List<UserLicense>();
            var datas = Work.UserLicenseRepository.GetData(l => l.UserId == userId);
            foreach (var data in datas)
                licenses.Add(AutoMapper.Mapper.Map<Core.Model.UserLicense, UserLicense>(data));
            return licenses;
        }

        public void save()
        {
            Work.UserLicenseRepository.Save();
        }
    }
}
