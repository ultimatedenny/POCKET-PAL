using POCKETPAL.Classes;
using POCKETPAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReportPage : ContentPage
    {
        string profilesender, devicesender, errordetail;
        public ReportPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    string UseID, UseNam, UseDep, UsePlant, UseEmail, UseTel, UsePin, BusFunc;

                    var firstLaunch = VersionTracking.IsFirstLaunchEver;
                    var firstLaunchCurrent = VersionTracking.IsFirstLaunchForCurrentVersion;
                    var firstLaunchBuild = VersionTracking.IsFirstLaunchForCurrentBuild;
                    var currentVersion = VersionTracking.CurrentVersion;
                    var currentBuild = VersionTracking.CurrentBuild;
                    var previousVersion = VersionTracking.PreviousVersion;
                    var previousBuild = VersionTracking.PreviousBuild;
                    var firstVersion = VersionTracking.FirstInstalledVersion;
                    var firstBuild = VersionTracking.FirstInstalledBuild;
                    var versionHistory = VersionTracking.VersionHistory;
                    var buildHistory = VersionTracking.BuildHistory;
                    var appName = AppInfo.Name;
                    var packageName = AppInfo.PackageName;
                    var version = AppInfo.VersionString;
                    var build = AppInfo.BuildString;
                    var device = DeviceInfo.Model;
                    var manufacturer = DeviceInfo.Manufacturer;
                    var deviceName = DeviceInfo.Name;
                    var OS = DeviceInfo.VersionString;
                    var platform = DeviceInfo.Platform;
                    var idiom = DeviceInfo.Idiom;
                    var deviceType = DeviceInfo.DeviceType;
                    var CoonectionAccess = Connectivity.NetworkAccess;
                    var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                    var orientation = DeviceDisplay.MainDisplayInfo.Orientation;
                    var rotation = DeviceDisplay.MainDisplayInfo.Rotation;
                    var width = DeviceDisplay.MainDisplayInfo.Width;
                    var height = DeviceDisplay.MainDisplayInfo.Height;
                    var density = DeviceDisplay.MainDisplayInfo.Density;

                    UseID = GetValue("UseID");
                    UseNam = GetValue("UseNam");
                    UseDep = GetValue("UseDep");
                    UsePlant = GetValue("UsePlant");
                    UseEmail = GetValue("UseEmail");
                    UseTel = GetValue("UseTel");
                    UsePin = GetValue("UsePin");
                    BusFunc = GetValue("BunsFunc");
                    string DATETIME = DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt");



                    profilesender = "USERNAME = " + UseID + "\n" +
                                    "NAME = " + UseNam;

                    devicesender = "Version : " + version + "\n" +
                                       "Build : " + build + "\n" +
                                       "Device : " + device + "\n" +
                                       "OS: " + OS + "\n" +
                                       "Platform : " + platform + "\n" +
                                       "Idiom : " + idiom + "\n" +
                                       "Device Type : " + deviceType + "\n" +
                                       "Connection Type : " + CoonectionAccess;

                    errordetail = BODY_TextBox.Text;

                    TO_TextBox.Text = "helpdesk@shimano.freshservice.com";
                    CC_TextBox.Text = "deniandrean@sbm.shimano.com.sg";

                    BCC_TextBox.Text = "";
                    SUBJECT_TextBox.Text = "[BUG REPORT] - POCKETPAL " + DATETIME.ToUpper() + "";
                    BODY_TextBox.Text = "";

                });
            });
        }

        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }
        public async void SendReport(object sender, EventArgs e)
        {
            try
            {
                var message = new EmailMessage();
                message.To = Split(TO_TextBox.Text);
                message.Cc = Split(CC_TextBox.Text);
                message.Bcc = Split(BCC_TextBox.Text);
                message.Bcc = Split(BCC_TextBox.Text);
                message.BodyFormat = EmailBodyFormat.PlainText;
                message.Subject = SUBJECT_TextBox.Text;
                message.Body = profilesender + "\n\n" + errordetail + "\n\n" + devicesender;
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException)
            {
                DependencyService.Get<IMessage>().ShortAlert("Your phone doesn't set a default email.");
            }
            catch (Exception msg)
            {
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
            }
        }
        List<string> Split(string recipients)
           => recipients?.Split(new char[] { ',', ';', ' ' }, StringSplitOptions.RemoveEmptyEntries)?.ToList();
    }
}