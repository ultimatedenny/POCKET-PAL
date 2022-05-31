using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Models
{
    public class ModelErecruitment
    {
        public string BatchComp { get; set; }
        public string InvitationDate { get; set; }
        public string RequestDate { get; set; }
        public string TotalCandidate { get; set; }
        public string StatusBatch { get; set; }
        public string BatchEmp { get; set; }
        public string NameEmp { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirthEmp { get; set; }
        public string InvitationCodeEmp { get; set; }
        public string StatusEmp { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string Present { get; set; }
        public string Absent { get; set; }
        public string Percentage { get; set; }

        public string Invited { get; set; }
        public string NotInvited { get; set; }
    }
}