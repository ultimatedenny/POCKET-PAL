using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using POCKETPAL.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Collections.ObjectModel;
using POCKETPAL.Classes;

namespace POCKETPAL.Views
{
    [DesignTimeVisible(false)]
    public partial class ErecruitmentPageDetail : ContentPage
    {
        Random rnd = new Random();
        string BatchComp, BatchStatus, TempItem, BunsFunc, UseID;
        //private UnderConstructionPage _underconstructionpage;
        private ObservableCollection<BatchList> items = new ObservableCollection<BatchList>();
        public ErecruitmentPageDetail()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            await Task.Run(() =>
            {
                Task.Delay(100);
                Device.BeginInvokeOnMainThread(() =>
                {
                    BatchComp = GetValue("BatchComp");
                    BatchStatus = GetValue("BatchStatus");
                    BunsFunc = GetValue("BunsFunc");
                    UseID = GetValue("UseID");
                    this.BindingContext = this;
                    GetCandidate();
                });
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
        public async void GetCandidate()
        {
            string url = "" + Address.Api + "/Erecruitment/GetCandidateList?BatchComp=" + BatchComp + "";
            try
            {
                CollectionViewEmp.ItemsSource = null;
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
                    foreach (var result in model)
                    {
                        if (string.IsNullOrEmpty(result.StatusEmp) || string.IsNullOrEmpty(result.StatusEmp))
                        {
                            model2.Add(new BatchList()
                            {
                                BatchEmp = result.BatchEmp.ToString(),
                                NameEmp = result.NameEmp.ToString(),
                                PhoneNumber = result.PhoneNumber.ToString(),
                                DateOfBirthEmp = result.DateOfBirthEmp.ToString(),
                                InvitationCodeEmp = result.InvitationCodeEmp.ToString(),
                                StatusEmp = result.StatusEmp.ToString(),
                                StatusEmpText = "UNKNOWN",
                                ColorStatus = "#000000"
                            });
                        }
                        else
                        {
                            result.StatusEmpText = result.StatusEmp;
                            if (result.StatusEmpText == "PRESENT")
                            {
                                model2.Add(new BatchList()
                                {
                                    BatchEmp = result.BatchEmp.ToString(),
                                    NameEmp = result.NameEmp.ToString(),
                                    PhoneNumber = result.PhoneNumber.ToString(),
                                    DateOfBirthEmp = result.DateOfBirthEmp.ToString(),
                                    InvitationCodeEmp = result.InvitationCodeEmp.ToString(),
                                    StatusEmp = result.StatusEmp.ToString(),
                                    ColorStatus = "#43a047"
                                });
                            }
                            else if (result.StatusEmpText == "ABSENT")
                            {
                                model2.Add(new BatchList()
                                {
                                    BatchEmp = result.BatchEmp.ToString(),
                                    NameEmp = result.NameEmp.ToString(),
                                    PhoneNumber = result.PhoneNumber.ToString(),
                                    DateOfBirthEmp = result.DateOfBirthEmp.ToString(),
                                    InvitationCodeEmp = result.InvitationCodeEmp.ToString(),
                                    StatusEmp = result.StatusEmp.ToString(),
                                    ColorStatus = "#e53935"
                                });
                            }
                            else
                            {
                                model2.Add(new BatchList()
                                {
                                    BatchEmp = result.BatchEmp.ToString(),
                                    NameEmp = result.NameEmp.ToString(),
                                    PhoneNumber = result.PhoneNumber.ToString(),
                                    DateOfBirthEmp = result.DateOfBirthEmp.ToString(),
                                    InvitationCodeEmp = result.InvitationCodeEmp.ToString(),
                                    StatusEmp = result.StatusEmp.ToString(),
                                    ColorStatus = "#000000"
                                });
                            }
                        }
                    }
                    CollectionViewEmp.ItemsSource = model2;
                    ObservableCollection<BatchList> items = new ObservableCollection<BatchList>();
                }
                else
                {
                    ThrowException(response.StatusCode.ToString(), "GetCandidate");
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "GetCandidate");
            }
        }
        public async void OnSearch(object sender, EventArgs e)
        {
            string url = "" + Address.Api + "/Erecruitment/GetEmpListBatch?BatchComp=" + BatchComp + "&NameEmp=" + SearchName.Text + "";
            try
            {
                CollectionViewEmp.ItemsSource = null;
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
                    foreach (var result in model)
                    {
                        if (string.IsNullOrEmpty(result.StatusEmp) || string.IsNullOrEmpty(result.StatusEmp))
                        {
                            model2.Add(new BatchList()
                            {
                                BatchEmp = result.BatchEmp.ToString(),
                                NameEmp = result.NameEmp.ToString(),
                                PhoneNumber = result.PhoneNumber.ToString(),
                                DateOfBirthEmp = result.DateOfBirthEmp.ToString(),
                                InvitationCodeEmp = result.InvitationCodeEmp.ToString(),
                                StatusEmp = result.StatusEmp.ToString(),
                                StatusEmpText = "UNKNOWN",
                                ColorStatus = "#000000"
                            });
                        }
                        else
                        {
                            result.StatusEmpText = result.StatusEmp;
                            if (result.StatusEmpText == "PRESENT")
                            {
                                model2.Add(new BatchList()
                                {
                                    BatchEmp = result.BatchEmp.ToString(),
                                    NameEmp = result.NameEmp.ToString(),
                                    PhoneNumber = result.PhoneNumber.ToString(),
                                    DateOfBirthEmp = result.DateOfBirthEmp.ToString(),
                                    InvitationCodeEmp = result.InvitationCodeEmp.ToString(),
                                    StatusEmp = result.StatusEmp.ToString(),
                                    ColorStatus = "#43a047"
                                });
                            }
                            else if (result.StatusEmpText == "ABSENT")
                            {
                                model2.Add(new BatchList()
                                {
                                    BatchEmp = result.BatchEmp.ToString(),
                                    NameEmp = result.NameEmp.ToString(),
                                    PhoneNumber = result.PhoneNumber.ToString(),
                                    DateOfBirthEmp = result.DateOfBirthEmp.ToString(),
                                    InvitationCodeEmp = result.InvitationCodeEmp.ToString(),
                                    StatusEmp = result.StatusEmp.ToString(),
                                    ColorStatus = "#e53935"
                                });
                            }
                            else
                            {
                                model2.Add(new BatchList()
                                {
                                    BatchEmp = result.BatchEmp.ToString(),
                                    NameEmp = result.NameEmp.ToString(),
                                    PhoneNumber = result.PhoneNumber.ToString(),
                                    DateOfBirthEmp = result.DateOfBirthEmp.ToString(),
                                    InvitationCodeEmp = result.InvitationCodeEmp.ToString(),
                                    StatusEmp = result.StatusEmp.ToString(),
                                    ColorStatus = "#000000"
                                });
                            }
                        }
                    }
                    CollectionViewEmp.ItemsSource = model2;
                    ObservableCollection<BatchList> items = new ObservableCollection<BatchList>();
                }
                else
                {
                    ThrowException(response.StatusCode.ToString(), "OnSearch");
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "OnSearch");
            }
        }
        public async void AttendClick(object sender, EventArgs e)
        {
            if (BatchStatus == "PENDING")
            {
                BatchStatus = "ON-GOING";
                string url = "" + Address.Api + "/Erecruitment/UpdateBatch?Status=" + BatchStatus + "&Batch=" + BatchComp + "&UseId=" + UseID + "";
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var item = (Xamarin.Forms.Button)sender;
                        string _item = item.CommandParameter.ToString();
                        UpdateCandidate(_item);
                    }
                    else
                    {
                        ThrowException(response.StatusCode.ToString(), "AttendClick");
                    }
                }
                catch (Exception msg)
                {
                    ThrowException(msg.Message.ToString(), "AttendClick");
                }
            }
            else if (BatchStatus == "ON-GOING")
            {
                var item = (Xamarin.Forms.Button)sender;
                string _item = item.CommandParameter.ToString();
                UpdateCandidate(_item);
            }
            else if (BatchStatus == "FINISHED")
            {
                DependencyService.Get<IMessage>().ShortAlert("This Batch has been marked to completed, please ask IT to open this Batch again");
            }
            else if (BatchStatus == "CLOSED")
            {
                DependencyService.Get<IMessage>().ShortAlert("This Batch has been marked to completed, please ask IT to open this Batch again");
            }
        }
        public async void ConfirmClick(object sender, EventArgs e)
        {
            string url = "" + Address.Api + "/Erecruitment/GetBatchInformation?BatchComp=" + BatchComp + "";
            try
            {
                HttpResponseMessage response = new HttpResponseMessage();
                List<BatchList> model = new List<BatchList>();
                HttpClient client = new HttpClient();
                response = await client.GetAsync(url);
                string a, b, c, d;
                if (response.IsSuccessStatusCode)
                {
                    model.Clear();
                    var content = await response.Content.ReadAsStringAsync();
                    model = JsonConvert.DeserializeObject<List<BatchList>>(content);
                }

                string TotalCandidate = model[0].TotalCandidate.ToString();
                string Present = model[0].Present.ToString();
                string Absent = model[0].Absent.ToString();
                string Invited = model[0].Invited.ToString();
                string NotInvited = model[0].NotInvited.ToString();
                string Percent = model[0].Percentage.ToString();


                var action = await DisplayAlert("Complete Batch", "" +
                    "Batch: " + BatchComp + "\n" +
                    "Total Candidate: " + TotalCandidate + "\n" +
                    "Present: " + Present + "\n" +
                    "Absent: " + Absent + "\n" +
                    "Invited: " + Invited + "\n" +
                    "Not Invited: " + NotInvited + "\n" +
                    "Percentage: " + Percent + "\n" +
                    "", "Yes", "No");
                if (action)
                {
                    string url2 = "" + Address.Api + "/Erecruitment/UpdateBatch?Status=FINISHED&Batch=" + BatchComp + "&UseId=" + UseID + "";
                    try
                    {
                        if (BatchStatus == "FINISHED")
                        {
                            DependencyService.Get<IMessage>().ShortAlert("This Batch has been marked to completed, please ask IT to open this Batch again");
                        }
                        else
                        {
                            HttpResponseMessage response2 = new HttpResponseMessage();
                            HttpClient client2 = new HttpClient();
                            response2 = await client2.GetAsync(url2);
                            if (response.IsSuccessStatusCode)
                            {
                                var content2 = await response2.Content.ReadAsStringAsync();
                                DependencyService.Get<IMessage>().ShortAlert("Batch updated to success");
                                //App.Current.MainPage = new NavigationPage(new HomePage());

                                await Task.Delay(500);
                                if (Device.RuntimePlatform == Device.Android)
                                {
                                    App.Current.MainPage = new Xamarin.Forms.NavigationPage(new HomePage());
                                }
                                else if (Device.RuntimePlatform == Device.iOS)
                                {
                                    await Navigation.PushAsync(new HomePage());
                                }
                            }
                            else
                            {
                                ThrowException(response.StatusCode.ToString(), "ConfirmClick");
                            }
                        }
                    }
                    catch (Exception msg)
                    {
                        ThrowException(msg.Message.ToString(), "ConfirmClick");
                    }
                }
                else
                {
                    DependencyService.Get<IMessage>().ShortAlert("Update Canceled");
                }
            }
            catch (Exception msg)
            {
                ThrowException(msg.Message.ToString(), "ConfirmClick");
            }
        }
        public async void UpdateCandidate(string _item)
        {
            TempItem = _item;
            string _BatchEmp = _item.Substring(0, 15);
            string _Status = _item.Substring(16);
            if (_Status == "UNKNOWN" || _Status == "INVITED" || _Status == "NOT INVITED")
            {
                string url = "" + Address.Api + "/Erecruitment/UpdateEmployee?Status=PRESENT&BatchEmp=" + _BatchEmp + "&UseId=" + UseID + "";
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        GetCandidate();
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Update Canceled");
                    }
                }
                catch (Exception msg)
                {
                    ThrowException(msg.Message.ToString(), "UpdateCandidate");
                }
            }
            else if (_Status == "PRESENT")
            {
                string url = "" + Address.Api + "/Erecruitment/UpdateEmployee?Status=ABSENT&BatchEmp=" + _BatchEmp + "&UseId=" + UseID + "";
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        GetCandidate();
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Update Canceled");
                    }
                }
                catch (Exception msg)
                {
                    ThrowException(msg.Message.ToString(), "UpdateCandidate");
                }
            }
            else if (_Status == "ABSENT")
            {
                string url = "" + Address.Api + "/Erecruitment/UpdateEmployee?Status=PRESENT&BatchEmp=" + _BatchEmp + "&UseId=" + UseID + "";
                try
                {
                    HttpResponseMessage response = new HttpResponseMessage();
                    HttpClient client = new HttpClient();
                    response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        GetCandidate();
                    }
                    else
                    {
                        DependencyService.Get<IMessage>().ShortAlert("Update Canceled");
                    }
                }
                catch (Exception msg)
                {
                    ThrowException(msg.Message.ToString(), "UpdateCandidate");
                }
            }
            else
            {
                DependencyService.Get<IMessage>().ShortAlert("Update Cancled");
            }
            GetCandidate();
        }
        public async void ThrowException(string message, string function)
        {
            var action = await DisplayAlert("Warning", message.ToString(), "Report", "Dismiss");
            if (action == true)
            {
                await Shell.Current.GoToAsync($"ReportPage");
            }
        }
        public void RefEmp(object sender, EventArgs e)
        {
            GetCandidate();
            RefreshEmp.IsRefreshing = false;
        }
    }
}
