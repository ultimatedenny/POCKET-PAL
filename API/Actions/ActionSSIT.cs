using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using POCKETPAL_API.Class;
using POCKETPAL_API.Models;

namespace POCKETPAL_API.Actions
{
    public class ActionSSIT
    {
        DataTable dt;
        DataSet ds;
        ClassCreateIssuLog CCIL = new ClassCreateIssuLog();

        public string ConnectionDB = ConfigurationManager.ConnectionStrings["POCKETPALCON"].ToString();
        public string ConnectionDBSSIT = ConfigurationManager.ConnectionStrings["ADVJEQDBCON"].ToString();
        public PP<ModelUser> GetIsUserSSITVerified(string UserId)
        {
            try
            {
                DataTable dt = new DataTable();
                string query;
                query = @"EXEC sp_PP_GET_USER_SSIT_VERIFIED '" + UserId + "'";
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(ConnectionDBSSIT, query, null, null, false);
                }
                var data0 = (from rw in dt.AsEnumerable()
                             select new ModelUser
                             {
                                 IsActive = rw["SUCCESS"].ToBoolean(),
                                 BusinessFunction = rw["SSITUserGroup"].ToString(),
                                 UserID = rw["SSITUserID"].ToString(),
                                 UDID = rw["SSITApprCat"].ToString(),
                             }).FirstOrDefault();
                if (data0.IsActive == true)
                {
                    return new PP<ModelUser>
                    {
                        Success = true,
                        Message = ClassString.SUCCESS.ToString(),
                        data = data0
                    };
                }
                else
                {
                    return new PP<ModelUser>
                    {
                        Success = false,
                        Message = ClassString.USERNOTVERIFIED.ToString(),
                        data = data0
                    };
                }
            }
            catch (Exception msg)
            {
                return new PP<ModelUser>
                {
                    Success = false,
                    Message = msg.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelCountSSIT> GetCountDataSSIT_BY_USERGROUP(string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            try
            {
                DataSet ds = new DataSet();
                string query = @"EXEC sp_PP_GET_SCRAP_SSIT '" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + Product + @"','" + SSITCode + @"'
                                 EXEC sp_PP_GET_PR_SSIT '" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + Product + @"','" + SSITCode + @"'";
                using (var sql = new ClassMSSQL())
                {
                    ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
                }

                var data0 = new ModelCountSSIT()
                {
                    CountScrap = Convert.ToInt32(ds.Tables[0].Rows.Count),
                    CountPR = Convert.ToInt32(ds.Tables[2].Rows.Count),
                };

                return new PP<ModelCountSSIT>
                {
                    Success = true,
                    Message = ClassString.SUCCESS.ToString(),
                    data = data0
                };

            }
            catch (Exception msg)
            {
                return new PP<ModelCountSSIT>
                {
                    Success = false,
                    Message = msg.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelSSIT> CheckScanSSIT(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            try
            {
                DataSet ds = new DataSet();
                var data0 = new ModelSSIT();
                string query = "";
                if (Type == "SCRAP")
                {
                    query = @"EXEC sp_PP_GET_SCRAP_SSIT '" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + Product + @"','" + SSITCode + @"'";
                    using (var sql = new ClassMSSQL())
                    {
                        ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
                    }
                    data0 = (from rw in ds.Tables[1].AsEnumerable()
                             select new ModelSSIT
                             {
                                 Items = rw["Items"].ToString(),
                                 SSIT_ID = rw["SSIT_ID"].ToString(),
                                 Material = rw["Material"].ToString(),
                                 MaterialDesc = rw["MaterialDesc"].ToString(),
                                 DeptSubmitted = rw["DeptSubmitted"].ToString(),
                                 Qty = rw["Qty"].ToString(),
                                 BaseUOM = rw["BaseUOM"].ToString(),
                                 UnitPrice = rw["UnitPrice"].ToString(),
                                 Amount = rw["Amount"].ToString(),
                                 Currency = rw["Currency"].ToString(),
                                 SubmitBy = rw["SubmitBy"].ToString(),
                                 SubmitDate = rw["SubmitDate"].ToString(),
                                 NCCode = rw["NCCode"].ToString(),
                                 NCCodeDet = rw["NCCodeDet"].ToString(),
                                 NCDetail = rw["NCDetail"].ToString(),
                                 RootCause = rw["RootCause"].ToString(),
                                 CanApproved = rw["CanApproved"].ToString(),
                             }).FirstOrDefault();
                }
                else
                {
                    query = @"EXEC sp_PP_GET_PR_SSIT '" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + Product + @"','" + SSITCode + @"'";
                    using (var sql = new ClassMSSQL())
                    {
                        ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
                    }
                    data0 = (from rw in ds.Tables[1].AsEnumerable()
                             select new ModelSSIT
                             {
                                 Items = rw["Items"].ToString(),
                                 SSIT_ID = rw["SSIT_ID"].ToString(),
                                 Material = rw["Material"].ToString(),
                                 MaterialDesc = rw["MaterialDesc"].ToString(),
                                 DeptSubmitted = "",
                                 Qty = rw["Qty"].ToString(),
                                 BaseUOM = rw["BaseUOM"].ToString(),
                                 UnitPrice = rw["UnitPrice"].ToString(),
                                 Amount = rw["Amount"].ToString(),
                                 Currency = rw["Currency"].ToString(),
                                 SubmitBy = rw["SubmitBy"].ToString(),
                                 SubmitDate = rw["SubmitDate"].ToString(),
                                 NCCode = rw["NCCode"].ToString(),
                                 NCCodeDet = rw["NCCodeDet"].ToString(),
                                 NCDetail = rw["NCDetail"].ToString(),
                                 RootCause = rw["RootCause"].ToString(),
                                 CanApproved = rw["CanApproved"].ToString(),
                             }).FirstOrDefault();
                }


                return new PP<ModelSSIT>
                {
                    Success = true,
                    Message = ClassString.SUCCESS.ToString(),
                    data = data0
                };
            }
            catch (Exception msg)
            {
                return new PP<ModelSSIT>
                {
                    Success = false,
                    Message = msg.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelMessage> SaveScanSSIT(string Type, string UserId, string UserGroup, string ApprCat, string SSITCode)
        {
            try
            {
                DataSet ds = new DataSet();
                var data0 = new ModelMessage();
                string query = "";

                query = @"EXEC sp_PP_SAVE_SCAN_SSIT '" + Type + @"','" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + SSITCode + @"'";
                using (var sql = new ClassMSSQL())
                {
                    ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
                }
                data0 = (from rw in ds.Tables[0].AsEnumerable()
                         select new ModelMessage
                         {
                             Message = rw["Message"].ToString(),
                         }).FirstOrDefault();

                return new PP<ModelMessage>
                {
                    Success = true,
                    Message = data0.Message.ToString(),
                    data = data0
                };
            }
            catch (Exception msg)
            {
                return new PP<ModelMessage>
                {
                    Success = false,
                    Message = msg.Message.ToString(),
                    data = null
                };
            }
        }

        public List<ModelSSIT> DetailListSSIT_SCRAP(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            DataSet ds = new DataSet();
            string query = @"EXEC sp_PP_GET_SCRAP_SSIT '" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + Product + @"','" + SSITCode + @"'";
            using (var sql = new ClassMSSQL())
            {
                ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
            }
            var _GetList = (from rw in ds.Tables[0].AsEnumerable()
                            select new ModelSSIT
                            {
                                Items = rw["Items"].ToString(),
                                SSIT_ID = rw["SSIT_ID"].ToString(),
                                Material = rw["Material"].ToString(),
                                MaterialDesc = rw["MaterialDesc"].ToString(),
                                DeptSubmitted = "",
                                Qty = rw["Qty"].ToString(),
                                BaseUOM = rw["BaseUOM"].ToString(),
                                UnitPrice = rw["UnitPrice"].ToString(),
                                Amount = rw["Amount"].ToString(),
                                Currency = rw["Currency"].ToString(),
                                SubmitBy = rw["SubmitBy"].ToString(),
                                SubmitDate = rw["SubmitDate"].ToString(),
                                NCCode = rw["NCCode"].ToString(),
                                NCCodeDet = rw["NCCodeDet"].ToString(),
                                NCDetail = rw["NCDetail"].ToString(),
                                RootCause = rw["RootCause"].ToString(),
                                CanApproved = rw["CanApproved"].ToString(),
                            }).ToList();
            return _GetList;
        }

        public List<ModelPlantSSIT> GETPLANT()
        {
            DataSet ds = new DataSet();
            string query = @"Select Plant,Description from TPLANT order by Plant";
            using (var sql = new ClassMSSQL())
            {
                ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
            }
            var _GetList = (from rw in ds.Tables[0].AsEnumerable()
                            select new ModelPlantSSIT
                            {
                                PlantCode = rw["Plant"].ToString(),
                                PlantDesc = rw["Description"].ToString(),
                            }).ToList();
            return _GetList;
        }
        public List<ModelVendorSSIT> GETVENDOR(string Plant)
        {
            DataSet ds = new DataSet();
            string query = @"SELECT Vendor,VendorName from TVENDOR WHERE Plant='" + Plant + "'";
            using (var sql = new ClassMSSQL())
            {
                ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
            }
            var _GetList = (from rw in ds.Tables[0].AsEnumerable()
                            select new ModelVendorSSIT
                            {
                                VendorCode = rw["Vendor"].ToString(),
                                VendorDesc = rw["VendorName"].ToString(),
                            }).ToList();
            return _GetList;
        }
        public List<ModelProductSSIT> GETPRODUCT()
        {
            DataSet ds = new DataSet();
            string query = @"SELECT DISTINCT Product FROM TPRODUCT ORDER BY Product ASC";
            using (var sql = new ClassMSSQL())
            {
                ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
            }
            var _GetList = (from rw in ds.Tables[0].AsEnumerable()
                            select new ModelProductSSIT
                            {
                                ProductCode = rw["Product"].ToString(),
                                ProductDesc = rw["Product"].ToString(),
                            }).ToList();
            return _GetList;
        }

        public List<ModelSSIT> DetailListSSIT_PR(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            DataSet ds = new DataSet();
            string query = @"EXEC sp_PP_GET_PR_SSIT '" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + Product + @"','" + SSITCode + @"'";
            using (var sql = new ClassMSSQL())
            {
                ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
            }
            var _GetList = (from rw in ds.Tables[0].AsEnumerable()
                            select new ModelSSIT
                            {
                                Items = rw["Items"].ToString(),
                                SSIT_ID = rw["SSIT_ID"].ToString(),
                                Material = rw["Material"].ToString(),
                                MaterialDesc = rw["MaterialDesc"].ToString(),
                                DeptSubmitted = "",
                                Qty = rw["Qty"].ToString(),
                                BaseUOM = rw["BaseUOM"].ToString(),
                                UnitPrice = rw["UnitPrice"].ToString(),
                                Amount = rw["Amount"].ToString(),
                                Currency = rw["Currency"].ToString(),
                                SubmitBy = rw["SubmitBy"].ToString(),
                                SubmitDate = rw["SubmitDate"].ToString(),
                                NCCode = rw["NCCode"].ToString(),
                                NCCodeDet = rw["NCCodeDet"].ToString(),
                                NCDetail = rw["NCDetail"].ToString(),
                                RootCause = rw["RootCause"].ToString(),
                                CanApproved = rw["CanApproved"].ToString(),
                            }).ToList();
            return _GetList;
        }

        public PP<ModelMessage> ApproveSSIT(string Type, string UserId, string UserGroup, string ApprCat, string SSITCode, string SSITCount, string Amount, string AppVersion, string Vendor, string Action)
        {
            try
            {
                DataSet ds = new DataSet();
                var data0 = new ModelMessage();
                string query = "";
                if (Type == "SCRAP")
                {
                    query = @"EXEC sp_PP_APPROVE_SCRAP_SSIT '" + Type + @"','" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + SSITCode + @"','" + SSITCount + @"','" + Amount + @"','" + AppVersion + @"'";
                }
                else
                {
                    query = @"EXEC sp_PP_APPROVE_PR_SSIT '" + Type + @"','" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + SSITCode + @"','" + SSITCount + @"','" + Amount + @"','" + AppVersion + @"','" + Vendor + @"','" + Action + @"'";
                }
                using (var sql = new ClassMSSQL())
                {
                    ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
                }
                data0 = (from rw in ds.Tables[0].AsEnumerable()
                         select new ModelMessage
                         {
                             Message = rw["Message"].ToString(),
                         }).FirstOrDefault();

                return new PP<ModelMessage>
                {
                    Success = true,
                    Message = data0.Message.ToString(),
                    data = data0
                };
            }
            catch (Exception msg)
            {
                return new PP<ModelMessage>
                {
                    Success = false,
                    Message = msg.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelMessage> RejectSSIT(string Type, string UserId, string UserGroup, string ApprCat, string SSITCode, string SSITCount, string Amount, string AppVersion, string Reason)
        {
            try
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                var data0 = new ModelMessage();
                string query = "";
                if (Type == "SCRAP")
                {
                    query = @"EXEC sp_PP_REJECT_SCRAP_SSIT '" + Type + @"','" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + SSITCode + @"','" + SSITCount + @"','" + Amount + @"','" + AppVersion + @"','" + Reason + @"'";
                }
                else
                {
                    query = @"EXEC sp_PP_REJECT_PR_SSIT '" + Type + @"','" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + SSITCode + @"','" + SSITCount + @"','" + Amount + @"','" + AppVersion + @"','" + Reason + @"'";
                }
                using (var sql = new ClassMSSQL())
                {
                    ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
                }
                data0 = (from rw in ds.Tables[0].AsEnumerable()
                         select new ModelMessage
                         {
                             Message = rw["Message"].ToString(),
                         }).FirstOrDefault();

                return new PP<ModelMessage>
                {
                    Success = true,
                    Message = data0.Message.ToString(),
                    data = data0
                };
            }
            catch (Exception msg)
            {
                return new PP<ModelMessage>
                {
                    Success = false,
                    Message = msg.Message.ToString(),
                    data = null
                };
            }
        }

        public List<ModelSSIT> DetailSummarySSIT_SCRAP(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string query = @"EXEC sp_PP_GET_SUMMARY_SCAN_SCRAP_SSIT '" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + Product + @"','" + SSITCode + @"'";
            using (var sql = new ClassMSSQL())
            {
                ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
            }
            var _GetList = (from rw in ds.Tables[0].AsEnumerable()
                            select new ModelSSIT
                            {
                                Items = rw["Items"].ToString(),
                                SSIT_ID = rw["SSIT_ID"].ToString(),
                                Material = rw["Material"].ToString(),
                                MaterialDesc = rw["MaterialDesc"].ToString(),
                                DeptSubmitted = "",
                                Qty = rw["Qty"].ToString(),
                                BaseUOM = rw["BaseUOM"].ToString(),
                                UnitPrice = rw["UnitPrice"].ToString(),
                                Amount = rw["Amount"].ToString(),
                                Currency = rw["Currency"].ToString(),
                                SubmitBy = rw["SubmitBy"].ToString(),
                                SubmitDate = rw["SubmitDate"].ToString(),
                                NCCode = rw["NCCode"].ToString(),
                                NCCodeDet = rw["NCCodeDet"].ToString(),
                                NCDetail = rw["NCDetail"].ToString(),
                                RootCause = rw["RootCause"].ToString(),
                                CanApproved = rw["CanApproved"].ToString(),
                            }).ToList();
            return _GetList;
        }
        //https://sbm-apps.dev/POCKETPAL-API/SSIT/GetCountDataSSIT_BY_USERGROUP?UserId=Burhanis&UserGroup=SBM QC&ApprCat=2&Product=&SSITCode=
        public List<ModelSSIT> DetailSummarySSIT_PR(string Type, string UserId, string UserGroup, string ApprCat, string Product, string SSITCode)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            string query = @"EXEC sp_PP_GET_SUMMARY_SCAN_PR_SSIT '" + UserId + @"','" + UserGroup + @"','" + ApprCat + @"','" + Product + @"','" + SSITCode + @"'";
            using (var sql = new ClassMSSQL())
            {
                ds = sql.ExecDSQuery(ConnectionDBSSIT, query, null, null, false);
            }
            var _GetList = (from rw in ds.Tables[0].AsEnumerable()
                            select new ModelSSIT
                            {
                                Items = rw["Items"].ToString(),
                                SSIT_ID = rw["SSIT_ID"].ToString(),
                                Material = rw["Material"].ToString(),
                                MaterialDesc = rw["MaterialDesc"].ToString(),
                                DeptSubmitted = "",
                                Qty = rw["Qty"].ToString(),
                                BaseUOM = rw["BaseUOM"].ToString(),
                                UnitPrice = rw["UnitPrice"].ToString(),
                                Amount = rw["Amount"].ToString(),
                                Currency = rw["Currency"].ToString(),
                                SubmitBy = rw["SubmitBy"].ToString(),
                                SubmitDate = rw["SubmitDate"].ToString(),
                                NCCode = rw["NCCode"].ToString(),
                                NCCodeDet = rw["NCCodeDet"].ToString(),
                                NCDetail = rw["NCDetail"].ToString(),
                                RootCause = rw["RootCause"].ToString(),
                                CanApproved = rw["CanApproved"].ToString(),
                            }).ToList();
            return _GetList;
        }

    }
}