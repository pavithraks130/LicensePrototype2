using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;

namespace License.Logic.ServiceLogic
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
        
    }
}
