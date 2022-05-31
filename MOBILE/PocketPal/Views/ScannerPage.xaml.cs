using Newtonsoft.Json;
using Plugin.XF.TouchID;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ScannerPage : ContentPage
    {
        ZXingScannerPage scanPage;
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;
        Stopwatch stopwatch;
        float whitePosition = 0f;
        float transparentPosition = 0f;
        double cornerRadius => 30;
        Color transparentColour;
        Color whiteColour;
        bool pageIsActive;
        double cornersStrokeThickness => 4;
        string result, UseID, UseNam;
        string QRCODE, DATE, TIME, USEID, USENAM, INOUT;
        string UseDep, UsePlant, UseEmail, UseTel, UsePin, BUSFUNC;
        string LOGID, TIMEIN, TIMEOUT, DATEVISIT, NEEDLUNCH, NEEDSTAY, DATEOFEND, STATUS;
        string ID, NAME, EMPLOYEENO, DEPARTMENT, PLANT, SHIMANOBADGE;
        string EPNo, Destination, Date, Out, In, CompTrans, Status, ExpireTicket, Approver, CreatedBy, ActIn, ActOut, Employee;
        int CANCHECKIN;
        int CHECKINTYPE;
        string DeviceSecurity;
        bool BiometricResult;
        bool CanEPBeforeTimeOut = false;
        public static string public_QR;
        public ScannerPage(string PARAM)
        {
            InitializeComponent();
            Shell.Current.GoToAsync($"//{nameof(HomePage)}");
        }
        public static string _public_QR
        {
            get { return public_QR; }
            set { public_QR = value; }
        }
        public ScannerPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                GetPermission();
            });
        } 
        public async void GetPermission()
        {
            bool allowed = false;
            allowed = await GoogleVisionBarCodeScanner.Methods.AskForRequiredPermission();
            if (allowed == false)
            {
                await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
            }
            else
            {
                GetData();
            }
        }
        public void GetData()
        {
            transparentColour = Color.FromHex("#00000000");
            whiteColour = Color.FromHex("#80ffffff");

            UseID = GetValue("UseID");
            UseNam = GetValue("UseNam");
            UseDep = GetValue("UseDep");
            UsePlant = GetValue("UsePlant");
            UseEmail = GetValue("UseEmail");
            UseTel = GetValue("UseTel");
            UsePin = GetValue("UsePin");

            BUSFUNC = GetValue("BusFunc");

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
            DeviceSecurity = GetValue("DEVICESEC");

            GetExitPermitAllowBeforeTimeOut();
        }
        public async void GetExitPermitAllowBeforeTimeOut()
        {
            string url = "" + Address.Api + "/ExitPermit/GetExitPermitAllowBeforeTimeOut";
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RespondApi model2 = JsonConvert.DeserializeObject<RespondApi>(content);
                    if (model2.Success == true)
                    {
                        ScanArea.IsScanning  = true;
                        ScanArea.IsAnalyzing = true;
                        CanEPBeforeTimeOut = true;
                    }
                    else
                    {
                        CanEPBeforeTimeOut = false;
                        DependencyService.Get<IMessage>().ShortAlert(model2.Message);
                    }
                }
                else
                {
                    CanEPBeforeTimeOut = false;
                    DependencyService.Get<IMessage>().ShortAlert(response.IsSuccessStatusCode.ToString());
                }
            }
            catch (Exception msg)
            {
                CanEPBeforeTimeOut = false;
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
            }
        }
        public void ResultScan(ZXing.Result result)
        {
            ScanArea.IsAnalyzing = false;
            Device.BeginInvokeOnMainThread(() =>
            {
                ScanResult(result.Text);
            });
        }
        public void ScanResult(string Result)
        {
            string url = "";
            try
            {
                ScanArea.IsAnalyzing = false;
                if (string.IsNullOrEmpty(DeviceSecurity) || DeviceSecurity == "0")
                {
                    url = "" + Address.Api + "/User/GetIsBusFuncAllowed?BusFunc=" + BUSFUNC + "&ScannedCode=" + Result;
                    UsePinSecurity(url, Result);
                }
                else
                {
                    url = "" + Address.Api + "/User/GetIsBusFuncAllowed?BusFunc=" + BUSFUNC + "&ScannedCode=" + Result;
                    UseDeviceSecurity(url, Result);
                }
            }
            catch(Exception msg)
            {
                ScanArea.IsAnalyzing = true;
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
            }
        }
        public async void UsePinSecurity(string url, string Result)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RespondApi models = JsonConvert.DeserializeObject<RespondApi>(content);
                    if (models.Success == true)
                    {
                        if (Result.Contains("QR-VMS"))
                        {
                            ScanArea.IsAnalyzing = true;
                            QRVMS(Result);
                        }
                        else if (Result.Contains("QR-EP"))
                        {
                            QREP(Result);
                        }
                        else if ((Result.Contains("QR-ER")) ||(Result.Contains("JA-") && Result.Length > 16))
                        {
                            QRJA(Result);
                        }
                        else
                        {
                            ScanArea.IsAnalyzing = true;
                            DependencyService.Get<IMessage>().ShortAlert(Result);
                        }
                    }
                    else
                    {
                        ScanArea.IsAnalyzing = true;
                        DependencyService.Get<IMessage>().ShortAlert(models.Message.ToString());
                    }
                }
                else
                {
                    ScanArea.IsAnalyzing = true;
                    DependencyService.Get<IMessage>().ShortAlert(response.StatusCode.ToString());
                }
            }
            catch(Exception msg)
            {
                ScanArea.IsAnalyzing = true;
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
            }
        }
        public async void UseDeviceSecurity(string url, string Result)
        {
            if (TouchID.IsDeviceSecured())
            {
                var dialogConfig = new DialogConfiguration(
                    dialogTitle: "Pocket Pal Authentication",
                    dialogDescritpion: "Please enter your device security",
                    successAction: () =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            UsePinSecurity(url, Result);
                        });
                    },
                    alterAuthButtonText: "Use PIN",
                    fingerprintDialogConfiguration: new FingerprintDialogConfiguration
                    {
                        FingerprintHintString = "Touch Sensor",
                        FingerprintNotRecoginzedString = "Not regonized"
                    },
                    failedAction: () =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            DisplayAlert("Alert", "Too many unsuccessful attempt, please try again later", "OK");
                        });
                    });
                await TouchID.Authenticate(dialogConfig);
            }
            else
            {
                var action = await DisplayAlert("Alert", "Unable to proceed, please add your Device Security such as Password, PIN, or Pattern.", "Phone Setting", "Dismiss");
                if (action)
                {
                    DeviceSecurity = "0";
                    AddValue("DEVICESEC", DeviceSecurity);
                    TouchID.PromptSecuritySettings();
                }
            }
        }
        public async void QREP(string Result)
        {
            string EPNo_key = Result.ToString();
            string EPNOreal = EPNo_key.Substring(3);
            string url = "" + Address.Api + "/ExitPermit/GetExitPermit?EPNo=" + EPNOreal + "";
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                List<EPMaster> model = new List<EPMaster>();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<List<EPMaster>>(content);

                    EPNo = model[0].EPNo.ToString();
                    Destination = model[0].Destination.ToString();
                    Out = model[0].Out.ToString();
                    In = model[0].In.ToString();
                    Status = model[0].Status.ToString();
                    ExpireTicket = model[0].ExpireTicket.ToString();
                    Employee = model[0].Employee.ToString();
                    Date = model[0].Date.ToString();

                    if (model[0].CompTrans.ToString() == "FALSE" || model[0].CompTrans.ToString() == "False" || model[0].CompTrans.ToString() == null || model[0].CompTrans.ToString() == "0")
                    {
                        CompTrans = "No";
                    }
                    else if (model[0].CompTrans.ToString() == "TRUE" || model[0].CompTrans.ToString() == "True" || model[0].CompTrans.ToString() == "1" || model[0].CompTrans.ToString() == "Yes")
                    {
                        CompTrans = "Yes";
                    }

                    CreatedBy = model[0].CreatedBy.ToString();
                    ActOut = model[0].ActOut.ToString();
                    ActIn = model[0].ActIn.ToString();

                    DateTime _Date = Convert.ToDateTime(Date + " " + Out);

                    if (CanEPBeforeTimeOut == true)
                    {
                        if (DateTime.Now < _Date)
                        {
                            await DisplayAlert("Warning", "Exit Permit only allows employees to leave at requested hours, please try again later.", "Dismiss", " ");
                            return;
                        }
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var action = await DisplayAlert("Exit Permit",
                         "EP No: " + EPNo + "\n" +
                         "Destination: " + Destination + "\n" +
                         "Time Out: " + Out + "\n" +
                         "Time In: " + In + "\n" +
                         "Comp. Transport: " + CompTrans + "\n" +
                         "Status: " + Status + "\n" +
                         "Requestor: " + CreatedBy + "\n" +
                         "Employee: " + Employee + "\n" +
                         "", "Yes", "No");
                       
                        if (action == true)
                        {
                            if (Status == "PENDING")
                            {
                                DependencyService.Get<IMessage>().ShortAlert("Exit Permit Still Pending Approval");
                                ScanArea.IsAnalyzing = true;
                            }
                            else if (Status == "COMPLETED")
                            {
                                DependencyService.Get<IMessage>().ShortAlert("Exit Permit has been Completed");
                                ScanArea.IsAnalyzing = true;
                            }
                            else if (Status == "APPROVED")
                            {

                                if (ActOut == "00:00:00" || ActOut == "00:00:000" || ActOut == "00:00")
                                {
                                    string url1 = "" + Address.Api + "/ExitPermit/UpdateExitPermitOut?EPNo=" + EPNo + "&UseId=" + UseID + "";
                                    try
                                    {
                                        HttpResponseMessage response1 = new HttpResponseMessage();
                                        HttpClient client1 = new HttpClient();
                                        response = await client1.GetAsync(url1);
                                        if (response1.IsSuccessStatusCode)
                                        {
                                            var content1 = await response.Content.ReadAsStringAsync();
                                            DependencyService.Get<IMessage>().ShortAlert("Exit Permit Updated");
                                            ScanArea.IsAnalyzing = true;
                                        }
                                        else
                                        {
                                            DependencyService.Get<IMessage>().ShortAlert("Fail Update");
                                            ScanArea.IsAnalyzing = true;
                                        }
                                    }
                                    catch (Exception msg)
                                    {
                                        DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                                        ScanArea.IsAnalyzing = true;
                                    }
                                }
                                else
                                {
                                    string url1 = "" + Address.Api + "/ExitPermit/UpdateExitPermitIn?EPNo=" + EPNo + "&UseId=" + UseID + "";
                                    try
                                    {
                                        HttpResponseMessage response1 = new HttpResponseMessage();
                                        HttpClient client1 = new HttpClient();
                                        response = await client1.GetAsync(url1);
                                        if (response1.IsSuccessStatusCode)
                                        {
                                            var content1 = await response.Content.ReadAsStringAsync();
                                            DependencyService.Get<IMessage>().ShortAlert("Exit Permit Updated");
                                            ScanArea.IsAnalyzing = true;
                                        }
                                        else
                                        {
                                            DependencyService.Get<IMessage>().ShortAlert("Fail Update");
                                            ScanArea.IsAnalyzing = true;
                                        }
                                    }
                                    catch (Exception msg)
                                    {
                                        DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                                        ScanArea.IsAnalyzing = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ScanArea.IsAnalyzing = true;
                            DependencyService.Get<IMessage>().ShortAlert("Update Canceled");
                        }
                    });
                }
                else
                {
                    ScanArea.IsAnalyzing = true;
                    DependencyService.Get<IMessage>().ShortAlert(response.IsSuccessStatusCode.ToString());
                }

            }
            catch (Exception msg)
            {
                ScanArea.IsAnalyzing = true;
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
            }
        }
        public async void QRJA(string Result)
        {
            string BatchEmp = Result.Substring(0, 18);
            string url = "" + Address.Api + "/Erecruitment/GetEmpListBatchQR?BatchEmp=" + BatchEmp.Replace("QR-","") + "";
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                List<BatchList> model = new List<BatchList>();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                string a, b, c, d, e, f;
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<List<BatchList>>(content);
                    a = model[0].BatchEmp.ToString();
                    b = model[0].NameEmp.ToString();
                    c = model[0].PhoneNumber.ToString();
                    d = model[0].StatusEmp.ToString();
                    e = model[0].StatusBatch.ToString();
                    f = model[0].BatchComp.ToString();

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var action = await DisplayAlert("Candidate Confirmation",
                         "Candidate Batch: " + a + "\n" +
                         "Name: " + b + "\n" +
                         "Phone Number: " + c + "\n" +
                         "Status: " + d + "\n" +
                         "", "Yes", "No");
                        if (action == true)
                        {
                            if (e == "PENDING")
                            {
                                e = "ON-GOING";
                                string urlS = "" + Address.Api + "/Erecruitment/UpdateBatch?Status=" + e + "&Batch=" + f + "&UseId=" + UseID + "";
                                try
                                {
                                    HttpResponseMessage responseS = new HttpResponseMessage();
                                    HttpClient clientS = new HttpClient();
                                    responseS = await client.GetAsync(urlS);
                                    if (responseS.IsSuccessStatusCode)
                                    {
                                        UpdateCandidate(a, d);
                                    }
                                    else
                                    {
                                        ScanArea.IsAnalyzing = true;
                                        DependencyService.Get<IMessage>().ShortAlert("Update cancelled");
                                    }
                                }
                                catch (Exception msg)
                                {
                                    ScanArea.IsAnalyzing = true;
                                    DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                                }
                            }
                            else if (e == "ON-GOING")
                            {
                                UpdateCandidate(a, d);
                            }
                            else if (e == "FINISHED")
                            {
                                ScanArea.IsAnalyzing = true;
                                DependencyService.Get<IMessage>().ShortAlert("This Batch has been marked to completed, please ask IT to open this Batch again");
                            }
                            else if (e == "CLOSED")
                            {
                                ScanArea.IsAnalyzing = true;
                                DependencyService.Get<IMessage>().ShortAlert("This Batch has been marked to completed, please ask IT to open this Batch again");
                            }
                        }
                        else
                        {
                            ScanArea.IsAnalyzing = true;
                            DependencyService.Get<IMessage>().ShortAlert("Update canceled");
                        }
                    });
                }
                else
                {
                    ScanArea.IsAnalyzing = true;
                    DependencyService.Get<IMessage>().ShortAlert(response.StatusCode.ToString());
                }
            }
            catch (Exception msg)
            {
                ScanArea.IsAnalyzing = true;
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
            }
        }
        public async void QRVMS(string Result)
        {
            public_QR = Result;
            await Shell.Current.GoToAsync($"PinCheckPage");
            //string NAME   = GetValue("UseNam");
            //string USEID  = GetValue("UseID");
            //string QR     = Result;
            //string url    = "" + Address.Api + "/VisitorTS/QRCheck?Client=" + QR + "&UseID=" + USEID + "&UseNam=" + NAME + "";
            //try
            //{
            //    HttpResponseMessage response = new HttpResponseMessage();
            //    List<RespondApi> model = new List<RespondApi>();
            //    HttpClient client = new HttpClient();
            //    response = await client.GetAsync(url);
            //    if (response.IsSuccessStatusCode)
            //    {
            //        model.Clear();
            //        var content = await response.Content.ReadAsStringAsync();
            //        model = JsonConvert.DeserializeObject<List<RespondApi>>(content);
            //        if (model[0].Success == true)
            //        {
            //            //await Shell.Current.Navigation.PushModalAsync(new PinCheckPage(), false);
            //            await Shell.Current.GoToAsync($"PinCheckPage");
            //        }
            //        else
            //        {   
            //            DependencyService.Get<IMessage>().ShortAlert(model[0].Message.ToString());
            //        }
            //    }
            //    else
            //    {
            //        DependencyService.Get<IMessage>().ShortAlert(response.IsSuccessStatusCode.ToString());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DependencyService.Get<IMessage>().ShortAlert(ex.Message.ToString());
            //}
        }
        public async void UpdateCandidate(string a, string d)
        {
            if (d == "INVITED" || d == "NOT INVITED")
            {
                string url1 = "" + Address.Api + "/Erecruitment/UpdateEmployee?Status=PRESENT&BatchEmp=" + a + "&UseId=" + UseID + "";
                try
                {
                    HttpResponseMessage response1 = new HttpResponseMessage();
                    HttpClient client1 = new HttpClient();
                    response1 = await client1.GetAsync(url1);
                    if (response1.IsSuccessStatusCode)
                    {
                        var content1 = await response1.Content.ReadAsStringAsync();
                        ScanArea.IsAnalyzing = true;
                    }
                    else
                    {
                        ScanArea.IsAnalyzing = true;
                        DependencyService.Get<IMessage>().ShortAlert("Fail Update");
                    }
                }
                catch (Exception msg)
                {
                    ScanArea.IsAnalyzing = true;
                    DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                }
            }
            else if (d == "PRESENT")
            {
                string url2 = "" + Address.Api + "/Erecruitment/UpdateEmployee?Status=ABSENT&BatchEmp=" + a + "&UseId=" + UseID + "";
                try
                {
                    HttpResponseMessage response2 = new HttpResponseMessage();
                    HttpClient client2 = new HttpClient();
                    response2 = await client2.GetAsync(url2);
                    if (response2.IsSuccessStatusCode)
                    {
                        var content2 = await response2.Content.ReadAsStringAsync();
                        ScanArea.IsAnalyzing = true;
                    }
                    else
                    {
                        ScanArea.IsAnalyzing = true;
                        DependencyService.Get<IMessage>().ShortAlert("Fail Update");
                    }
                }
                catch (Exception msg)
                {
                    ScanArea.IsAnalyzing = true;
                    DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
                }
            }
            else if (d == "ABSENT")
            {
                string url3 = "" + Address.Api + "/Erecruitment/UpdateEmployee?Status=PRESENT&BatchEmp=" + a + "&UseId=" + UseID + "";
                try
                {
                    HttpResponseMessage response3 = new HttpResponseMessage();
                    HttpClient client3 = new HttpClient();
                    response3 = await client3.GetAsync(url3);
                    if (response3.IsSuccessStatusCode)
                    {
                        var content3 = await response3.Content.ReadAsStringAsync();
                        ScanArea.IsAnalyzing = true;
                    }
                    else
                    {
                        ScanArea.IsAnalyzing = true;
                        DependencyService.Get<IMessage>().ShortAlert("Fail Update");
                    }
                }
                catch (Exception msg)
                {
                    ScanArea.IsAnalyzing = true;
                    DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
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
        private void Scanning1(object sender, EventArgs e)
        {
            //scanPage = new ZXingScannerPage();
            //scanPage.OnScanResult += (result) =>
            //{
            //    scanPage.IsScanning = false;

            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Navigation.PopAsync();
            //        await DisplayAlert("Scanned Barcode", result.Text, "OK");
            //    });
            //};
            //Navigation.PushAsync(scanPage);
        }
        public void Scanning2(object sender, EventArgs e)
        {
            //var customoverlay = new StackLayout
            //{
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    VerticalOptions = LayoutOptions.FillAndExpand
            //};
            //var torch = new Button
            //{
            //    Text = "toggle torch"
            //};
            //torch.Clicked += delegate
            //{
            //    scanPage.ToggleTorch();
            //};
            //customoverlay.Children.Add(torch);

            //scanPage = new ZXingScannerPage(new ZXing.Mobile.MobileBarcodeScanningOptions { AutoRotate = true }, customOverlay: customoverlay);
            //scanPage.OnScanResult += (result) =>
            //{
            //    scanPage.IsScanning = false;

            //    Device.BeginInvokeOnMainThread(async () =>
            //    {
            //        await Navigation.PopAsync();
            //        await DisplayAlert("scanned barcode", result.Text, "ok");
            //    });
            //};
            //Navigation.PushAsync(scanPage);
        }
    }
}