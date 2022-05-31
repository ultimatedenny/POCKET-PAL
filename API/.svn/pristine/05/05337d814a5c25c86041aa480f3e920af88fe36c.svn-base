using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using POCKETPAL_API.Class;
using POCKETPAL_API.Models;

namespace POCKETPAL_API.Actions
{
    public class ActionExitPermit
    {
        DataTable dt;
        public string DBCONNECTION = ConfigurationManager.ConnectionStrings["POCKETPALCON"].ToString();
        public string ConnectionVisibukku = ConfigurationManager.ConnectionStrings["DBVISITORMS"].ToString();
        ClassNotification _Nofif = new ClassNotification();
        public List<ModelExitPermitHeader> GetExitPermit(string EPNo)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC PP_GET_EXITPERMIT '"+ EPNo + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var _GetExitPermitList = (from rw in dt.AsEnumerable()
                                      select new ModelExitPermitHeader
                                      {
                                          EPNo = rw["EPNo"].ToString(),
                                          Destination = rw["Destination"].ToString(),
                                          Date = rw["Date"].ToString(),
                                          Out = rw["Out"].ToString(),
                                          In = rw["In"].ToString(),
                                          CompTrans = rw["CompTrans"].ToString(),
                                          Status = rw["Status"].ToString(),
                                          ExpireTicket = rw["ExpireTicket"].ToString(),
                                          Approver = rw["Approver"].ToString(),
                                          CreatedBy = rw["CreatedBy"].ToString(),
                                          Employee = rw["Employee"].ToString(),
                                          ActOut = rw["ActOut"].ToString(),
                                          ActIn = rw["ActIn"].ToString(),
                                      }).ToList();
            return _GetExitPermitList;
        }
        public List<Respond> UpdateExitPermitOut(string EPNo, string UseId)
        {
            string TIMEOUT = DateTime.Now.ToString("HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new DataTable();
            string query = @"UPDATE DBVisitorMS.dbo.EPMASTER SET ACTOUT = '" + TIMEOUT + @"',
                                                 UpdateBy = '" + UseId + @"',
                                                 UpdateDate = '" + date + @"'
                                                 WHERE EPNo = '" + EPNo + @"'
            SELECT 'SUCCESS' as isSuccess, 'Update Successfully' as Message";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var _UpdateBatch = (from rw in dt.AsEnumerable()
                                select new Respond
                                {
                                    isSuccess = rw["isSuccess"].ToString(),
                                    Message = rw["Message"].ToString(),
                                }).ToList();
            return _UpdateBatch;
        }
        public List<Respond> UpdateExitPermitIn(string EPNo, string UseId)
        {
            string TIMEIN = DateTime.Now.ToString("HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            DataTable dt = new DataTable();
            string query = @"UPDATE DBVisitorMS.dbo.EPMASTER SET ACTIN = '" + TIMEIN + @"',
                                                 UpdateBy = '" + UseId + @"',
                                                 UpdateDate = '" + date + @"',
                                                 STATUS = 'COMPLETED'
                                                 WHERE EPNo = '" + EPNo + @"'
            SELECT 'SUCCESS' as isSuccess, 'Update Successfully' as Message";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var _UpdateBatch = (from rw in dt.AsEnumerable()
                                select new Respond
                                {
                                    isSuccess = rw["isSuccess"].ToString(),
                                    Message = rw["Message"].ToString(),
                                }).ToList();
            return _UpdateBatch;
        }
        public List<ModelPlant> GetPlant()
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [spGetPlantEP]";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var _plants = (from rw in dt.AsEnumerable()
                           select new ModelPlant
                           {
                               PlantCode = rw["PlantCode"].ToString(),
                               PlantAbbreviation = rw["PlantAbbreviation"].ToString()
                           }).ToList();
            return _plants;
        }
        public List<ModelExitPermitTransTme> GetCompanyTransTime()
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [PP_GET_COMTRANSTIME]";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var data0 = (from rw in dt.AsEnumerable()
                           select new ModelExitPermitTransTme
                           {
                               Time = rw["Time"].ToString()
                           }).ToList();
            return data0;
        }
        public List<ModelDepartment> GetDept(string Plant)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [spGetDeptEP] '"+ Plant + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var _data = (from rw in dt.AsEnumerable()
                         select new ModelDepartment
                         {
                             DepartmentID = rw["DepartmentID"].ToString(),
                             DepartmentName = rw["DepartmentName"].ToString()
                         }).ToList();
            return _data;
        }
        public List<Approver> GetApprover(string Dept, string Plant)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [spGetApprovalEP] '" + Dept + "', '" + Plant + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var _Approver = (from rw in dt.AsEnumerable()
                             select new Approver
                             {
                                 UserId = rw["UserId"].ToString(),
                                 Username = rw["Username"].ToString(),
                             }).ToList();
            return _Approver;
        }
        public List<Requester> GetUserEP(string UseId)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [spGetUser] '" + UseId + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var _Requester = (from rw in dt.AsEnumerable()
                             select new Requester
                             {
                                 UseId = rw["UseId"].ToString(),
                                 UseNam = rw["UseNam"].ToString(),
                             }).ToList();
            return _Requester;
        }
        public PP<ModelExitPermitMessage> PostEP(string UseId, string UseDep, string PlantID, string Destination, string Out, string In, string Remarks,
                         string ActOut, string ActIn, string Date, string CompTrans, string CompTransTime,
                         string OTH, string Status, string Approver, ModelExitPermitDetail[] EPDetailss)
        {
            string TimeOut = Out;
            string TimeIn = In;
            string ActTimeIn = ActOut;
            string ActTimeOut = ActIn;
            string CompTransTime01 = CompTransTime;
            string OTH01 = OTH;
            if (Out.Contains("."))
            {
                TimeOut = Out.Replace(".", ":");
            }
            if (In.Contains("."))
            {
                TimeIn = In.Replace(".", ":");
            }
            if (ActIn.Contains("."))
            {
                ActTimeIn = ActOut.Replace(".", ":");
            }
            if (ActOut.Contains("."))
            {
                ActTimeOut = ActIn.Replace(".", ":");
            }
            if (CompTransTime.Contains("."))
            {
                CompTransTime01 = CompTransTime.Replace(".", ":");
            }
            if (OTH.Contains("."))
            {
                OTH01 = OTH.Replace(".", ":");
            }


            string query = "";
            query = "EXEC [PP_POST_EP01] '" + UseId + "', '" + UseDep + "', '" + PlantID + "', '" + Destination + "', '" + TimeOut + "', '" + TimeIn + "','" + Remarks + "','" + ActTimeOut + "', '" + ActTimeIn + "','" + Date + "', '" + CompTrans + "', '" + CompTransTime01 + "', '" + OTH01 + "', '" + Status + "', '" + Approver + "'";
            DataTable dt = new DataTable();
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
            }
            var data0 = (from rw in dt.AsEnumerable() 
                            select new ModelExitPermitMessage
                            { 
                                Message = rw["MESSAGE"].ToString(),
                                UDID = rw["UDID"].ToString(),
                                Success = rw["SUCCESS"].ToBoolean() 
                            }).FirstOrDefault();

            if (data0.Success == true)
            {
                query = "";
                if(EPDetailss != null)
                {
                    string UDID = data0.UDID;
                    foreach (var item in EPDetailss)
                    {
                        var detailId = Guid.NewGuid();
                        string UseId2 = item.UseId;
                        query = "EXEC [PP_POST_EP02] '" + UseId2 + "', '" + UDID + "'";
                        using (var sql = new ClassMSSQL())
                        {
                            dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
                        }
                        data0 = (from rw in dt.AsEnumerable() 
                                 select new ModelExitPermitMessage 
                                 { 
                                     Message = rw["MESSAGE"].ToString(), 
                                     MasterIdForNotif = rw["MasterIdForNotif"].ToString(), 
                                     EPNoNotif = rw["EPNoNotif"].ToString() 
                                 }).FirstOrDefault();
                    }
                    _Nofif.GetUserApprovalEP(new Guid(data0.MasterIdForNotif.ToString()), data0.EPNoNotif.ToString());

                    return new PP<ModelExitPermitMessage>
                    {
                        Success = true,
                        Message = data0.Message,
                        data = data0
                    };
                }
                else
                {
                    query = "EXEC [PP_POST_EP03] '" + data0.UDID.ToString() + "'";
                    using (var sql = new ClassMSSQL())
                    {
                        dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
                    }
                    data0 = (from rw in dt.AsEnumerable() 
                             select new ModelExitPermitMessage 
                             { 
                                 Message = "PLEASE FILL USERNAME" 
                             }).FirstOrDefault();

                    return new PP<ModelExitPermitMessage>
                    {
                        Success = false,
                        Message = data0.Message,
                        data = data0
                    };
                }
            }
            else
            {
                return new PP<ModelExitPermitMessage>
                {
                    Success = false,
                    Message = data0.Message,
                    data = null
                };
            }
        }
        public PP<ModelResponse> GetExitPermitAllowBeforeTimeOut()
        {
            try
            {
                DataTable dt = new DataTable();
                string query = @"EXEC PP_GET_EP_ALLOW_BEFORE_TIMEOUT";
                using (var sql = new ClassMSSQL())
                {
                    dt = sql.ExecDTQuery(DBCONNECTION, query, null, null, false);
                }
                var data = (from rw in dt.AsEnumerable()
                             select new ModelResponse
                             {
                                 Success = rw["SUCCESS"].ToBoolean(),
                                 Message = rw["MESSAGE"].ToString()
                             }).FirstOrDefault();

                return new PP<ModelResponse>
                {
                    Success = data.Success,
                    Message = data.Message,
                    data = null
                };
            }
            catch(Exception msg)
            {
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = msg.Message.ToString(),
                    data = null
                };
            }
        }
        public class Respond
        {
            public string isSuccess { get; set; }
            public string Message { get; set; }
        }
    }
}