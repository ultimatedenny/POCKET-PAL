using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace POCKETPAL.Models
{
    public class AppInfoModel
    {
        public string AppName { get; set; }
        public float AppVersion { get; set; }
        public bool AppMaintenance { get; set; }
        public bool UpdateMandatory { get; set; }
    }
    public class AppInfoViewModel
    {
        public string AppPackageName => AppInfo.PackageName;

        public string AppName => AppInfo.Name;

        public string AppVersion => AppInfo.VersionString;

        public string AppBuild => AppInfo.BuildString;

        public string AppTheme => AppInfo.RequestedTheme.ToString();

        public Command ShowSettingsUICommand { get; }

        public AppInfoViewModel()
        {
            ShowSettingsUICommand = new Command(() => AppInfo.ShowSettingsUI());
        }
    }
}
