using POCKETPAL_API.Class;
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net;
using System.Text;

namespace POCKETPAL_API.Models
{
    public class ClassNotification
    {
        public string ConnectionEApproval = ConfigurationManager.ConnectionStrings["DBTOKEN"].ToString();
        public string DBCONNECTION = ConfigurationManager.ConnectionStrings["POCKETPALCON"].ToString();
        public string GetUserApprovalEP(Guid MasterId, string EPNo_)
        {
            string query = @"exec [PP_POST_EPGETTOKEN] '" + MasterId + "'";
            var sql = new ClassMSSQL();
            var GetUseID = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            string No = EPNo_;

            if (GetUseID != null || GetUseID.Rows.Count > 0)
            {
                foreach (DataRow item1 in GetUseID.Rows)
                {
                    string UseId = item1["USEID"].ToString();
                    string UseNam = item1["USENAM"].ToString();
                    GetUserToken(No, UseId, UseNam);
                }
            }
            return null;
        }
        public string GetUserToken(string No, string UseId, string UseNam)
        {
            var deviceId = "";
            var title = "";
            var body = "";

            title = "New Exit Permit";
            body = "Dear " + UseNam + "," + "\nYou have a pending Exit Permit Approval for " + No;

            var click_action = "";
            string query = @"select Token from tblToken where useid = '" + UseId + "'";
            var sql = new ClassMSSQL();
            var GetToken = sql.ExecDTQuery(ConnectionEApproval, query, null, null, false);
            if (GetToken != null || GetToken.Rows.Count > 0)
            {
                foreach (DataRow TokenId in GetToken.Rows)
                {
                    deviceId = TokenId["Token"].ToString();
                    SendNotificationJSON(deviceId, title, body, click_action);
                }
            }
            return null;
        }
        public string SendNotificationJSON(string deviceId, string title, string body, string click_action)
        {
            string SERVER_KEY_TOKEN = ConfigurationManager.AppSettings["FCMServerToken"].ToString();
            var SENDER_ID = ConfigurationManager.AppSettings["FCMSenderID"];

            WebProxy proxy = new WebProxy("http://172.18.100.54:80", true);
            proxy.Credentials = new NetworkCredential("sbm_admindino", "ojolali");
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

            byte[] byteArray = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(a));
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
    }
}