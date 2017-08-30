using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using License.Models;

namespace LicenseServer.Logic.DataLogic
{
    public class ClientAppVerificationSettingsLogic : BaseLogic
    {

        public List<ClientAppVerificationSettings> GetAll()
        {
            var settingsList = this.Work.ClientApplicationSettingsRepository.GetData().ToList();
            return Mapper.Map<List<ClientAppVerificationSettings>>(settingsList).ToList();
        }

        public ClientAppVerificationSettings Create(ClientAppVerificationSettings setting)
        {
            var obj = Mapper.Map<Core.Model.ClientAppVerificationSettings>(setting);
            if (this.Work.ClientApplicationSettingsRepository.GetData(s => s.ApplicationCode.ToLower() == setting.ApplicationCode.ToLower()).Any())
            {
                ErrorMessage = "Application Key is already generated for the specified Application Code";
                return null;
            }
            obj.ApplicationSecretkey = LicenseKey.EncryptDecrypt.EncryptString(LicenseKey.EncryptDecrypt.globalPassPhrase, setting.ApplicationCode.ToString());
            obj = this.Work.ClientApplicationSettingsRepository.Create(obj);
            this.Work.ClientApplicationSettingsRepository.Save();
            return Mapper.Map<ClientAppVerificationSettings>(obj);
        }

        public  bool ValidateKey(ClientAppVerificationSettings setting)
        {
            //var obj = this.Work.ClientApplicationSettingsRepository.GetData(s => s.ApplicationCode == setting.ApplicationCode.ToString()).FirstOrDefault();
            //if (obj != null)
            //    return Mapper.Map<ClientAppVerificationSettings>(obj);
            //return null;
            var decryptedString = LicenseKey.EncryptDecrypt.DecryptString(setting.ApplicationSecretkey, setting.ApplicationCode);
            if (decryptedString == LicenseKey.EncryptDecrypt.globalPassPhrase)
                return true;
            else
                return false;
        }
    }
}
