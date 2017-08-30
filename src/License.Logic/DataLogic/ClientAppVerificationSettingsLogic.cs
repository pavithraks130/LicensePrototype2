using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using License.Models;
namespace License.Logic.DataLogic
{
    public class ClientAppVerificationSettingsLogic : BaseLogic
    {
        public void UpdateAppSettings(List<ClientAppVerificationSettings> settings)
        {
            settings.ForEach(s => {
                var obj = AutoMapper.Mapper.Map<License.Core.Model.ClientAppVerificationSettings>(s);
                this.Work.ClientAppVerificationRepository.Create(obj);
                this.Work.ClientAppVerificationRepository.Save();
            });
        }

        public bool ValidateKey(ClientAppVerificationSettings setting)
        {
            //var obj = this.Work.ClientAppVerificationRepository.GetData(s => s.ApplicationCode == setting.ApplicationCode.ToString()).FirstOrDefault();
            //if (obj != null)
            //    return AutoMapper.Mapper.Map<ClientAppVerificationSettings>(obj);
            //return null;
            var decryptedString = LicenseKey.EncryptDecrypt.DecryptString(setting.ApplicationSecretkey, setting.ApplicationCode);
            if (decryptedString == LicenseKey.EncryptDecrypt.globalPassPhrase)
                return true;
            else
                return false;
        }

    }
}
