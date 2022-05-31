using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using POCKETPAL.Classes;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinChangePage : ContentPage
    {
        int A = 0, B = 0, Pin = 0, Auth = 0;
        string UseID, UseNam, UseDep, UsePlant, UseEmail, UseTel, UsePin;
        public PinChangePage()
        {
            InitializeComponent();
            UseID = GetValue("UseID");
            UseNam = GetValue("UseNam");
            UseDep = GetValue("UseDep");
            UsePlant = GetValue("UsePlant");
            UseEmail = GetValue("UseEmail");
            UseTel = GetValue("UseTel");
            UsePin = GetValue("UsePin");
        }
        public async void Complete(object sender, EventArgs e)
        {
            if (Auth == 0)
            {
                if (Pin1.PINValue != UsePin)
                {
                    txtPIN.Text = "Please try again";
                    Pin1.PINValue = "";
                    Auth = 0;
                }
                else
                {
                    txtPIN.Text = "Please enter your new PIN";
                    Pin1.PINValue = "";
                    Auth = 1;
                }
            }
            else
            {
                if (A == 0)
                {
                    A = Convert.ToInt32(Pin1.PINValue);
                    txtPIN.Text = "Please re-enter your new PIN";
                    Pin1.PINValue = "";
                }
                else
                {
                    B = Convert.ToInt32(Pin1.PINValue);
                    if (A != B)
                    {
                        txtPIN.Text = "PIN didn't match, please try again.";
                        Pin1.PINValue = "";
                        A = 0;
                        B = 0;
                        Auth = 0;
                    }
                    else
                    {
                        txtPIN.Text = "Success";
                        Pin = Convert.ToInt32(Pin1.PINValue);
                        Pin1.PINValue = "";
                        A = 0;
                        B = 0;
                        Auth = 0;
                        await EditPin();
                        AddValue("UsePin", Convert.ToString(Pin));
                        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                    }
                }
            }
        }
        private async Task<JsonWriter> EditPin()
        {
            string url = "" + Address.Api + "/User/AddPin";
            try
            {
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "POST";

                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(new { UseID = UseID, UsePin = Pin });
                    streamWriter.Write(json);
                }
                using (WebResponse response = await request.GetResponseAsync())
                {
                    using (Stream stream = response.GetResponseStream())
                    {

                    }
                }
                return null;
            }
            catch (Exception)
            {
                DependencyService.Get<IMessage>().ShortAlert("Oops something wrong");
            }
            return null;
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