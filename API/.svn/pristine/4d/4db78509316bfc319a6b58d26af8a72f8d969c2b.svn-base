using System.Web.Configuration;
using System.Web.Mvc;
using POCKETPAL_API.Actions;


namespace POCKETPAL_API.Controllers
{
    public class ErecruitmentController : Controller
    {
        ErecruitmentAction EREC = new ErecruitmentAction();
        public ActionResult Index()
        {
            string Id = "Erecruitment Controller";
            string APIName = WebConfigurationManager.AppSettings["APIName"];
            string APIVersion = WebConfigurationManager.AppSettings["APIVersion"];
            string Final = APIName + " - " + APIVersion + " [" + Id + "]";
            return Json(Final, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchList(string status, string BatchComp)
        {
            return Json(EREC.GetBatchList(status, BatchComp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchStatus(string x)
        {
            return Json(EREC.GetBatchStatus(x), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBatchInformation(string BatchComp)
        {
            return Json(EREC.GetBatchInformation(BatchComp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCandidateList(string BatchComp)
        {
            return Json(EREC.GetCandidateList(BatchComp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmpListBatch(string BatchComp, string NameEmp)
        {
            return Json(EREC.GetEmpListBatch(BatchComp, NameEmp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetEmpListBatchQR(string BatchEmp)
        {
            return Json(EREC.GetEmpListBatchQR(BatchEmp), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateBatch(string Status, string Batch, string UseId)
        {
            return Json(EREC.UpdateBatch(Status, Batch, UseId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult UpdateEmployee(string Status, string BatchEmp, string UseId)
        {
            return Json(EREC.UpdateEmployee(Status, BatchEmp, UseId), JsonRequestBehavior.AllowGet);
        }
    }
}