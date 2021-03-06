using POCKETPAL_API.Actions;
using System.Web.Configuration;
using System.Web.Mvc;

namespace POCKETPAL_API.Controllers
{
    public class MasterController : Controller
    {
        ActionMaster _mdAction = new ActionMaster();
        public ActionResult Index()
        {
            string Id = "Master Controller";
            string APIName = WebConfigurationManager.AppSettings["APIName"];
            string APIVersion = WebConfigurationManager.AppSettings["APIVersion"];
            string Final = APIName + " - " + APIVersion + " ["+ Id + "]";
            return Json(Final, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPlantCountry()
        {
            return Json(_mdAction.GetPlantCountry(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPlantList(string PlantCountry)
        {
            return Json(_mdAction.GetPlantforDDList(PlantCountry), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDepartmentforDDList(string PlantCode)
        {
            return Json(_mdAction.GetDepartmentforDDList(PlantCode), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetApprovalGroupsDL()
        {
            return Json(_mdAction.GetAprrovalGroupsDL(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMenuList()
        {
            return Json(_mdAction.GetMenuList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetVersion()
        {
            return Json(_mdAction.GetVersion(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNews()
        {
            return Json(_mdAction.GetNews(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetLinks()
        {
            return Json(_mdAction.GetLinks(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMenuAccess(string MenuName, string BusFunc)
        {
            return Json(_mdAction.MenuAccess(MenuName, BusFunc), JsonRequestBehavior.AllowGet);
        }
    }
}