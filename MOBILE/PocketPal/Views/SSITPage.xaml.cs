using Newtonsoft.Json;
using POCKETPAL.Classes;
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
	public partial class SSITPage : ContentPage
	{
        string SSITUserGroup, SSITApprCat, SSITUserID;
        bool IsVerified = true;
        string ssitMenu = "";
        public SSITPage ()
		{
			InitializeComponent ();
		}

        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread( () =>
                {
                    Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged; ;
                    CountDataSSIT();
                });
            });
        }

        public async void CountDataSSIT()
        {
            startLoading();

            SSITUserGroup = GetValue("SSITUserGroup");
            SSITApprCat = GetValue("SSITApprCat");
            SSITUserID = GetValue("SSITUserID");
            string url = "" + Address.Api + "/SSIT/GetCountDataSSIT_BY_USERGROUP?UserId=" + SSITUserID + "&UserGroup=" + SSITUserGroup + "&ApprCat=" + SSITApprCat + "&Product=&SSITCode=";
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RootSSITCountModel model2 = JsonConvert.DeserializeObject<RootSSITCountModel>(content);
                    badgeScrap.BadgeText = model2.Data.CountScrap.ToString();
                    badgePR.BadgeText = model2.Data.CountPR.ToString();

                    if (model2.Data.CountScrap > 0)
                    {
                        badgeScrap.BackgroundColor = Color.Red;
                        badgeScrap.BadgeTextColor = Color.White;
                    }
                    else
                    {
                        badgeScrap.BackgroundColor = Color.Transparent;
                        badgeScrap.BadgeTextColor = Color.Transparent;
                    }

                    if (model2.Data.CountPR > 0)
                    {
                        badgePR.BackgroundColor = Color.Red;
                        badgePR.BadgeTextColor = Color.White;
                    }
                    else
                    {
                        badgePR.BackgroundColor = Color.Transparent;
                        badgePR.BadgeTextColor = Color.Transparent;
                    }
                    stopLoading();
                }
                else
                {
                    stopLoading();
                    await DisplayAlert("Error", response.RequestMessage.ToString(), "Close");
                }
            }
            catch (Exception msg)
            {
                stopLoading();
                await DisplayAlert("Error", msg.Message.ToString(), "Close");
            }
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess.ToString();
            var profiles = e.ConnectionProfiles.ToString();
            if (access != "Internet")
            {
                string msg = "You don't have connection";
                DependencyService.Get<IMessage>().ShortAlert(msg);
            }
        }

        public void btnSSITScrap(object sender, EventArgs e)
        {
            ssitMenu = "Scrap";
            GoQRCodePage(ssitMenu);
        }

        public void btnSSITPR(object sender, EventArgs e)
        {
            ssitMenu = "Purchase Return";
            GoQRCodePage(ssitMenu);
        }

        public async void GoQRCodePage(string ssitMenu)
        {
            if (IsVerified == true)
            {
                bool allowed = false;
                allowed = await GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();
                if (allowed)
                {
                    //await Shell.Current.GoToAsync($"QRCodeSSITPage");
                    //await Shell.Current.GoToAsync($"QRCodeSSITPage?ssitMenu={ssitMenu}");
                    await Navigation.PushAsync(new QRCodeSSITPage(ssitMenu));
                }
                else
                {
                    await DisplayAlert("Alert", "You have to provide Camera permission", "Ok");
                }
            }
            else
            {
                ThrowVerify();
            }
        }

        public async void ThrowVerify()
        {
            var action = await DisplayAlert("Account Unverified", "To access this feature please verify your account on SETTING > VERIFY ACCOUNT", "SETTING", "DISMISS");
            if (action == true)
            {
                await Navigation.PushAsync(new SettingPage());
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
    }
}