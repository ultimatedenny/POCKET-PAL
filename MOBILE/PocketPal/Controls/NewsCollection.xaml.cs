using POCKETPAL.Models;
using POCKETPAL.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewsCollection : ContentView
    {
        public NewsCollection()
        {
            InitializeComponent();
            this.BindingContext = this;
        }
        public List<NewsModel> News { get => GetNews(); }
        private List<NewsModel> GetNews()
        {
            var NewsLists = new List<NewsModel>();
            NewsLists.Add(new NewsModel
            {
                MasterId = "FC4C46AA-7809-44A6-9ECE-279081AAD95D",
                NewsId = "NW210426-001",
                Header = "Peraturan Perusahaan Shimano 2020",
                BodyTEXT = "Peraturan Perusahaan Shimano 2020",
                BodyHTML = "",
                CreatedBy = "HR Shimano",
                CreatedDate = "2021-04-26",
                FeaturedImage = "https://i.ibb.co/D5CKQKt/BG-01.jpg",
                Status = "",
                Viewer = ""
            });
            return NewsLists;
        }
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (cvBanners.Position == 2)
                {
                    cvBanners.Position = 0;
                    return;
                }
                cvBanners.Position += 1;
            });
        }
        public void Tittle_Tapped(object sender, EventArgs e)
        {
            string param = "FC4C46AA-7809-44A6-9ECE-279081AAD95D";
            string path = Address.VMS + "/Blog/Details?masterId=" + param;
            string title = "Peraturan Perusahaan Shimano 2020";
            AddValue("TempPath", path);
            AddValue("TempTitle", title);
            Shell.Current.GoToAsync($"WebviewPage");
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