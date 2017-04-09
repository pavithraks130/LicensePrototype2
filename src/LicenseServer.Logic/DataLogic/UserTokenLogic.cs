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
            foreach (var t in tokenList)
            {
                var obj = Mapper.Map<DataModel.UserToken>(t);
                tokenListObj.Add(obj);
            }
            return tokenListObj;
        }

        public UserToken CreateUserToken(UserToken t)
        {
            var token = IsTokenGenerated(t.Email);
            if (token != null)
                return token;
            var obj = Mapper.Map<Core.Model.UserToken>(t);

            LicenseKey.GenerateKey keyGen = new LicenseKey.GenerateKey();
            keyGen.LicenseTemplate = "xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";
            keyGen.UseBase10 = false;
            keyGen.UseBytes = false;
            keyGen.CreateKey();
            obj.Token = keyGen.GetLicenseKey();            

            var tokenObj = Work.UserTokenRepository.Create(obj);
            Work.UserTokenRepository.Save();
            if (tokenObj != null)
                return Mapper.Map<LicenseServer.DataModel.UserToken>(tokenObj);
            return null;
        }

        public UserToken IsTokenGenerated(string email)
        {
            var tokenObj = Work.UserTokenRepository.GetData(u => u.Email == email).FirstOrDefault();
            return Mapper.Map<LicenseServer.DataModel.UserToken>(tokenObj);
        }

        public bool VerifyUserToken(UserToken t)
        {
            var obj = Work.UserTokenRepository.GetData(u => u.Email == t.Email && u.Token == t.Token);
            return obj.Count() > 0;
        }

    }
}
