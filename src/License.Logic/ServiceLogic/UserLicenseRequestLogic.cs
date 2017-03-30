using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Model;

namespace License.Logic.ServiceLogic
{
    public class UserLicenseRequestLogic : BaseLogic
    {
        /// <summary>
        /// Creating the single object  to DB
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool CreateRequest(UserLicenseRequest req)
        {
            var obj = AutoMapper.Mapper.Map<Core.Model.UserLicenseRequest>(req);
            obj = Work.UserLicenseRequestRepo.Create(obj);
            return obj.Id > 0;
        }

        /// <summary>
        /// Updating the single object  to DB
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        private bool UpdateRequest(UserLicenseRequest req)
        {
            var obj = Work.UserLicenseRequestRepo.GetById(req.Id);
            obj.IsApproved = req.IsApproved;
            obj.IsRejected = req.IsRejected;
            obj.ApprovedBy = req.ApprovedBy;
            obj = Work.UserLicenseRequestRepo.Update(obj);
            Work.UserLicenseRequestRepo.Save();
            return obj != null;
        }

        public void Create(List<UserLicenseRequest> reqList)
        {
            bool status = true;
            int i = 0;
            foreach (var obj in reqList)
            {
                status &= CreateRequest(obj);
                i++;
            }
            if (i > 0)
                Work.UserLicenseRequestRepo.Save();
        }

        public void Update(List<UserLicenseRequest> reqList)
        {
            bool status = true;
            int i = 0;
            foreach (var obj in reqList)
            {
                status &= UpdateRequest(obj);
                i++;
            }
            if (i > 0)
                Work.UserLicenseRequestRepo.Save();
        }

        /// <summary>
        /// Function to get the Licence Request of all the users based  on the admin id
        /// </summary>
        /// <param name="adminId"></param>
        /// <returns></returns>
        public List<UserLicenseRequest> GetRequestList(string adminId)
        {
            var userlist = Work.UserInviteRepository.GetData(f => f.AdminId == adminId).ToList();
            if (userlist.Count > 0)
            {
                var idList = userlist.Select(u => u.InviteeUserId).ToList();
                var licReqList = Work.UserLicenseRequestRepo.GetData(f => idList.Contains(f.Requested_UserId) && String.IsNullOrEmpty(f.ApprovedBy)).ToList();
                if (licReqList.Count > 0)
                {
                    List<UserLicenseRequest> userLicReq = new List<UserLicenseRequest>();
                    foreach (var obj in licReqList)
                    {
                        var tempObj = AutoMapper.Mapper.Map<License.Model.UserLicenseRequest>(obj);
                        userLicReq.Add(tempObj);
                    }
                    return userLicReq;
                }
            }
            return null;
        }

        /// <summary>
        /// Get the Requested License based on the UserId1
        /// </summary>
        /// <returns></returns>
        public List<UserLicenseRequest> GetLicenseRequest(string userId)
        {
            var licReqList = Work.UserLicenseRequestRepo.GetData(f=> f.Requested_UserId == userId).ToList();
            if (licReqList.Count > 0)
            {
                List<UserLicenseRequest> userLicReq = new List<UserLicenseRequest>();
                foreach (var obj in licReqList)
                {
                    var tempObj = AutoMapper.Mapper.Map<License.Model.UserLicenseRequest>(obj);
                    userLicReq.Add(tempObj);
                }
                return userLicReq;
            }
            return null;
        }



    }
}
