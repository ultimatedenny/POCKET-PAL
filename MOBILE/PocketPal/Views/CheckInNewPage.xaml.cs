using Newtonsoft.Json;
using POCKETPAL.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using POCKETPAL.Classes;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckInNewPage : ContentPage
    {
        int num;
        int pnl;
        int CANCHECKIN, CHECKINTYPE;
        bool Meal;
        string UseID, UseNam, UseDep, UseEmail, UseTel, UsePin, SaveDateOfEnd, UsePlant;
        string ID, NAME, EMPLOYEENO, DEPARTMENT, PLANT, SHIMANOBADGE;
        string LOGID, TIMEIN, TIMEOUT, DATEVISIT, NEEDLUNCH, NEEDSTAY, DATEOFEND, STATUS;
        public CheckInNewPage()
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

            //TeamShimanoDetails
            ID = GetValue("ID");
            NAME = GetValue("NAME");
            EMPLOYEENO = GetValue("EMPLOYEENO");
            DEPARTMENT = GetValue("DEPARTMENT");
            PLANT = GetValue("PLANT");
            SHIMANOBADGE = GetValue("SHIMANOBADGE");

            btnBack.IsEnabled = false;
            pnl1.IsVisible = true;
            pnl2.IsVisible = false;
            pnl3.IsVisible = false;
            btnNext.Text = "NEXT";
            btnMealYes.IsEnabled = true;
            btnMealNo.IsEnabled = false;

            txtName.Text = UseNam;
            txtTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            txtDate.Date = DateTime.Today;
            txtDate.MinimumDate = DateTime.Today;

            CANCHECKIN = Convert.ToInt32(GetValue("CANCHECKIN"));
            CHECKINTYPE = Convert.ToInt32(GetValue("CHECKINTYPE"));

            btnNext.IsEnabled = false;
            PP_GET_TS_ID_VMS();
        }
        private void btnNextClick(object sender, EventArgs e)
        {
            if (pnl <= 1)
            {
                pnl = num + 1;
                if (pnl == 1)
                {
                    one();
                }
                else if (pnl == 2)
                {
                    two();
                }
                else if (pnl == 3)
                {
                    three();
                }
            }
            else if (pnl < 3)
            {
                pnl = pnl + 1;
                if (pnl == 1)
                {
                    one();
                }
                else if (pnl == 2)
                {
                    two();
                }
                else if (pnl == 3)
                {
                    three();
                }
                else if (pnl > 3)
                {
                    execute();
                }
            }
            else if (pnl >= 3)
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
            else if (pnl == 3)
            {
                three();
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
            btnNext.Text = "NEXT";
            btnBack.Text = "BACK";
            btnNext.IsEnabled = true;
            btnBack.IsEnabled = true;
        }
        void three()
        {
            pnl1.IsVisible = false;
            pnl2.IsVisible = false;
            pnl3.IsVisible = true;

            btnNext.IsEnabled = true;
            btnNext.Text = "CHECK IN NOW";
            btnBack.Text = "BACK";
            btnBack.IsEnabled = true;
        }
        public async void execute()
        {
            var action = await DisplayAlert("Check In", "Are you going to check in for this session ?", "Yes", "No");
            if (action)
            {
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    string URL = string.Empty;
                    int TDIS = Convert.ToInt32(ID);
                    string HOST = GetValue("UseID");
                    string HOSTDEP = GetValue("UseDep");
                    string DOS = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    string PLANT = UsePlant;
                    string DOE = txtDate.Date.ToString("yyyy-MM-dd");
                    bool LUNCH = Meal;
                    bool STAY = false;
                    List<RespondApi> model = new List<RespondApi>();
                    string BASE = "" + Address.Api + "/VisitorTS/PP_CHECKINTS_VMS";
                    if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) == Convert.ToDateTime(txtDate.Date.ToString("yyyy-MM-dd")))
                    {
                        STAY = false;
                        URL = BASE + "?TSVISITORID=" + TDIS + "&HostName=" + HOST + "&HostDepartment=" + HOSTDEP + "&NeedLunch=" + LUNCH + "&Plant=" + PLANT + "&NeedStay=" + STAY + "&DateOfEnd=" + DOE;
                        response = await client.GetAsync(URL);
                        if (response.IsSuccessStatusCode)
                        {
                            /////////////////////
                            string NAME = GetValue("UseNam");
                            string USEID = GetValue("UseID");
                            string QR = ScannerPage.public_QR.ToString();
                            string urls = "" + Address.Api + "/VisitorTS/QRCheck?Client=" + QR + "&UseID=" + USEID + "&UseNam=" + NAME + "";
                            try
                            {
                                HttpResponseMessage responses = new HttpResponseMessage();
                                List<RespondApi> models = new List<RespondApi>();
                                HttpClient clients = new HttpClient();
                                responses = await client.GetAsync(urls);
                                if (responses.IsSuccessStatusCode)
                                {
                                    models.Clear();
                                    var contents = await responses.Content.ReadAsStringAsync();
                                    models = JsonConvert.DeserializeObject<List<RespondApi>>(contents);
                                    if (models[0].Success == true)
                                    {
                                        await Navigation.PushAsync(new ScannerPage("HOME"));
                                    }
                                    else
                                    {
                                        DependencyService.Get<IMessage>().ShortAlert(model[0].Message.ToString());
                                    }
                                }
                                else
                                {
                                    DependencyService.Get<IMessage>().ShortAlert(response.IsSuccessStatusCode.ToString());
                                }
                            }
                            catch (Exception ex)
                            {
                                DependencyService.Get<IMessage>().ShortAlert(ex.Message.ToString());
                            }
                            /////////////////////
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().ShortAlert(response.RequestMessage.ToString());
                        }
                    }
                    else if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) < Convert.ToDateTime(txtDate.Date.ToString("yyyy-MM-dd")))
                    {
                        STAY = true;
                        URL = BASE + "?TSVISITORID=" + TDIS + "&HostName=" + HOST + "&HostDepartment=" + HOSTDEP + "&NeedLunch=" + LUNCH + "&Plant=" + PLANT + "&NeedStay=" + STAY + "&DateOfEnd=" + DOE;
                        response = await client.GetAsync(URL);
                        if (response.IsSuccessStatusCode)
                        {
                            //await Shell.Current.Navigation.PushAsync(new HomePage());
                            //await Shell.Current.GoToAsync($"/{nameof(HomePage)}"); 
                            await Navigation.PushAsync(new ScannerPage("HOME"));
                        }
                        else
                        {
                            DependencyService.Get<IMessage>().ShortAlert(response.RequestMessage.ToString());
                        }

                    }
                    else if (Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd")) > Convert.ToDateTime(txtDate.Date.ToString("yyyy-MM-dd")))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Oops, it seem you put yesterday date");
                    }
                }
                catch (Exception msg)
                {
                    DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                }
            }
        }
        public async void PP_GET_TS_ID_VMS()
        {
            string url = "" + Address.Api + "/VisitorTS/PP_GET_TS_ID_VMS?UseNam=" + UseNam;
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Root_TSID_Model datas = JsonConvert.DeserializeObject<Root_TSID_Model>(content);
                    AddValue("ID", (datas.Data.ID.ToString() == null) ? "" : datas.Data.ID.ToString());
                    AddValue("NAME", (datas.Data.NAME.ToString() == null) ? "" : datas.Data.NAME.ToString());
                    AddValue("EMPLOYEENO", (datas.Data.EMPLOYEENO.ToString() == null) ? "" : datas.Data.EMPLOYEENO.ToString());
                    AddValue("PLANT", (datas.Data.PLANT.ToString() == null) ? "" : datas.Data.PLANT.ToString());
                    AddValue("DEPARTMENT", (datas.Data.DEPARTMENT.ToString() == null) ? "" : datas.Data.DEPARTMENT.ToString());
                    AddValue("SHIMANOBADGE", (datas.Data.SHIMANOBADGE.ToString() == null) ? "" : datas.Data.SHIMANOBADGE.ToString());

                    ID = GetValue("ID");
                    NAME = GetValue("NAME");
                    EMPLOYEENO = GetValue("EMPLOYEENO");
                    DEPARTMENT = GetValue("DEPARTMENT");
                    PLANT = GetValue("PLANT");
                    SHIMANOBADGE = GetValue("SHIMANOBADGE");

                    btnNext.IsEnabled = true;
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert(response.RequestMessage.ToString());
                }
            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().ShortAlert(ex.Message.ToString());
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