using License.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Logic.DataLogic
{
    public class VISMADataLogic:BaseLogic
    {
        /// <summary>
        /// Creating CSV  record in db
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<VISMAData> CreateCSVData(List<VISMAData> listData)
        {
            List<VISMAData> list = new List<VISMAData>();
            foreach (var data in listData)
            {
                var obj = AutoMapper.Mapper.Map<License.Core.Model.VISMAData >(data);
                obj = Work.VISMADataRepository.Create(obj);
                Work.ProductLicenseRepository.Save();
                list.Add(AutoMapper.Mapper.Map<VISMAData>(obj));
            }
            return list ;
        }
    }
}
