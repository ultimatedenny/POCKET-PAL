using System;
using System.Linq;
using System.Configuration;
using System.Data;
using POCKETPAL_API.Class;
using POCKETPAL_API.Models;
using log4net;
using System.Globalization;

namespace POCKETPAL_API.Actions
{
    public class ActionUser
    {
        DataTable dt;
        public string POCKETPALCON = ConfigurationManager.ConnectionStrings["POCKETPALCON"].ToString();
        ILog Logging = LogManager.GetLogger(typeof(ActionUser));
        public void CreateLog(string message)
        {
            Logging.Info(message.ToUpper());
        }
        public PP<ModelResponse> GetIsUserVerified(string UserId)
        {
            try
            {
                CreateLog("Entering GetIsUserVerified...");
                DataTable dt = new DataTable();
                string query;
                query = @"EXEC PP_GET_IS_USER_VERIFIED '" + UserId + "'";
                CreateLog(query);
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                }
                var Result = (from rw in dt.AsEnumerable()
                             select new ModelResponse
                             {
                                 Message = rw["Message"].ToString(),
                                 Success = rw["Success"].ToBoolean()

                             }).FirstOrDefault();
                CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                CreateLog(Result.Message.ToString());
                if (Result.Success == true)
                {
                    return new PP<ModelResponse>
                    {
                        Success = true,
                        Message = ClassString.USERAREVERIFIED.ToString(),
                        data = null
                    };
                }
                else
                {
                    return new PP<ModelResponse>
                    {
                        Success = false,
                        Message = ClassString.USERNOTVERIFIED.ToString(),
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelResponse> RequestVerifyUser(string UserId, string UseNam, string EmployeeBadge)
        {
            try 
            {
                CreateLog("Entering RequestVerifyUser...");
                DataTable dt = new DataTable();
                string query;
                query = @"EXEC PP_VERIFY_USER '" + UserId + "','" + UseNam + "','" + EmployeeBadge + "'";
                CreateLog(query);
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                }
                var Result = (from rw in dt.AsEnumerable()
                             select new ModelResponse
                             {
                                 Message = rw["Message"].ToString(),
                                 Success = rw["Success"].ToBoolean()
                             }).FirstOrDefault();
                CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                CreateLog(Result.Message.ToString());
                if (Result.Success == true)
                {
                    return new PP<ModelResponse>
                    {
                        Success = true,
                        Message = Result.Message.ToString(),
                        data = null
                    };
                }
                else
                {
                    return new PP<ModelResponse>
                    {
                        Success = false,
                        Message = Result.Message.ToString(),
                        data = null
                    };
                }
            }
            catch(Exception ex)
            {
                CreateLog(ex.ToString());
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelResponse> DeleteToken(string UserId, string UserToken)
        {
            try
            {
                CreateLog("Entering DeleteToken...");
                DataTable dt = new DataTable();
                string query;
                query = @"EXEC [PP_DELETE_TOKEN] '" + UserId + "','" + UserToken + "'";
                CreateLog(query);
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                }
                var Result = (from rw in dt.AsEnumerable()
                             select new ModelResponse
                             {
                                 Success = rw["SUCCESS"].ToBoolean(),
                                 Message = rw["MESSAGE"].ToString()
                             }).FirstOrDefault();
                CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                CreateLog(Result.Message.ToString());
                if (Result.Success == true)
                {
                    return new PP<ModelResponse>
                    {
                        Success = true,
                        Message = ClassString.SUCCESS.ToString(),
                        data = null
                    };
                }
                else
                {
                    return new PP<ModelResponse>
                    {
                        Success = false,
                        Message = ClassString.FAILED.ToString(),
                        data = null
                    };
                }
            }
            catch(Exception ex)
            {
                CreateLog(ex.ToString());
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelResponse> RegisterToken(string UserId, string UseNam, string BusFunc, string UserToken)
        {
            try
            {
                CreateLog("Entering RegisterToken...");
                string query;
                query = "EXEC [PP_CREATE_TOKEN] '" + UserId + "','" + UseNam + "','" + BusFunc + "','" + UserToken + "'";
                CreateLog(query);
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                }
                var Result = (from rw in dt.AsEnumerable()
                             select new ModelResponse
                             {
                                 Message = rw["MESSAGE"].ToString()
                             }).FirstOrDefault();

                CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                CreateLog(Result.Message.ToString());

                if (Result.Success == true)
                {
                    return new PP<ModelResponse>
                    {
                        Success = true,
                        Message = ClassString.SUCCESS.ToString(),
                        data = null
                    };
                }
                else
                {
                    return new PP<ModelResponse>
                    {
                        Success = false,
                        Message = ClassString.FAILED.ToString(),
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelResponse> AddPin(string UserId, string UserPin, string BusFunc, string UseNam)
        {
            try
            {
                CreateLog("Entering AddPin...");
                string query = "EXEC [PP_CREATE_PIN] '" + UserId + "','" + UserPin + "','" + BusFunc + "','" + UseNam + "'";
                CreateLog(query);
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                }
                var Result = (from rw in dt.AsEnumerable()
                             select new ModelResponse
                             {
                                 Success = rw["SUCCESS"].ToBoolean(),
                                 Message = rw["MESSAGE"].ToString()
                             }).FirstOrDefault();
                CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                CreateLog(Result.Message.ToString());
                if (Result.Success == true)
                {
                    return new PP<ModelResponse>
                    {
                        Success = true,
                        Message = ClassString.SUCCESS.ToString(),
                        data = null
                    };
                }
                else
                {
                    return new PP<ModelResponse>
                    {
                        Success = false,
                        Message = ClassString.FAILED.ToString(),
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelResponse> GetIsBusFuncAllowed(string BusFunc, string ScannedCode)
        {
            try
            {
                CreateLog("Entering GetIsBusFuncAllowed...");
                DataTable dt = new DataTable();
                string query;
                string _ScannedCode = "";
                if (ScannedCode.Contains("QR-EP"))
                {
                    _ScannedCode = "QR-EP";
                }
                else if (ScannedCode.Contains("QR-VMS"))
                {
                    _ScannedCode = "QR-VM";
                }
                else if (ScannedCode.Contains("JA-"))
                {
                    _ScannedCode = "QR-JA";
                }
                else
                {
                    _ScannedCode = ScannedCode;
                }
                query = @"EXEC [PP_GET_IS_BUSFUNC_ALLOWED] '" + BusFunc + "','" + _ScannedCode + "'";
                CreateLog(query);
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                }
                var Result = (from rw in dt.AsEnumerable()
                             select new ModelResponse
                             {
                                 Success = rw["SUCCESS"].ToBoolean(),
                                 Message = rw["MESSAGE"].ToString()
                             }).FirstOrDefault();
                CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                CreateLog(Result.Message.ToString());
                if (Result.Success == true)
                {
                    return new PP<ModelResponse>
                    {
                        Success = true,
                        Message = ClassString.SUCCESS.ToString(),
                        data = null
                    };
                }
                else
                {
                    return new PP<ModelResponse>
                    {
                        Success = false,
                        Message = ClassString.FAILED.ToString(),
                        data = null
                    };
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                return new PP<ModelResponse>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelUser> SignIn(string UseId, string UsePass, string DeviceName, string Model, string Manufacture, string OS)
        {
            try
            {
                CreateLog("Entering SignIn...");
                var _AuthSignIn = AuthSignIn(UseId, UsePass);
                CreateLog("AuthSignIn Success is " + Convert.ToString(_AuthSignIn) + "");
                if (_AuthSignIn == false)
                {
                    return new PP<ModelUser>
                    {
                        Success = false,
                        Message = ClassString.SIGNINFAILED.ToString(),
                        data = null
                    };
                }
                else
                {
                    CreateLog(ClassString.SIGNINFAILED.ToString());
                    string query;
                    DataTable dt = new DataTable();
                    query = @"EXEC [PP_UPDATE_LASTLOGIN] '" + UseId + "','" + DeviceName + "'";
                    CreateLog(query);
                    using (ClassMSSQL s = new ClassMSSQL())
                    {
                        dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                    }
                    var Result = (from row in dt.AsEnumerable()
                                 select new ModelResponse()
                                 {
                                     Success = row["Success"].ToBoolean(),
                                     Message = row["Message"].ToString()
                                 }).FirstOrDefault();
                    CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                    CreateLog(Result.Message.ToString());
                    if (Result.Success == true)
                    {
                        query = @"EXEC [PP_GET_USER_SUCCESS_LOGIN] '" + UseId + "'";
                        CreateLog(query);
                        using (ClassMSSQL s = new ClassMSSQL())
                        {
                            dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                        }
                        var data1 = (from row in dt.AsEnumerable()
                                     select new ModelUser()
                                     {
                                         ID = Convert.ToInt32(row["ID"]),
                                         PlantCode = row["PlantCode"].ToString(),
                                         UserID = row["UserID"].ToString(),
                                         UserName = row["UserName"].ToString(),
                                         Department = row["Department"].ToString(),
                                         Email = row["Email"].ToString(),
                                         Phone = row["Phone"].ToString(),
                                         BusinessFunction = row["BusinessFunction"].ToString(),
                                         Photo = row["Photo"].ToString(),
                                         LineID = row["LineID"].ToString(),
                                         IsActive = row["IsActive"].ToBoolean(),
                                         IsWindowsAuthentication = row["IsWindowsAuthentication"].ToBoolean(),
                                         LastLoginDevice = row["LastLoginDevice"].ToString(),
                                         LastLoginDate = Convert.ToDateTime(DateTime.Parse(row["LastLoginDate"].ToString()).Date).ToString("dd-MM-yyyy HH:mm tt"),
                                         DomainName = row["DomainName"].ToString(),
                                         Salutation = row["Salutation"].ToString(),
                                         UserPIN = row["UserPIN"].ToString(),
                                         //LastLoginDate = Convert.ToDateTime(DateTime.Parse(row["LastLoginDate"].ToString()).Date),
                                     }).FirstOrDefault();

                        query = @"EXEC [PP_UPDATE_USER_DEVICE_LOGIN]'" + UseId + "','" + DeviceName + "','" + OS + "','" + Model + "','" + Manufacture + "'";
                        CreateLog(query);
                        using (ClassMSSQL s = new ClassMSSQL())
                        {
                            dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                        }
                        ///-- AUTO VERIFY
                        if(data1.IsWindowsAuthentication == true)
                        {
                            CreateLog("AUTO VERIFY, data1 is true");
                            query = @"EXEC [PP_AUTO_VERIFY_USERWIN]'" + data1.UserID + "','" + data1.UserName + "'";
                            CreateLog(query);
                            using (ClassMSSQL s = new ClassMSSQL())
                            {
                                dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                            }
                        }
                        ///-- AUTO VERIFY
                        CreateLog(ClassString.SIGNINSUCCESSED.ToString());
                        return new PP<ModelUser>
                        {
                            Success = true,
                            Message = ClassString.SIGNINSUCCESSED.ToString(),
                            data = data1
                        };
                    }
                    else
                    {
                        return new PP<ModelUser>
                        {
                            Success = false,
                            Message = ClassString.SIGNINFAILED.ToString(),
                            data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                return new PP<ModelUser>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public PP<ModelUser> SignUp(ModelUser NewData, string Model, string Manufacture, string DeviceName, string OS)
        {
            try
            {
                CreateLog("Entering SignUp2...");
                var _AuthSignIn2 = AuthSignUp2(NewData);
                CreateLog("AuthSignIn2 Success is " + Convert.ToString(_AuthSignIn2) + "");
                if (_AuthSignIn2 == false)
                {
                    return new PP<ModelUser>
                    {
                        Success = false,
                        Message = ClassString.SIGN_UP_FAILED.ToString(),
                        data = null
                    };
                }
                else
                {
                    string query;
                    DataTable dt = new DataTable();
                    query = "EXEC [PP_CREATE_USERMDCDB] " +
                            "'" + NewData.PlantCode + "','" + NewData.UserID + "','" + NewData.UserName + "','" + NewData.Password + "','" + NewData.Department + "'," +
                            "'" + NewData.Email + "','" + NewData.Phone + "','" + NewData.Photo + "','" + NewData.LineID + "'," +
                            "" + NewData.IsWindowsAuthentication + ",'" + NewData.Salutation + "'," +
                            "'" + Model + "','" + Manufacture + "','" + DeviceName + "','" + OS + "'";
                    CreateLog(query);
                    using (ClassMSSQL s = new ClassMSSQL())
                    {
                        dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                    }
                    var Result = (from row in dt.AsEnumerable()
                                  select new ModelResponse()
                                  {
                                      Success = row["SUCCESS"].ToBoolean(),
                                      Message = row["MESSAGE"].ToString(),

                                  }).FirstOrDefault();
                    CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                    CreateLog(Result.Message.ToString());
                    if (Result.Success == true)
                    {
                        query = "EXEC [PP_CREATE_USERVMS] " +
                           "'" + NewData.PlantCode + "','" + NewData.UserID + "','" + NewData.UserName + "','" + NewData.Password + "','" + NewData.Department + "'," +
                           "'" + NewData.Email + "','" + NewData.Phone + "','" + NewData.Photo + "','" + NewData.LineID + "'," +
                           "" + NewData.IsWindowsAuthentication + ",'" + NewData.Salutation + "'";
                        CreateLog(query);
                        using (ClassMSSQL s = new ClassMSSQL())
                        {
                            dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                        }
                        var Result1 = (from row in dt.AsEnumerable()
                                       select new ModelResponse()
                                       {
                                           Success = row["SUCCESS"].ToBoolean(),
                                           Message = row["MESSAGE"].ToString(),

                                       }).FirstOrDefault();
                        CreateLog("Result1 Success is " + Convert.ToString(Result1.Success) + "");
                        CreateLog(Result1.Message.ToString());
                        if (Result1.Success == true)
                        {
                            if (NewData.IsWindowsAuthentication == true)
                            {
                                query = @"EXEC [PP_AUTO_VERIFY_USERWIN]'" + NewData.UserID + "','" + NewData.UserName + "'";
                                CreateLog(query);
                                using (ClassMSSQL s = new ClassMSSQL())
                                {
                                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                                }
                            }
                            return new PP<ModelUser>
                            {
                                Success = true,
                                Message = ClassString.SUCCESS.ToString(),
                                data = null
                            };
                        }
                        else
                        {
                            return new PP<ModelUser>
                            {
                                Success = false,
                                Message = ClassString.SIGN_UP_FAILED.ToString(),
                                data = null
                            };
                        }
                    }
                    else
                    {
                        return new PP<ModelUser>
                        {
                            Success = false,
                            Message = ClassString.SIGN_UP_FAILED.ToString(),
                            data = null
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                return new PP<ModelUser>
                {
                    Success = false,
                    Message = ex.Message.ToString(),
                    data = null
                };
            }
        }
        public bool AuthSignIn(string UseId, string UsePass)
        {
            try
            {
                CreateLog("Entering AuthSignIn...");
                ClassWinAuth _wa = new ClassWinAuth();
                string query;
                query = @"exec [PP_GET_IS_USER_EXISTS] '" + UseId + "'";
                CreateLog(query);
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                }
                var Result = (from row in dt.AsEnumerable()
                            select new ModelResponse() { Success = row["Success"].ToBoolean(), Message = row["Message"].ToString() }).FirstOrDefault();
                CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                CreateLog(Result.Message.ToString());
                if (Result.Success == true)
                {
                    query = @"exec [PP_GET_USER_EXIST] '" + UseId + "'";
                    CreateLog(query);
                    using (ClassMSSQL s = new ClassMSSQL())
                    {
                        dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                    }
                    var Result1 = (from row in dt.AsEnumerable()
                                 select new ModelUser()
                                 {
                                     UserID = row["USERID"].ToString(),
                                     Password = row["PASSWORD"].ToString(),
                                     DomainName = row["DOMAINNAME"].ToString(),
                                     IsWindowsAuthentication = row["ISWINDOWSAUTHENTICATION"].ToBoolean()
                                 }).FirstOrDefault();
                    CreateLog("Result1 IsWindowsAuthentication is " + Convert.ToString(Result1.IsWindowsAuthentication) + "");
                    CreateLog(Result1.IsWindowsAuthentication.ToString());
                    if (Result1.IsWindowsAuthentication == false)
                    {
                        query = @"EXEC [PP_GET_COMPARE_PASSWORD] '" + UseId + "','" + UsePass + "'";
                        CreateLog(query);
                        using (ClassMSSQL s = new ClassMSSQL())
                        {
                            dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                        }
                        var Result2 = (from row in dt.AsEnumerable() select new ModelResponse() { Success = row["Success"].ToBoolean(), Message = row["Message"].ToString() }).FirstOrDefault();
                        CreateLog("Result2 Success is " + Convert.ToString(Result2.Success) + "");
                        CreateLog(Result2.Message.ToString());
                        if (Result2.Success == true)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        CreateLog("Entering WinAuth...");
                        return _wa.WinAuth(UseId, UsePass, false, Result1.DomainName);
                    }
                }
                else
                {
                    CreateLog("Entering WinAuth...");
                    return false;
                }
            }
            catch(Exception ex) 
            {
                CreateLog(ex.ToString());
                return false;
            }
        }
        public bool AuthSignUp2(ModelUser NewData)
        {
            try
            {
                CreateLog("Entering SignUp2...");
                string query;
                DataTable dt = new DataTable();
                query = @"EXEC PP_IS_USER_REGISTERED '" + NewData.UserID + "'";
                CreateLog(query);
                using (ClassMSSQL s = new ClassMSSQL())
                {
                    dt = s.ExecDTQuery(POCKETPALCON, query, null, null, false);
                }
                var Result = (from row in dt.AsEnumerable()
                             select new ModelResponse()
                             {
                                 Success = row["SUCCESS"].ToBoolean(),
                                 Message = row["MESSAGE"].ToString(),

                             }).FirstOrDefault();
                CreateLog("Result Success is " + Convert.ToString(Result.Success) + "");
                CreateLog(Result.Message.ToString());
                if (Result.Success == true)
                {
                    if (NewData.IsWindowsAuthentication == true)
                    {
                        ClassWinAuth _wa = new ClassWinAuth();
                        return _wa.WinAuth(NewData.UserID, NewData.Password, false, NewData.DomainName);
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                CreateLog(ex.ToString());
                return false;
            }
        }
    }
}