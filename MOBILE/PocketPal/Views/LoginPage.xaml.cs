using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using POCKETPAL.Models;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Essentials;
using App.User.LocationInfo.Services;
using POCKETPAL.Classes;
using Plugin.FirebasePushNotification;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage(string user, string pass )
        {
            InitializeComponent();
            usernameLogin.Text = user;
            passwordLogin.Text = pass;
            CheckBoxRememberme.IsChecked = true;
            Login1();
        }
        public LoginPage()
        {
            InitializeComponent();
        }

        public void LoginClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    
                });
            });
        }
        public void CheckConnection(object sender, ConnectivityChangedEventArgs e)
        {
            try
            {
                var access = e.NetworkAccess.ToString();
                var profiles = e.ConnectionProfiles.ToString();
                if (access != "Internet")
                {
                    loginBtn.IsEnabled = false;
                    DependencyService.Get<IMessage>().ShortAlert("No Connection");
                }
                else
                {
                    loginBtn.IsEnabled = true;
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "CHECKCONNECTION");
            }
        }

        public async void GetLocation()
        {
            try
            {
                var locationStatus = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
                while (locationStatus != PermissionStatus.Granted)
                {
                    locationStatus = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                    var ip = await TrackingService.GetUserIPAdressAsync();
                    var country_code = await TrackingService.GetUserCountryCodeAsync();

                    AddValue("IP", ip.ToString());
                    AddValue("COUNTRY", country_code.ToString());

                }
                if (locationStatus == PermissionStatus.Granted)
                {
                    var ip = await TrackingService.GetUserIPAdressAsync();
                    var country_code = await TrackingService.GetUserCountryCodeAsync();

                    AddValue("IP", ip.ToString());
                    AddValue("COUNTRY", country_code.ToString());
                }
                else
                {
                    await DisplayAlert("Alert", "You have to provide location permission", "OK");
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "GETLOCATION");
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
        public void AddValue(string key, string value)
        {
            Preferences.Set(key, value);
        }
        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }

        private void ShowPass(object sender, EventArgs e)
        {
            if (passwordLogin.IsPassword == true)
            {
                eyeicon.Source = ImageSource.FromFile("round_visibility_black_48.png");
                passwordLogin.IsPassword = false;
            }
            else
            {
                eyeicon.Source = ImageSource.FromFile("round_visibility_off_black_48.png");
                passwordLogin.IsPassword = true;
            }
        }

        private void LoginClick(object sender, EventArgs e)
        {
            Login1();
        }
        public async void Login1()
        {
            loginBtn.IsEnabled = false;
            startLoading();
            var Model = DeviceInfo.Model.ToUpper();
            var Manufacture = DeviceInfo.Manufacturer.ToUpper();
            var DeviceName = DeviceInfo.Name.ToUpper();
            if (DeviceName.Contains("'"))
            {
                DeviceName = DeviceName.Replace("'", "");
            }
            var OS = DeviceInfo.VersionString.ToUpper();
            string username = usernameLogin.Text;
            string password = passwordLogin.Text;
            string url = "" + Address.Api + "/User/SignIn?UseId=" + username + "&UsePass=" + password + "&DeviceName=" + DeviceName + "&Model=" + Model + "&Manufacture=" + Manufacture + "&OS=" + OS + "";
            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    stopLoading();
                    DependencyService.Get<IMessage>().LongAlert("Please enter your credentials");
                }
                else
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
                            AddValue("UsePass", passwordLogin.Text);
                            AddValue("UsePin", datas.Data.UserPIN.ToString());
                            AddValue("Domain", datas.Data.DomainName.ToString());
                            AddValue("UsePlant", datas.Data.PlantCode.ToString());
                            AddValue("UseDep", datas.Data.Department.ToString());
                            AddValue("UseTel", datas.Data.Phone.ToString());
                            AddValue("BusFunc", datas.Data.BusinessFunction.ToString());
                            if (CheckBoxRememberme.IsChecked == true)
                            {
                                AddValue("Username", usernameLogin.Text);
                                AddValue("Password", passwordLogin.Text);
                                AddValue("RememberMe", "Yes");
                                if (datas.Data.UserPIN.ToString() == "0" || datas.Data.UserPIN.ToString() == "" || datas.Data.UserPIN.ToString() == null)
                                {
                                    Application.Current.MainPage = new NavigationPage(new PinCreatePage());
                                }
                                else
                                {
                                    TokenDelete();
                                }
                            }
                            else
                            {
                                AddValue("RememberMe", "No");
                                if (datas.Data.UserPIN.ToString() == "0" || datas.Data.UserPIN.ToString() == "" || datas.Data.UserPIN.ToString() == null)
                                {
                                    Application.Current.MainPage = new NavigationPage(new PinCreatePage());
                                }
                                else
                                {
                                    TokenDelete();
                                }
                            }
                        }
                        else
                        {
                            loginBtn.IsEnabled = true;
                            stopLoading();
                            DependencyService.Get<IMessage>().LongAlert(datas.Message.ToString());
                        }
                    }
                    else
                    {
                        loginBtn.IsEnabled = true;
                        stopLoading();
                        DependencyService.Get<IMessage>().LongAlert(response.StatusCode.ToString());
                    }
                }
            }
            catch (Exception msg)
            {
                loginBtn.IsEnabled = true;
                stopLoading();
                DependencyService.Get<IMessage>().LongAlert(msg.Message.ToString());
            }
        }
        public async void TokenRegister()
        {
            string UseId = GetValue("UseID");
            string UseNam = GetValue("UseNam");
            string BusFunc = GetValue("BusFunc");
            var CURRENTTOKEN = CrossFirebasePushNotification.Current.Token.ToString();

            if (UseId == "" || UseNam == "" || BusFunc == "")
            {
                DependencyService.Get<IMessage>().ShortAlert("Register Token Failed");
                loginBtn.IsEnabled = true;
                startLoading();
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
                        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                    }
                    else
                    {
                        loginBtn.IsEnabled = true;
                        stopLoading();
                        DependencyService.Get<IMessage>().LongAlert(response.StatusCode.ToString());
                    }
                }
                catch (Exception msg)
                {
                    DependencyService.Get<IMessage>().LongAlert(msg.Message.ToString());
                }
            }
        }
        public async void TokenDelete()
        {
            var CURRENTTOKEN = CrossFirebasePushNotification.Current.Token.ToString();
            //string CURRENTTOKEN = Application.Current.Properties["Fcmtocken"].ToString();
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
                    loginBtn.IsEnabled = true;
                    stopLoading();
                    DependencyService.Get<IMessage>().LongAlert(response.StatusCode.ToString());
                }
            }
            catch (Exception msg)
            {
                DependencyService.Get<IMessage>().LongAlert(msg.Message.ToString());
            }
        }
        private void SignUpClick(object sender, EventArgs e)
        {
            //var token = CrossFirebasePushNotification.Current.Token.ToString();
            //DependencyService.Get<IMessage>().ShortAlert(token);
            Shell.Current.GoToAsync($"RegisterPage");
        }

        private void ForgotPassword(object sender, EventArgs e)
        {
            DependencyService.Get<IMessage>().ShortAlert("This menu undermaintenance, please do manual reset with IT Dept.");
        }
        public void startLoading()
        {
            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;
        }
        public void stopLoading()
        {
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }
    }
}