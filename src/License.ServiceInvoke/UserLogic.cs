using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace License.ServiceInvoke
{
    public class UserLogic
    {
        public string ErrorMessage { get; set; }

        private FileIO _fileIO = null;
        private APIInvoke _invoke = null;
        public UserLogic()
        {
            _fileIO = new FileIO();
            _invoke = new APIInvoke();
        }

       

        //public void UpdateFeatureToFile<T>(T userlicdtls, string featurefileName)
        //{
        //    _fileIO.SaveDatatoFile(userlicdtls, featurefileName);
        //}

        public WebAPIResponse<T> GetUserDetails<T>(WebAPIRequest<T> request)
        {
            var response = _invoke.InvokeService<T, T>(request);
            if (response.Status)
            {
                var fileName = request.Id + ".txt";
                var jsonData = JsonConvert.SerializeObject(response.ResponseData);
                _fileIO.SaveDatatoFile(jsonData, fileName);
            }
            else
                ErrorMessage = response.Error.error + " " + response.Error.Message;
            return response;
            //HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            //var response = client.GetAsync("api/User/GetDetailsById/" + AppState.Instance.User.UserId).Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    UserDetails details = JsonConvert.DeserializeObject<UserDetails>(jsonData);
            //    if (details != null)
            //    {
            //        var fileName = AppState.Instance.User.UserId + ".txt";
            //        jsonData = JsonConvert.SerializeObject(details);
            //        FileIO.SaveDatatoFile(jsonData, fileName);
            //    }
            //}
        }

        public void CreateCredentialFile(Object userobj, string password, string loginFileName)
        {
            Models.User user = (Models.User)userobj;
            List<Models.User> userList = new List<Models.User>();
            string thumbPrint = string.Empty;
            user.PasswordHash = HashPassword(password, out thumbPrint);
            user.ThumbPrint = thumbPrint;

            if (_fileIO.IsFileExist(loginFileName))
                userList = _fileIO.GetDataFromFile<List<Models.User>>(loginFileName);
            if (!userList.Any(u => u.Email == user.Email))
                userList.Add(user);
            _fileIO.SaveDatatoFile<List<Models.User>>(userList, loginFileName);
        }

        public string HashPassword(string password, out string thumbprint)
        {
            int size = 10;
            thumbprint = CreateSalt(size);
            return CreatePasswordhash(password, thumbprint);
        }

        public string CreateSalt(int size)
        {
            byte[] bytedata = new byte[size];
            var rngProvider = new RNGCryptoServiceProvider();
            rngProvider.GetBytes(bytedata);
            return Convert.ToBase64String(bytedata);
        }

        public string CreatePasswordhash(string password, string thumbPrint)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(password + thumbPrint);
            SHA256Managed sha256 = new SHA256Managed();
            var byteHashData = sha256.ComputeHash(bytes);
            return ByteArrayToHexString(byteHashData);
        }

        public String ByteArrayToHexString(byte[] bytes)
        {
            int length = bytes.Length;

            char[] chars = new char[length << 1];

            for (int i = 0, j = 0; i < length; i++, j++)
            {
                byte b = (byte)(bytes[i] >> 4);
                chars[j] = (char)(b > 9 ? b + 0x37 : b + 0x30);

                j++;

                b = (byte)(bytes[i] & 0x0F);
                chars[j] = (char)(b > 9 ? b + 0x37 : b + 0x30);
            }

            return new String(chars);
        }
    }
}
