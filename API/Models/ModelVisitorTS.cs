using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Models
{
    public class ModelVisitorTS
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string EmployeeNo { get; set; }
        public string Department { get; set; }
        public string ShimanoBadge { get; set; }
        public string Plant { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Photoes { get; set; }
        public string CreUser { get; set; }
        public string CreDate { get; set; }
        public string ChgUser { get; set; }
        public string ChgDate { get; set; }
    }

    public class ModelVisitorTS_WIFI
    {
        public string NAME { get; set; }
        public string EMAIL { get; set; }
        public string BUSFUNC { get; set; }
        public string TYPE { get; set; }
    }
}