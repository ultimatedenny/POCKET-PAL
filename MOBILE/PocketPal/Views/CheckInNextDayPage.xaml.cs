using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using System.Collections.Generic;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckInNextDayPage : ContentPage
    {
        int num;
        int pnl;
        int CANCHECKIN, CHECKINTYPE;
        bool Meal, needstay;
        string UseID, UseNam, UseDep, UseEmail, UseTel, UsePin, SaveDateOfEnd, UsePlant;
        string ID, NAME, EMPLOYEENO, DEPARTMENT, PLANT, SHIMANOBADGE;
        string LOGID, TIMEIN, TIMEOUT, DATEVISIT, NEEDLUNCH, NEEDSTAY, DATEOFEND, STATUS;
        public CheckInNextDayPage()
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

            LOGID = GetValue("LOGID");
            TIMEIN = GetValue("TIMEIN");
            TIMEOUT = GetValue("TIMEOUT");
            DATEVISIT = GetValue("DATEVISIT");
            NEEDLUNCH = GetValue("NEEDLUNCH");
            NEEDSTAY = GetValue("NEEDSTAY");
            DATEOFEND = GetValue("DATEOFEND");
            STATUS = GetValue("STATUS");

            btnBack.IsEnabled = false;
            pnl1.IsVisible = true;
            pnl2.IsVisible = false;
            pnl3.IsVisible = false;
            btnNext.Text = "NEXT";
            btnMealYes.IsEnabled = true;
            btnMealNo.IsEnabled = false;

            txtGreeting.Text = "WELCOME BACK !";
            txtName.Text = UseNam;
            txtTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            txtDate.Date = DateTime.Today;
            txtDate.MinimumDate = DateTime.Today;

            CANCHECKIN = Convert.ToInt32(GetValue("CANCHECKIN"));
            CHECKINTYPE = Convert.ToInt32(GetValue("CHECKINTYPE"));

            if (DATEOFEND != null || DATEOFEND != "" || DATEOFEND == "0")
            {
                needstay = true;
            }
            else
            {
                needstay = false;
            }
        }
        private void btnNextClick(object sender, EventArgs e)
        {
            if (pnl <= 1)
            {
                pnl = num + 1;
                if (pnl == 2)
                {
                    two();
                }
            }
            else if (pnl == 2)
            {
                execute();
            }
        }
        private void btnBackClick(object sender, EventArgs e)
        {
            pnl = pnl - 1;
            if (pnl == 1)
            {
                one();
            }
            else if (pnl == 2)
            {
                two();
            }
            else if (pnl <= 1)
            {
                pnl = 1;
                one();
            }
        }
        private void btnMealYesClick(object sender, EventArgs e)
        {
            Meal = true;
            btnMealYes.IsEnabled = false;
            btnMealNo.IsEnabled = true;
        }
        private void btnMealNoClick(object sender, EventArgs e)
        {
            Meal = false;
            btnMealYes.IsEnabled = true;
            btnMealNo.IsEnabled = false;
        }
        void one()
        {
            pnl1.IsVisible = true;
            pnl2.IsVisible = false;
            pnl3.IsVisible = false;
            btnNext.Text = "NEXT";
            btnBack.Text = "HOME";
            btnNext.IsEnabled = true;
            btnBack.IsEnabled = false;
        }
        void two()
        {
            pnl1.IsVisible = false;
            pnl2.IsVisible = true;
            pnl3.IsVisible = false;
            btnNext.Text = "CHECK IN NOW";
            btnBack.Text = "BACK";
            btnNext.IsEnabled = true;
            btnBack.IsEnabled = true;
        }
        async void execute()
        {
            var action = await DisplayAlert("Check In", "Are you going to check in for this session ?", "Yes", "No");
            if (action)
            {
                try
                {
                    //string url1;
                    //string url;
                    //string TSVisitorId = "";
                    //string TimeIn = "";
                    //string DateVisit = "";
                    //string NeedLunch = "";
                    //string Plant = "";
                    //string DateOfEnd = "";

                    //HttpResponseMessage response = new HttpResponseMessage();
                    //HttpClient client = new HttpClient();

                    //TSVisitorId = ID;
                    //TimeIn = DateTime.Now.ToString("HH:mm");
                    //DateVisit = DateTime.Now.ToString("yyyy-MM-dd");
                    //if (Meal == true)
                    //{
                    //    NeedLunch = "true";
                    //}
                    //else
                    //{
                    //    NeedLunch = "false";
                    //}
                    //Plant = UsePlant;
                    //DateOfEnd = DateTime.Now.ToString("yyyy-MM-dd");
                    //url1 = "?TSVisitorId=" + TSVisitorId + "" + "&TimeIn=" + TimeIn + "" + "&DateVisit=" + DateVisit + "" + "&NeedLunch=" + NeedLunch + "" + "&Plant=" + Plant + "" + "&NeedStay=" + needstay + "" + "&DateOfEnd=" + DATEOFEND + "";
                    //url = "" + Address.Api + "/VisitorTS/CheckIn" + url1;
                    //response = await client.GetAsync(url);
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    await Task.Delay(500);
                    //    if (Device.RuntimePlatform == Device.Android)
                    //    {
                    //        App.Current.MainPage = new Xamarin.Forms.NavigationPage(new HomePage());
                    //    }
                    //    else if (Device.RuntimePlatform == Device.iOS)
                    //    {
                    //        await Navigation.PushAsync(new HomePage());
                    //    }
                    //}
                    //else
                    //{
                    //    DependencyService.Get<IMessage>().ShortAlert((response.ToString()).Substring(0, 19));
                    //}
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    string URL = string.Empty;
                    int TDIS = Convert.ToInt32(ID);
                    string HOST = GetValue("UseID");
                    string HOSTDEP = GetValue("UseDep");
                    string DOS = GetValue("DATEVISIT");
                    string PLANT = UsePlant;
                    string DOE = GetValue("DATEOFEND");
                    bool LUNCH = Meal;
                    bool STAY = false;
                    List<RespondApi> model = new List<RespondApi>();
                    string BASE = "" + Address.Api + "/VisitorTS/PP_CHECKINTS_VMS";
                    if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) == Convert.ToDateTime(DOE))
                    {
                        STAY = false;
                        URL = BASE + "?TSVISITORID=" + TDIS + "&HostName=" + HOST + "&HostDepartment=" + HOSTDEP + "&NeedLunch=" + LUNCH + "&Plant=" + PLANT + "&NeedStay=" + STAY + "&DateOfEnd=" + DOE;
                        response = await client.GetAsync(URL);
                        if (response.IsSuccessStatusCode)
                        {
                            //await Shell.Current.GoToAsync(new HomePage());
                            //await Shell.Current.GoToAsync($"{nameof(HomePage)}"); 
                            await Navigation.PushAsync(new ScannerPage("HOME"));
                        }
                        else
                        { 
                            DependencyService.Get<IMessage>().ShortAlert(response.RequestMessage.ToString()); 
                        }
                    }
                    else if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) < Convert.ToDateTime(DOE))
                    {
                        STAY = true;
                        URL = BASE + "?TSVISITORID=" + TDIS + "&HostName=" + HOST + "&HostDepartment=" + HOSTDEP + "&NeedLunch=" + LUNCH + "&Plant=" + PLANT + "&NeedStay=" + STAY + "&DateOfEnd=" + DOE;
                        response = await client.GetAsync(URL);
                        if (response.IsSuccessStatusCode)
                        {
                            //await Shell.Current.GoToAsync($"{nameof(HomePage)}");
                            await Navigation.PushAsync(new ScannerPage("HOME"));
                        }
                        else 
                        { 
                            DependencyService.Get<IMessage>().ShortAlert(response.RequestMessage.ToString()); 
                        }

                    }
                    else if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) > Convert.ToDateTime(DOE))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Oops, it seem you put yesterday date");
                    }
                }
                catch (Exception msg)
                {
                    DependencyService.Get<IMessage>().ShortAlert(msg.ToString());
                }
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
    }
}