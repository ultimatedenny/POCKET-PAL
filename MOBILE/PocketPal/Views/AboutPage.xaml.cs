using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POCKETPAL.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using Plugin.FirebasePushNotification;
using POCKETPAL.Classes;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Title = "About Application";
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

                    lblName.Text = appName;
                    lblPackage.Text = packageName;
                    lblAppVersion.Text = version;
                    lblAppBuild.Text = build;
                    var CURRENTTOKEN = CrossFirebasePushNotification.Current.Token.ToString();
                    TokenDevice.Text = CURRENTTOKEN;
                });
            });
        }
        private void OpenSetting(object sender, EventArgs e)
        {
            AppInfo.ShowSettingsUI();
        }

        private async void CopyToken(object sender, EventArgs e)
        {
            await Clipboard.SetTextAsync(TokenDevice.Text);
            DependencyService.Get<IMessage>().ShortAlert("Token copied to clipboard !");
        }
    }
}
