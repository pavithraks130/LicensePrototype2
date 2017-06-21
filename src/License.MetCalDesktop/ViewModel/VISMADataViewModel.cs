using License.MetCalDesktop.Common;
using License.MetCalDesktop.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace License.MetCalDesktop.ViewModel
{
    public class VISMADataViewModel : BaseEntity
    {
        private ObservableCollection<VISMAData> _VISMADataCollections = new ObservableCollection<VISMAData>();
        private string errorMsg;
        private string expirationDate;
        private string result;
        public ICommand GetVISMADataCommand { get; set; }
        public ICommand GetVISMADataByTestDeviceCommand { get; set; }
        public ObservableCollection<VISMAData> VISMADataCollections { get => _VISMADataCollections; set => _VISMADataCollections = value; }
        public string ErrorMsg
        {
            get
            {
                return errorMsg;
            }
            set
            {
                errorMsg = value; OnPropertyChanged("ErrorMsg");
                OnPropertyChanged("ErrorMsg");
            }
        }
        public string TestDevice
        {
            get { return expirationDate; }
            set { expirationDate = value; OnPropertyChanged("TestDevice"); }
        }
        public string Result
        {
            get { return result; }
            set { result = value; OnPropertyChanged("Result"); }
        }

        public VISMADataViewModel()
        {

            GetVISMADataCommand = new RelayCommand(OnLoadVISMAData);
            GetVISMADataByTestDeviceCommand = new RelayCommand(GetVISMADataByTestDevice);
        }

        private void GetVISMADataByTestDevice(object parm)
        {
            if (!string.IsNullOrEmpty(TestDevice))
            {
                HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
                HttpResponseMessage response = null;
                response = client.GetAsync("api/VISMAData/GetVISMADataByTestDevice/" + TestDevice).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsStringAsync().Result;
                    var records = JsonConvert.DeserializeObject<List<VISMAData>>(data);
                   // ErrorMsg = string.Empty;
                    if (records.Count == 0)
                    {
                        Result = "Data Not Found ";
                        _VISMADataCollections.Clear();
                    }
                    foreach (var item in records)
                    {
                        _VISMADataCollections.Clear();
                        _VISMADataCollections.Add(item);
                        DateTime expDate = Convert.ToDateTime(item.ExpirationDate);
                        if (expDate.Date <= DateTime.Now.Date)
                        {
                            Result = "TestDevice already expired,expired date:" + item.ExpirationDate;
                        }
                        else
                        {
                            Result = "Test Device expiry date:" + item.ExpirationDate;                         }
                        break;
                    }
                }
                else
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var failureResult = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    Result = string.Empty;
                    ErrorMsg = failureResult.Message;
                }
                client.Dispose();
            }
            else
            {
                Result = string.Empty;
                ErrorMsg = "Please enter valid data";
            }
        }

        private void OnLoadVISMAData(object parm)
        {
            VISMADataCollections.Clear();
            HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            HttpResponseMessage response = null;
            response = client.GetAsync("api/VISMAData/GetAllVISMAData/").Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var records = JsonConvert.DeserializeObject<List<VISMAData>>(data);
                foreach (var item in records)
                {
                    VISMADataCollections.Add(item);
                }
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var failureResult = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                Result = string.Empty;
                ErrorMsg = failureResult.Message;
            }
            client.Dispose();
        }
    }
}
