using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseServer.DataModel;
using AutoMapper;

namespace LicenseServer.Logic
{
    public class UserTokenLogic : BaseLogic
    {
        public List<UserToken> GetUsertokenList()
        {
            List<UserToken> tokenListObj = new List<UserToken>();
            var tokenList = Work.UserTokenRepository.GetData();
            foreach(var t in tokenList)
            {
                var obj = Mapper.Map<DataModel.UserToken>(t);
                tokenListObj.Add(obj);
            }
            return tokenListObj;
        }

        public UserToken CreateUserToken(UserToken t)
        {
            var obj = Mapper.Map<Core.Model.UserToken>(t);
            Work.UserTokenRepository.Create(obj);
            Work.UserTokenRepository.Save();
            return obj;
        }

        public bool VerifyUserToken(UserToken t)
        {
            var obj = Work.UserTokenRepository.GetData(u => u.Email == t.Email && u.Token == t.Token);
            return obj.Count() > 0;
        }

    }
}
