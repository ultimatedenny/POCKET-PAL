using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Models
{
    public class ModelAppClient
    {
        public string AppName { get; set; }
        public string AppVersion { get; set; }
        public bool AppMaintenance { get; set; }
        public bool UpdateMandatory { get; set; }
    }
}