using License.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Logic.DataLogic
{
    public class CSVFileLogic:BaseLogic
    {
        /// <summary>
        /// Creating CSV  record in db
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<CSVFile> CreateCSVData(List<CSVFile> listData)
        {
            List<CSVFile> list = new List<CSVFile>();
            foreach (var data in listData)
            {
                var obj = AutoMapper.Mapper.Map<License.Core.Model.CSVFile>(data);
                obj = Work.CSVFileRepository.Create(obj);
                Work.ProductLicenseRepository.Save();
                list.Add(AutoMapper.Mapper.Map<CSVFile>(obj));
            }
            return list ;
        }
    }
}
