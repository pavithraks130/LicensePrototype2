using License.Models;
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
        /// Creating VISMA  record in db
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public List<VISMAData> CreateVISMAData(List<VISMAData> listData)
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

        /// <summary>
        /// Update VISMA Data into database
        /// </summary>
        /// <param name="_VISMAData"></param>
        /// <returns></returns>
        public VISMAData UpdateVISMAData(VISMAData _VISMAData)
        {
            List<VISMAData> list = new List<VISMAData>();
            var obj = Work.VISMADataRepository.GetById(_VISMAData.Id);
            if (obj != null)
            {
                obj.TestDevice = _VISMAData.TestDevice;
                obj.ExpirationDate = _VISMAData.ExpirationDate;
                obj = Work.VISMADataRepository.Update(obj);
                Work.VISMADataRepository.Save();
                return AutoMapper.Mapper.Map<VISMAData>(obj);
            }
            else
                ErrorMessage = "Specified data not exist";
            return null;
        }

        /// <summary>
        /// Delete VISMA Data by Id
        /// </summary>
        /// <param name="_VISMADataId"></param>
        /// <returns></returns>
        public bool DeleteVISMAData(int  _VISMADataId)
        {
            List<VISMAData> dataList = new List<VISMAData>();
            bool status = Work.VISMADataRepository.Delete(_VISMADataId);
            Work.VISMADataRepository.Save();
            return status;
        }
        /// <summary>
        /// Retrieve VISMA data based on test device
        /// </summary>
        /// <param name="testDevice"></param>
        /// <returns></returns>
        public List<VISMAData> GetVISMADataByTestDevice(string testDevice)
        {
            List<VISMAData> dataList = new List<VISMAData>();
            var list = Work.VISMADataRepository.GetData(t => t.TestDevice == testDevice);
            dataList = list.Select(l => AutoMapper.Mapper.Map<VISMAData>(l)).ToList();
            return dataList;
        }
        /// <summary>
        /// To retrieve entire VISMA Data from DB
        /// </summary>
        /// <returns></returns>
        public List<VISMAData> GetAllVISMAData()
        {
            List<VISMAData> dataList = new List<VISMAData>();
            var list = Work.VISMADataRepository.GetData();
            dataList = list.Select(l => AutoMapper.Mapper.Map<VISMAData>(l)).ToList();
            return dataList;
        }

    }
}