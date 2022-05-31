using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POCKETPAL.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace POCKETPAL
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
            Routing.RegisterRoute(nameof(ScannerPage), typeof(ScannerPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));
            Routing.RegisterRoute(nameof(SettingPage), typeof(SettingPage));
            Routing.RegisterRoute(nameof(ReportPage), typeof(ReportPage));
            Routing.RegisterRoute(nameof(WebviewPage), typeof(WebviewPage));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
            Routing.RegisterRoute(nameof(ExitPermitPage), typeof(ExitPermitPage));
            Routing.RegisterRoute(nameof(SSITPage), typeof(SSITPage));
            Routing.RegisterRoute(nameof(QRCodeSSITPage), typeof(QRCodeSSITPage));
            Routing.RegisterRoute(nameof(SSITSummaryScan), typeof(SSITSummaryScan));
            Routing.RegisterRoute(nameof(PinChangePage), typeof(PinChangePage));
            Routing.RegisterRoute(nameof(PinCheckPage), typeof(PinCheckPage));
            Routing.RegisterRoute(nameof(IntroPage), typeof(IntroPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ErecruitmentPage), typeof(ErecruitmentPage));
            Routing.RegisterRoute(nameof(ErecruitmentPageDetail), typeof(ErecruitmentPageDetail));

            Routing.RegisterRoute(nameof(CheckInNewPage), typeof(CheckInNewPage));
            Routing.RegisterRoute(nameof(CheckInNextDayPage), typeof(CheckInNextDayPage));
            Routing.RegisterRoute(nameof(CheckInSameDayPage), typeof(CheckInSameDayPage));
            Routing.RegisterRoute(nameof(CheckOutPage), typeof(CheckOutPage));
  //ErecruitmentPageDetail
        }
    }
}