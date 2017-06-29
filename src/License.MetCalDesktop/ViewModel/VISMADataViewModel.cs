using License.MetCalDesktop.Common;
using License.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using License.ServiceInvoke;

namespace License.MetCalDesktop.ViewModel
{
    /// <summary>
    /// This view model is used to  handle VISMA Data
    /// </summary>
    public class VISMADataViewModel : BaseEntity
    {
        private ObservableCollection<VISMAData> _VISMADataCollections = new ObservableCollection<VISMAData>();
        private string errorMsg;
        private string expirationDate;
        private string result;
        public ICommand GetVISMADataCommand { get; set; }
        public ICommand GetVISMADataByTestDeviceCommand { get; set; }

        private APIInvoke _invoke = null;
        /// <summary>
        /// To display VISMA Data 
        /// </summary>
        public ObservableCollection<VISMAData> VISMADataCollections { get => _VISMADataCollections; set => _VISMADataCollections = value; }

        /// <summary>
        /// TestDevice property from VISMA Data Table
        /// </summary>
        public string TestDevice
        {
            get
            {
                return expirationDate;
            }
            set
            {
                expirationDate = value;
                OnPropertyChanged("TestDevice");
            }
        }
        /// <summary>
        /// Result after search 
        /// </summary>
        public string Result
        {
            get { return result; }
            set { result = value; OnPropertyChanged("Result"); }
        }

        public VISMADataViewModel()
        {
            GetVISMADataCommand = new RelayCommand(OnLoadVISMAData);
            GetVISMADataByTestDeviceCommand = new RelayCommand(GetVISMADataByTestDevice);
            TestDevice = Constants.SEARCHDATA;
            _invoke = new APIInvoke();
        }

        /// <summary>
        /// Get VISMAData By TestDevice id
        /// </summary>
        /// <param name="parm"></param>
        private void GetVISMADataByTestDevice(object parm)
        {
            if (!string.IsNullOrEmpty(TestDevice) && !(TestDevice == Constants.SEARCHDATA))
            {

                WebAPIRequest<VISMAData> request = new WebAPIRequest<VISMAData>()
                {
                    AccessToken = AppState.Instance.OnPremiseToken.access_token,
                    Functionality = Functionality.GetVISMADataByTestDevice,
                    InvokeMethod = Method.GET,
                    ServiceModule = Modules.VISMAData,
                    ServiceType = ServiceType.OnPremiseWebApi,
                    Id = TestDevice
                };
                var response = _invoke.InvokeService<VISMAData, List<VISMAData>>(request);
                if (response.Status)
                {
                    VISMADataCollections = new ObservableCollection<VISMAData>(response.ResponseData);
                }
                else
                    Result = response.Error.error + " " + response.Error.Message;
                //HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
                //HttpResponseMessage response = null;
                //response = client.GetAsync("api/VISMAData/GetVISMADataByTestDevice/" + TestDevice).Result;
                //if (response.IsSuccessStatusCode)
                //{
                //    var data = response.Content.ReadAsStringAsync().Result;
                //    var records = JsonConvert.DeserializeObject<List<VISMAData>>(data);
                //    if (records.Count == 0)
                //    {
                //        Result = "Data Not Found ";
                //        _VISMADataCollections.Clear();
                //    }
                //    foreach (var item in records)
                //    {
                //        _VISMADataCollections.Clear();
                //        _VISMADataCollections.Add(item);
                //        DateTime expDate = Convert.ToDateTime(item.ExpirationDate);
                //        if (expDate.Date <= DateTime.Now.Date)
                //        {
                //            Result = "TestDevice is already expired.\n expired date:" + item.ExpirationDate;
                //        }
                //        else
                //        {
                //            Result = "Test Device is about to expire on:" + item.ExpirationDate;
                //            break;
                //        }
                //    }
                //}
                //else
                //{
                //    var jsonData = response.Content.ReadAsStringAsync().Result;
                //    var failureResult = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                //    Result = failureResult.Message;
                //}
                //client.Dispose();
            }
            else
            {
                Result = "Please enter valid data";
            }
        }

        /// <summary>
        /// Load  All VISMA Data
        /// </summary>
        /// <param name="parm"></param>
        private void OnLoadVISMAData(object parm)
        {
            VISMADataCollections.Clear();
            WebAPIRequest<VISMAData> request = new WebAPIRequest<VISMAData>()
            {
                AccessToken = AppState.Instance.OnPremiseToken.access_token,
                Functionality = Functionality.All,
                InvokeMethod = Method.GET,
                ServiceModule = Modules.VISMAData,
                ServiceType = ServiceType.OnPremiseWebApi
            };
            var response = _invoke.InvokeService<VISMAData, List<VISMAData>>(request);
            if (response.Status)
                VISMADataCollections = new ObservableCollection<VISMAData>(response.ResponseData);
            else
                Result = response.Error.error + " " + response.Error.Message;
            //HttpClient client = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
            //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
            //HttpResponseMessage response = null;
            //response = client.GetAsync("api/VISMAData/GetAllVISMAData/").Result;
            //if (response.IsSuccessStatusCode)
            //{
            //    var data = response.Content.ReadAsStringAsync().Result;
            //    var records = JsonConvert.DeserializeObject<List<VISMAData>>(data);
            //    foreach (var item in records)
            //    {
            //        VISMADataCollections.Add(item);
            //    }
            //}
            //else
            //{
            //    var jsonData = response.Content.ReadAsStringAsync().Result;
            //    var failureResult = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
            //    Result = failureResult.Message;
            //}
            //client.Dispose();
        }
    }
}
