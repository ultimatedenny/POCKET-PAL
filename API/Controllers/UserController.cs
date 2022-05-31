using System.Web.Configuration;
using System.Web.Mvc;
using POCKETPAL_API.Actions;
using POCKETPAL_API.Models;

namespace POCKETPAL_API.Controllers
{
    public class UserController : Controller
    {
        ActionUser USER = new ActionUser();
        public ActionResult Index()
        {
            string Id = "User Controller";
            string APIName = WebConfigurationManager.AppSettings["APIName"];
            string APIVersion = WebConfigurationManager.AppSettings["APIVersion"];
            string Final = APIName + " - " + APIVersion + " [" + Id + "]";
            return Json(Final, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SignUp(
            ModelUser NewData,  
            string UDID,        string UseNam,  string UseId, string UseArea,
            string UseEmail,    string UsePass, string UseTel, string VenComp, string VenEmpNo,
            string UsePlant,    string UseDep,  string RegType, string Model, string Manufacture, string DeviceName, string OS, string Salutation)
        {
            NewData.UserName = UseNam;
            NewData.UserID = UseId;
            NewData.Email = UseEmail;
            if (RegType == "1")
            {
                NewData.IsWindowsAuthentication = true;
            }
            else
            {
                NewData.IsWindowsAuthentication = false;
            }
            NewData.Password = UsePass;
            NewData.IsActive = true;
            NewData.Department = UseDep;
            NewData.PlantCode = UsePlant;
            NewData.Phone = UseTel;
            NewData.Salutation = Salutation;
            NewData.UDID = UDID;
            NewData.DomainName = "SHIMANOACE";
            return Json(USER.SignUp(NewData, Model, Manufacture, DeviceName, OS), JsonRequestBehavior.AllowGet);
        }

        public JsonResult SignIn(string UseId, string UsePass, string DeviceName, string Model, string Manufacture, string OS)
        {
            return Json(USER.SignIn(UseId, UsePass, DeviceName, Model, Manufacture, OS), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AddPin(string UserId, string UserPin, string BusFunc, string UseNam)
        {
            return Json(USER.AddPin(UserId, UserPin, BusFunc, UseNam), JsonRequestBehavior.AllowGet);
        }
        public JsonResult RegisterToken(string UserId, string UseNam, string BusFunc, string UserToken)
        {
            return Json(USER.RegisterToken(UserId, UseNam, BusFunc, UserToken), JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteToken(string UserId, string UserToken)
        {
            return Json(USER.DeleteToken(UserId, UserToken), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIsUserVerified(string UserId)
        {
            return Json(USER.GetIsUserVerified(UserId), JsonRequestBehavior.AllowGet);
        }
        public JsonResult RequestVerifyUser(string UserId, string UseNam, string EmployeeBadge)
        {
            return Json(USER.RequestVerifyUser(UserId, UseNam, EmployeeBadge), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIsBusFuncAllowed(string BusFunc, string ScannedCode)
        {
            return Json(USER.GetIsBusFuncAllowed(BusFunc, ScannedCode), JsonRequestBehavior.AllowGet);
        }
    }
}