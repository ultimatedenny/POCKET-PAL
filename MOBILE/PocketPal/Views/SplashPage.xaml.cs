using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Essentials;
using POCKETPAL;
using POCKETPAL.Models;
using POCKETPAL.Classes;
using Plugin.FirebasePushNotification;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SplashPage : ContentPage
    {
        string UsePass = "";
        string AppName = "";
        string AppVersion = "";
        bool AppMaintenance = false;
        bool UpdateMandatory = false;
        public SplashPage()
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
                    CheckConnection();
                });
            });
        }
        public void CheckConnection()
        {
            var access = Connectivity.NetworkAccess;
            if (access == NetworkAccess.Internet)
            {
                GetVersion();
            }
            else
            {
                ThrowException("Offline", "You don't have connection.");
            }
        }
        public async void GetVersion()
        {
            string url = "" + Address.Api + "/Master/GetVersion";
            try
            {
                List<AppInfoModel> model = new List<AppInfoModel>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<List<AppInfoModel>>(content);
                    foreach (var Info in model)
                    {
                        AppName = Info.AppName.ToString();
                        AppVersion = Info.AppVersion.ToString();
                        AppMaintenance = Convert.ToBoolean(Info.AppMaintenance);
                        UpdateMandatory = Convert.ToBoolean(Info.UpdateMandatory);
                    }
                    if (Convert.ToDecimal(AppVersion) > Convert.ToDecimal(AppInfo.VersionString))
                    {
                        NeedUpdate(AppVersion);
                    }
                    else if (AppMaintenance == true)
                    {
                        ServerMaintenance("Our developers are hard at work updating your system. Please wait while we do this.");
                    }
                    else
                    {
                        string Username = GetValue("Username");
                        string Password = GetValue("Password");
                        string RememberMe = GetValue("RememberMe");
                        if (RememberMe == "Yes")
                        {
                            base.OnAppearing();
                            await SplashImage.ScaleTo(1, 1800);
                            DirectLogin(Username, Password);
                        }
                        else
                        {
                            base.OnAppearing();
                            await SplashImage.ScaleTo(1, 1800);
                            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                        }
                    }
                }
                else
                {
                    ThrowException(response.StatusCode.ToString(), "Oops, something wrong happend");
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "Oops, something wrong happend");
            }
        }
        public async void DirectLogin(string username, string password)
        {
            var Model = DeviceInfo.Model.ToUpper();
            var Manufacture = DeviceInfo.Manufacturer.ToUpper();
            var DeviceName = DeviceInfo.Name.ToUpper();
            if (DeviceName.Contains("'"))
            {
                DeviceName = DeviceName.Replace("'", "");
            }
            var OS = DeviceInfo.VersionString.ToUpper();
            string url = "" + Address.Api + "/User/SignIn?UseId=" + username + "&UsePass=" + password + "&DeviceName=" + DeviceName + "&Model=" + Model + "&Manufacture=" + Manufacture + "&OS=" + OS + "";
            try
            {
                List<User> model = new List<User>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    RootUser datas = JsonConvert.DeserializeObject<RootUser>(content);
                    if (datas.Success == true)
                    {
                        AddValue("UseID", datas.Data.UserID.ToString());
                        AddValue("UseNam", datas.Data.UserName.ToString());
                        AddValue("UseEmail", datas.Data.Email.ToString());
                        AddValue("Salutation", datas.Data.Salutation.ToString());
                        AddValue("UsePass", UsePass);
                        AddValue("UsePin", datas.Data.UserPIN.ToString());
                        AddValue("Domain", datas.Data.DomainName.ToString());
                        AddValue("UsePlant", datas.Data.PlantCode.ToString());
                        AddValue("UseDep", datas.Data.Department.ToString());
                        AddValue("UseTel", datas.Data.Phone.ToString());
                        AddValue("BusFunc", datas.Data.BusinessFunction.ToString());
                        TokenDelete();
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(datas.Message.ToString());
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
            catch (Exception msg)
            {
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
        public async void TokenDelete()
        {
            var CURRENTTOKEN = CrossFirebasePushNotification.Current.Token.ToString();
            string UseId = GetValue("UseID");
            string url = "" + Address.Api + "/User/DeleteToken?UserId=" + UseId + "&UserToken=" + CURRENTTOKEN;
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    await Task.Delay(500);
                    TokenRegister();
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
            catch (Exception msg)
            {
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
        public async void TokenRegister()
        {
            string UseId = GetValue("UseID");
            string UseNam = GetValue("UseNam");
            string BusFunc = GetValue("BusFunc");
            var CURRENTTOKEN = CrossFirebasePushNotification.Current.Token.ToString();
            string UsePin = GetValue("UsePin");
            if (string.IsNullOrEmpty(UseId) || string.IsNullOrEmpty(UseNam) || string.IsNullOrEmpty(BusFunc))
            {
                DependencyService.Get<IMessage>().ShortAlert("Register Token Failed");
            }
            else
            {
                string url = "" + Address.Api + "/User/RegisterToken?UserId=" + UseId + "&UseNam=" + UseNam + "&BusFunc=" + BusFunc + "&UserToken=" + CURRENTTOKEN + "";
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        if (UsePin == "0" || string.IsNullOrEmpty(UsePin))
                        {
                            await Shell.Current.GoToAsync($"//{nameof(PinCreatePage)}");
                        }
                        else
                        {
                            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                        }
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                }
                catch (Exception msg)
                {
                    DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
            }
        }
        public async void ThrowException(string message, string functions)
        {
            var action = await DisplayAlert("Warning", message.ToString(), "Report", "Dismiss");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"ReportPage");
            }
        }
        public async void NeedUpdate(string msg)
        {
            await DisplayAlert("Application Outdated", "Your PocketPal version is outdated. Please update application directly from Appstore or Google Play Store. \n\n" +
                    "Installed version: " + AppInfo.VersionString + "\n" +
                    "Newest  version: " + (msg.ToString()) + "\n\n" +
                    "If you still see this message, please report to IT by clicking report below.", "Ok", "Dismiss");
        }
        public async void ServerMaintenance(string msg)
        {
            await DisplayAlert("System Maintenance", msg.ToString() + "\n\n" +
                    "If you still see this message, please report to IT by clicking report below.", "Ok", "Dismiss");
        }
        public void AddValue(string key, string value)
        {
            Preferences.Set(key, value);
        }
        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }
    }
}