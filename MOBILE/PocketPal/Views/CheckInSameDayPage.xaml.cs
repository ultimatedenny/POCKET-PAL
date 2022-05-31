using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckInSameDayPage : ContentPage
    {
        int num;
        int pnl;
        int CANCHECKIN, CHECKINTYPE;
        bool Meal;
        string UseID, UseNam, UseDep, UseEmail, UseTel, UsePin, SaveDateOfEnd, UsePlant;
        string ID, NAME, EMPLOYEENO, DEPARTMENT, PLANT, SHIMANOBADGE;
        string LOGID, TIMEIN, TIMEOUT, DATEVISIT, NEEDLUNCH, NEEDSTAY, DATEOFEND, STATUS;
        public CheckInSameDayPage()
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

            txtName.Text = UseNam;
            txtTime.Text = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            btnMealYes.IsEnabled = true;
            btnMealNo.IsEnabled = false;

            txtDate.Date = DateTime.Today;
            txtDate.MinimumDate = DateTime.Today;

            CANCHECKIN = Convert.ToInt32(GetValue("CANCHECKIN"));
            CHECKINTYPE = Convert.ToInt32(GetValue("CHECKINTYPE"));
        }
        private void btnNextClick(object sender, EventArgs e)
        {
            //App.Current.MainPage = new NavigationPage(new HomePage());
            Navigation.PushAsync(new ScannerPage("HOME"));
            //Task.Delay(500);
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    App.Current.MainPage = new Xamarin.Forms.NavigationPage(new HomePage());
            //}
            //else if (Device.RuntimePlatform == Device.iOS)
            //{
            //    Navigation.PushAsync(new HomePage());
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