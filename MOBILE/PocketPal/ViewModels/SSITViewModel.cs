using MvvmHelpers.Commands;
using Newtonsoft.Json;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using POCKETPAL.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace POCKETPAL.ViewModels
{
    public class SSITViewModel: INotifyPropertyChanged
    {
        #region Propertites
        string SSITUserGroup, SSITApprCat, SSITUserID, _menuSSIT, _menuType,_Product;
        bool _IsRefreshing;
        private string _selectedButtonText = "Check All";
        private string _imgbtn = "ic_check_box_outline_blank";
        string _Vendor = "";
        string _Action = "";

        public string SelectedButtonText
        {
            get => _selectedButtonText;
            set { _selectedButtonText = value; OnPropertyChanged(); }
        }
        public string imgbtn
        {
            get => _imgbtn;
            set { _imgbtn = value; OnPropertyChanged(); }
        }
        public bool IsRefreshing
        {
            get => _IsRefreshing;
            set { _IsRefreshing = value; OnPropertyChanged(); }
        }
        public string menuSSIT
        {
            get => _menuSSIT;
            set { _menuSSIT = value; OnPropertyChanged(); }
        }

        private SSITPlantModel _SelectedPlant ;
        public SSITPlantModel SelectedPlant
        {
            get
            {
                return _SelectedPlant;
            }
            set
            {
                SetProperty(ref _SelectedPlant, value);
                GETVENDOR(_SelectedPlant.PlantCode);
            }
        }
        private SSITProductModel _SelectedProduct ;
        public SSITProductModel SelectedProduct
        {
            get
            {
                return _SelectedProduct;
            }
            set
            {
                SetProperty(ref _SelectedProduct, value);
                _Product = _SelectedProduct.ProductCode;
                LoadDataSSIT();
            }
        }
        private SSITVendorModel _SelectedVendor ;
        public SSITVendorModel SelectedVendor
        {
            //get => _SelectedVendor;
            //set { _SelectedVendor = value; OnPropertyChanged(); }
            get
            {
                return _SelectedVendor;
            }
            set
            {
                if (_SelectedVendor != value)
                {
                    SetProperty(ref _SelectedVendor, value);
                    if (value != null)
                    {
                        _Vendor = _SelectedVendor.VendorCode;

                    }
                }
                

                //if (SetProperty(ref _SelectedVendor, value) == true)
                //{
                //    if (_SelectedVendor.ToString() != "(null)")
                //    {
                //        _Vendor = _SelectedVendor.VendorCode;

                //    }
                //}
                //_Vendor = "";
                //if (!string.IsNullOrWhiteSpace(_SelectedVendor.ToString()))
                //{
                //_Vendor = _SelectedVendor.VendorCode;
                //}
                //GETACTION();
            }
        }
        private SSITActionModel _SelectedAction ;
        public SSITActionModel SelectedAction
        {
            get
            {
                return _SelectedAction;
            }
            set
            {
                SetProperty(ref _SelectedAction, value);
                //_Action = "";
                //if (!string.IsNullOrWhiteSpace(_SelectedAction.ToString()))
                //{
                _Action = _SelectedAction.ActionDesc;
                //}

            }
        }
        public string menuType
        {
            get => _menuType;
            set { _menuType = value; }
        }
        public ObservableCollection<SSITModel> ListSSIT { get; set; }
        public ObservableCollection<SSITPlantModel> ListSSITPlantModel { get; set; }
        public ObservableCollection<SSITVendorModel> ListSSITVendorModel { get; set; }
        public ObservableCollection<SSITActionModel> ListSSITActionModel { get; set; }
        public ObservableCollection<SSITProductModel> ListSSITProductModel { get; set; }
        #endregion

        public SSITViewModel(string _xmenuSSIT,string Type)
        {
            menuSSIT = _xmenuSSIT;
            menuType = Type;
            ListSSIT = new ObservableCollection<SSITModel>();
            ListSSITPlantModel = new ObservableCollection<SSITPlantModel>();
            ListSSITVendorModel = new ObservableCollection<SSITVendorModel>();
            ListSSITActionModel = new ObservableCollection<SSITActionModel>();
            ListSSITProductModel = new ObservableCollection<SSITProductModel>();
            IsEnabled = true;
            LoadProduct();
            LoadDataSSIT();
            LoadPlant();
            GETACTION();
        }

        public async void LoadDataSSIT()
        {
            
            IsRefreshing = true;
            string url = "";
            SSITUserGroup = GetValue("SSITUserGroup");
            SSITApprCat = GetValue("SSITApprCat");
            SSITUserID = GetValue("SSITUserID");

            List<SSITModel> ssitModel = new List<SSITModel>();
            if (_menuSSIT.ToUpper() == "SCRAP")
            {
                if (menuType == "LIST")
                {
                    url = "" + Address.Api + "/SSIT/DetailListSSIT_SCRAP?Type=" + _menuSSIT.ToUpper() + "&UserId=" + SSITUserID + "&UserGroup=" + SSITUserGroup + "&ApprCat=" + SSITApprCat + "&Product="+ _Product + "&SSITCode=";
                }
                else
                {
                    url = "" + Address.Api + "/SSIT/DetailSummarySSIT_SCRAP?Type=" + _menuSSIT.ToUpper() + "&UserId=" + SSITUserID + "&UserGroup=" + SSITUserGroup + "&ApprCat=" + SSITApprCat + "&Product=" + _Product + "&SSITCode=";

                }

            }
            else
            {
                if (menuType == "LIST")
                {
                    url = "" + Address.Api + "/SSIT/DetailListSSIT_PR?Type=" + _menuSSIT.ToUpper() + "&UserId=" + SSITUserID + "&UserGroup=" + SSITUserGroup + "&ApprCat=" + SSITApprCat + "&Product=" + _Product + "&SSITCode=";
                }
                else
                {
                    url = "" + Address.Api + "/SSIT/DetailSummarySSIT_PR?Type=" + _menuSSIT.ToUpper() + "&UserId=" + SSITUserID + "&UserGroup=" + SSITUserGroup + "&ApprCat=" + SSITApprCat + "&Product=" + _Product + "&SSITCode=";

                }
            }
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            response = await client.GetAsync(url);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    ssitModel.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    ssitModel = JsonConvert.DeserializeObject<List<SSITModel>>(content);

                    ListSSIT.Clear();
                    var _ssit = new SSITModel();
                    foreach (var _ssitModel in ssitModel)
                    {
                        _ssit = new SSITModel();
                        _ssit.Items = _ssitModel.Items;
                        _ssit.SSIT_ID = _ssitModel.SSIT_ID;
                        _ssit.Material = _ssitModel.Material;
                        _ssit.MaterialDesc = _ssitModel.MaterialDesc;
                        _ssit.Qty = _ssitModel.Qty;
                        _ssit.BaseUOM = _ssitModel.BaseUOM;
                        _ssit.Amount = _ssitModel.Amount;
                        _ssit.UnitPrice = _ssitModel.UnitPrice;
                        _ssit.Currency = _ssitModel.Currency;
                        _ssit.SubmitBy = _ssitModel.SubmitBy;
                        _ssit.SubmitDate = _ssitModel.SubmitDate;
                        _ssit.NCCode = _ssitModel.NCCode;
                        _ssit.NCCodeDet = _ssitModel.NCCodeDet;
                        _ssit.NCDetail = _ssitModel.NCDetail;
                        _ssit.CanApproved = _ssitModel.CanApproved;
                        ListSSIT.Add(_ssit);
                    }


                    IsRefreshing = false;
                }
                else
                {
                    IsRefreshing = false;
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async void LoadPlant()
        {
            ListSSITPlantModel.Clear();
            string url = "";
            List<SSITPlantModel> ssitModel = new List<SSITPlantModel>();
            url = "" + Address.Api + "/SSIT/GETPLANT";
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            response = await client.GetAsync(url);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    ssitModel.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    ssitModel = JsonConvert.DeserializeObject<List<SSITPlantModel>>(content);

                    var _SSITPlantModel = new SSITPlantModel();

                    foreach (var _ssitModel in ssitModel)
                    {
                        _SSITPlantModel = new SSITPlantModel();
                        _SSITPlantModel.PlantCode = _ssitModel.PlantCode;
                        _SSITPlantModel.PlantDesc = _ssitModel.PlantCode +" - "+_ssitModel.PlantDesc;
                        ListSSITPlantModel.Add(_SSITPlantModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        public async void LoadProduct()
        {
            ListSSITProductModel.Clear();
            string url = "";
            List<SSITProductModel> ssitModel = new List<SSITProductModel>();
            url = "" + Address.Api + "/SSIT/GETPRODUCT";
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            response = await client.GetAsync(url);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    ssitModel.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    ssitModel = JsonConvert.DeserializeObject<List<SSITProductModel>>(content);

                    var _SSITProductModel = new SSITProductModel();

                    foreach (var _ssitModel in ssitModel)
                    {
                        _SSITProductModel = new SSITProductModel();
                        _SSITProductModel.ProductCode = _ssitModel.ProductCode;
                        _SSITProductModel.ProductDesc = _ssitModel.ProductCode;
                        ListSSITProductModel.Add(_SSITProductModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        #region Methods
        private void BindData()
        {
            SSITModel emp = new SSITModel();
            emp.SSIT_ID = "abc@gmail.com";
            emp.Material = "Pragnesh";
            emp.MaterialDesc = "Male";
            ListSSIT.Add(emp);

            emp = new SSITModel();
            emp.SSIT_ID = "abc@gmail.com";
            emp.Material = "Pragnesh";
            emp.MaterialDesc = "Male";
            ListSSIT.Add(emp);

            emp = new SSITModel();
            emp.SSIT_ID = "abc@gmail.com";
            emp.Material = "Pragnesh";
            emp.MaterialDesc = "Male";
            ListSSIT.Add(emp);


            emp = new SSITModel();
            emp.SSIT_ID = "abc@gmail.com";
            emp.Material = "Pragnesh";
            emp.MaterialDesc = "Male";
            ListSSIT.Add(emp);

            emp = new SSITModel();
            emp.SSIT_ID = "abc@gmail.com";
            emp.Material = "Pragnesh";
            emp.MaterialDesc = "Male";
            ListSSIT.Add(emp);

            emp = new SSITModel();
            emp.SSIT_ID = "abc@gmail.com";
            emp.Material = "Pragnesh";
            emp.MaterialDesc = "Male";
            ListSSIT.Add(emp);

            emp = new SSITModel();
            emp.SSIT_ID = "abc@gmail.com";
            emp.Material = "Pragnesh";
            emp.MaterialDesc = "Male";
            ListSSIT.Add(emp);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        protected bool SetProperty<T>(ref T backfield, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(backfield, value))
            {
                return false;
            }
            backfield = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        #endregion

        #region Commands
        public ICommand SelectAllCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (SelectedButtonText == "Check All")
                    {
                        ListSSIT.ForEach(f => { f.IsChecked = true; });
                        SelectedButtonText = "UnCheck All";
                        imgbtn = "ic_check_box";
                    }
                    else
                    {
                        ListSSIT.ForEach(f => { f.IsChecked = false; });
                        SelectedButtonText = "Check All";
                        imgbtn = "ic_check_box_outline_blank";

                    }
                });
            }
        }
        public ICommand FetchSelectedItemCommand
        {
            get
            {
                return new Command(() =>
                {
                    var selectedItem = ListSSIT.Where(f => f.IsChecked == true).ToList();
                    App.Current.MainPage.DisplayAlert("Selected Item Count", selectedItem.Count + "", "OK");
                });
            }
        }
        public ICommand Approve
        {
            get
            {
                return new Command(() =>
                {
                    var selectedItem = ListSSIT.Where(f => f.IsChecked == true).ToList();

                    var _ssitselected = "";
                    decimal _ssitAmount = 0;

                    if (selectedItem.Count > 0)
                    {
                        foreach (var _selected in selectedItem)
                        {
                            _ssitselected += _selected.SSIT_ID + ",";
                            _ssitAmount += Convert.ToDecimal(_selected.Amount.Replace(",","."));
                        }
                        
                        if (menuSSIT.ToUpper() == "SCRAP" || (menuSSIT.ToUpper() == "PURCHASE RETURN" && Convert.ToInt32(SSITApprCat) > 1))
                        {
                            ApproveSSIT(selectedItem.Count.ToString(), _ssitselected, _ssitAmount.ToString());
                        }
                        else
                        {
                            if (_Vendor == "" || _Action == "")
                            {
                                Xamarin.Forms.DependencyService.Get<IMessage>().ShortAlert("Please choice vendor and action.");
                            }
                            else
                            {
                                ApproveSSIT(selectedItem.Count.ToString(), _ssitselected, _ssitAmount.ToString());
                            }
                        }
                    }
                    else
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().ShortAlert("Please select Material.");
                    }
                    
                });
            }
        }


        public void UnCheckAll()
        {
            ListSSIT.ForEach(f => { f.IsChecked = false; });
        }
        public void CheckAll()
        {
            ListSSIT.ForEach(f => { f.IsChecked = true; });
        }

        #endregion
        public void AddValue(string key, string value)
        {
            Preferences.Set(key, value);
        }
        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }

        public async void ApproveSSIT(string SSITCount, string _ssitselected, string _ssitAmount)
        {
            var action = await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Confirm Alert", "You will approved "+ SSITCount + " item with total amount is $" + _ssitAmount + "", "Yes", "No");

            if (action == true)
            {
                IsEnabled = false;
                var url = "" + Address.Api + "/SSIT/ApproveSSIT";
                var httpClient = new HttpClient();
                var data = new
                {
                    Type = _menuSSIT.ToUpper(),
                    UserId = SSITUserID,
                    UserGroup = SSITUserGroup,
                    ApprCat = SSITApprCat,
                    SSITCode = _ssitselected,
                    SSITCount = SSITCount,
                    Amount = _ssitAmount,
                    AppVersion = AppInfo.VersionString.ToString(),
                    Vendor = _Vendor,
                    Action = _Action,
                };

                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, jsonData);

                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        RootSSITModel model2 = JsonConvert.DeserializeObject<RootSSITModel>(content);

                        if (model2.Data != null)
                        {
                            Rfrsh();
                            Xamarin.Forms.DependencyService.Get<IMessage>().ShortAlert(model2.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
            }
        }

        public ICommand Reject
        {
            get
            {
                return new Command(() =>
                {
                    var selectedItem = ListSSIT.Where(f => f.IsChecked == true).ToList();

                    var _ssitselected = "";
                    decimal _ssitAmount = 0;

                    if (selectedItem.Count > 0)
                    {
                        foreach (var _selected in selectedItem)
                        {
                            _ssitselected += _selected.SSIT_ID + ",";
                            _ssitAmount += Convert.ToDecimal(_selected.Amount.Replace(",", "."));
                        }
                        RejectSSIT(selectedItem.Count.ToString(), _ssitselected, _ssitAmount.ToString());
                    }
                    else
                    {
                        Xamarin.Forms.DependencyService.Get<IMessage>().ShortAlert("Please select Material.");
                    }

                });
            }
        }

        public async void RejectSSIT(string SSITCount, string _ssitselected, string _ssitAmount)
        {
            var result = await Xamarin.Forms.Application.Current.MainPage.DisplayPromptAsync("Reason", "Please fill reason reject!", "OK", "Cancel");

            if (!string.IsNullOrWhiteSpace(result))
            {
                IsEnabled = false;
                var url = "" + Address.Api + "/SSIT/RejectSSIT";
                var httpClient = new HttpClient();
                var data = new
                {
                    Type = _menuSSIT.ToUpper(),
                    UserId = SSITUserID,
                    UserGroup = SSITUserGroup,
                    ApprCat = SSITApprCat,
                    SSITCode = _ssitselected,
                    SSITCount = SSITCount,
                    Amount = _ssitAmount,
                    AppVersion = AppInfo.VersionString.ToString(),
                    Reason = result,
                };

                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, jsonData);

                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        RootSSITModel model2 = JsonConvert.DeserializeObject<RootSSITModel>(content);

                        if (model2.Success ==true)
                        {
                            Xamarin.Forms.DependencyService.Get<IMessage>().ShortAlert("Success");

                            Rfrsh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error: {ex.Message}");
                }
            }
            else if (result=="")
            {
                Xamarin.Forms.DependencyService.Get<IMessage>().ShortAlert("Please fill reason.");
            }
            else
            {
                Xamarin.Forms.DependencyService.Get<IMessage>().ShortAlert("Cancel");
            }
        }
        private void Rfrsh()
        {
            ListSSIT.ForEach(f => { f.IsChecked = false; });
            ListSSIT.Clear();
            SelectedButtonText = "Check All";
            imgbtn = "ic_check_box_outline_blank";
            LoadDataSSIT();
            IsEnabled = true;
        }
        public ICommand Refresh
        {
            get
            {
                return new Command(() =>
                {
                    Rfrsh();
                });
            }
        }

        public async void GETVENDOR(string SelectPlant)
        {
            Keyword = "";
            _Vendor = "";
            ListSSITVendorModel.Clear();
            string url = "";
            List<SSITVendorModel> ssitModel = new List<SSITVendorModel>();
            url = "" + Address.Api + "/SSIT/GETVENDOR?Plant=" + SelectPlant + "";
            HttpResponseMessage response = new HttpResponseMessage();
            HttpClient client = new HttpClient();
            response = await client.GetAsync(url);
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    ssitModel.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    ssitModel = JsonConvert.DeserializeObject<List<SSITVendorModel>>(content);

                    var _SSITVendorModel = new SSITVendorModel();
                    Fruits.Clear();
                    IsVisible = false;
                    foreach (var _ssitModel in ssitModel)
                    {
                        _SSITVendorModel = new SSITVendorModel();
                        _SSITVendorModel.VendorCode = _ssitModel.VendorCode;
                        _SSITVendorModel.VendorDesc = _ssitModel.VendorCode + " - " + _ssitModel.VendorDesc;
                        Fruits.Add(_ssitModel.VendorCode + " - " + _ssitModel.VendorDesc);
                        ListSSITVendorModel.Add(_SSITVendorModel);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
        public async void GETACTION()
        {
            SSITActionModel action = new SSITActionModel();
            action.ActionCode = "CLAIM";
            action.ActionDesc = "CLAIM";
            ListSSITActionModel.Add(action);

            action = new SSITActionModel();
            action.ActionCode = "REWORK";
            action.ActionDesc = "REWORK";
            ListSSITActionModel.Add(action);

            action = new SSITActionModel();
            action.ActionCode = "REWORK AND CLAIM";
            action.ActionDesc = "REWORK AND CLAIM";
            ListSSITActionModel.Add(action);

            //string url = "";
            //List<SSITVendorModel> ssitModel = new List<SSITVendorModel>();
            //url = "" + Address.Api + "/SSIT/GETACTION";
            //HttpResponseMessage response = new HttpResponseMessage();
            //HttpClient client = new HttpClient();
            //response = await client.GetAsync(url);
            //try
            //{
            //    if (response.IsSuccessStatusCode)
            //    {
            //        ssitModel.Clear();
            //        var content = await response.Content.ReadAsStringAsync();
            //        ssitModel = JsonConvert.DeserializeObject<List<SSITVendorModel>>(content);

            //        var _SSITVendorModel = new SSITVendorModel();

            //        foreach (var _ssitModel in ssitModel)
            //        {
            //            _SSITVendorModel = new SSITVendorModel();
            //            _SSITVendorModel.VendorCode = _ssitModel.VendorCode;
            //            _SSITVendorModel.VendorDesc = _ssitModel.VendorCode + " - " + _ssitModel.VendorDesc;
            //            ListSSITVendorModel.Add(_SSITVendorModel);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    throw new Exception($"Error: {ex.Message}");
            //}
        }

        private string name;
        public string SearchEntry
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public ICommand SearchLIST
        {
            get
            {
                return new Command(() =>
                {
                    _Product = name;
                    LoadDataSSIT();
                });
            }
        }

        //TEST SEARCH BAR
        
        private string _keyword;
        public string Keyword
        {
            get { return _keyword; }

            set
            {
                _keyword = value;
                OnPropertyChanged();
            }
        }

        private List<string> _suggestions;
        public List<string> Suggestions
        {
            get { return _suggestions; }

            set
            {
                _suggestions = value;
                OnPropertyChanged();
            }
        }

        public Command SearchCommand
        {
            get
            {
                return new Command(Search);
            }
        }

        private void Search(object obj)
        {
            _Vendor = "";
            if (_keyword.Length >= 1)
            {
                Suggestions = Fruits.Where(f => f.ToLowerInvariant().Contains(_keyword)).ToList();
                if (Suggestions.Count > 0)
                {
                    IsVisible = true;
                }
                else
                {
                    IsVisible = false;
                }
                
            }
            else
            {
                IsVisible = false;
            }
        }

        public static List<string> Fruits { get; } = new List<string>();

       

        private bool _isvisible;
        public bool IsVisible
        {
            get { return _isvisible; }

            set
            {
                _isvisible = value;
                OnPropertyChanged();
            }
        }

        private bool _IsEnabled;
        public bool IsEnabled
        {
            get { return _IsEnabled; }

            set
            {
                _IsEnabled = value;
                OnPropertyChanged();
            }
        }

        public Command<string> AddComand
        {
            get
            {
                return new Command<string>(Add);
            }
        }

        private void Add(string vendor)
        {
            
            Keyword = vendor;

            string test;
            var words = Keyword.Split(new[] { " - " }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length >=1)
            {
                _Vendor = words[0].ToString();
            }

            IsVisible = false;
        }
    }
}
