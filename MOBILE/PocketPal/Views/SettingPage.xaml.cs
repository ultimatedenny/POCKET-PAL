using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.XF.TouchID;
using Xamarin.Essentials;
using System.Collections.Generic;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Plugin.FirebasePushNotification;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        string KeepScreenOn, DeviceSecurity;
        string Name, Email, UserId;

        public SettingPage()
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
                    var navigationPage = Application.Current.MainPage as NavigationPage;
                    if (null != navigationPage)
                    {
                        navigationPage.BarTextColor = Color.FromHex("#000000");
                        navigationPage.BarBackgroundColor = Color.FromHex("#FFFFFF");
                    }
                    Title = "Setting";
                    DeviceSecurity = GetValue("DEVICESEC");
                    KeepScreenOn = GetValue("SCREENON");

                    Name = GetValue("UseNam");
                    Email = GetValue("UseEmail");
                    UserId = GetValue("Username");

                    txtUseNam.Text = GetValue("UseNam");
                    txtUseEmail.Text = GetValue("UseEmail");
                    txtStatus.Text = GetValue("BusFunc");

                    CheckingDeviceSecurity();
                    CheckScreenOn();
                    CheckVerification();
                });
            });
        }
        public void RefreshPage(object sender, EventArgs e)
        {
            CheckingDeviceSecurity();
            CheckScreenOn();
            CheckVerification();
            
        }
        public void CheckingDeviceSecurity()
        {
            if (TouchID.IsDeviceSecured())
            {
                if (string.IsNullOrEmpty(DeviceSecurity) || DeviceSecurity == "0")
                {
                    DeviceSecSwitch.IsToggled = false;
                }
                else
                {
                    DeviceSecSwitch.IsToggled = true;
                }
            }
            else
            {
                DeviceSecurity = "0";
                AddValue("DEVICESEC", DeviceSecurity);
                DeviceSecSwitch.IsEnabled = false;
            }
        }
        public async void OnToggled2(object sender, ToggledEventArgs e)
        {
            if (TouchID.IsDeviceSecured() == true)
            {
                if (DeviceSecSwitch.IsToggled == false)
                {
                    DeviceSecurity = "0";
                    AddValue("DEVICESEC", DeviceSecurity);
                }
                else
                {
                    DeviceSecurity = "1";
                    AddValue("DEVICESEC", DeviceSecurity);
                }
            }
            else
            {
                var action = await DisplayAlert("alert", "unable to proceed, please add your device security such as password, pin, or pattern.", "phone setting", "dismiss");
                if (action == true)
                {
                    DeviceSecSwitch.IsToggled = false;
                    TouchID.PromptSecuritySettings();
                }
                else
                {
                    DeviceSecSwitch.IsToggled = false;
                }
                DeviceSecurity = "0";
                AddValue("DEVICESEC", DeviceSecurity);
            }
        }
        public void CheckScreenOn()
        {
            if (DeviceDisplay.KeepScreenOn == true)
            {
                if (string.IsNullOrEmpty(KeepScreenOn) || KeepScreenOn == "0")
                {
                    KeepScreenSwitch.IsToggled = false;
                    AddValue("SCREENON", "0");
                }
                else
                {
                    KeepScreenSwitch.IsToggled = true;
                    AddValue("SCREENON", "1");
                }
            }
            else
            {
                KeepScreenSwitch.IsToggled = false;
                AddValue("SCREENON", "0");
            }
        }
        public void OnToggled1(object sender, ToggledEventArgs e)
        {
            if (KeepScreenSwitch.IsToggled == true)
            {
                DeviceDisplay.KeepScreenOn = true;
                AddValue("SCREENON", "1");
            }
            else
            {
                DeviceDisplay.KeepScreenOn = false;
                AddValue("SCREENON", "0");
            }
        }
        public async void OpenChangePIN(object sender, EventArgs e)
        {
            if (DeviceSecurity == "1")
            {
                await DisplayAlert("Alert", "You are unable to change PocketPal PIN when using Device Security", "Dismiss");
            }
            else
            {
                await Shell.Current.GoToAsync($"PinChangePage");
            }
        }
        public void OpenTerms(object sender, EventArgs e)
        {
            string path = "https://raw.githubusercontent.com/PT-SHIMANOBATAM/TCPP/main/Pocket%20Pal%20-%20Terms%20%26%20Conditions.md";
            string title = "Terms & Conditions";
            AddValue("TempPath", path);
            AddValue("TempTitle", title);
            Shell.Current.GoToAsync($"WebviewPage");
        }
        public void OpenPrivacy(object sender, EventArgs e)
        {
            string path = "https://raw.githubusercontent.com/PT-SHIMANOBATAM/TCPP/main/Pocket%20Pal%20-%20Privacy%20Policy.md";
            string title = "Privacy Policy";
            AddValue("TempPath", path);
            AddValue("TempTitle", title);
            Shell.Current.GoToAsync($"WebviewPage");
        }
        public void OpenChangeLog(object sender, EventArgs e)
        {
            string path = "https://raw.githubusercontent.com/PT-SHIMANOBATAM/TCPP/main/Pocket%20Pal%20-%20Change%20Log.txt";
            string title = "Change Log";
            AddValue("TempPath", path);
            AddValue("TempTitle", title);
            Shell.Current.GoToAsync($"WebviewPage");
        }
        public async void OpenAbout(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"AboutPage");
        }
        public async void OpenBugReport(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"ReportPage");
        }
        public async void OpenVerification(object sender, EventArgs e)
        {
            string EmployeeBadge = await DisplayPromptAsync("PocketPal Verivifaction", "Please Enter Your Employee Badge Number", maxLength: 10, keyboard: Keyboard.Text);
            if (!string.IsNullOrWhiteSpace(EmployeeBadge))
            {

                DoVerification(EmployeeBadge);
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Please Enter Your Employee Badge Number");
            }
        }
        public async void CheckVerification()
        {
            RefTrue();
            string url = "" + Address.Api + "/User/GetIsUserVerified?" + "Userid=" + UserId + "";
            try
            {
                List<RespondApi> model = new List<RespondApi>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    RespondApi model2 = JsonConvert.DeserializeObject<RespondApi>(content);

                    if (model2.Success == true)
                    {
                        VerifyImage.Source = "https://img.icons8.com/fluent/512/000000/checkmark.png";
                        RefFalse();
                    }
                    else
                    {
                        VerifyImage.Source = "https://img.icons8.com/fluent/512/000000/delete-sign.png";
                        RefFalse();
                    }
                }
                else
                {
                    RefFalse();
                }
            }
            catch (Exception msg)
            {
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                RefFalse();
            }
        }
        public async void DoVerification(string EmployeeBadge)
        {
            RefTrue();
            string url = "" + Address.Api + "/User/RequestVerifyUser?" + "Userid=" + UserId + "" + "&UseNam=" + Name + "" + "&EmployeeBadge=" + EmployeeBadge + "";
            try
            {
                List<RespondApi> model = new List<RespondApi>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    RespondApi model2 = JsonConvert.DeserializeObject<RespondApi>(content);

                    RefFalse();
                    DependencyService.Get<IMessage>().LongAlert(model2.Message.ToString());
                }
                else
                {
                    RefFalse();
                }
            }
            catch (Exception msg)
            {
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                RefFalse();
            }
        }
        public void OpenChangePassword(object sender, EventArgs e)
        {
            DependencyService.Get<IMessage>().LongAlert("Menu unavailable for this moment");
        }
        public void RefFalse()
        {
            RefPage.IsRefreshing = false;
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }
        public async void SignOut(object sender, EventArgs e)
        {
            var action = await DisplayAlert("", "Do you really want to sign out ?", "Yes", "No");
            if (action)
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
                        Preferences.Clear();
                        AddValue("TOKENNEW", CURRENTTOKEN);
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().LongAlert(response.StatusCode.ToString());
                    }
                }
                catch (Exception msg)
                {
                    DependencyService.Get<IMessage>().LongAlert(msg.Message.ToString());
                }
            }
        }
        public async void DisableAccount(object sender, EventArgs e)
        {
            string EmployeeBadge = await DisplayPromptAsync("Disabled", "Please Enter Your Employee Badge Number", maxLength: 10, keyboard: Keyboard.Text);
            if (!string.IsNullOrWhiteSpace(EmployeeBadge))
            {

                DoVerification(EmployeeBadge);
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Please Enter Your Employee Badge Number");
            }
        }
        public void DeleteAccount(object sender, EventArgs e)
        {

        }
        public void RefTrue()
        {
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
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