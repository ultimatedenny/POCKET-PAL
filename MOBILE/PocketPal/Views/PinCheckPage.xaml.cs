using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using POCKETPAL.Views;
using POCKETPAL;
using System.Net.Http;
using Newtonsoft.Json;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PinCheckPage : ContentPage
    {

        string UseID, UseNam, UseDep, UsePlant, UseEmail, UseTel, UsePin;
        string LOGID, TIMEIN, TIMEOUT, DATEVISIT, NEEDLUNCH, NEEDSTAY, DATEOFEND, STATUS;
        string ID, NAME, EMPLOYEENO, DEPARTMENT, PLANT, SHIMANOBADGE;
        int CANCHECKIN;
        int CHECKINTYPE;
        public PinCheckPage()
        {
            InitializeComponent();

            UseID = GetValue("UseID");
            UseNam = GetValue("UseNam");
            UseDep = GetValue("UseDep");
            UsePlant = GetValue("UsePlant");
            UseEmail = GetValue("UseEmail");
            UseTel = GetValue("UseTel");
            UsePin = GetValue("UsePin");

            LOGID = GetValue("LOGID");
            TIMEIN = GetValue("TIMEIN");
            TIMEOUT = GetValue("TIMEOUT");
            DATEVISIT = GetValue("DATEVISIT");
            NEEDSTAY = GetValue("NEEDSTAY");
            DATEOFEND = GetValue("DATEOFEND");
            STATUS = GetValue("STATUS");

            ID = GetValue("ID");
            NAME = GetValue("NAME");
            EMPLOYEENO = GetValue("EMPLOYEENO");
            DEPARTMENT = GetValue("DEPARTMENT");
            PLANT = GetValue("PLANT");
            SHIMANOBADGE = GetValue("SHIMANOBADGE");

            CANCHECKIN = Convert.ToInt32(GetValue("CANCHECKIN"));
            CHECKINTYPE = Convert.ToInt32(GetValue("CHECKINTYPE"));
        }
        public async void Complete(object sender, EventArgs e)
        {
            int A = Convert.ToInt32(Pin1.PINValue);
            if (A != Convert.ToInt32(UsePin))
            {
                txtPIN.Text = "Please try again";
                Pin1.PINValue = null;
                A = 0;
            }
            else
            {
                if (CHECKINTYPE == 1)
                {
                    RegisterBadge();
                }
                else if (CHECKINTYPE == 4)
                {
                    CheckOut();
                }
                else if (CHECKINTYPE == 2)
                {
                    CheckInSameDay();
                }
                else if (CHECKINTYPE == 3)
                {
                    
                    await Shell.Current.GoToAsync($"{nameof(CheckInNextDayPage)}");
                }
                else if (CHECKINTYPE == 5 || CHECKINTYPE == 6)
                {
                    await Shell.Current.GoToAsync($"{nameof(CheckInNewPage)}");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Unknown CHECKINTYPE");
                }
            }
        }
        public async void RegisterBadge()
        {
            try
            {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                var stringChars = new char[8];
                var random = new System.Random();
                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }
                var finalString = new String(stringChars);
                string ShimanoBadge = "VMS-BADGE-" + finalString;
                string url1 = "?Name=" + UseNam + "" + "&EmployeeNo=" + UseID + "" + "&Department=" + UseDep + "" + "&ShimanoBadge=" + ShimanoBadge + "";
                string url2 = "&Plant=" + UsePlant + "" + "&Email=" + UseEmail + "" + "&Phone=" + UseTel + "" + "&Photoes=-";
                string url = "" + Address.Api + "/VisitorTS/PP_ADD_TEAM_SHIMANO_VMS" + url1 + url2;
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RespondApi model = JsonConvert.DeserializeObject<RespondApi>(content);
                    if(model.Success == true)
                    {
                        await Shell.Current.GoToAsync($"CheckInNewPage");
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert(model.Message);
                    }
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert((response.ToString()).Substring(0, 19));
                }
            }
            catch (Exception)
            {
                DependencyService.Get<IMessage>().ShortAlert("Oops, something wrong when Register Badge, please contact Administrator");
            }
        }
        public async void CheckOut()
        {
            try
            {
                string url = "" + Address.Api + "/VisitorTS/CheckOut?LogId=" + LOGID + "";
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                List<RespondApi> model = new List<RespondApi>();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    await Shell.Current.GoToAsync($"CheckOutPage");
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert((response.ToString()).Substring(0, 19));
                }
            }
            catch (Exception)
            {
                DependencyService.Get<IMessage>().ShortAlert("Oops, something wrong when Check Out, please contact Administrator");
            }
        }
        public async void CheckInSameDay()
        {
            try
            {
                string TSVisitorId = ID;
                string TimeIn = DateTime.Now.ToString("HH:mm:ss");
                string DateVisit = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string Plant = UsePlant;
                string DateOfEnd = DATEOFEND;
                string HostName = GetValue("UseID");
                string HostDept = GetValue("UseDep");
                bool NEEDLUNCH = Convert.ToBoolean(GetValue("NEEDLUNCH"));
                bool NEEDSTAY = Convert.ToBoolean(GetValue("NEEDSTAY"));
                string url = "" + Address.Api + "/VisitorTS/CheckInSameDay?TSVisitorId=" + TSVisitorId + "&HostName=" + HostName + "&HostDepartment=" + HostDept + "&TimeIn=" + TimeIn + "&DateVisit=" + DateVisit + "&NeedLunch=" + NEEDLUNCH + "&Status=CHECKIN&Plant=" + Plant + "&NeedStay=" + NEEDSTAY + "&DateOfEnd=" + DateOfEnd + "";
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                List<RespondApi> model = new List<RespondApi>();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    await Navigation.PushAsync(new ScannerPage("HOME"));
                   // await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
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