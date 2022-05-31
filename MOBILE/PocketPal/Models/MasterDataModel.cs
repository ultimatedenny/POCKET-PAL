using System;
using System.Collections.Generic;
using System.Text;

namespace POCKETPAL.Models
{
    public class RespondApi
    {
        public string Message { get; set; }
        public bool Success { get; set; }
    }
    public class PlantModels
    {
        public string PlantCode { get; set; }
        public string PlantAbbreviation { get; set; }
        public string PlantName { get; set; }
        public string PlantAddress { get; set; }
        public string PlantCountry { get; set; }
    }
    public class DeptModels
    {
        public string PlantCode { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
    }
    public class ApprovalModels
    {
        public string Plant { get; set; }
        public string Group { get; set; }
        public string Dept { get; set; }
    }

    public class TSID_Model
    {
        public string ID { get; set; }
        public string NAME { get; set; }
        public string EMPLOYEENO { get; set; }
        public string DEPARTMENT { get; set; }
        public string PLANT { get; set; }
        public string SHIMANOBADGE { get; set; }
    }
    public class TSSTATUS_Model
    {
        public string LOGID { get; set; }
        public string TIMEIN { get; set; }
        public string TIMEOUT { get; set; }
        public string DATEVISIT { get; set; }
        public string NEEDLUNCH { get; set; }
        public string NEEDSTAY { get; set; }
        public string DATEOFEND { get; set; }
        public string STATUS { get; set; }
    }
    public class TSQR_Model
    {
        public string QRCODE { get; set; }
        public string DATE { get; set; }
        public string TIME { get; set; }
        public string USEID { get; set; }
        public string USENAM { get; set; }
        public string INOUT { get; set; }
    }
    public class CompanyTransTime
    {
        public string Time { get; set; }
    }
    public class ApproverEPList
    {
        public string UserId { get; set; }
        public string Username { get; set; }
    }
    public class RequesterEPList
    {
        public string UseId { get; set; }
        public string UseNam { get; set; }
    }
    public class ModelExitPermitMessage
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UDID { get; set; }
    }
    public class EPMaster
    {
        public string MasterId { get; set; }
        public string SENo { get; set; }
        public string EPNo { get; set; }
        public string UseDep { get; set; }
        public string PlantId { get; set; }
        public string Destination { get; set; }
        public string Date { get; set; }
        public string Out { get; set; }
        public string In { get; set; }
        public string CompTrans { get; set; }
        public string CompTransTime { get; set; }
        public string Status { get; set; }
        public string ExpireTicket { get; set; }
        public string Approver { get; set; }
        public string ActIn { get; set; }
        public string ActOut { get; set; }
        public string UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string CreatedBy { get; set; }
        public string Employee { get; set; }
    }
    public class BatchList
    {
        public string BatchComp { get; set; }
        public string InvitationDate { get; set; }
        public string RequestDate { get; set; }
        public string TotalCandidate { get; set; }
        public string StatusBatch { get; set; }
        public string ColorStatus { get; set; }
        public string BatchEmp { get; set; }
        public string NameEmp { get; set; }
        public string PhoneNumber { get; set; }
        public string DateOfBirthEmp { get; set; }
        public string InvitationCodeEmp { get; set; }
        public string StatusEmp { get; set; }
        public string StatusEmpText { get; set; }
        public string UploadedBy { get; set; }
        public string UploadedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string UpdatedDate { get; set; }
        public string Absent { get; set; }
        public string Present { get; set; }
        public string Invited { get; set; }
        public string NotInvited { get; set; }
        public string Percentage { get; set; }
        public string Combined1
        {
            get
            {
                return string.Format("{0}|{1}", BatchEmp, StatusEmp);
            }
        }
        public string Combined2
        {
            get
            {
                return string.Format("{0}|{1}", BatchComp, StatusBatch);
            }
        }

    }
    public class Root_TSID_Model
    {
        public TSID_Model Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
    public class Root_TSSTATUS_Model
    {
        public TSSTATUS_Model Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
