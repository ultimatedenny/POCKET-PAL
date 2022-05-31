using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace POCKETPAL_API.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Notif(string deviceId)
        {
            //deviceId = "egIjNQCJQj-qYpusnmO-S8:APA91bG5scLvEuxYpG0YP5XffcPC9gRjZmf4mjif9k5__oIQhogSfqkcKAGTHKoXP5lDNOzj5G1lZlrDEeMVYNPyEKY-HNqUeQduXyDdYmsEEtRy08BgXLz54ca_U3NJx8wbwIQgm4nP";
            string title = "TEST NOTIF";
            string body = "MESSAGE";
            var click_action = "";
            return Json(TrySend(deviceId, title, body, click_action), JsonRequestBehavior.AllowGet);
        }
        public ActionResult GPLAYNOTIF(string Message, string deviceId)
        {
            string title = "GOOGLE PLAY NOTIF";
            string body = Message.ToString();
            var click_action = "";
            return Json(TrySend(deviceId, title, body, click_action), JsonRequestBehavior.AllowGet);
        }
        public ActionResult VDSNOTIF(string Category, string Message, string deviceId)
        {
            string title = "VDS NOTIFICATION";
            string body = "["+ Category + "] MESSAGE: "+ Message.ToString()+ "";
            var click_action = "";
            return Json(TrySend(deviceId, title, body, click_action), JsonRequestBehavior.AllowGet);
        }
        public ActionResult EPROCNOTIF(string title, string body, string deviceId)
        {
            var click_action = "";
            return Json(TrySend(deviceId, title, body, click_action), JsonRequestBehavior.AllowGet);
        }
        public string TrySend(string deviceId, string title, string body, string click_action)
        {
            try
            {
                string SERVER_KEY_TOKEN = ConfigurationManager.AppSettings["FCMServerToken"].ToString();
                string SENDER_ID = ConfigurationManager.AppSettings["FCMSenderID"];
                WebProxy proxy = new WebProxy("http://172.18.100.54:80", true);
                proxy.Credentials = new NetworkCredential("sbm_apps_ftp", "admin212");
                WebRequest.DefaultWebProxy = proxy;
                WebRequest tRequest;
                tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = " application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", SERVER_KEY_TOKEN));
                tRequest.Headers.Add(string.Format("Sender: id={0}", SENDER_ID));
                var a = new
                {
                    notification = new
                    {
                        title,
                        body,
                        icon = "https://domain/path/to/logo.png",
                        click_action,
                        sound = "mySound"
                    },
                    to = deviceId
                };

                byte[] byteArray = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(a));
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = tRequest.GetResponse();
                dataStream = tResponse.GetResponseStream();

                StreamReader tReader = new StreamReader(dataStream);
                string sResponseFromServer = tReader.ReadToEnd();

                tReader.Close();
                dataStream.Close();
                tResponse.Close();

                return sResponseFromServer;
            }
            catch(Exception msg)
            {
                return msg.Message.ToString();
            }
        }
    }
}