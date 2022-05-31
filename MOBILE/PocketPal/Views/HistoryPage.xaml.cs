using Newtonsoft.Json;
using Plugin.FirebasePushNotification;
using POCKETPAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HistoryPage : ContentPage
    {
        bool IsVerified = false;
        public HistoryPage()
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
                    GetIsUserVerified();
                });
            });
        }
        public async void GetIsUserVerified()
        {
            string UseID = GetValue("UseID");
            string url = "" + Address.Api + "/User/GetIsUserVerified?UserId=" + UseID;
            try
            {
                startLoading();
                List<RespondApi> model = new List<RespondApi>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    RespondApi model2 = JsonConvert.DeserializeObject<RespondApi>(content);
                    IsVerified = model2.Success;
                    stopLoading();
                    GetHistory();
                }
                else
                {
                    stopLoading();
                    IsVerified = false;
                }
            }
            catch (Exception msg)
            {
                stopLoading();
                IsVerified = false;
                ThrowException(msg.Message.ToString(), "Oops, something wrong happend");
            }
        }
        public void GetHistory()
        {
            if (IsVerified == true)
            {
                string UseID  = GetValue("UseID");
                string UseNam = GetValue("UseNam");
                string ID     = GetValue("ID");
                string TOKEN  = CrossFirebasePushNotification.Current.Token.ToString();
                string path   = Address.VMS+ "/History/Index?UseId=" + UseID + "&VisitorId=" + ID;
                Title = "History - " + UseNam;
                webpage.Source = path;
            }
            else
            {
                ThrowVerify();
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
        private void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.Contains(".pdf") ||
                e.Url.Contains(".xlsx") || //SBMB-IT001-210827-001.xlsx
                e.Url.Contains(".xlsm") ||
                e.Url.Contains(".xlsb") ||
                e.Url.Contains(".xltx") ||
                e.Url.Contains(".xltm") ||
                e.Url.Contains(".xls") ||
                e.Url.Contains(".xlam") ||
                e.Url.Contains(".xlw") ||
                e.Url.Contains(".xlr") ||
                e.Url.Contains(".prn") ||
                e.Url.Contains(".txt") ||
                e.Url.Contains(".csv") ||
                e.Url.Contains(".dif") ||
                e.Url.Contains(".slk") ||
                e.Url.Contains(".dbf") ||
                e.Url.Contains(".ods") ||
                e.Url.Contains(".xps") ||
                e.Url.Contains(".wmf ") ||
                e.Url.Contains(".emf") ||
                e.Url.Contains(".bmp") ||
                e.Url.Contains(".png") ||
                e.Url.Contains(".jpg") ||
                e.Url.Contains(".jpeg") ||
                e.Url.Contains(".tiff") ||
                e.Url.Contains(".doc") ||
                e.Url.Contains(".docm") ||
                e.Url.Contains(".docx") ||
                e.Url.Contains(".dotx") ||
                e.Url.Contains(".htm") ||
                e.Url.Contains(".html") ||
                e.Url.Contains(".wps") ||
                e.Url.Contains(".rtf") ||
                e.Url.Contains(".pptx") ||
                e.Url.Contains(".ppt") ||
                e.Url.Contains(".rtf") ||
                e.Url.Contains(".rtf")

                )
            {
                var pdfUrl = new Uri(e.Url);
                Launcher.OpenAsync(pdfUrl);
                e.Cancel = true;

                popupLoadingView.IsVisible = true;
                activityIndicator.IsRunning = true;
                Task.Delay(500);
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
            }
            else
            {
                popupLoadingView.IsVisible = true;
                activityIndicator.IsRunning = true;
            }
        }
        private void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }
        public async void ThrowException(string message, string function)
        {
            var action = await DisplayAlert("Warning", message.ToString(), "Report", "Dismiss");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"ReportPage");
            }
        }
        public async void ThrowVerify()
        {
            var action = await DisplayAlert("Account Unverified", "To access this feature please verify your account on SETTING > VERIFY ACCOUNT", "SETTING", "DISMISS");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"SettingPage");
            }
        }
    }
}