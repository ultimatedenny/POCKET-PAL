using System;
using System.Collections.Generic;
using POCKETPAL.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using System.Linq;
using Rg.Plugins.Popup.Services;
using POCKETPAL.Classes;
using System.Threading.Tasks;

namespace POCKETPAL.Views
{
    [DesignTimeVisible(false)]

    public partial class ErecruitmentPage : ContentPage
    {
        Random rnd = new Random();
        string UseID;
        string BatchComp, BunsFunc;
        //private OfflinePage _offlinepages;
        Dictionary<string, string> _itmList;
        //private DoesntAllowedPage _DoesntAllowedPage;
        private ObservableCollection<BatchList> items = new ObservableCollection<BatchList>();
        public ErecruitmentPage()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    GetStatus();
                    GetBatch();
                    UseID = GetValue("UseID");
                    BunsFunc = GetValue("BunsFunc");
                    this.BindingContext = this;
                });
            });
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Navigation.RemovePage(this);
        }
        private async void OnSearch(object sender, EventArgs e)
        {
            string X = "";
            if (BatchPicker.SelectedItem == null)
            {
                X = "";
            }
            else
            {
                X = BatchPicker.SelectedItem.ToString();
            }
            string url = "" + Address.Api + "/Erecruitment/GetBatchList?Status=" + X + "&BatchComp=" + BatchComp_TextBox.Text + "";
            try
            {
                CollectionViewBatch.ItemsSource = null;
                List<BatchList> model = new List<BatchList>();
                List<BatchList> model2 = new List<BatchList>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    model2.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<List<BatchList>>(content);
                }
                foreach (var result in model)
                {
                    if (string.IsNullOrEmpty(result.StatusBatch) || result.StatusBatch == "PENDING")
                    {
                        model2.Add(new BatchList()
                        {
                            BatchComp = result.BatchComp.ToString(),
                            InvitationDate = result.InvitationDate.ToString(),
                            RequestDate = result.RequestDate.ToString(),
                            TotalCandidate = result.TotalCandidate.ToString(),
                            StatusBatch = result.StatusBatch.ToString(),
                            ColorStatus = "#8e24aa"
                        });
                    }
                    else if (result.StatusBatch == "ON-GOING")
                    {
                        model2.Add(new BatchList()
                        {
                            BatchComp = result.BatchComp.ToString(),
                            InvitationDate = result.InvitationDate.ToString(),
                            RequestDate = result.RequestDate.ToString(),
                            TotalCandidate = result.TotalCandidate.ToString(),
                            StatusBatch = result.StatusBatch.ToString(),
                            ColorStatus = "#fdd835"
                        });
                    }
                    else if (result.StatusBatch == "FINISHED")
                    {
                        model2.Add(new BatchList()
                        {
                            BatchComp = result.BatchComp.ToString(),
                            InvitationDate = result.InvitationDate.ToString(),
                            RequestDate = result.RequestDate.ToString(),
                            TotalCandidate = result.TotalCandidate.ToString(),
                            StatusBatch = result.StatusBatch.ToString(),
                            ColorStatus = "#43a047"
                        });
                    }
                    else if (result.StatusBatch == "CLOSED")
                    {
                        model2.Add(new BatchList()
                        {
                            BatchComp = result.BatchComp.ToString(),
                            InvitationDate = result.InvitationDate.ToString(),
                            RequestDate = result.RequestDate.ToString(),
                            TotalCandidate = result.TotalCandidate.ToString(),
                            StatusBatch = result.StatusBatch.ToString(),
                            ColorStatus = "#43a047"
                        });
                    }
                }
                CollectionViewBatch.ItemsSource = model2;
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "OnSearch");
            }
        }
        public async void GetStatus()
        {
            string url = "" + Address.Api + "/Erecruitment/GetBatchStatus";
            try
            {
                List<BatchList> batchLists = new List<BatchList>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();

                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    batchLists.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    batchLists = JsonConvert.DeserializeObject<List<BatchList>>(content);
                }
                _itmList = new Dictionary<string, string>();
                foreach (var status in batchLists)
                {
                    _itmList.Add(status.StatusBatch, status.StatusBatch);
                }
                BatchPicker.ItemsSource = _itmList.Values.ToList();
                BatchPicker.SelectedIndex = -1;
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "GetStatus");
            }
        }
        public async void GetBatch()
        {
            string url = "" + Address.Api + "/Erecruitment/GetBatchList?Status=&BatchComp=";
            try
            {
                CollectionViewBatch.ItemsSource = null;
                List<BatchList> model = new List<BatchList>();
                List<BatchList> model2 = new List<BatchList>();
                HttpResponseMessage response = new HttpResponseMessage();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    model2.Clear();
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<List<BatchList>>(content);
                }
                foreach (var result in model)
                {
                    if (string.IsNullOrEmpty(result.StatusBatch) || result.StatusBatch == "PENDING")
                    {
                        model2.Add(new BatchList()
                        {
                            BatchComp = result.BatchComp.ToString(),
                            InvitationDate = result.InvitationDate.ToString(),
                            RequestDate = result.RequestDate.ToString(),
                            TotalCandidate = result.TotalCandidate.ToString(),
                            StatusBatch = result.StatusBatch.ToString(),
                            ColorStatus = "#8e24aa"
                        });
                    }
                    else if (result.StatusBatch == "ON-GOING")
                    {
                        model2.Add(new BatchList()
                        {
                            BatchComp = result.BatchComp.ToString(),
                            InvitationDate = result.InvitationDate.ToString(),
                            RequestDate = result.RequestDate.ToString(),
                            TotalCandidate = result.TotalCandidate.ToString(),
                            StatusBatch = result.StatusBatch.ToString(),
                            ColorStatus = "#fdd835"
                        });
                    }
                    else if (result.StatusBatch == "FINISHED")
                    {
                        model2.Add(new BatchList()
                        {
                            BatchComp = result.BatchComp.ToString(),
                            InvitationDate = result.InvitationDate.ToString(),
                            RequestDate = result.RequestDate.ToString(),
                            TotalCandidate = result.TotalCandidate.ToString(),
                            StatusBatch = result.StatusBatch.ToString(),
                            ColorStatus = "#43a047"
                        });
                    }
                    else if (result.StatusBatch == "CLOSED")
                    {
                        model2.Add(new BatchList()
                        {
                            BatchComp = result.BatchComp.ToString(),
                            InvitationDate = result.InvitationDate.ToString(),
                            RequestDate = result.RequestDate.ToString(),
                            TotalCandidate = result.TotalCandidate.ToString(),
                            StatusBatch = result.StatusBatch.ToString(),
                            ColorStatus = "#43a047"
                        });
                    }

                }
                CollectionViewBatch.ItemsSource = model2;
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "GetBatch");
            }
        }
        public async void ConfirmClick(object sender, EventArgs e)
        {
            if (BunsFunc == "SYSTEM-ADMIN" || BunsFunc == "SYSTEM-MANAGER")
            {
                var item = (Xamarin.Forms.Button)sender;
                var action = await DisplayAlert("Confirm Batch", "Are you going to approve Batch: " + item.CommandParameter.ToString() + "", "Yes", "No");
                if (action == true)
                {
                    string url = "" + Address.Api + "/Erecruitment/UpdateBatch?Status=FINISHED&Batch=" + item.CommandParameter.ToString() + "&UseId=" + UseID + "";
                    try
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        HttpClient client = new HttpClient();
                        response = await client.GetAsync(url);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            GetBatch();
                        }
                        else
                        {
                            ThrowException(response.StatusCode.ToString(), "ConfirmClick");
                        }
                    }
                    catch (Exception msg)
                    {
                        ThrowException(msg.Message.ToString(), "ConfirmClick");
                    }
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("You are not allowed for this action");
            }
        }
        public void DetailClicked(object sender, EventArgs e)
        {
            var item = (Button)sender;
            string _item = item.CommandParameter.ToString();
            string _BatchComp = _item.Substring(0, 11);
            string _BatchStatus = _item.Substring(12);
            string a = UseID.ToUpper();
            if (a == "SPLB0021" || a == "SBM_DENI" || a == "SBM_FIN180093")
            {
                AddValue("BatchComp", _BatchComp);
                AddValue("BatchStatus", _BatchStatus);
                Shell.Current.GoToAsync($"ErecruitmentPageDetail");
            }
            else
            {
                DisplayAlert("Pocket-Pal", "Unfortunately, you don't have permission.", "Ok", " ");
            }
        }


        public void RefBatch(object sender, EventArgs e)
        {
            GetBatch();
            RefreshBatch.IsRefreshing = false;
        }
        public void AddValue(string key, string value)
        {
            Preferences.Set(key, value);
        }
        public string GetValue(string key)
        {
            return Preferences.Get(key, "");
        }
        public async void ThrowException(string message, string function)
        {
            var action = await DisplayAlert("Warning", message.ToString(), "Report", "Dismiss");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"ReportPage");
            }
        }
        public void DetailsInvoked(object sender, EventArgs e)
        {
            var item = (SwipeItemView)sender;
            string _item = item.CommandParameter.ToString();
            string _BatchComp = _item.Substring(0, 11);
            string _BatchStatus = _item.Substring(12);
            string USEID = UseID.ToUpper();
            if (USEID == "SPLB0021" || USEID == "SBM_DENI" || USEID == "SBM_FIN180093" || USEID == "SBM_HR210474")
            {
                AddValue("BatchComp", _BatchComp);
                AddValue("BatchStatus", _BatchStatus);
                Shell.Current.GoToAsync($"ErecruitmentPageDetail");
            }
            else
            {
                DisplayAlert("Pocket-Pal", "Unfortunately, you don't have permission.", "Ok", " ");
            }
        }
        public async void ConfirmInvoked(object sender, EventArgs e)
        {
            var item = (SwipeItemView)sender;
            string _item = item.CommandParameter.ToString();
            string USEID = UseID.ToUpper();
            if (USEID == "SPLB0021" || USEID == "SBM_DENI" || USEID == "SBM_FIN180093" || USEID == "SBM_HR210474")
            {
                var action = await DisplayAlert("Confirm Batch", "Are you going to approve Batch: " + _item + "", "Yes", "No");
                if (action == true)
                {
                    string url = "" + Address.Api + "/Erecruitment/UpdateBatch?Status=FINISHED&Batch=" + _item + "&UseId=" + UseID + "";
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        GetBatch();
                    }
                    else
                    {
                        ThrowException(response.StatusCode.ToString(), "ConfirmInvoked");
                    }
                }
                else
                {
                    await DisplayAlert("Pocket-Pal", "Unfortunately, you don't have permission.", "Ok", " ");
                }
            }
            else
            {
                await DisplayAlert("Pocket-Pal", "Unfortunately, you don't have permission.", "Ok", " ");
            }
        }
    }
}

