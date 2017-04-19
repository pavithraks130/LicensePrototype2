﻿using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;

namespace License.Logic.DataLogic
{
    public class UserLicenseLogic : BaseLogic
    {
        private bool CreateUserLicense(UserLicense lic)
        {
            var obj = AutoMapper.Mapper.Map<UserLicense, Core.Model.UserLicense>(lic);
            obj = Work.UserLicenseRepository.Create(obj);
            Work.UserLicenseRepository.Save();
            LicenseLogic licLogic = new LicenseLogic();
            licLogic.UpdateLicenseStatus(obj.LicenseId, true);
            return obj.Id > 0;
        }


        /// <summary>
        /// creating the UserLicense this can be used to update the single user.
        /// </summary>
        /// <param name="licList"></param>
        /// <returns></returns>
        public bool CreataeUserLicense(List<UserLicense> licList)
        {
            LicenseLogic licLogic = new LicenseLogic();
            foreach (var lic in licList)
            {
                var userLicList = Work.UserLicenseRepository.GetData(ul => ul.UserId == lic.UserId).ToList();
                var data = Work.LicenseDataRepository.GetData(l => l.ProductId == lic.License.ProductId && l.UserSubscriptionId == lic.License.UserSubscriptionId).ToList().Select(l => l.Id);
                var obj = userLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId));
                if (obj == null)
                {
                    var licId = licLogic.GetUnassignedLicense(lic.License.UserSubscriptionId, lic.License.ProductId).Id;
                    UserLicense ul = new UserLicense();
                    ul.UserId = lic.UserId;
                    ul.LicenseId = licId;
                    ul.TeamId = lic.TeamId;
                    CreateUserLicense(ul);
                }
                userLicList.Remove(obj);
            }
            return true;
        }

        /// <summary>
        /// function to create the License  for multiple User . This function will be used for bulk license mapping to 
        /// multiple User.
        /// </summary>
        /// <param name="licList"></param>
        /// <param name="userIdList"></param>
        /// <returns></returns>
        public bool CreateMultiUserLicense(UserLicenseDataMapping model)
        {
            LicenseLogic licLogic = new LicenseLogic();
            foreach (var userId in model.UserList)
            {
                var userLicList = Work.UserLicenseRepository.GetData(ul => ul.UserId == userId).ToList();
                foreach (var lic in model.LicenseDataList)
                {
                    var data = Work.LicenseDataRepository.GetData(l => l.ProductId == lic.ProductId && l.UserSubscriptionId == lic.UserSubscriptionId).ToList().Select(l => l.Id);
                    var obj = userLicList.FirstOrDefault(ul => data.Contains(ul.LicenseId));
                    if (obj == null)
                    {
                        var licId = licLogic.GetUnassignedLicense(lic.UserSubscriptionId, lic.ProductId).Id;
                        UserLicense ul = new UserLicense()
                        {
                            UserId = userId,
                            LicenseId = licId,
                            TeamId = model.TeamId
                        };
                        CreateUserLicense(ul);
                    }
                    userLicList.Remove(obj);
                }

            }
            return true;
        }

        private bool RevokeUserLicense(Core.Model.UserLicense lic)
        {
            var obj = Work.UserLicenseRepository.GetData(r => r.LicenseId == lic.LicenseId && r.UserId == lic.UserId).FirstOrDefault();
            if (obj == null)
                return false;
            var obj1 = Work.UserLicenseRepository.Delete(obj);
            Work.UserLicenseRepository.Save();
            LicenseLogic licLogic = new LicenseLogic();
            licLogic.UpdateLicenseStatus(obj1.LicenseId, false);
            return obj1 != null;
        }

        public bool RevokeUserLicense(UserLicenseDataMapping model)
        {
            LicenseLogic licLogic = new LicenseLogic();
            foreach (var userId in model.UserList)
            {
                foreach (var lic in model.LicenseDataList)
                {
                    var obj = Work.UserLicenseRepository.GetData(l => l.UserId == userId && l.License.ProductId == lic.ProductId && l.License.UserSubscriptionId == lic.UserSubscriptionId && l.TeamId == model.TeamId).FirstOrDefault();
                    RevokeUserLicense(obj);
                }
            }
            return true;
        }

        public int GetUserLicenseCount(int userSubscriptionId, int productId)
        {
            var licenseIdList = Work.LicenseDataRepository.GetData(l => l.UserSubscriptionId == userSubscriptionId && l.ProductId == productId).Select(l => l.Id).ToList();
            return Work.UserLicenseRepository.GetData(ul => licenseIdList.Contains(ul.LicenseId)).Count();
        }

        public List<UserLicense> GetUserLicense(string userId)
        {
            List<UserLicense> licenses = new List<UserLicense>();
            var datas = Work.UserLicenseRepository.GetData(l => l.UserId == userId);
            foreach (var data in datas)
                licenses.Add(AutoMapper.Mapper.Map<Core.Model.UserLicense, UserLicense>(data));
            return licenses;
        }

        public List<UserLicense> GetUserLicense(string userId, int teamId)
        {
            List<UserLicense> licenses = new List<UserLicense>();
            var datas = Work.UserLicenseRepository.GetData(l => l.UserId == userId && l.TeamId == teamId);
            foreach (var data in datas)
                licenses.Add(AutoMapper.Mapper.Map<Core.Model.UserLicense, UserLicense>(data));
            return licenses;
        }
    }
}
