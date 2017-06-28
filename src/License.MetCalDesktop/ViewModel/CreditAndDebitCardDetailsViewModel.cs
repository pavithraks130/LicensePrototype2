using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using License.MetCalDesktop.Model;
using License.MetCalDesktop.Common;
using System.ComponentModel;
using System.Windows.Input;
using System.Net.Http;
using Newtonsoft.Json;
using License.Models;
using License.ServiceInvoke;


namespace License.MetCalDesktop.ViewModel
{
    internal class CreditAndDebitCardDetailsViewModel : BaseEntity, IDataErrorInfo
    {
        #region Private fields

        private readonly CardDetails cardDetails;
        private ObservableCollection<short> _cardValidityYear = new ObservableCollection<short>();
        private ObservableCollection<string> _cardValidityMonth = new ObservableCollection<string>();
        private string tax;
        private string currentDate;
        private string expDate;
        private string totalCost;

        #endregion Private fields

        /// <summary>
        /// credit or debit card holder name
        /// </summary>
        public string CardName
        {
            get
            {
                return cardDetails.Name;
            }
            set
            {
                cardDetails.Name = value;
                OnPropertyChanged("CardName");
            }
        }

        /// <summary>
        /// credit or debit card CVV number
        /// </summary>
        public short CardCVV
        {
            get
            {
                return cardDetails.CVVNum;
            }
            set
            {
                cardDetails.CVVNum = value;
                OnPropertyChanged("CardName");
            }
        }

        /// <summary>
        /// credit or debit card number
        /// </summary>
        public string CardNumber
        {
            get
            {
                return cardDetails.Number;
            }
            set
            {
                cardDetails.Number = value;
                OnPropertyChanged("CardNumber");
            }
        }

        /// <summary>
        /// Month collection
        /// </summary>
        public ObservableCollection<string> CardValidityMonth
        {
            get
            {
                return _cardValidityMonth;
            }
            set
            {
                _cardValidityMonth = value;
                OnPropertyChanged("CardValidityMonth");
            }
        }

        /// <summary>
        /// credit or debit card expiry date month 
        /// </summary>
        public string SelectedMonth
        {
            get
            {
                return cardDetails.Month;
            }
            set
            {
                cardDetails.Month = value;
                OnPropertyChanged("SelectedMonth");
            }
        }

        /// <summary>
        /// Year collection.
        /// </summary>
        public ObservableCollection<short> CardValidityYear
        {
            get
            {
                return _cardValidityYear;
            }
            set
            {
                _cardValidityYear = value;
            }

        }

        /// <summary>
        /// credit or debit card expiry date year 
        /// </summary>
        public short SelectedYear
        {
            get
            {
                return cardDetails.Years;
            }
            set
            {
                cardDetails.Years = value;
                OnPropertyChanged("SelectedYear");
            }
        }

        /// <summary>
        /// Purchase action
        /// </summary>
        public RelayCommand PurchaseCommand { get; private set; }
         public ICommand PayOfflineCommand { get; set; }

        #region Order Details Summary
        /// <summary>
        /// CurrentDate
        /// </summary>
        public string CurrentDate
        {
            get
            {
                return currentDate;
            }

            set
            {
                currentDate = value;
            }
        }

        /// <summary>
        /// Expiry Date
        /// </summary>
        public string ExpDate
        {
            get
            {
                return expDate;
            }

            set
            {
                expDate = value;
            }
        }

        /// <summary>
        /// Tax amount
        /// </summary>
        public string Tax
        {
            get { return tax; }
            set { tax = value; }
        }
        /// <summary>
        /// Total cost of licence
        /// </summary>
        public string TotalCost
        {
            get { return totalCost; }
            set { totalCost = value; }

        }

        #endregion Order Details Summary

