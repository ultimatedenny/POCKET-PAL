using Newtonsoft.Json;
using POCKETPAL.Classes;
using POCKETPAL.Models;
using POCKETPAL.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QRCodeSSITPage : TabbedPage
    {
        string SSITUserGroup, SSITApprCat, SSITUserID;
        string _menuSSIT = "";
        bool _actionscaning = true;
        public QRCodeSSITPage (string menuSSIT)
        {
            InitializeComponent();
            this.BindingContext = new SSITViewModel(menuSSIT,"LIST");
            _menuSSIT = menuSSIT;
            Title = menuSSIT;
            tab1.Title = "Scan "+ menuSSIT;
            tab2.Title = "List " + menuSSIT;
            SSITUserGroup = GetValue("SSITUserGroup");
            SSITApprCat = GetValue("SSITApprCat");
            SSITUserID = GetValue("SSITUserID");

            if (menuSSIT.ToUpper() == "SCRAP" || (menuSSIT.ToUpper() == "PURCHASE RETURN" && Convert.ToInt32(SSITApprCat)>1))
            {
                PickerPlant.IsVisible = false;
                PickerVendor.IsVisible = false;
                PickerAction.IsVisible = false;
                EntryVendor.IsVisible = false;

                PickerPlant_view.IsVisible = false;
                PickerVendor_view.IsVisible = false;
                PickerAction_view.IsVisible = false;
                EntryVendor_view.IsVisible = false;
            }
        }

        protected async override void OnAppearing()
        {
            scanningZxing.IsAnalyzing = true;
            _actionscaning = true;
        }

        void ZXingScannerView_OnScanResult(ZXing.Result result)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (_actionscaning == true)
                {
                    resultqrcode.Text = result.Text;
                    SSITScanning(result.Text);
                }
            });

        }

        public void AddValue(string key, string value)
        {
            Preferences.Set(key, value);
        }
        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }

        public async void CheckBarcodeResult()
        {
            _actionscaning = false;
            btnSearch.IsEnabled = false;
            var result = resultqrcode.Text;
            var action = await DisplayAlert("QR Code", _menuSSIT + " " + result, "Yes", "No");
            if (action == true)
            {

                resultqrcode.Text = "";
            }
            else
            {
                resultqrcode.Text = "";
            }

        }

        public async void SSITScanning(string SSITID)
        {
            _actionscaning = false;
            btnSearch.IsEnabled = false;

            string url = "" + Address.Api + "/SSIT/CheckScanSSIT?Type="+ _menuSSIT.ToUpper() + "&UserId=" + SSITUserID + "&UserGroup=" + SSITUserGroup + "&ApprCat=" + SSITApprCat + "&Product=&SSITCode="+ SSITID + "";
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RootSSITModel model2 = JsonConvert.DeserializeObject<RootSSITModel>(content);

                    if (model2.Data != null)
                    {
                        if (model2.Data.CanApproved.ToString() == "1")
                        {
                            var action = await DisplayAlert("Result Scanning", "Material " + model2.Data.Material.ToString()+" " + model2.Data.MaterialDesc.ToString(), "Yes", "No");
                            if (action == true)
                            {
                                SaveResultScan(SSITID);
                            }
                        }
                        else
                        {
                            await DisplayAlert("Message Alert", "You can’t approve this material due to amount of this material " + model2.Data.Amount.ToString(), "Close");
                        }
                    }
                    else
                    {
                        await DisplayAlert("Message Alert", "Material doesn’t belong to " + _menuSSIT + "", "Close");
                    }

                    _actionscaning = true;
                    resultqrcode.Text = "";
                    btnSearch.IsEnabled = true;
                }
                else
                {
                    _actionscaning = true;
                    resultqrcode.Text = "";
                    btnSearch.IsEnabled = true;
                    await DisplayAlert("Error", response.RequestMessage.ToString(), "Close");
                }
            }
            catch (Exception msg)
            {
                _actionscaning = true;
                resultqrcode.Text = "";
                btnSearch.IsEnabled = true;
                await DisplayAlert("Error", msg.Message.ToString(), "Close");
            }
        }
        private void btnSearch_Clicked(object sender, EventArgs e)
        {
            var scan = resultqrcode.Text.Replace(" ","");
            if (scan != "")
            {
                SSITScanning(scan);
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Please type SSIT ID.");
            }
            
        }
        public async void btnSummaryScanApproval_Clicked(object sender, EventArgs e)
        {
            _actionscaning = false;
            await Navigation.PushModalAsync(new SSITSummaryScan(_menuSSIT));
        }

        private void searchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var vm = BindingContext as SSITViewModel;
            vm.SearchCommand.Execute(null);
        }

        private void searchResults_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vendor = e.Item as string;
            var vm = BindingContext as SSITViewModel;
            vm.AddComand.Execute(vendor);
        }

        public ObservableCollection<SSITModel> ListSSIT { get; set; }

        private void Chacked_All(object sender, CheckedChangedEventArgs e)
        {
            SSITViewModel SVM = BindingContext as SSITViewModel;
            if (CheckAll.IsChecked == true)
            {
                SVM.CheckAll();
            }
            else
            {
                SVM.UnCheckAll();
            }
        }

        private void TabbedPage_Appearing(object sender, EventArgs e)
        {
            _actionscaning = true;
        }
        public async void SaveResultScan(string SSITID)
        {
            string url = "" + Address.Api + "/SSIT/SaveScanSSIT?Type=" + _menuSSIT.ToUpper() + "&UserId=" + SSITUserID + "&UserGroup=" + SSITUserGroup + "&ApprCat=" + SSITApprCat + "&SSITCode=" + SSITID + "";
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    RootSSITModel model2 = JsonConvert.DeserializeObject<RootSSITModel>(content);

                    if (model2.Data != null)
                    {
                        DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                        //await DisplayAlert("Info", model2.Message.ToString(), "Close");
                    }
                }
                else
                {
                    await DisplayAlert("Error", response.RequestMessage.ToString(), "Close");
                }
            }
            catch (Exception msg)
            {
                await DisplayAlert("Error", msg.Message.ToString(), "Close");
            }
        }

        

    }
}