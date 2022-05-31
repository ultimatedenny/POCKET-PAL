using Newtonsoft.Json;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckOutPage : ContentPage
    {
        int num;
        int pnl;
        int CANCHECKIN, CHECKINTYPE;
        bool Meal;
        string UseID, UseNam, UseDep, UseEmail, UseTel, UsePin, SaveDateOfEnd, UsePlant;
        string ID, NAME, EMPLOYEENO, DEPARTMENT, PLANT, SHIMANOBADGE;
        string LOGID, TIMEIN, TIMEOUT, DATEVISIT, NEEDLUNCH, NEEDSTAY, DATEOFEND, STATUS;
        public CheckOutPage()
        {
            InitializeComponent();
            num = 1;
            UseID = GetValue("UseID");
            UseNam = GetValue("UseNam");
            UseDep = GetValue("UseDep");
            UsePlant = GetValue("UsePlant");
            UseEmail = GetValue("UseEmail");
            UseTel = GetValue("UseTel");
            UsePin = GetValue("UsePin");

            ID = GetValue("ID");
            NAME = GetValue("NAME");
            EMPLOYEENO = GetValue("EMPLOYEENO");
            DEPARTMENT = GetValue("DEPARTMENT");
            PLANT = GetValue("PLANT");
            SHIMANOBADGE = GetValue("SHIMANOBADGE");

            pnl1.IsVisible = true;
            pnl2.IsVisible = false;
            pnl3.IsVisible = false;

            btnNext.Text = "HOME";
            btnBack.IsVisible = false;

            txtGreeting.Text = "GOOD BYE !";
            txtInOut.Text = "CHECK OUT";
            txtName.Text = UseNam;
            txtTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            btnMealYes.IsEnabled = true;
            btnMealNo.IsEnabled = false;

            txtDate.Date = DateTime.Today;
            txtDate.MinimumDate = DateTime.Today;

            CANCHECKIN = Convert.ToInt32(GetValue("CANCHECKIN"));
            CHECKINTYPE = Convert.ToInt32(GetValue("CHECKINTYPE"));
        }
        private async void btnNextClick(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ScannerPage("HOME"));
            //await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            //LOGID = GetValue("LOGID");
            //TIMEOUT = DateTime.Now.ToString("HH:mm:ss");
            //string url = "" + Address.Api + "/VisitorTS/CheckOut?LogId=" + LOGID + "";
            //try
            //{
            //    List<TSID_Model> model = new List<TSID_Model>();
            //    HttpResponseMessage response = new HttpResponseMessage();
            //    HttpClient client = new HttpClient();
            //    response = await client.GetAsync(url);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        var content = await response.Content.ReadAsStringAsync();
            //        RespondApi models = JsonConvert.DeserializeObject<RespondApi>(content);
            //        if (models.Success == true)
            //        {
            //            await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            //        }
            //        else
            //        {
            //            DependencyService.Get<IMessage>().ShortAlert(models.Message);
            //        }
            //    }
            //    else
            //    {
            //        DependencyService.Get<IMessage>().ShortAlert(response.RequestMessage.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DependencyService.Get<IMessage>().ShortAlert(ex.Message.ToString());
            //}
        }
        private void btnBackClick(object sender, EventArgs e)
        {

        }
        private void btnMealYesClick(object sender, EventArgs e)
        {

        }
        private void btnMealNoClick(object sender, EventArgs e)
        {

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