        #region IDataErrorInfo
        /// <summary>
        /// Error data 
        /// </summary>
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Data validation on user input.
        /// </summary>
        /// <param name="columnName">property name</param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                if ("CardName" == columnName)
                {
                    if (string.IsNullOrEmpty(CardName))
                    {
                        return "Name field is empty";
                    }
                }
                if ("CardNumber" == columnName)
                {
                    if (string.IsNullOrEmpty(CardNumber))
                    {
                        return "Card number field is empty";
                    }
                    if (CardNumber.Length != Constants.CARD_NUMBER_LENGTH)
                    {
                        return "Invalid card number";
                    }
                }
                if ("SelectedMonth" == columnName)
                {
                    if (string.IsNullOrEmpty(SelectedMonth))
                    {
                        return "Month field is empty";
                    }
                }
                if ("SelectedYear" == columnName)
                {
                    if (string.IsNullOrEmpty(SelectedYear.ToString()))
                    {
                        return "Year field is empty";
                    }
                }
                if ("CardCVV" == columnName)
                {
                    if (string.IsNullOrEmpty(CardCVV.ToString()))
                    {
                        return "CVV field is empty";
                    }
                    if (CardCVV.ToString().Length != Constants.CARD_CVV_LENGTH)
                    {
                        return "Invalid CVV";
                    }
                }
                return string.Empty;
            }
        }

        #endregion IDataErrorInfo

        /// <summary>
        ///Initialization of CreditAndDebitCardDetailsViewModel
        /// </summary>
        public CreditAndDebitCardDetailsViewModel()
        {
            cardDetails = new CardDetails();
            currentDate = "License Issued Date - " + DateTime.Now.ToString("M/d/yyyy");
            DateTime theDate = DateTime.Now;
            DateTime yearInTheFuture = theDate.AddYears(1);
            expDate = "License Expiry Date - " + yearInTheFuture;
            totalCost = "Total cost - "+ AppState.Instance.SelectedSubscription.Price + "$";
            tax = "Tax - "+ AppState.Instance.SelectedSubscription.Price * .05 + "$";
            LoadListOfYears();
            LoadListOfMonths();
            PurchaseCommand = new RelayCommand(OnPurchase);
            PayOfflineCommand = new RelayCommand(PayOffline);

        }

        private void PayOffline(object obj)
        {
            //Add to cart
            CartItem item = new CartItem();
            item.SubscriptionId = Convert.ToInt32(AppState.Instance.SelectedSubscription.Id);
            item.Quantity = 1;
            item.DateCreated = DateTime.Now;
            item.UserId = AppState.Instance.User.ServerUserId;
            HttpClient client0 = AppState.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client0.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
            var response = client0.PostAsJsonAsync("api/Cart/Create", item).Result;
            client0.Dispose();

            //=========================================

            PurchaseOrder poOrder = new PurchaseOrder();
            HttpClient client = AppState.CreateClient(ServiceType.CentralizeWebApi.ToString());
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
             response =  client.PostAsync("api/cart/offlinepayment/" + AppState.Instance.User.ServerUserId, null).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                if (!string.IsNullOrEmpty(jsonData))
                    poOrder = JsonConvert.DeserializeObject<PurchaseOrder>(jsonData);
                AppState.Instance.purchaseOrder = poOrder;
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                //var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                //ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            NavigateNextPage("OfflinePayment", null);
        }

        /// <summary>
        /// Load month list collection data.
        /// </summary>
        private void LoadListOfMonths()
        {
            for (short month = Constants.START_MONTH; month <= Constants.END_MONTH; month++)
            {
                _cardValidityMonth.Add(Convert.ToString((MonthEnum)month));
            }
        }

        /// <summary>
        /// Load year list collection data.
        /// </summary>
        private void LoadListOfYears()
        {
            _cardValidityYear = new ObservableCollection<short>();
            for (short year = Constants.START_YEAR; year < Constants.END_YEAR; year++)
            {
                _cardValidityYear.Add(year);
            }
        }

        /// <summary>
        /// Performing the licence purchase action.
        /// </summary>
        /// <param name="parm"></param>
        private void OnPurchase(object parm)
        {
            if (!string.IsNullOrEmpty(CardName) && !string.IsNullOrEmpty(CardNumber) && !string.IsNullOrEmpty(SelectedMonth)
                && !string.IsNullOrEmpty(SelectedYear.ToString()) && !string.IsNullOrEmpty(CardCVV.ToString()))
            {
                //Add to cart
                CartItem item = new CartItem();
                item.SubscriptionId = Convert.ToInt32(AppState.Instance.SelectedSubscription.Id);
                item.Quantity = 1;
                item.DateCreated = DateTime.Now;
                item.UserId = AppState.Instance.User.ServerUserId;
                HttpClient client0 = AppState.CreateClient(ServiceType.CentralizeWebApi.ToString());
                client0.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
                var response = client0.PostAsJsonAsync("api/Cart/Create", item).Result;
                client0.Dispose();
                // ===================
                
                SubscriptionList userSubscriptionList = null;
                var serviceType = System.Configuration.ConfigurationManager.AppSettings.Get("ServiceType");
                HttpClient client = AppState.CreateClient(ServiceType.CentralizeWebApi.ToString());
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.CentralizedToken.access_token);
                response = client.PostAsync("api/cart/OnlinePayment/" + AppState.Instance.User.ServerUserId, null).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsondata = response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrEmpty(jsondata))
                    {
                        userSubscriptionList = JsonConvert.DeserializeObject<SubscriptionList>(jsondata);
                        HttpClient client1 = AppState.CreateClient(ServiceType.OnPremiseWebApi.ToString());
                        client1.DefaultRequestHeaders.Add("Authorization", "Bearer " + AppState.Instance.OnPremiseToken.access_token);
                        userSubscriptionList.UserId = AppState.Instance.User.UserId;
                        var response1 = client1.PostAsJsonAsync("api/UserSubscription/SyncSubscription", userSubscriptionList).Result;
                        client1.Dispose();
                    }
                }
                var kvp = new Dictionary<string, string>();
                kvp.Add("Amount", AppState.Instance.SelectedSubscription.Price.ToString());
                NavigateNextPage("RedirectToAmountPaymentPage", kvp);
            }
        }
    }


}
