using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        Dictionary<int, string> _itmList0;
        Dictionary<string, string> _itmList1;
        Dictionary<string, string> _itmList2;
        Dictionary<string, string> _itmList3;

        public ObservableCollection<string> Items { get; set; }
        public RegisterPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    swtichSignupType.IsToggled = false;
                    getPlantArea();
                    VendorRegArea.IsVisible = false;
                    //getPlant();
                    //getApprovalGroup();
                    var navigationPage = Application.Current.MainPage as NavigationPage;
                    if (null != navigationPage)
                    {
                        navigationPage.BarTextColor = Color.FromHex("#000000");
                        navigationPage.BarBackgroundColor = Color.FromHex("#FFFFFF");
                    }
                    Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
                });
            });
        }
        public void siguptype(object sender, ToggledEventArgs e)
        {
            if (swtichSignupType.IsToggled == false)
            {
                windowstype.IsVisible = false;
                normaltype.IsVisible = true;
                txtSignUpType.Text = "Register Normal Account";
                usernameSignup.Placeholder = "Employee Number";
                phoneSignup.Placeholder = "Phone Number";
            }
            else
            {
                windowstype.IsVisible = true;
                normaltype.IsVisible = false;
                txtSignUpType.Text = "Register with Windows Account";
                usernameSignup.Placeholder = "Windows ID";
                phoneSignup.Placeholder = "Phone Number (Optional)";
            }
        }
        public async void getPlantArea()
        {
            PickerArea.SelectedIndex = -1;
            string url = "" + Address.Api + "/Master/GetPlantCountry";
            try
            {
                popupLoadingView.IsVisible = true;
                activityIndicator.IsRunning = true;
                lblLoadingText.Text = "Loading Country list...";
                List<PlantModels> plantList = new List<PlantModels>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();

                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    plantList.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    plantList = JsonConvert.DeserializeObject<List<PlantModels>>(content);
                    _itmList1 = new Dictionary<string, string>();
                    foreach (var plant in plantList)
                    {
                        _itmList1.Add(plant.PlantCountry.ToString(), plant.PlantCountry.ToString());
                    }
                    PickerArea.ItemsSource = _itmList1.Values.ToList();
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                }
                else
                {
                    ThrowException(response.StatusCode.ToString(), "getPlantArea");
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                    return;
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "getPlantArea");
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                lblLoadingText.Text = "";
                return;
            }
        }
        public async void getPlant(object sender, EventArgs e)
        {
            PickerPlant.SelectedIndex = -1;
            if (PickerArea.SelectedItem != null)
            {
                popupLoadingView.IsVisible = true;
                activityIndicator.IsRunning = true;
                lblLoadingText.Text = "Loading Plant list...";
                string url = "" + Address.Api + "/Master/GetplantList?PlantCountry=" + PickerArea.SelectedItem.ToString() + "";
                try
                {
                    List<PlantModels> plantList = new List<PlantModels>();
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    PickerPlant.ItemsSource = null;
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        plantList.Clear();
                        var content = await response.Content.ReadAsStringAsync();
                        plantList = JsonConvert.DeserializeObject<List<PlantModels>>(content);
                        _itmList1 = new Dictionary<string, string>();
                        foreach (var plant in plantList)
                        {
                            _itmList1.Add(plant.PlantCode.ToString(), plant.PlantCode + " - " + plant.PlantName.ToString());
                        }
                        PickerPlant.ItemsSource = _itmList1.Values.ToList();
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        lblLoadingText.Text = "";
                    }
                    else
                    {
                        ThrowException(response.StatusCode.ToString(), "getPlant");
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        lblLoadingText.Text = "";
                        return;
                    }
                }
                catch (Exception msg)
                {
                    ThrowException(msg.Message.ToString(), "getPlant");
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                    return;
                }
            }
        }
        public async void getDept(object sender, EventArgs e)
        {
            PickerDept.SelectedIndex = -1;
            if (PickerPlant.SelectedItem != null)
            {
                string comp = (PickerPlant.SelectedItem.ToString()).Substring(0, 4);
                if (comp == "9999")
                {
                    swtichSignupType.IsToggled = false;
                    swtichSignupType.IsEnabled = false;
                }
                else
                {
                    swtichSignupType.IsEnabled = true;
                }
                string url = "" + Address.Api + "/Master/GetDepartmentforDDList?PlantCode=" + (PickerPlant.SelectedItem.ToString()).Substring(0, 4);
                try
                {
                    popupLoadingView.IsVisible = true;
                    activityIndicator.IsRunning = true;
                    lblLoadingText.Text = "Loading Department list...";
                    List<DeptModels> deptList = new List<DeptModels>();
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        deptList.Clear();
                        var content = await response.Content.ReadAsStringAsync();
                        deptList = JsonConvert.DeserializeObject<List<DeptModels>>(content);
                        _itmList2 = new Dictionary<string, string>();
                        _itmList2.Clear();
                        foreach (var dept in deptList)
                        {
                            _itmList2.Add(dept.DepartmentID, dept.DepartmentID + " - " + dept.DepartmentName);
                        }
                        PickerDept.ItemsSource = _itmList2.Values.ToList();
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        lblLoadingText.Text = "";
                    }
                    else
                    {
                        ThrowException(response.StatusCode.ToString(), "getDept");
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        lblLoadingText.Text = "";
                        return;
                    }
                }
                catch (Exception msg)
                {
                    ThrowException(msg.Message.ToString(), "getDept");
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                    return;
                }
            }
        }
        private void getDept2(object sender, EventArgs e)
        {
            if (PickerDept.SelectedItem != null)
            {
                if (PickerDept.SelectedItem.ToString() == "OTH - Other Dept")
                {
                    VendorRegArea.IsVisible = true;
                }
                else
                {
                    VendorRegArea.IsVisible = false;
                }
            }
        }
        async void getApprovalGroup()
        {
            string url = "" + Address.Api + "/MasterData/GetApprovalGroupsDL";
            try
            {
                List<ApprovalModels> ApprovalModels = new List<ApprovalModels>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();

                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    ApprovalModels.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    ApprovalModels = JsonConvert.DeserializeObject<List<ApprovalModels>>(content);
                }
                _itmList3 = new Dictionary<string, string>();
                foreach (var approval in ApprovalModels)
                {
                    _itmList3.Add(approval.Group, approval.Group);
                }
                //PickerApproval.ItemsSource = _itmList3.Values.ToList();
                //PickerApproval.SelectedIndex = -1;
            }
            catch (Exception)
            {
                //DependencyService.Get<Toast>().Show(x.ToString());
                DependencyService.Get<IMessage>().ShortAlert("Unable to connect, please check your connection.");
                return;
            }
        }
        public void ShowPass(object sender, EventArgs args)
        {
            //passwordSignup.IsPassword = passwordSignup.IsPassword ? false : true;
            if (passwordSignup.IsPassword == true)
            {
                eyeicon.Source = ImageSource.FromFile("ic_eyeoff.png");
                passwordSignup.IsPassword = false;
            }
            else
            {
                eyeicon.Source = ImageSource.FromFile("ic_eyeon.png");
                passwordSignup.IsPassword = true;
            }
        }
        public async void SignUpClick(object sender, EventArgs e)
        {
            var device = DeviceInfo.Model;
            var manufacturer = DeviceInfo.Manufacturer;
            var deviceName = DeviceInfo.Name;
            if (deviceName.Contains("'"))
            {
                deviceName = deviceName.Replace("'", "");
            }
            var version = DeviceInfo.VersionString;
            Guid uniqueid = Guid.NewGuid();
            try
            {
                signupBtn.IsEnabled = false;
                var regex = new Regex(@"\s");
                if (swtichSignupType.IsToggled == false)
                {
                    if (PickerArea.SelectedIndex == -1 || PickerPlant.SelectedIndex == -1 || PickerDept.SelectedIndex == -1 ||
                        string.IsNullOrEmpty(usernameSignup.Text) || string.IsNullOrEmpty(fullnameSignup.Text) ||
                        string.IsNullOrEmpty(emailSignup.Text) ||
                        string.IsNullOrEmpty(fullnameSignup.Text) || string.IsNullOrEmpty(passwordSignup.Text))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Sorry, we are unable to complete the signup process. Please complete all field.");
                        signupBtn.IsEnabled = true;
                    }
                    else if (usernameSignup.Text.Contains(" ") || regex.IsMatch(usernameSignup.Text) == true)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("username cannot contain space character");
                        signupBtn.IsEnabled = true;
                    }
                    else if (passwordSignup.Text.Length < 5)
                    {
                        DependencyService.Get<IMessage>().ShortAlert("password must be more than 5 characters");
                        signupBtn.IsEnabled = true;
                    }
                    else
                    {
                        string comp = (PickerPlant.SelectedItem.ToString()).Substring(0, 4);
                        string urlx;
                        if (comp == "9999")
                        {
                            if (string.IsNullOrEmpty(vendorEmpNo.Text) || string.IsNullOrEmpty(vendorCompany.Text))
                            {
                                DependencyService.Get<IMessage>().ShortAlert("Sorry, we are unable to complete the signup process. Please complete all field.");
                                signupBtn.IsEnabled = true;
                            }
                            else
                            {
                                string UDID = (uniqueid.ToString().ToUpper());
                                string Area = PickerArea.SelectedItem.ToString();
                                string Plant = (PickerPlant.SelectedItem.ToString()).Substring(0, 4);
                                string VenComp = vendorCompany.Text;
                                string VenEmpNo = vendorEmpNo.Text;
                                var DeptString = PickerDept.SelectedItem.ToString().IndexOf(" ");
                                var _DeptString = PickerDept.SelectedItem.ToString().Substring(0, DeptString);
                                string name = fullnameSignup.Text.ToUpper();
                                string username = usernameSignup.Text.ToLower();
                                string email = emailSignup.Text;
                                string phone = (phoneSignup.Text == null) ? "" : phoneSignup.Text;
                                string password = passwordSignup.Text;
                                string regtype = "0";
                                string Salutation = PickerSaluation.SelectedItem.ToString();

                                string url1 = "?UDID=" + UDID + "" + "&UseNam=" + name + "" + "&UseId=" + username + "" + "&UsePass=" + password + "";
                                string url2 = "&UseEmail=" + email + "" + "&UsePlant=" + Plant + "" + "&UseArea=" + Area + "" + "&VenComp=" + VenComp + "" + "&VenEmpNo=" + VenEmpNo + "";
                                string url3 = "&UseDep=" + _DeptString.ToString() + "" + "&RegType=" + regtype + "" + "&UseTel=" + phone + "";
                                string url4 = "&Model=" + (device.ToString()).ToUpper() + "" + "&Manufacture=" + (manufacturer.ToString()).ToUpper() + "" + "&DeviceName=" + (deviceName.ToString()).ToUpper() + "" + "&OS=" + (version.ToString()).ToUpper() + "&Salutation=" + Salutation + "";
                                string url = "" + Address.Api + "/User/SignUp" + url1 + url2 + url3 + url4;
                                urlx = url;
                                HttpResponseMessage response = new HttpResponseMessage();
                                HttpClient client = new HttpClient();
                                List<RespondApi> model = new List<RespondApi>();
                                response = await client.GetAsync(urlx);
                                if (response.IsSuccessStatusCode)
                                {
                                    var content = await response.Content.ReadAsStringAsync();
                                    RootUser model2 = JsonConvert.DeserializeObject<RootUser>(content);

                                    if (model2.Success == true)
                                    {
                                        DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                                        signupBtn.IsEnabled = true;
                                        AddValue("UseID", usernameSignup.Text.ToLower());
                                        AddValue("UsePass", passwordSignup.Text);
                                        AddValue("UseNam", fullnameSignup.Text.ToUpper());
                                        await Shell.Current.GoToAsync($"//{nameof(PinCreatePage)}");
                                    }
                                    else
                                    {
                                        DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                                        signupBtn.IsEnabled = true;
                                    }
                                }
                                else
                                {
                                    signupBtn.IsEnabled = true;
                                    ThrowException(response.StatusCode.ToString(), "SignUpClick");
                                }
                            }
                        }
                        else
                        {
                            string UDID = (uniqueid.ToString().ToUpper());
                            string Area = PickerArea.SelectedItem.ToString();
                            string Plant = (PickerPlant.SelectedItem.ToString()).Substring(0, 4);
                            var DeptString = PickerDept.SelectedItem.ToString().IndexOf(" ");
                            var _DeptString = PickerDept.SelectedItem.ToString().Substring(0, DeptString);
                            string name = fullnameSignup.Text.ToUpper();
                            string username = usernameSignup.Text.ToLower();
                            string email = emailSignup.Text;
                            string phone = (phoneSignup.Text == null) ? "" : phoneSignup.Text;
                            string password = passwordSignup.Text;
                            string regtype = "0";
                            string Salutation = PickerSaluation.SelectedItem.ToString();

                            string url1 = "?UDID=" + UDID + "" + "&UseNam=" + name + "" + "&UseId=" + username + "" + "&UsePass=" + password + "";
                            string url2 = "&UseEmail=" + email + "" + "&UsePlant=" + Plant + "" + " &UseArea = " + Area + "";
                            string url3 = "&UseDep=" + _DeptString.ToString() + "" + "&RegType=" + regtype + "" + "&UseTel=" + phone + "";
                            string url4 = "&Model=" + (device.ToString()).ToUpper() + "" + "&Manufacture=" + (manufacturer.ToString()).ToUpper() + "" + "&DeviceName=" + (deviceName.ToString()).ToUpper() + "" + "&OS=" + (version.ToString()).ToUpper() + "&Salutation=" + Salutation + "";
                            string url = "" + Address.Api + "/User/SignUp" + url1 + url2 + url3 + url4;
                            urlx = url;

                            HttpResponseMessage response = new HttpResponseMessage();
                            HttpClient client = new HttpClient();
                            List<RespondApi> model = new List<RespondApi>();
                            response = await client.GetAsync(urlx);
                            if (response.IsSuccessStatusCode)
                            {
                                var content = await response.Content.ReadAsStringAsync();
                                RootUser model2 = JsonConvert.DeserializeObject<RootUser>(content);

                                if (model2.Success == true)
                                {
                                    DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                                    signupBtn.IsEnabled = true;
                                    AddValue("UseID", usernameSignup.Text.ToLower());
                                    AddValue("UseNam", fullnameSignup.Text.ToUpper());
                                    AddValue("UsePass", passwordSignup.Text);
                                    await Shell.Current.GoToAsync($"//{nameof(PinCreatePage)}");
                                }
                                else
                                {
                                    DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                                    signupBtn.IsEnabled = true;
                                }
                            }
                            else
                            {
                                signupBtn.IsEnabled = true;
                                ThrowException(response.StatusCode.ToString(), "SignUpClick");
                            }
                        }
                    }
                }
                else
                {
                    if (PickerArea.SelectedIndex == -1 || PickerPlant.SelectedIndex == -1 || PickerDept.SelectedIndex == -1 ||
                       string.IsNullOrEmpty(usernameSignup.Text) || string.IsNullOrEmpty(fullnameSignup.Text) || string.IsNullOrEmpty(emailSignup.Text))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Sorry, we are unable to complete the signup process. Please complete all field.");
                        signupBtn.IsEnabled = true;
                    }
                    else
                    {
                        string UDID = (uniqueid.ToString().ToUpper());
                        string Area = PickerArea.SelectedItem.ToString();
                        string Plant = (PickerPlant.SelectedItem.ToString()).Substring(0, 4);

                        var DeptString = PickerDept.SelectedItem.ToString().IndexOf(" ");
                        var _DeptString = PickerDept.SelectedItem.ToString().Substring(0, DeptString);
                        string name = fullnameSignup.Text.ToUpper();
                        string username = usernameSignup.Text.ToLower();
                        string email = emailSignup.Text;
                        string phone = (phoneSignup.Text == null) ? "" : phoneSignup.Text;
                        string extnumber = extSignup.Text;
                        string password = passwordSignup.Text;
                        string regtype = "1";
                        string Salutation = PickerSaluation.SelectedItem.ToString();

                        string url1 = "?UDID=" + UDID + "" + "&UseNam=" + name + "" + "&UseId=" + username + "&UsePass=" + password + "";
                        string url2 = "&UseEmail=" + email + "" + "&UsePlant=" + Plant + "" + " &UseArea = " + Area + "";
                        string url3 = "&UseDep=" + _DeptString.ToString() + "" + "&RegType=" + regtype + "" + "&UseTel=" + ("(" + extnumber + ")" + phone) + "";
                        string url4 = "&Model=" + (device.ToString()).ToUpper() + "" + "&Manufacture=" + (manufacturer.ToString()).ToUpper() + "" + "&DeviceName=" + (deviceName.ToString()).ToUpper() + "" + "&OS=" + (version.ToString()).ToUpper() + "&Salutation=" + Salutation + "";
                        string url = "" + Address.Api + "/User/SignUp" + url1 + url2 + url3 + url4;

                        HttpResponseMessage response = new HttpResponseMessage();
                        HttpClient client = new HttpClient();
                        List<RespondApi> model = new List<RespondApi>();
                        response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            RootUser model2 = JsonConvert.DeserializeObject<RootUser>(content);
                            if (model2.Success == true)
                            {
                                DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                                signupBtn.IsEnabled = true;
                                AddValue("UseID", usernameSignup.Text.ToLower());
                                AddValue("UseNam", fullnameSignup.Text.ToUpper());
                                AddValue("UsePass", passwordSignup.Text);
                                await Shell.Current.GoToAsync($"//{nameof(PinCreatePage)}");
                                //App.Current.MainPage = new NavigationPage(new PinCreatePage());
                            }
                            else
                            {
                                DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                                signupBtn.IsEnabled = true;
                            }
                        }
                        else
                        {
                            signupBtn.IsEnabled = true;
                            ThrowException(response.StatusCode.ToString(), "SignUpClick");
                        }
                    }
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "SignUpClick");
                signupBtn.IsEnabled = true;
            }
        }
        async void OpenTerms(object sender, EventArgs e)
        {
            string path = "https://raw.githubusercontent.com/PT-SHIMANOBATAM/TCPP/main/Pocket%20Pal%20-%20Terms%20%26%20Conditions.md";
            string title = "Terms & Conditions";
            AddValue("TempPath", path);
            AddValue("TempTitle", title);
            await Shell.Current.GoToAsync($"WebviewPage");
        }
        public void AddValue(string key, string value)
        {
            Preferences.Set(key, value);
        }
        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }
        public async void ThrowException(string message, string function)
        {
            var action = await DisplayAlert("Warning", message.ToString(), "Report", "Dismiss");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"ReportPage");
            }
        }
        public void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess.ToString();
            var profiles = e.ConnectionProfiles.ToString();
            if (access != NetworkAccess.Internet.ToString())
            {
                string msg = "You don't have connection";
                ThrowException(msg, "Connectivity_ConnectivityChanged");
            }
        }
    }
}