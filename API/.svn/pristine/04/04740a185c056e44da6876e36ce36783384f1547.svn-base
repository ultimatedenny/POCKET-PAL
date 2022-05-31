using Newtonsoft.Json;
using POCKETPAL_API.Actions;
using POCKETPAL_API.Class;
using POCKETPAL_API.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace POCKETPAL_API.Controllers
{
    public class VisitorTSController : Controller
    {
        public string finalString;
        ActionVisitor visAct = new ActionVisitor();
        public ActionResult Index()
        {
            string APIName = WebConfigurationManager.AppSettings["APIName"];
            string APIVersion = WebConfigurationManager.AppSettings["APIVersion"];
            string Final = APIName + " - " + APIVersion;
            return Json(Final, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PP_ADD_TEAM_SHIMANO_VMS(string Name, string EmployeeNo, string Department, string ShimanoBadge, string Plant, string Email, string Phone, string Photoes)
        {
            ModelVisitorTS Data = new ModelVisitorTS
            {
                Name = Name,
                EmployeeNo = EmployeeNo,
                Department = Department,
                ShimanoBadge = ShimanoBadge,
                Plant = Plant,
                Email = Email,
                Phone = Phone,
                Photoes = Photoes,
                CreUser = "POCKET PAL API"
            };
            return Json(visAct.PP_ADD_TEAM_SHIMANO_VMS(Data), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PP_CHECKINTS_VMS(int TSVisitorId, string HostName, string HostDepartment, bool NeedLunch, string Plant, bool NeedStay, string DateOfEnd)
        {
            ModelVisitorTSLog Data = new ModelVisitorTSLog
            {
                LogId = 0,
                TSVisitorId = TSVisitorId,
                HostName = HostName,
                HostDepartment = HostDepartment,
                TimeOut = null,
                NeedLunch = NeedLunch,
                Status = "CHECKIN",
                Plant = Plant,
                NeedStay = NeedStay,
                DateOfEnd = DateOfEnd
            };
            return Json(visAct.PP_CHECKINTS_VMS(Data), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckOut(string LogId)
        {
            ModelVisitorTSLog NewData = new ModelVisitorTSLog
            {
                LogId   = Convert.ToInt32(LogId),
                TimeOut = DateTime.Now.ToString("HH:mm:ss"),
                Status  = "CHECKOUT"
            };
            return Json(visAct.PP_CHECKINTS_VMS(NewData), JsonRequestBehavior.AllowGet);
        }

        public ActionResult CheckInSameDay(ModelVisitorTSLog NewData, int TSVisitorId, string HostName, string HostDepartment, string TimeIn, string DateVisit, bool NeedLunch, string Status, string Plant, bool NeedStay, string DateOfEnd)
        {
            NewData.TSVisitorId = TSVisitorId;
            NewData.HostName = HostName;
            NewData.HostDepartment = HostDepartment;
            NewData.TimeIn = TimeIn;
            NewData.DateVisit = DateVisit;
            NewData.NeedLunch = NeedLunch;
            NewData.Status = Status;
            NewData.Plant = Plant;
            NewData.NeedStay = NeedStay;
            NewData.DateOfEnd = DateOfEnd;
            return Json(visAct.CheckInSameDay(NewData), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PP_GET_TS_ID_VMS(string UseNam)
        {
            return Json(visAct.PP_GET_TS_ID_VMS(UseNam), JsonRequestBehavior.AllowGet);
        }

        public JsonResult PP_GET_TS_STATUS_VMS(string TSVisitorId)
        {
            return Json(visAct.PP_GET_TS_STATUS_VMS(TSVisitorId), JsonRequestBehavior.AllowGet);
        }

        public ActionResult QRCheck(string Client, string UseID, string UseNam)
        {
            return Json(visAct.QRCheck(Client, UseID, UseNam), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PP_POST_QR_VMS()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var stringChars = new char[16];
            var random = new Random();
            string QRCODE = String.Empty;
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            string codeRAW = new String(stringChars);
            QRCODE = "QR-VMS" + codeRAW.ToUpper();
            return Json(visAct.PP_POST_QR_VMS(QRCODE), JsonRequestBehavior.AllowGet);
        }
        public ActionResult PP_GET_LOGIN_INFO_VMS(string QRCode)
        {
            return Json(visAct.PP_GET_LOGIN_INFO_VMS(QRCode), JsonRequestBehavior.AllowGet);
        }

        public ActionResult PP_DELETE_QR_VMS()
        {
            return Json(visAct.PP_DELETE_QR_VMS(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSSIDTempDuration()
        {
            return Json(visAct.GetSSIDTempDuration(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult PP_GET_PASSWORD_RECEPTIONIST(string PASSWORD)
        {
            return Json(visAct.PP_GET_PASSWORD_RECEPTIONIST(PASSWORD), JsonRequestBehavior.AllowGet);
        }
        //PP_GET_PASSWORD_RECEPTIONIST
        public async Task<ActionResult> LoginArubaTemp(string NAME, string EMAIL, string BUSFUNC, string TYPE, int CUSTOMHOUR)
        {
            try
            {
                if (!String.IsNullOrEmpty(NAME) || !String.IsNullOrEmpty(EMAIL) || !String.IsNullOrEmpty(BUSFUNC))
                {
                    if (BUSFUNC == "SYSTEM-VISITOR" || BUSFUNC == "SYSTEM-ADMIN")
                    {
                        var handler = new HttpClientHandler();
                        handler.ServerCertificateCustomValidationCallback = (requestMessage, certificate, chain, policyErrors) => true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        HttpClient client = new HttpClient(handler);

                        List<Root> model = new List<Root>();
                        string url = "https://172.18.102.208:4343/v1/api/login" + "?username=admin&password=inf0rm3dp";
                        HttpResponseMessage response = new HttpResponseMessage();
                        response = await client.GetAsync(url);

                        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        var stringChars = new char[3];
                        var random = new Random();
                        for (int i = 0; i < stringChars.Length; i++)
                        {
                            stringChars[i] = chars[random.Next(chars.Length)];
                        }

                        var regdate = "";
                        var regtime = "";
                        var enddate = regdate;
                        var endtime = "";
                        var finalString = new String(stringChars);
                        int digits = new Random().Next(100000, 999999);
                        string username = "G-" + finalString;
                        string password = digits.ToString();

                        if (TYPE == "0")
                        {
                            regdate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
                            regtime = DateTime.Now.ToString("HH:mm");
                            enddate = regdate;
                            endtime = DateTime.Now.AddMinutes(10).ToString("HH:mm");
                        }
                        else if (TYPE == "1")
                        {
                            regdate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
                            regtime = DateTime.Now.ToString("HH:mm");
                            enddate = regdate;
                            endtime = DateTime.Now.AddHours(8).ToString("HH:mm");
                        }
                        else if (TYPE == "2")
                        {
                            regdate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
                            regtime = DateTime.Now.ToString("HH:mm");
                            enddate = regdate;
                            endtime = DateTime.Now.AddHours(CUSTOMHOUR).ToString("HH:mm");
                        }
                        else if (TYPE == "3")
                        {
                            regdate = DateTime.Now.ToString("MM/dd/yyyy").Replace("-", "/");
                            regtime = DateTime.Now.ToString("HH:mm");
                            enddate = regdate;
                            endtime = "23:59";
                            EMAIL = "MAIL-" + finalString + "@gmail.com";
                        }

                        if (response.IsSuccessStatusCode)
                        {
                            var Check = visAct.CheckSSID(EMAIL);
                            if (Check.Success == true)
                            {
                                string Result = await response.Content.ReadAsStringAsync();
                                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                                Root sub = JsonConvert.DeserializeObject<Root>(Result);
                                string TokenAruba = sub._global_result.XCsrfToken.ToString();
                                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://172.18.102.208:4343/v1/configuration/object/userdb_add?config_path=%2Fmm"))
                                {
                                    request.Headers.TryAddWithoutValidation("Accept", "application/json");
                                    request.Headers.TryAddWithoutValidation("X-CSRF-Token", TokenAruba);
                                    request.Content = new StringContent("{ \"user-role\": \"Emp-IOT\", \"user-email\": \"" + EMAIL + "\", \"mode\": false, \"g_fullname\": \"" + NAME + "\", \"g_company\": \"SHIMANO BATAM\", \"g_phone\": \"087714444433\", \"g_comments\": \"\", \"sp_name\": \"POCKETPAL-API\", \"sp_email\": \"" + EMAIL + "\", \"sp_fullname\": \"POCKETPAL-API\", \"sp_dept\": \"IT\", \"opt1\": \"\", \"opt2\": \"\", \"opt3\": \"\", \"opt4\": \"\", \"start_day\": \"" + regdate + "\", \"start_time\": \"" + regtime + "\", \"expiry\": true, \"expmins\": \"\", \"expday\": \"" + enddate + "\", \"exptime\": \"" + endtime + "\", \"remote_ip\": \"\", \"name\": \"" + username + "\", \"passwd\": \"" + password + "\" }");
                                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                                    var response1 = await client.SendAsync(request);
                                    if (response1.IsSuccessStatusCode)
                                    {
                                        var Result2 = await response1.Content.ReadAsStringAsync();
                                        Root Result3 = JsonConvert.DeserializeObject<Root>(Result2);
                                        if (Result3._global_result.Status.ToString() == "0")
                                        {
                                            if ((visAct.PostSSID(Result3)).Success == true)
                                            {
                                                return Json(new { Data = Result3, Message = "Success", Success = true }, JsonRequestBehavior.AllowGet);
                                            }
                                            else
                                            {
                                                return Json(new { Data = "", Message = "-", Success = false }, JsonRequestBehavior.AllowGet);
                                            }
                                        }
                                        else
                                        {
                                            return Json(new { Data = "", Message = Result2.ToString(), Success = false }, JsonRequestBehavior.AllowGet);
                                        }
                                    }
                                    else
                                    {
                                        return Json(new { Data = "", Message = response1.IsSuccessStatusCode.ToString(), Success = false }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                            }
                            else
                            {
                                return Json(new { Data = "", Message = Check.Message.ToString(), Success = false }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        else
                        {
                            return Json(new { Data = "", Message = response.StatusCode.ToString(), Success = 0 }, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        return Json(new { Data = "", Message = "BUSFUNC not allowed", Success = false }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { Data = "", Message = "Invalid data", Success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { Data = "", Message = ex.Message.ToString(), Success = false }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}