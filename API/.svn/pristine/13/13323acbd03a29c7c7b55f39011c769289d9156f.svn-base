using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Models
{
    public class GlobalResult
    {
        [JsonProperty("status")]
        public long Status { get; set; }

        [JsonProperty("status_str")]
        public string StatusStr { get; set; }

        [JsonProperty("UIDARUBA")]
        public string Uidaruba { get; set; }

        [JsonProperty("X-CSRF-Token")]
        public string XCsrfToken { get; set; }
    }
    public class UserdbGuestAdd
    {
        [JsonProperty("user-role")]
        public string UserRole { get; set; }

        [JsonProperty("user-email")]
        public string UserEmail { get; set; }
        public bool mode { get; set; }
        public string g_fullname { get; set; }
        public string g_company { get; set; }
        public string g_phone { get; set; }
        public string g_comments { get; set; }
        public string sp_name { get; set; }
        public string sp_email { get; set; }
        public string sp_fullname { get; set; }
        public string sp_dept { get; set; }
        public string opt1 { get; set; }
        public string opt2 { get; set; }
        public string opt3 { get; set; }
        public string opt4 { get; set; }
        public string start_day { get; set; }
        public string start_time { get; set; }
        public bool expiry { get; set; }
        public string expmins { get; set; }
        public string expday { get; set; }
        public string exptime { get; set; }
        public string remote_ip { get; set; }
        public string name { get; set; }
        public string passwd { get; set; }
        public Result _result { get; set; }
    }
    public class Root
    {
        public UserdbGuestAdd userdb_add { get; set; }
        public GlobalResult _global_result { get; set; }
    }
    public class Root_SSID
    {
        public Root Data { get; set; }
        public string Message { get; set; }
        public string Success { get; set; }
    }
    public class Result
    {
        public string _Result { get; set; }
    }
}