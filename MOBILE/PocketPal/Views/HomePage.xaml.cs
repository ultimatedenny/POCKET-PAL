using Plugin.FirebasePushNotification;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using System.Net.Http;
using Newtonsoft.Json;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        string UseID, UseNam, UseDep, UseEmail, UseTel, UsePin, UsePlant;
        //string _SSIDSuggest;
        string ID, NAME, EMPLOYEENO, DEPARTMENT, PLANT, SHIMANOBADGE;
        string LOGID, TIMEIN, TIMEOUT, DATEVISIT, NEEDLUNCH, NEEDSTAY, DATEOFEND, STATUS, BunsFunc;
        int CANCHECKIN = 1;
        int CHECKINTYPE = 1;
        bool isValid, isValid2;
        bool IsVerified = false;
        public HomePage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Sheet.CloseSheet();
                    await SheetVMS.CloseSheet();
                    Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
                    GetData();
                    bool allowed = false;
                    allowed = await GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();
                    if (allowed == false)
                    {
                        await DisplayAlert("Alert", "You have to provide Camera permission", "Ok");
                    }
                });
            });
        }
        public async void GetData()
        {
            UseID = GetValue("UseID");
            UseNam = GetValue("UseNam");
            UseDep = GetValue("UseDep");
            UsePlant = GetValue("UsePlant");
            UseEmail = GetValue("UseEmail");
            UseTel = GetValue("UseTel");
            UsePin = GetValue("UsePin");
            BunsFunc = GetValue("BusFunc");
            ID = GetValue("ID");
            NAME = GetValue("NAME");
            EMPLOYEENO = GetValue("EMPLOYEENO");
            DEPARTMENT = GetValue("DEPARTMENT");
            PLANT = GetValue("PLANT");
            SHIMANOBADGE = GetValue("SHIMANOBADGE");
            txtUseNam.Text = UseNam;
            txtUseEmail.Text = UseEmail;
            var CURRENTTOKEN = CrossFirebasePushNotification.Current.Token.ToString();
            AddValue("TOKENOLD", CURRENTTOKEN);
            await PP_GET_TS_ID_VMS();
        }

        public void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess.ToString();
            var profiles = e.ConnectionProfiles.ToString();
            if (access != "Internet")
            {
                string msg = "You don't have connection";
                ThrowException(msg, "Oops, something wrong happend");
            }
        }

        public async void GetIsUserVerified()
        {
            string url = "" + Address.Api + "/User/GetIsUserVerified?UserId=" + UseID;
            try
            {
                startLoading();
                List<RespondApi> model = new List<RespondApi>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    RespondApi model2 = JsonConvert.DeserializeObject<RespondApi>(content);
                    IsVerified = model2.Success;
                    stopLoading();
                    //GetNews();
                }
                else
                {
                    stopLoading();
                    IsVerified = false;
                }
            }
            catch (Exception msg)
            {
                stopLoading();
                IsVerified = false;
                ThrowException(msg.Message.ToString(), "Oops, something wrong happend");
            }
        }

        public void ChangeToColor_InActive()
        {
            VMS_BANNER.IsVisible = false;
            VMS_BANNER.BackgroundGradientStops.Clear();
            VMS_BANNER.BackgroundGradientStops.Clear();
            VMS_BANNER.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#87a8d0"), Offset = 0 });
            VMS_BANNER.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#b9ceeb"), Offset = Convert.ToSingle(0.75) });
            VMS_BANNER.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#deecfc"), Offset = 0 });
            VMS_BANNER.IsVisible = true;

            VMS_BANNER_BOTTOM.IsVisible = false;
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Clear();
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Clear();
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#87a8d0"), Offset = 0 });
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#b9ceeb"), Offset = Convert.ToSingle(0.75) });
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#deecfc"), Offset = 0 });
            VMS_BANNER_BOTTOM.IsVisible = true;
        }

        public void ChangeToColor_Active()
        {
            VMS_BANNER.IsVisible = false;
            VMS_BANNER.BackgroundGradientStops.Clear();
            VMS_BANNER.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#63a4ff"), Offset = 0 });
            VMS_BANNER.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#1976d2"), Offset = Convert.ToSingle(0.75) });
            VMS_BANNER.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#004ba0"), Offset = 0 });
            VMS_BANNER.IsVisible = true;

            VMS_BANNER_BOTTOM.IsVisible = false;
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Clear();
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#63a4ff"), Offset = 0 });
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#1976d2"), Offset = Convert.ToSingle(0.75) });
            VMS_BANNER_BOTTOM.BackgroundGradientStops.Add(new Xamarin.Forms.PancakeView.GradientStop() { Color = Color.FromHex("#004ba0"), Offset = 0 });
            VMS_BANNER_BOTTOM.IsVisible = true;
        }

        private async void btnExitPermitTap(object sender, EventArgs e)
        {
            bool isAllow = await GetMenuAccess("EXITPERMIT");
            if (isAllow == true)
            {
                await Shell.Current.GoToAsync($"ExitPermitPage");
            }
            else
            {
                ThrowMenuAccessFail();
            }
        }

        public async void btnErecTap(object sender, EventArgs e)
        {
            bool isAllow = await GetMenuAccess("ERECRUITMENT");
            if (isAllow == true)
            {
                await Shell.Current.GoToAsync($"ErecruitmentPage");
            }
            else
            {
                ThrowMenuAccessFail();
            }
        }

        public async void btnCKSTap(object sender, EventArgs e)
        {
            bool isAllow = await GetMenuAccess("CKS");
            if (isAllow == true)
            {
                string TOKEN = CrossFirebasePushNotification.Current.Token.ToString();
                if (String.IsNullOrEmpty(TOKEN))
                {
                    DependencyService.Get<IMessage>().ShortAlert("CKS need device Token, please logout and re-open application");
                }
                else
                {
                    string path = "" + Address.CKS + "/Mobile/Index?Token=" + TOKEN;
                    string title = "Central Knowledge System";
                    AddValue("TempPath", path);
                    AddValue("TempTitle", title);
                    await Shell.Current.GoToAsync($"WebviewPage");
                }
            }
            else
            {
                ThrowMenuAccessFail();
            }
        }

        public async void btnEapprovalTap(object sender, EventArgs e)
        {
            bool isAllow = await GetMenuAccess("EAPPROVAL");
            if (isAllow == true)
            {
                string TOKEN = CrossFirebasePushNotification.Current.Token.ToString();
                if (String.IsNullOrEmpty(TOKEN))
                {
                    DependencyService.Get<IMessage>().ShortAlert("E-Approval need device Token, please logout and re-open application");
                }
                else
                {
                    string path = "" + Address.EAPPROVAL + "/login?token=" + TOKEN;
                    string title = "E-Approval";
                    AddValue("TempPath", path);
                    AddValue("TempTitle", title);
                    await Shell.Current.GoToAsync($"WebviewPage");
                }
            }
            else
            {
                ThrowMenuAccessFail();
            }
        }

        public void btnVisitorTap(object sender, EventArgs e)
        {
            GetVisitorMS();
        }

        public async void btnMoreApps(object sender, EventArgs e)
        {
            try
            {
                var mainDisplayInfo = DeviceDisplay.MainDisplayInfo;
                var height = mainDisplayInfo.Height;
                var result = ((height * 50) / 100) / 2;
                Sheet.SheetHeight = 512;
                await Sheet.OpenSheet();
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "btnMoreApps");
            }
        }

        public async void btnSSITTap(object sender, EventArgs e)
        {
            bool IsSSITVerified = await GetIsUserSSITVerified();
            if (IsSSITVerified == true)
            {
                await Shell.Current.GoToAsync($"SSITPage");
            }
            else
            {
                ThrowMenuAccessFail();
            }
        }

        public async Task<bool> GetIsUserSSITVerified()
        {
            string url = "" + Address.Api + "/SSIT/GetIsUserSSITVerified?UserId=" + UseID;
            try
            {
                startLoading();
                List<RespondApi> model = new List<RespondApi>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                model.Clear();
                var content = await response.Content.ReadAsStringAsync();
                RootUser model2 = JsonConvert.DeserializeObject<RootUser>(content);
                AddValue("SSITUserGroup", model2.Data.BusinessFunction.ToString());
                AddValue("SSITApprCat", model2.Data.UDID.ToString());
                AddValue("SSITUserID", model2.Data.UserID.ToString());
                stopLoading();
                return model2.Success;
            }
            catch (Exception)
            {
                stopLoading();
                return false;
            }
        }

        public async Task<bool> GetMenuAccess(string MenuName)
        {
            string url = "" + Address.Api + "/Master/GetMenuAccess?MenuName=" + MenuName + "&BusFunc=" + BunsFunc + "";
            try
            {
                startLoading();
                List<RespondApi> model = new List<RespondApi>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    RootUser model2 = JsonConvert.DeserializeObject<RootUser>(content);
                    stopLoading();
                    return model2.Success;
                }
                else
                {
                    stopLoading();
                    return false;
                }
            }
            catch (Exception msg)
            {
                stopLoading();
                ThrowException(msg.Message.ToString(), "Oops, something wrong happend");
                return false;
            }
        }

        public async Task<bool> PP_GET_TS_ID_VMS()
        {
            isValid = false;
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

                    ID = (datas.Data.ID.ToString() == null) ? "" : datas.Data.ID.ToString();
                    NAME = (datas.Data.NAME.ToString() == null) ? "" : datas.Data.SHIMANOBADGE.ToString();
                    EMPLOYEENO = (datas.Data.EMPLOYEENO.ToString() == null) ? "" : datas.Data.EMPLOYEENO.ToString();
                    PLANT = (datas.Data.PLANT.ToString() == null) ? "" : datas.Data.PLANT.ToString();
                    DEPARTMENT = (datas.Data.DEPARTMENT.ToString() == null) ? "" : datas.Data.DEPARTMENT.ToString();
                    SHIMANOBADGE = (datas.Data.SHIMANOBADGE.ToString() == null) ? "" : datas.Data.SHIMANOBADGE.ToString();

                    if (!string.IsNullOrEmpty(SHIMANOBADGE))
                    {
                        VMS_BADGE.Text = SHIMANOBADGE;
                        VMS_BADGE_BOTTOM.Text = SHIMANOBADGE;
                    }
                    else
                    {
                        VMS_BADGE.Text = "";
                        VMS_BADGE_BOTTOM.Text = "";
                    }
                    isValid = true;
                    await PP_GET_TS_STATUS_VMS();
                }
                else
                {
                    isValid = false;
                    ThrowException(response.StatusCode.ToString(), "GetTSId");
                }
            }
            catch (Exception msg)
            {
                isValid = false;
                ThrowException(msg.Message.ToString(), "GetTSId");
            }
            return isValid;
        }

        public async Task<bool> PP_GET_TS_STATUS_VMS()
        {
            isValid2 = false;
            string url = "" + Address.Api + "/VisitorTS/PP_GET_TS_STATUS_VMS?TSVisitorId=" + ID;
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Root_TSSTATUS_Model datas = JsonConvert.DeserializeObject<Root_TSSTATUS_Model>(content);
                    AddValue("LOGID", (datas.Data.LOGID.ToString() == null) ? "" : datas.Data.LOGID.ToString());
                    AddValue("TIMEIN", (datas.Data.TIMEIN.ToString() == null) ? "" : datas.Data.TIMEIN.ToString());
                    AddValue("TIMEOUT", (datas.Data.TIMEOUT.ToString() == null) ? "" : datas.Data.TIMEOUT.ToString());
                    AddValue("DATEVISIT", (datas.Data.DATEVISIT.ToString() == null) ? "" : datas.Data.DATEVISIT.ToString());
                    AddValue("NEEDLUNCH", (datas.Data.NEEDLUNCH.ToString() == null) ? "" : datas.Data.NEEDLUNCH.ToString());
                    AddValue("NEEDSTAY", (datas.Data.NEEDSTAY.ToString() == null) ? "" : datas.Data.NEEDSTAY.ToString());
                    AddValue("DATEOFEND", (datas.Data.DATEOFEND.ToString()== null) ? "" : datas.Data.DATEOFEND.ToString());
                    AddValue("STATUS", (datas.Data.STATUS.ToString() == null) ? "" : datas.Data.STATUS.ToString());

                    LOGID = (datas.Data.LOGID.ToString() == null) ? "" : datas.Data.LOGID.ToString();
                    TIMEIN = (datas.Data.TIMEIN.ToString() == null) ? "" : datas.Data.TIMEIN.ToString();
                    TIMEOUT = (datas.Data.TIMEOUT.ToString() == null) ? "" : datas.Data.TIMEOUT.ToString();
                    DATEVISIT = (datas.Data.DATEVISIT.ToString() == null) ? "" : datas.Data.DATEVISIT.ToString();
                    NEEDLUNCH = (datas.Data.NEEDLUNCH.ToString() == null) ? "" : datas.Data.NEEDLUNCH.ToString();
                    NEEDSTAY = (datas.Data.NEEDSTAY.ToString() == null) ? "" : datas.Data.NEEDSTAY.ToString();
                    DATEOFEND = (datas.Data.DATEOFEND.ToString() == null) ? "" : datas.Data.DATEOFEND.ToString();
                    STATUS = (datas.Data.STATUS.ToString() == null) ? "" : datas.Data.STATUS.ToString();

                    if (string.IsNullOrEmpty(LOGID) && string.IsNullOrEmpty(SHIMANOBADGE))
                    {
                        CANCHECKIN = 1;
                        CHECKINTYPE = 1;
                    }
                    if (string.IsNullOrEmpty(LOGID) && !string.IsNullOrEmpty(SHIMANOBADGE))
                    {
                        CANCHECKIN = 1;
                        CHECKINTYPE = 6;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(LOGID))
                        {
                            string _datevisit = Convert.ToDateTime(DATEVISIT).ToString("yyyy-MM-dd");
                            string _dateofend = Convert.ToDateTime(DATEOFEND).ToString("yyyy-MM-dd");
                            string _datetimenow = DateTime.Now.ToString("yyyy-MM-dd");

                            if (!string.IsNullOrEmpty(TIMEIN) && string.IsNullOrEmpty(TIMEOUT) && (Convert.ToDateTime(_datevisit) == Convert.ToDateTime(_datetimenow)) && STATUS == "CHECKIN")
                            {
                                CANCHECKIN = 1;
                                CHECKINTYPE = 4;
                                VMS_BADGE.Text = SHIMANOBADGE;
                            }
                            else if (!string.IsNullOrEmpty(TIMEIN) && !string.IsNullOrEmpty(TIMEOUT) && (Convert.ToDateTime(_datevisit) == Convert.ToDateTime(_datetimenow)) && STATUS == "CHECKOUT")
                            {
                                CANCHECKIN = 1;
                                CHECKINTYPE = 2;
                            }
                            else if (!string.IsNullOrEmpty(TIMEIN) && !string.IsNullOrEmpty(TIMEOUT) && (Convert.ToDateTime(_datevisit) < Convert.ToDateTime(_datetimenow)) && (Convert.ToDateTime(_datetimenow) <= Convert.ToDateTime(_dateofend)) && STATUS == "CHECKOUT")
                            {
                                CANCHECKIN = 1;
                                CHECKINTYPE = 3;
                            }
                            else if (!string.IsNullOrEmpty(TIMEIN) && (Convert.ToDateTime(_datetimenow) > Convert.ToDateTime(_dateofend)) && (Convert.ToDateTime(_datevisit) < Convert.ToDateTime(_datetimenow)))
                            {
                                CANCHECKIN = 1;
                                CHECKINTYPE = 5;
                            }
                        }
                    }
                    AddValue("CANCHECKIN", Convert.ToString(CANCHECKIN));
                    AddValue("CHECKINTYPE", Convert.ToString(CHECKINTYPE));
                    isValid2 = true;
                }
                else
                {
                    isValid2 = false;
                }
            }
            catch (Exception msg)
            {
                isValid2 = false;
                ThrowException(msg.Message.ToString(), "GetTSStatus");
            }
            return isValid2;
        }

        private void VISITORMS_QR_TOUCHDOWN(object sender, EventArgs e)
        {
            GetVisitorMS();
        }

        public void VISITORMS_QR_TOUCHUP(object sender, EventArgs e)
        {

        }

        public async void VISITORMS_WIFI_TOUCHDOWN(object sender, EventArgs e)
        {
            if (SHIMANOBADGE != "")
            {
                string url = "" + Address.Api + "/VisitorTS/LoginArubaTemp?NAME=" + UseNam.ToUpper() + "&EMAIL=" + UseEmail.ToUpper() + "&BUSFUNC=" + BunsFunc.ToUpper() + "&TYPE=0&CUSTOMHOUR=1";
                try
                {
                    startLoading();
                    List<RespondApi> model = new List<RespondApi>();
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        model.Clear();
                        var content = await response.Content.ReadAsStringAsync();
                        Root_SSID Root = JsonConvert.DeserializeObject<Root_SSID>(content);
                        if (Root.Success == true)
                        {
                            wifiPopUp.IsVisible = true;
                            SSID_LABEL.Text = "SBM-GUEST";
                            USERNAME_LABEL.Text = Root.Data.userdb_add.name;
                            PASSWORD_LABEL.Text = Root.Data.userdb_add.passwd;
                            EXPIRE_LABEL.Text = Root.Data.userdb_add.expday + ", " + Root.Data.userdb_add.exptime;
                            stopLoading();
                        }
                        else
                        {
                            ThrowException(Root.Message.ToString(), "Oops, something wrong happend");
                            stopLoading();
                        }
                    }
                    else
                    {
                        ThrowException(response.RequestMessage.ToString(), "Oops, something wrong happend");
                        stopLoading();
                    }
                }
                catch (Exception msg)
                {
                    stopLoading();
                    ThrowException(msg.Message.ToString(), "Oops, something wrong happend");
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("You are not SBM Visitor");
            }
        }

        public void WIFIPOPUP_CLOSED_BUTTON(object sender, EventArgs e)
        {
            wifiPopUp.IsVisible = false;
        }

        public void VISITORMS_WIFI_TOUCHUP(object sender, EventArgs e)
        {

        }

        public async void GetVisitorMS()
        {
            bool isAllow = await GetMenuAccess("VISITORMS");
            if (isAllow == true)//
            {
                isValid = await PP_GET_TS_ID_VMS();
                if (isValid == true)
                {
                    isValid2 = await PP_GET_TS_STATUS_VMS();
                    if (isValid2 == true)
                    {
                        bool allowed = false;
                        allowed = await GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();
                        if (allowed)
                        {
                            await Shell.Current.GoToAsync($"{nameof(ScannerPage)}");
                            //if (CANCHECKIN == 1 && CHECKINTYPE == 4)
                            //{
                            //    var action = await DisplayAlert("Check out", "Are you going check out for this session ?", "Yes", "No");
                            //    if (action)
                            //    {
                            //        await Shell.Current.GoToAsync($"CheckOutPage");
                            //    }
                            //}
                            //else
                            //{
                            //    await Shell.Current.GoToAsync($"ScannerPage");
                            //}
                        }
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("VisitorMS need camera pemission, please allow on Setting on phone.");
                    }
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Getting TeamShimano ID Failed");
                }
            }
            else
            {
                ThrowMenuAccessFail();
            }
        }

        public void startLoading()
        {
            popupLoadingView.IsVisible = true;
            activityIndicator.IsRunning = true;
        }

        public void stopLoading()
        {
            popupLoadingView.IsVisible = false;
            activityIndicator.IsRunning = false;
        }

        public async void ThrowException(string message, string functions)
        {
            var action = await DisplayAlert("Warning", message.ToString(), "Report", "Dismiss");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"ReportPage");
            }
        }

        public async void ThrowMenuAccessFail()
        {
            await DisplayAlert("Access Failed", "Menu accessing failed, your group is not registered.", "Ok");
        }

        public void AddValue(string key, string value)
        {
            Preferences.Set(key, value);
        }

        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }

        private async void btnVisitorTapNew(object sender, EventArgs e)
        {
            try
            {
                await Sheet.CloseSheet();
                await SheetVMS.OpenSheet();
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "btnMoreApps");
            }
        }
    }
}