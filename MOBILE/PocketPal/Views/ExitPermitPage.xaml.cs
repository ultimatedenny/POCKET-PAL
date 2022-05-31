using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using POCKETPAL.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Essentials;
using POCKETPAL.Classes;
using static POCKETPAL.Controls.SegmentedControl;

namespace POCKETPAL.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExitPermitPage : ContentPage
	{
        Dictionary<int, string> _itmList;
        Dictionary<string, string> _comptransTimeLst;
        Dictionary<string, string> _usecomptransLst;
        Dictionary<int, string> _itmList0;
        Dictionary<int, int> _itmList1;
        Dictionary<string, string> _itmList2;
        Dictionary<string, string> _itmList3;
        string feedback = "";
        TimeSpan min = new TimeSpan(8, 0, 0);
        TimeSpan max = new TimeSpan(15, 0, 0);
        private Random random = new Random();
        private ObservableCollection<RepeaterItem> items = new ObservableCollection<RepeaterItem>();
        bool isValid;
        public ExitPermitPage ()
		{
			InitializeComponent ();
		}
        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    InitialData();
                });
            });
        }
        public void InitialData()
        {
            SetItemsSource();
            GetPlant();
            GetCompanyTransTime();
            txtDate.MinimumDate = DateTime.Now;
            CompTransTime.IsEnabled = false;
            OtherCompTrans.IsEnabled = false;
            var navigationPage = Application.Current.MainPage as NavigationPage;
            if (null != navigationPage)
            {
                navigationPage.BarTextColor = Color.FromHex("#000000");
                navigationPage.BarBackgroundColor = Color.FromHex("#FFFFFF");
            }
            Title = "Exit Permit";
        }
        public void CompTransSelected(object sender, EventArgs e)
        {
            if (CompTrans.SelectedIndex == 0)
            {
                CompTransTime.IsEnabled = true;
                OtherCompTrans.IsEnabled = false;
            }
            else
            {
                CompTransTime.IsEnabled = false;
                CompTransTime.SelectedIndex = 0;
                OtherCompTrans.IsEnabled = false;
            }
        }
        public void CompTransTimeSelected(object sender, EventArgs e)
        {
            if (CompTransTime.SelectedItem.ToString() == "OTHERS")
            {
                OtherCompTrans.IsEnabled = true;
            }
            else
            {
                OtherCompTrans.IsEnabled = false;
            }
        }
        public void SetItemsSource()
        {
            items = GetRandomItems();
            MainRepeater.ItemsSource = items;
        }
        
        private ObservableCollection<RepeaterItem> GetRandomItems()
        {
            var count = random.Next(0);
            var result = new ObservableCollection<RepeaterItem>();

            for (int i = 0; i < count; i++)
            {
                var item = GetRandomItem();
                result.Add(item);
            }
            return result;
        }
        private RepeaterItem GetRandomItem()
        {
            var username = usernameSignup.Text;
            var item = new RepeaterItem(username);
            return item;
        }
        public class RepeaterItem
        {
            public RepeaterItem(string username)
            {
                UseId = $"{username}";
            }
            public string UseId { get; set; }
        }
        public void ChangeSource_OnClicked(object sender, EventArgs e)
        {
            SetItemsSource();
        }
        public async void AddItem_OnClicked(object sender, EventArgs e)
        {
            try
            {
                if (items.Count == 4)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Exit Permit only allow up to 4 Employees");
                }
                else
                {
                    if (string.IsNullOrEmpty(usernameSignup.Text))
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Please type Employee Number or Windows ID");
                    }
                    else
                    {
                        var newItem = GetRandomItem();
                        isValid = await GetUser(usernameSignup.Text);
                        if (isValid == true)
                        {
                            if (items.Count == 0)
                            {
                                items.Add(newItem);
                                usernameSignup.Text = "";
                            }
                            else
                            {
                                var isduplicate = (from x in items
                                                   where x.UseId == newItem.UseId
                                                   select new { value = x.UseId }).ToList();

                                if (isduplicate.Count >= 1)
                                {
                                    usernameSignup.Text = "";
                                    DependencyService.Get<IMessage>().ShortAlert("User already on the list");
                                }
                                else
                                {
                                    items.Add(newItem);
                                    usernameSignup.Text = "";
                                }
                            }
                        }
                        else
                        {
                            usernameSignup.Text = "";
                            DependencyService.Get<IMessage>().ShortAlert("User not found");
                        }
                    }
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "AddItem_OnClicked");
            }
        }
        public void DeleteClicked(object sender, EventArgs e)
        {
            var item = (Xamarin.Forms.Button)sender;
            RepeaterItem listitem = (from itm in items
                                     where itm.UseId == item.CommandParameter.ToString()
                                     select itm)
                            .FirstOrDefault<RepeaterItem>();
            items.Remove(listitem);
        }
        public async void GetCompanyTransTime()
        {
            string url = "" + Address.Api + "/ExitPermit/GetCompanyTransTime";
            try
            {
                popupLoadingView.IsVisible = true;
                activityIndicator.IsRunning = true;

                _usecomptransLst = new Dictionary<string, string>();
                _usecomptransLst.Add("Yes", "Yes");
                _usecomptransLst.Add("No", "No");
                CompTrans.ItemsSource = _usecomptransLst.Values.ToList();
                CompTrans.SelectedIndex = -1;

                lblLoadingText.Text = "Loading Time list...";
                List<CompanyTransTime> model = new List<CompanyTransTime>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<List<CompanyTransTime>>(content);
                    _comptransTimeLst = new Dictionary<string, string>();
                    foreach (var result in model)
                    {
                        _comptransTimeLst.Add(result.Time, result.Time);
                    }
                    CompTransTime.ItemsSource = _comptransTimeLst.Values.ToList();
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                }
                else
                {
                    ThrowException(response.StatusCode.ToString(), "GetCompanyTransTime");
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "GetCompanyTransTime");
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                lblLoadingText.Text = "";
            }
        }
        public async void GetPlant()
        {
            PickerPlant.SelectedIndex = -1;
            string url = "" + Address.Api + "/ExitPermit/GetPlantEP";
            try
            {
                popupLoadingView.IsVisible = true;
                activityIndicator.IsRunning = true;
                lblLoadingText.Text = "Loading Plant list...";
                List<PlantModels> plantList = new List<PlantModels>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    plantList.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    plantList = JsonConvert.DeserializeObject<List<PlantModels>>(content);

                    _itmList1 = new Dictionary<int, int>();
                    foreach (var plant in plantList)
                    {
                        _itmList1.Add(Convert.ToInt32(plant.PlantCode), Convert.ToInt32(plant.PlantCode));
                    }
                    PickerPlant.ItemsSource = _itmList1.Values.ToList();
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                }
                else
                {
                    ThrowException(response.StatusCode.ToString(), "GetPlant");
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "GetPlant");
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                lblLoadingText.Text = "";
                return;
            }
        }
        public async void GetDept(object sender, EventArgs e)
        {
            PickerDept.SelectedIndex = -1;
            if (PickerPlant.SelectedItem != null)
            {
                string url = "" + Address.Api + "/ExitPermit/GetDeptEP?Plant=" + (PickerPlant.SelectedItem.ToString()).Substring(0, 4);
                try
                {
                    popupLoadingView.IsVisible = true;
                    activityIndicator.IsRunning = true;
                    lblLoadingText.Text = "Loading Department list...";
                    List<DeptModels> deptList = new List<DeptModels>();
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        deptList.Clear();
                        var content = await response.Content.ReadAsStringAsync();
                        deptList = JsonConvert.DeserializeObject<List<DeptModels>>(content);
                        _itmList2 = new Dictionary<string, string>();
                        _itmList2.Clear();
                        foreach (var dept in deptList)
                        {
                            _itmList2.Add(dept.DepartmentID, dept.DepartmentID + " - " + dept.DepartmentName);
                        }
                        PickerDept.ItemsSource = _itmList2.Values.ToList();
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        lblLoadingText.Text = "";
                    }
                    else
                    {
                        ThrowException(response.StatusCode.ToString(), "GetDept");
                    }
                }
                catch (Exception msg)
                {
                    ThrowException(msg.Message.ToString(), "GetDept");
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                    return;
                }
            }
        }
        public async void GetApprover(object sender, EventArgs e)
        {
            PickerApprover.SelectedIndex = -1;
            if (PickerDept.SelectedItem != null)
            {
                string dept0 = PickerDept.SelectedItem.ToString();
                string dept1 = dept0.Substring(0, dept0.IndexOf(" - "));
                string url = "" + Address.Api + "/ExitPermit/GetApproverEP?Dept=" + dept1 + "&Plant=" + PickerPlant.SelectedItem.ToString() + "";
                try
                {
                    popupLoadingView.IsVisible = true;
                    activityIndicator.IsRunning = true;
                    lblLoadingText.Text = "Loading Approver list...";
                    List<ApproverEPList> Approval = new List<ApproverEPList>();
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        Approval.Clear();
                        var content = await response.Content.ReadAsStringAsync();
                        Approval = JsonConvert.DeserializeObject<List<ApproverEPList>>(content);
                        _itmList2 = new Dictionary<string, string>();
                        _itmList2.Clear();
                        foreach (var approve in Approval)
                        {
                            _itmList2.Add(approve.UserId, approve.UserId + " - " + approve.Username);
                        }
                        PickerApprover.ItemsSource = _itmList2.Values.ToList();
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        lblLoadingText.Text = "";
                        PickerApprover.SelectedIndex = 0;
                    }
                    else
                    {
                        ThrowException(response.StatusCode.ToString(), "GetApprover");
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        lblLoadingText.Text = "";
                        return;
                    }
                }
                catch (Exception msg)
                {
                    ThrowException(msg.Message.ToString(), "GetApprover");
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                    return;
                }
            }
        }
        public async Task<bool> GetUser(string UseId)
        {
            string url = "" + Address.Api + "/ExitPermit/GetUserEP?UseID=" + UseId + "";
            try
            {
                isValid = false;
                popupLoadingView.IsVisible = true;
                activityIndicator.IsRunning = true;
                lblLoadingText.Text = "Finding User...";
                List<RequesterEPList> Requester = new List<RequesterEPList>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    Requester.Clear();
                    string UseIds = "";
                    string UseNams = "";
                    var content = await response.Content.ReadAsStringAsync();
                    Requester = JsonConvert.DeserializeObject<List<RequesterEPList>>(content);
                    foreach (var approve in Requester)
                    {
                        UseIds = approve.UseId.ToString();
                        UseNams = approve.UseNam.ToString();
                    }
                    if (UseIds == "Not Found")
                    {
                        isValid = false;
                    }
                    else
                    {
                        isValid = true;
                    }
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                    return isValid;
                }
                else
                {
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    lblLoadingText.Text = "";
                    ThrowException(response.ToString(), "GetUser");
                    isValid = false;
                    return isValid;
                }

            }
            catch (Exception msg)
            {
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                lblLoadingText.Text = "";
                ThrowException(msg.ToString(), "GetUser");
                isValid = false;
                return isValid;
            }
        }
        private void SegmentedControlView_SelectedTabIndexChanged(object sender, SelectedTabIndexEventArgs e)
        {
            if (e.SelectedTabIndex == 0)
            {
                ContentView1.IsVisible = true;
                ContentView2.IsVisible = false;
            }
            if (e.SelectedTabIndex == 1)
            {
                ContentView1.IsVisible = false;
                ContentView2.IsVisible = true;
            }
        }
        public async void Submit_OnClicked(object sender, EventArgs e)
        {
            try
            {
                popupLoadingView.IsVisible = true;
                activityIndicator.IsRunning = true;
                var httpClient = new HttpClient();
                var url = "" + Address.Api + "/ExitPermit/PostEP";
                string dept0 = PickerDept.SelectedItem.ToString();
                string dept1 = dept0.Substring(0, dept0.IndexOf(" - "));
                string plant = PickerPlant.SelectedItem.ToString().Substring(0, 4);
                string approver0 = PickerApprover.SelectedItem.ToString();
                string approver1 = approver0.Substring(0, approver0.IndexOf(" - "));
                string Out0 = DateTime.Today.Add(txtTimeOut.Time).ToString("HH:mm");
                string In0 = DateTime.Today.Add(txtTimeIn.Time).ToString("HH:mm");
                string OTH0 = DateTime.Today.Add(OtherCompTrans.Time).ToString("HH:mm");
                string UseId = GetValue("UseID");
                var DateTimeNow = DateTime.Now;
                var DateTimeApplyOut = txtDate.Date + txtTimeOut.Time;
                var DateTimeApplyIn = txtDate.Date + txtTimeIn.Time;
                string CompTransTimeValue;

                if (CompTrans.SelectedIndex == -1)
                {
                    DependencyService.Get<IMessage>().ShortAlert("Please choose transport time");
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    return;
                }
                var CompTransValue = CompTrans.SelectedItem.ToString();
                if (CompTransValue == "No")
                {
                    CompTransTimeValue = "";
                }
                else
                {
                    if (CompTransTime.SelectedIndex != -1)
                    {
                        if (CompTransTime.SelectedItem.ToString() == "OTHERS")
                        {
                            CompTransTimeValue = DateTime.Today.Add(OtherCompTrans.Time).ToString("HH:mm");
                        }
                        else
                        {
                            CompTransTimeValue = CompTransTime.SelectedItem.ToString();
                        }
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Please choose transport time");
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        return;
                    }
                }

                var data = new
                {
                    UseDep = dept1,
                    PlantID = plant,
                    Out = Out0,
                    In = In0,
                    Remarks = txtRemarks.Text,
                    ActOut = "",
                    ActIn = "",
                    Date = txtDate.Date.ToString("yyyy-MM-dd"),
                    CompTrans = CompTransValue,
                    CompTransTime = CompTransTimeValue,
                    OTH = OTH0,
                    Status = "PENDING",
                    Approver = approver1,
                    Destination = txtDestination.Text,
                    UseId = UseId,
                    EPDetailss = items
                };

                List<ModelExitPermitMessage> model = new List<ModelExitPermitMessage>();
                var jsonData = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url, jsonData);
                if (response.IsSuccessStatusCode)
                {
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    var content = await response.Content.ReadAsStringAsync();

                    RespondApi model2 = JsonConvert.DeserializeObject<RespondApi>(content);

                    if (model2.Success == true)
                    {
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                        //await PopupNavigation.Instance.PushAsync(new ExitPermitFeedbackPage(model2.Message.ToString(), model2.Success));
                        ClearData();
                    }
                    else
                    {
                        popupLoadingView.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        DependencyService.Get<IMessage>().ShortAlert(model2.Message.ToString());
                        //await PopupNavigation.Instance.PushAsync(new ExitPermitFeedbackPage(model2.Message.ToString(), model2.Success));
                    }
                }
                else
                {
                    popupLoadingView.IsVisible = false;
                    activityIndicator.IsRunning = false;
                    DependencyService.Get<IMessage>().ShortAlert(response.StatusCode.ToString());
                }
            }
            catch (Exception msg)
            {
                popupLoadingView.IsVisible = false;
                activityIndicator.IsRunning = false;
                DependencyService.Get<IMessage>().ShortAlert(msg.Message.ToString());
            }
        }
        void ClearData()
        {
            PickerPlant.SelectedIndex = -1;
            PickerDept.SelectedIndex = -1;
            PickerApprover.SelectedIndex = -1;
            txtDestination.Text = "";
            txtRemarks.Text = "";
            CompTrans.SelectedIndex = -1;

            RepeaterItem listitem = (from itm in items
                                     select itm)
                            .FirstOrDefault<RepeaterItem>();
            items.Remove(listitem);
        }
        void CheckData()
        {
            if (PickerPlant.SelectedIndex == -1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Please choose your Plant");
            }
            else if (PickerDept.SelectedIndex == -1)
            {
                DependencyService.Get<IMessage>().ShortAlert("Please choose your Departement");
            }
            else if (txtDestination.Text == "" || txtDestination.Text.Length < 2)
            {
                DependencyService.Get<IMessage>().ShortAlert("Please enter your destination");
            }
            else if (Convert.ToDateTime(txtTimeOut) > DateTime.Now)
            {
                DependencyService.Get<IMessage>().ShortAlert("You cannot enter passed time");
            }
        }
        public async void ThrowException(string message, string function)
        {
            var action = await DisplayAlert("Warning", message.ToString(), "Report", "Dismiss");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"ReportPage");
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