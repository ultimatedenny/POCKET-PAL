using System;
using System.Collections.Generic;
using System.Text;

namespace POCKETPAL.Models
{
    public class NewsModel
    {
        public string MasterId { get; set; }
        public string NewsId { get; set; }
        public string Header { get; set; }
        public string BodyTEXT { get; set; }
        public string BodyHTML { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string FeaturedImage { get; set; }
        public string Status { get; set; }
        public string Viewer { get; set; }
    }
}
