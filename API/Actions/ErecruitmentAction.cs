using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using POCKETPAL_API.Class;
using POCKETPAL_API.Models;
using log4net;

namespace POCKETPAL_API.Actions
{
    public class ErecruitmentAction
    {
        public string ConnectionDB = System.Configuration.ConfigurationManager.ConnectionStrings["POCKETPALCON"].ToString();
        public string ConnectionMDCDB = System.Configuration.ConfigurationManager.ConnectionStrings["MDCDB"].ToString();
        public string ConnectionDB3 = System.Configuration.ConfigurationManager.ConnectionStrings["DBTOKEN"].ToString();
        public string ConnectionDB4 = System.Configuration.ConfigurationManager.ConnectionStrings["DBVISITORMS"].ToString();
        
        ///DONE
        public List<ModelErecruitment> GetBatchList(string status, string BatchComp)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [spGetBatchListER] '"+ BatchComp + "','"+ status + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _GeBatchList = (from rw in dt.AsEnumerable()
                                select new ModelErecruitment
                                {
                                    BatchComp = rw["BatchComp"].ToString(),
                                    InvitationDate = rw["InvitationDate"].ToString(),
                                    RequestDate = (Convert.ToDateTime(rw["RequestDate"].ToString())).ToString("dd-MM-yyyy"),
                                    TotalCandidate = rw["TotalCandidate"].ToString(),
                                    StatusBatch = rw["StatusBatch"].ToString(),
                                }).ToList();
            return _GeBatchList;
        }
        ///
        ///DONE
        public List<ModelErecruitment> GetBatchStatus(string x)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [spGetBatchStatusER]";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _GetBatchStatus = (from rw in dt.AsEnumerable()
                                   select new ModelErecruitment
                                   {
                                       StatusBatch = rw["StatusBatch"].ToString(),
                                   }).ToList();
            return _GetBatchStatus;
        }
        ///
        ///DONE
        public List<ModelErecruitment> GetCandidateList(string BatchComp)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [spGetCandidateListER] '" + BatchComp + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _GeBatchList = (from rw in dt.AsEnumerable()
                                select new ModelErecruitment
                                {
                                    BatchEmp = rw["BatchEmp"].ToString(),
                                    NameEmp = rw["NameEmp"].ToString(),
                                    PhoneNumber = rw["PhoneNumber"].ToString(),
                                    DateOfBirthEmp = rw["DateOfBirthEmp"].ToString(),
                                    InvitationCodeEmp = rw["InvitationCodeEmp"].ToString(),
                                    StatusEmp = rw["StatusEmp"].ToString(),
                                }).ToList();
            return _GeBatchList;
        }
        //
        ///DONE
        public List<ModelErecruitment> GetEmpListBatch(string BatchComp, string NameEmp)
        {
            DataTable dt = new DataTable();
            string query = @"EXEC [spGetEmpListBatchER] '"+ BatchComp + "','"+ NameEmp + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _GeBatchList = (from rw in dt.AsEnumerable()
                                select new ModelErecruitment
                                {
                                    BatchEmp = rw["BatchEmp"].ToString(),
                                    NameEmp = rw["NameEmp"].ToString(),
                                    PhoneNumber = rw["PhoneNumber"].ToString(),
                                    DateOfBirthEmp = rw["DateOfBirthEmp"].ToString(),
                                    InvitationCodeEmp = rw["InvitationCodeEmp"].ToString(),
                                    StatusEmp = rw["StatusEmp"].ToString(),
                                }).ToList();
            return _GeBatchList;
        }
        //
        ///DONE
        public List<ModelErecruitment> GetEmpListBatchQR(string BatchEmp)
        {
            DataTable dt = new DataTable();
            string query = @"exec [spGetEmpListBatchQRER] '" + BatchEmp + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _GeBatchList = (from rw in dt.AsEnumerable()
                                select new ModelErecruitment
                                {
                                    BatchComp = rw["BatchComp"].ToString(),
                                    BatchEmp = rw["BatchEmp"].ToString(),
                                    NameEmp = rw["NameEmp"].ToString(),
                                    PhoneNumber = rw["PhoneNumber"].ToString(),
                                    DateOfBirthEmp = rw["DateOfBirthEmp"].ToString(),
                                    InvitationCodeEmp = rw["InvitationCodeEmp"].ToString(),
                                    StatusEmp = rw["StatusEmp"].ToString(),
                                    StatusBatch = rw["StatusBatch"].ToString(),
                                }).ToList();
            return _GeBatchList;
        }
        //
        ///DONE
        public List<ModelErecruitment> GetBatchInformation(string BatchComp)
        {
            DataTable dt = new DataTable();
            string query = @"SELECT 
                            COUNT(BATCHNO) As TOTALCANDIDATE,
                            SUM(CASE WHEN StatusEmp = '2200' THEN 1 ELSE 0 END) As PRESENT,
                            SUM(CASE WHEN StatusEmp = '2300' THEN 1 ELSE 0 END) As ABSENT,
                            SUM(CASE WHEN StatusEmp = '2100' THEN 1 ELSE 0 END) As INVITED,
                            SUM(CASE WHEN StatusEmp = '2700' THEN 1 ELSE 0 END) As NOTINVITED,
                            FORMAT((CONVERT(FLOAT,SUM(CASE WHEN STATUSEMP = 'PRESENT' THEN 1 ELSE 0 END))/CONVERT(FLOAT,COUNT(BATCHNO)))*100,'N2') AS PERCENTAGE
                            FROM DBERECRUITMENT.DBO.BATCHDETAIL
                            WHERE BATCHNO = '" + BatchComp + "' AND INVITATIONEMP IS NOT NULL GROUP BY BATCHNO, INVITATIONDATE";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _GetBatchInfomation = (from rw in dt.AsEnumerable()
                                       select new ModelErecruitment
                                       {
                                           TotalCandidate = rw["TOTALCANDIDATE"].ToString(),
                                           Present = rw["PRESENT"].ToString(),
                                           Absent = rw["ABSENT"].ToString(),
                                           Invited = rw["INVITED"].ToString(),
                                           NotInvited = rw["NOTINVITED"].ToString(),
                                           Percentage = rw["PERCENTAGE"].ToString() + "%"
                                       }).ToList();
            return _GetBatchInfomation;
        }
        //
        ///DONE
        public List<ModelResponse> UpdateBatch(string Status, string Batch, string UseId)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DataTable dt = new DataTable();
            string query = @"exec [spUpdateBatchER] '"+ Batch + "','" + UseId + "','" + date + "','" + Status + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _UpdateBatch = (from rw in dt.AsEnumerable()
                                select new ModelResponse
                                {
                                    Success = rw["Success"].ToBoolean(),
                                    Message = rw["Message"].ToString(),
                                }).ToList();
            return _UpdateBatch;
        }
        //
        ///DONE
        public List<ModelResponse> UpdateEmployee(string Status, string BatchEmp, string UseId)
        {
            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            DataTable dt = new DataTable();
            string query = @"exec [spUpdateEmployeeER] '"+ BatchEmp + "','" + UseId + "','" + date + "','" + Status + "'";
            using (var sql = new ClassMSSQL())
            {
                dt = sql.ExecDTQuery(ConnectionDB, query, null, null, false);
            }
            var _UpdateBatch = (from rw in dt.AsEnumerable()
                                select new ModelResponse
                                {
                                    Success = rw["Success"].ToBoolean(),
                                    Message = rw["Message"].ToString(),
                                }).ToList();
            return _UpdateBatch;
        }
    }
}