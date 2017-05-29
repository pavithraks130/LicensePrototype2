using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.DataModel;


namespace License.Logic.DataLogic
{
    public class LicenseLogic : BaseLogic
    {
        public List<LicenseData> GetLicenseList(int subscriptionId)
        {
            List<LicenseData> dataList = new List<LicenseData>();
            var list = Work.LicenseDataRepository.GetData(l => l.UserSubscriptionId == subscriptionId);
            foreach (var obj in list)
                dataList.Add(AutoMapper.Mapper.Map<Core.Model.LicenseData, LicenseData>(obj));
            return dataList;
        }

        public LicenseData GetUnassignedLicense(int userSubscriptionId, int productId)
        {
            var obj = Work.LicenseDataRepository.GetData(f => f.UserSubscriptionId == userSubscriptionId && f.ProductId == productId && f.IsMapped == false).FirstOrDefault();
            return AutoMapper.Mapper.Map<Core.Model.LicenseData, LicenseData>(obj);
        }

        public LicenseData GetUnassignedLicenseForTeam( int productId)
        {
            var obj = Work.LicenseDataRepository.GetData(f=>f.ProductId == productId && f.IsMapped == false).FirstOrDefault();
            return AutoMapper.Mapper.Map<Core.Model.LicenseData, LicenseData>(obj);
        }

        public void UpdateLicenseStatus(int licId, bool status)
        {
            var obj = Work.LicenseDataRepository.GetById(licId);
            obj.IsMapped = status;
            Work.LicenseDataRepository.Update(obj);
            Work.LicenseDataRepository.Save();
        }

        private void CreateLicenseData(LicenseData data)
        {
            var obj = AutoMapper.Mapper.Map<LicenseData, License.Core.Model.LicenseData>(data);
            obj = Work.LicenseDataRepository.Create(obj);
        }

        public void CreateLicenseData(List<LicenseData> dataList)
        {
            foreach (var data in dataList)
                CreateLicenseData(data);
            Work.LicenseDataRepository.Save();
        }

        public LicenseData GetLicenseById(int id)
        {
            var obj = Work.LicenseDataRepository.GetById(id);
            return AutoMapper.Mapper.Map<Core.Model.LicenseData, LicenseData>(obj);
        }

        public void UpdateRenewalLicenseKeys(List<LicenseKeyProductMapping> licKeysMapping, int userSubscriptionId)
        {
            var licenseDatalist = Work.LicenseDataRepository.GetData(l => l.UserSubscriptionId == userSubscriptionId);
            var productIdList = licKeysMapping.Select(s => s.ProductId).Distinct();
            foreach (var pro in productIdList)
            {
                int i = 0;
                var renewalKeys = licKeysMapping.Where(l => l.ProductId == pro).ToList();
                var existingKeys = licenseDatalist.Where(l => l.ProductId == pro).ToList();
                foreach (var keys in renewalKeys)
                {
                    if (i < existingKeys.Count)
                    {
                        existingKeys[i].LicenseKey = keys.LicenseKey;
                        Work.LicenseDataRepository.Update(existingKeys[i]);
                    }
                    else
                    {
                        Core.Model.LicenseData licenseData = new Core.Model.LicenseData();
                        licenseData.LicenseKey = keys.LicenseKey;
                        licenseData.ProductId = keys.ProductId;
                        licenseData.UserSubscriptionId = userSubscriptionId;
                        Work.LicenseDataRepository.Create(licenseData);
                    }
                    i++;
                }
                if (i > 0)
                    Work.LicenseDataRepository.Save();
            }
           

        }

    }
}
