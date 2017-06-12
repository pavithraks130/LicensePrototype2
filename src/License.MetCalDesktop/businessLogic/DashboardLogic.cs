using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using License.MetCalDesktop.Model;
using License.MetCalDesktop.Common;


namespace License.MetCalDesktop.businessLogic
{
    public class DashboardLogic
    {
        private string featurefileName = "LicenseMetCalDesktopFetaures.txt";

        public void LoadFeaturesOnline()
        {
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            FetchUserSubscription userSub = new FetchUserSubscription();
            userSub.TeamId = AppState.Instance.SelectedTeam.Id;
            userSub.IsFeatureRequired = true;
            userSub.UserId = AppState.Instance.User.UserId;
            var response = client.PostAsJsonAsync("api/UserLicense/GetUserLicenseByUser", userSub).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var details = JsonConvert.DeserializeObject<UserLicenseDetails>(jsonData);
                AppState.Instance.UserLicenseList = details.SubscriptionDetails;
                UpdateFeatureToFile();
            }
        }

        public void LoadFeatureOffline()
        {
            if (FileIO.IsFileExist(featurefileName))
            {
                var featuresList = FileIO.GetJsonDataFromFile(featurefileName);
                var licenseDetails = JsonConvert.DeserializeObject<List<UserLicenseDetails>>(featuresList);
                var userLisense = licenseDetails.FirstOrDefault(l => l.UserId == AppState.Instance.User.UserId);
                AppState.Instance.UserLicenseList = userLisense.SubscriptionDetails;
            }
        }

        public async Task UpdateFeatureToFile()
        {
            List<UserLicenseDetails> userlicdtls = new List<UserLicenseDetails>();
            if (FileIO.IsFileExist(featurefileName))
            {
                var featuresList = FileIO.GetJsonDataFromFile(featurefileName);
                var licenseDetails = JsonConvert.DeserializeObject<List<UserLicenseDetails>>(featuresList);
                var userLisense = licenseDetails.FirstOrDefault(l => l.UserId == AppState.Instance.User.UserId);
                if (userLisense == null)
                    licenseDetails.Add(new UserLicenseDetails() { UserId = AppState.Instance.User.UserId, SubscriptionDetails = AppState.Instance.UserLicenseList });
                else
                    userLisense.SubscriptionDetails = AppState.Instance.UserLicenseList;
            }
            else
                userlicdtls.Add(new UserLicenseDetails() { UserId = AppState.Instance.User.UserId, SubscriptionDetails = AppState.Instance.UserLicenseList });
            var jsonData = JsonConvert.SerializeObject(userlicdtls);
            FileIO.SaveDatatoFile(jsonData, featurefileName);
        }

    }
}
