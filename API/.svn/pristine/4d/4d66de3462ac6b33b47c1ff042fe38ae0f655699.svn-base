using POCKETPAL_API.Class;
using POCKETPAL_API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace POCKETPAL_API.Actions
{
    public class ActionMaster
    {
        public string ConnectionDB = System.Configuration.ConfigurationManager.ConnectionStrings["POCKETPALCON"].ToString();
        public string ConnectionVMS = System.Configuration.ConfigurationManager.ConnectionStrings["DBVISITORMS"].ToString();
        public string ConnectionMDCDB = System.Configuration.ConfigurationManager.ConnectionStrings["MDCDB"].ToString();

        public List<ModelPlant> GetPlantCountry()
        {
            DataTable dt = new DataTable();
            string query = @"select Distinct PlantCountry from MDCDB.DBO.mmPlant ";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionMDCDB, query, null, null, false);
            }
            var _plants = (from rw in dt.AsEnumerable()
                           select new ModelPlant
                           {
                               PlantCountry = rw["PlantCountry"].ToString()
                           }).ToList();
            return _plants;
        }
        public List<ModelPlant> GetPlantforDDList(string PlantCountry)
        {
            DataTable dt = new DataTable();
            string query = @"CREATE TABLE #temp1(
                            PlantCode [int] NULL,
                            PlantAbbreviation [nvarchar](50) NULL,
                            PlantName [nvarchar](50) NULL,
                            PlantAddress [nvarchar](255) NULL,
                            PlantCountry [nvarchar](20) NULL,
                            )
                            insert into #temp1 (PlantCode, PlantAbbreviation, PlantName, PlantAddress, PlantCountry) 
                            SELECT PlantCode, PlantAbbreviation, PlantName, PlantAddress, PlantCountry FROM mmPlant where PlantCountry = '" + PlantCountry + @"'
                            insert into #temp1 (PlantCode, PlantAbbreviation, PlantName, PlantAddress, PlantCountry) 
                            SELECT  PlantCode, PlantAbbreviation, PlantName, PlantAddress, PlantCountry FROM mmPlant where PlantCountry = 'COMMON'
                            SELECT * FROM #temp1
                            DROP TABLE #temp1";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionMDCDB, query, null, null, false);
            }
            var _plants = (from rw in dt.AsEnumerable()
                           select new ModelPlant
                           {
                               PlantCode = rw["PlantCode"].ToString(),
                               PlantAbbreviation = rw["PlantAbbreviation"].ToString(),
                               PlantName = rw["PlantName"].ToString(),
                               PlantAddress = rw["PlantAddress"].ToString(),
                               PlantCountry = rw["PlantCountry"].ToString(),
                           }).ToList();
            return _plants;
        }
        public List<ModelDepartment> GetDepartmentforDDList(string PlantCode)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT DepartmentID, DepartmentName FROM mmDepartment where PlantCode = '" + PlantCode + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionMDCDB, query, null, null, false);
            }

            var _data = (from rw in dt.AsEnumerable()
                         select new ModelDepartment
                         {
                             DepartmentID = rw["DepartmentID"].ToString(),
                             DepartmentName = rw["DepartmentName"].ToString()
                         }).ToList();
            return _data;
        }
        public List<ModelAppClient> GetVersion()
        {
            string query = @"select * from mmAppClient where appname = 'PocketPal'";
            DataTable dt = new DataTable();
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(ConnectionMDCDB, query, null, null, false);
            }
            var _datas = (from rw in dt.AsEnumerable()
                          select new ModelAppClient
                          {
                              AppName = rw["AppName"].ToString(),
                              AppVersion = rw["AppVersion"].ToString(),
                              AppMaintenance = rw["AppMaintenance"].ToBoolean(),
                              UpdateMandatory = rw["UpdateMandatory"].ToBoolean(),
                          }).ToList();
            return _datas;
        }
        public List<ModelNewsMaster> GetNews()
        {
            string query = @"SELECT TOP 3 * FROM NewsMaster order by CreatedDate desc";
            DataTable dt = new DataTable();
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(ConnectionVMS, query, null, null, false);
            }
            var _datas = (from rw in dt.AsEnumerable()
                          select new ModelNewsMaster
                          {
                              MasterId = rw["MasterId"].ToString(),
                              NewsId = rw["NewsId"].ToString(),
                              Header = rw["Header"].ToString(),
                              BodyTEXT = rw["BodyTEXT"].ToString(),
                              BodyHTML = rw["BodyHTML"].ToString(),
                              CreatedBy = rw["CreatedBy"].ToString(),
                              CreatedDate = rw["CreatedDate"].ToString(),
                              FeaturedImage = rw["FeaturedImage"].ToString(),
                              Status = rw["Status"].ToString(),
                              Viewer = rw["Viewer"].ToString()
                          }).ToList();
            return _datas;
        }

        public List<ModelApprovalGroup> GetAprrovalGroupsDL()
        {
            string query = @"SELECT DISTINCT ApprovalGroup from ApprovalGroup";
            DataTable dt = new DataTable();
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _datas = (from rw in dt.AsEnumerable()
                          select new ModelApprovalGroup
                          {
                              Group = rw["ApprovalGroup"].ToString()
                          }).ToList();
            return _datas;
        }
        public List<ModelMenuList> GetMenuList()
        {
            string query = @"select * from MenuClient";
            DataTable dt = new DataTable();
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _datas = (from rw in dt.AsEnumerable()
                          select new ModelMenuList
                          {
                              Menu_Name = rw["Menu_Name"].ToString(),
                              IsActive = rw["IsActive"].ToString(),
                          }).ToList();
            return _datas;
        }
        public List<ModelLinks> GetLinks()
        {
            string query = @"EXEC PP_GET_LINKS";
            DataTable dt = new DataTable();
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var data0 = (from rw in dt.AsEnumerable()
                          select new ModelLinks
                          {
                              GrpCod = rw["GrpCod"].ToString(),
                              Cod = rw["Cod"].ToString(),
                              CodAbb = rw["CodAbb"].ToString(),
                              CodDesc = rw["CodDesc"].ToString(),
                          }).ToList();
            return data0;
        }
        public PP<ModelMenuAccess> MenuAccess(string MENUNAME, string BUSFUNC)
        {
            string query = @"EXEC [PP_GET_MENU_ACCESS] '"+ MENUNAME + "','" + BUSFUNC + "'";
            DataTable dt = new DataTable();
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var data0 = (from rw in dt.AsEnumerable()
                         select new ModelMenuAccess
                         {
                             MenuName = rw["MenuName"].ToString(),
                             IsAllow = rw["IsAllow"].ToBoolean()
                            
                         }).FirstOrDefault();

            if (data0.IsAllow == true)
            {
                return new PP<ModelMenuAccess>
                {
                    Success = true,
                    Message = "Access granted",
                    data = data0
                };
            }
            else
            {
                return new PP<ModelMenuAccess>
                {
                    Success = false,
                    Message = "Access not granted",
                    data = data0
                };
            }
        }
    }
}