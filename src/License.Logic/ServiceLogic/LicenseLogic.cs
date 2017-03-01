using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model.Model;

namespace License.Logic.ServiceLogic
{
    public class LicenseLogic : BaseLogic
    {
        public List<LicenseData> GetLicenseList(int subscriptionId)
        {
            List<LicenseData> dataList = new List<LicenseData>();
            var list = Work.LicenseDataRepository.GetData(l => l.SubscriptionId == subscriptionId);
            foreach (var obj in list)
                dataList.Add(AutoMapper.Mapper.Map<Core.Model.LicenseData, LicenseData>(obj));
            return dataList;
        }

        public bool CreateLicenseData(LicenseData data)
        {
            var obj = AutoMapper.Mapper.Map<LicenseData, License.Core.Model.LicenseData>(data);
            obj = Work.LicenseDataRepository.Create(obj);
            Work.LicenseDataRepository.Save();
            return obj.Id > 0;
        }
    }
}
