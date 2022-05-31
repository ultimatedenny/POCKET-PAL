using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using POCKETPAL_API.Class;
using POCKETPAL_API.Models;

namespace POCKETPAL_API.Actions
{

    public class ActionVisitor
    {
        public string CONNECTIONVMS = ConfigurationManager.ConnectionStrings["DBVISITORMS"].ToString();
        public string CONNECTIONPP = ConfigurationManager.ConnectionStrings["POCKETPALCON"].ToString();
        public PP<string> PP_ADD_TEAM_SHIMANO_VMS(ModelVisitorTS NewData)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [PP_ADD_TEAM_SHIMANO_VMS] '" + NewData.Name + @"'
                       ,'" + NewData.EmployeeNo + @"'
                       ,'" + NewData.Department + @"'
                       ,'" + NewData.ShimanoBadge + @"'
                       ,'" + NewData.Plant + @"'
                       ,'" + NewData.Email + @"'
                       ,'" + NewData.Phone + @"'
                       ,'" + NewData.Photoes + @"'
                       ,'" + NewData.CreUser + @"'";
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
            }
            return dt.OneRowdtToResult();
        }
        public PP<string> PP_CHECKINTS_VMS(ModelVisitorTSLog Data)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [PP_CHECKINTS_VMS] 
                            '"  + Data.LogId + "'," +
                            "'" + Data.TSVisitorId + "'," +
                            "'" + Data.HostName + "'," +
                            "'" + Data.HostDepartment + "'," +
                            "'" + Data.TimeOut + "'," +
                            ""  + Data.NeedLunch + "," +
                            "'" + Data.Status + "'," +
                            "'" + Data.Plant + "'," +
                            ""  + Data.NeedStay + "," +
                            "'" + Data.DateOfEnd + "'";
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
            }
            return dt.OneRowdtToResult();
        }
        public PP<string> CheckInSameDay(ModelVisitorTSLog NewData)
        {
            DataTable dt = new DataTable();
            string query = @"
            BEGIN DECLARE @maxId int set @MaxId = (select isnull (max(cast(LogId as int))+1,1) from DBVisitorMS.dbo.VisitLog_TS)
            INSERT INTO [dbo].[VisitLog_TS]
            (LogId,[TSVisitorId],[HostName],[HostDepartment],[TimeIn],[DateVisit],[NeedLunch],[Status],[Plant],[NeedStay],[DateOfEnd])
            VALUES(@MaxId," + NewData.TSVisitorId + @",
            '" + NewData.HostName + @"',
            '" + NewData.HostDepartment + @"',
            '" + NewData.TimeIn + @"',
            '" + NewData.DateVisit + @"',
            '" + NewData.NeedLunch + @"',
            '" + NewData.Status + @"',
            '" + NewData.Plant + @"',
            '" + NewData.NeedStay + @"',
            '" + NewData.DateOfEnd + @"') 
            SELECT 'SUCCESS' as isSuccess, 'Insert Successfully' as message END";
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(CONNECTIONVMS, query, null, null, false);
            }
            return dt.OneRowdtToResult();
        }
        public PP<ModelGetTSId> PP_GET_TS_ID_VMS(string UseNam)
        {
            try
            {
                DataTable dt = new DataTable();
                string query = @"EXEC [PP_GET_TS_ID_VMS] '" + UseNam + "'";
                using (var sql = new ClassMSSQL())
                {
                    dt = sql.ExecDTQuery(CONNECTIONPP, query, null, null, false);
                }
                var data = (from row in dt.AsEnumerable()
                            select new ModelGetTSId
                            {
                                ID = (row["ID"].ToString() == null) ? "" : row["ID"].ToString(),
                                NAME = (row["NAME"].ToString() == null) ? "" : row["NAME"].ToString(),
                                EMPLOYEENO = (row["EMPLOYEENO"].ToString() == null) ? "" : row["EMPLOYEENO"].ToString(),
                                DEPARTMENT = (row["DEPARTMENT"].ToString() == null) ? "" : row["DEPARTMENT"].ToString(),
                                PLANT = (row["PLANT"].ToString() == null) ? "" : row["PLANT"].ToString(),
                                SHIMANOBADGE = (row["SHIMANOBADGE"].ToString() == null) ? "" : row["SHIMANOBADGE"].ToString(),
                            }).FirstOrDefault();

                if (data != null)
                {
                    return new PP<ModelGetTSId>
                    {
                        Success = true,
                        Message = "Success",
                        data = data
                    };
                }
                else
                {
                    return new PP<ModelGetTSId>
                    {
                        Success = false,
                        Message = "No Data",
                        data = data
                    };
                }
            }
            catch(Exception ex)
            {
                return new PP<ModelGetTSId>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelGetTSStatus> PP_GET_TS_STATUS_VMS(string TSVisitorId)
        {
            try
            {
                DataTable dt = new DataTable();
                string query = @"EXEC [PP_GET_TS_STATUS_VMS] '"+ TSVisitorId + "'";
                using (var sql = new ClassMSSQL())
                {
                    dt = sql.ExecDTQuery(CONNECTIONPP, query, null, null, false);
                }
                var data = (from row in dt.AsEnumerable()
                                  select new ModelGetTSStatus
                                  {
                                      LOGID = (row["LOGID"].ToString() == null) ? "" : row["LOGID"].ToString(),
                                      TIMEIN = (row["TIMEIN"].ToString() == null) ? "" : row["TIMEIN"].ToString(),
                                      TIMEOUT = (row["TIMEOUT"].ToString() == null) ? "" : row["TIMEOUT"].ToString(),
                                      DATEVISIT = (row["DATEVISIT"].ToString() == null) ? "" : row["DATEVISIT"].ToString(),
                                      NEEDLUNCH = (row["NEEDLUNCH"].ToString() == null) ? "" : row["NEEDLUNCH"].ToString(),
                                      NEEDSTAY = (row["NEEDSTAY"].ToString() == null) ? "" : row["NEEDSTAY"].ToString(),
                                      DATEOFEND = (row["DATEOFEND"].ToString() == null) ? "" : row["DATEOFEND"].ToString(),
                                      STATUS = (row["STATUS"].ToString() == null) ? "" : row["STATUS"].ToString(),
                                  }).FirstOrDefault();

                if (dt != null)
                {
                    return new PP<ModelGetTSStatus>
                    {
                        Success = true,
                        Message = "Success",
                        data = data
                    };
                }
                else
                {
                    return new PP<ModelGetTSStatus>
                    {
                        Success = false,
                        Message = "No Data",
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new PP<ModelGetTSStatus>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }

        public PP<ModelRespond>PP_GET_PASSWORD_RECEPTIONIST (string PASSWORD)
        {
            try
            {
                DataTable dt = new DataTable();
                string query = @"EXEC [PP_GET_PASSWORD_RECEPTIONIST] '" + PASSWORD + "'";
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
                }
                var data = (from row in dt.AsEnumerable()
                            select new ModelRespond
                            {
                                Success = Convert.ToBoolean(row["Success"]),
                                Message = Convert.ToString(row["Message"].ToString()),

                            }).FirstOrDefault();

                return new PP<ModelRespond>
                {
                    Success = data.Success,
                    Message = data.Message.ToString(),
                    data = data
                };
            }
            catch(Exception ex)
            {
                return new PP<ModelRespond>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
            
        }
        public List<ModelRespond> QRCheck(string Client, string UseID, string UseNam)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [PP_EXECUTE_QR_VMS] '"+ Client + "','" + UseID + "','" + UseNam + "','" + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + "'";
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
            }
            var data = (from row in dt.AsEnumerable()
                           select new ModelRespond
                           {
                               Success = Convert.ToBoolean(row["SUCCESS"]),
                               Message = (row["Message"].ToString() == null) ? "" : row["Message"].ToString()
                           }).ToList();
            return data;
        }
        public PP<ModelQRApi> PP_POST_QR_VMS(string QRCODE)
        {
            try
            {
                DataTable dt = new DataTable();
                string Date = DateTime.Now.ToString("yyyy-MM-dd");
                string Time = DateTime.Now.ToString("HH:mm:ss");
                string query = @"EXEC [PP_POST_QR_VMS] '" + QRCODE + "','" + Date + "','" + Time + "'";
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
                }
                var data = (from row in dt.AsEnumerable()
                            select new ModelQRApi
                            {
                                QRCode = (row["QRCode"].ToString() == null) ? "" : row["QRCode"].ToString(),
                                Date   = (DateTime.Parse(row["Date"].ToString()).ToString() == null) ? "" : DateTime.Parse(row["Date"].ToString()).ToString(),
                                Time   = (TimeSpan.Parse(row["Time"].ToString()).ToString() == null) ? "" : TimeSpan.Parse(row["Time"].ToString()).ToString(),
                                UseID  = (row["UseID"].ToString() == null) ? "" : row["UseID"].ToString(),
                                UseNam = (row["UseNam"].ToString() == null) ? "" : row["UseNam"].ToString(),
                                InOut  = (row["In/Out"].ToString() == null) ? "" : row["In/Out"].ToString(),
                                IsUsed = bool.Parse(row["IsUsed"].ToString()).ToString()
                            }).FirstOrDefault();
                if (dt.Rows.Count != 0 || data != null)
                {
                    return new PP<ModelQRApi>
                    {
                        Success = true,
                        Message = "",
                        data = data
                    };
                }
                else
                {
                    return new PP<ModelQRApi>
                    {
                        Success = false,
                        Message = "",
                        data = data
                    };
                }
            }
            catch(Exception ex)
            {
                return new PP<ModelQRApi>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelQRApi2> PP_GET_LOGIN_INFO_VMS(string QRCode)
        {
            try
            {
                DataTable dt = new DataTable();
                string query = @"EXEC PP_GET_LOGIN_INFO_VMS '" + QRCode + "'";
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
                }
                var data = (from rw in dt.AsEnumerable()
                            select new ModelQRApi2
                            {
                                UseNam = rw["USENAM"].ToString(),
                            }).FirstOrDefault();

                if (dt.Rows.Count != 0 || data != null)
                {
                    return new PP<ModelQRApi2>
                    {
                        Success = true,
                        Message = "Success",
                        data = data
                    };
                }
                else
                {
                    return new PP<ModelQRApi2>
                    {
                        Success = false,
                        Message = "No Data",
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                return new PP<ModelQRApi2>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public List<ModelResponse> PP_DELETE_QR_VMS()
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [PP_DELETE_QR_VMS]";
            using (ClassMSSQL s = new ClassMSSQL())
            {
                dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
            }
            var data = (from rw in dt.AsEnumerable()
                        select new ModelResponse
                        {
                            Message = rw["MESSAGE"].ToString(),
                            Success = rw["SUCCESS"].ToBoolean(),
                        }).ToList();
            return data;
        }
        public PP<ModelResponse> CheckSSID(string USEID)
        {
            try
            {
                DataTable dt = new DataTable();
                string query = @"EXEC [spCheckSSID] '" + USEID + "'";
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
                }
                var result = (from rw in dt.AsEnumerable()
                              select new ModelResponse
                              {
                                  Success = Convert.ToBoolean(rw["Success"]),
                                  Message = rw["Message"].ToString()
                              }).FirstOrDefault();

                return new PP<ModelResponse>
                {
                    Success = result.Success,
                    Message = result.Message,
                    data = null
                };
            }
            catch (Exception ex)
            {
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelResponse> PostSSID(Root Data)
        {
            try
            {
                DataTable dt = new DataTable();
                string query = @"EXEC [spPostSSID] 
                '" + Data.userdb_add.UserEmail + "'," +
                "'" + Data.userdb_add.mode + "'," +
                "'" + Data.userdb_add.g_fullname + "'," +
                "'" + Data.userdb_add.g_company + "'," +
                "'" + Data.userdb_add.g_phone + "'," +
                "'" + Data.userdb_add.start_day + "'," +
                "'" + Data.userdb_add.start_time + "'," +
                "'" + Data.userdb_add.expiry + "'," +
                "'" + Data.userdb_add.expmins + "'," +
                "'" + Data.userdb_add.expday + "'," +
                "'" + Data.userdb_add.exptime + "'," +
                "'" + Data.userdb_add.remote_ip + "'," +
                "'" + Data.userdb_add.name + "'," +
                "'" + Data.userdb_add.passwd + "'";

                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(CONNECTIONPP, query, null, null, false);
                }
                var result = (from rw in dt.AsEnumerable()
                              select new ModelResponse
                              {
                                  Success = Convert.ToBoolean(rw["Success"]),
                                  Message = rw["Message"].ToString()
                              }).FirstOrDefault();

                return new PP<ModelResponse>
                {
                    Success = result.Success,
                    Message = result.Message,
                    data = null
                };
            }
            catch(Exception ex)
            {
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public List<Result> GetSSIDTempDuration()
        {
            return null;    
        }
        public class Result
        {
            public string _Result { get; set; }
        }
    }
}