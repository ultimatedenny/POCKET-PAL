using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Models
{
    public class ModelVisitorTSLog
    {
        public int LogId { get; set; }
        public int TSVisitorId { get; set; }
        public string HostName { get; set; }
        public string HostDepartment { get; set; }
        public string TimeIn { get; set; }
        public string TimeOut { get; set; }
        public string Plant { get; set; }
        public string DateVisit { get; set; }
        public bool NeedLunch { get; set; }
        public string Status { get; set; }
        public bool NeedStay { get; set; }
        public string DateOfEnd { get; set; }
    }
}