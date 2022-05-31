using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using POCKETPAL.Classes;
using System.Net.Http;
using POCKETPAL.Models;
using System.Collections.Generic;

namespace POCKETPAL.Views
{
    public partial class PinCreatePage : ContentPage
    {
        int A = 0,
            B = 0,
            Pin = 0;
        string UseID, BusFunc, UseNam, UsePass;
        public PinCreatePage()
        {
            InitializeComponent();
            Pin1.AutoDismissKeyboard = false;
            UseID = GetValue("UseID");
            BusFunc = "SYSTEM-USER";
            UseNam = GetValue("UseNam");
            UsePass = GetValue("UsePass");
        }
        public void Complete(object sender, EventArgs e)
        {
            if (A <= 0)
            {
                A = Convert.ToInt32(Pin1.PINValue);
                txtPIN.Text = "Re-type PIN";
                txtPIN.TextColor = Color.FromHex("#2d6ab5");
                Pin1.PINValue = null;
            }
            else
            {
                B = Convert.ToInt32(Pin1.PINValue);
                Pin1.AutoDismissKeyboard = true;
                if (A != B)
                {
                    txtPIN.Text = "PIN didn't match";
                    txtPIN.TextColor = Color.Maroon;
                    Pin1.PINValue = "";
                    A = 0;
                    B = 0;
                }
                else
                {
                    Pin = Convert.ToInt32(Pin1.PINValue);
                    Pin1.PINValue = "";
                    A = 0;
                    B = 0;
                    txtPIN.Text = "Success !";
                    txtPIN.TextColor = Color.FromHex("#2d6ab5");
                    AddPin(Pin);
                }
            }
        }
        public async void AddPin(int Pin)
        {
            try
            {
                string _Pin = Convert.ToString(Pin);
                string url = "" + Address.Api + "/User/AddPin" + "?UserId=" + UseID + "&UserPin=" + _Pin + "&BusFunc=" + BusFunc + "&UseNam=" + UseNam;
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                List<RespondApi> model = new List<RespondApi>();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    await Navigation.PushAsync(new LoginPage(UseID, UsePass));
                    //await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert(response.StatusCode.ToString());
                }
            }
            catch (Exception msg)
            {
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
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
        public async void ThrowException(string message, string function)
        {
            var action = await DisplayAlert("Warning", message.ToString(), "Report", "Dismiss");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"ReportPage");
            }
        }
    }
}
