using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using POCKETPAL.Views;

namespace POCKETPAL.Views
{
    public partial class IntroPage : ContentPage
    {
        string UseID, UseNam;
        public IntroPage()
        {
            InitializeComponent();
            UseID = GetValue("UseID");
            UseNam = GetValue("UseNam");
            txtName.Text = "Welcome, " + UseNam;
        }
        public void AddValue(string key, string value)
        {
            Preferences.Set(key, value);
        }
        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }
        public async Task LoginAsync(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
