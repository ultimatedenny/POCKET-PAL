using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POCKETPAL_API.Models
{
    public class ModelUser
    {
        [Key]
        public int ID { get; set; }
        public string PlantCode { get; set; }
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string BusinessFunction { get; set; }
        public string Photo { get; set; }
        public string LineID { get; set; }
        public bool IsActive { get; set; }
        public bool IsWindowsAuthentication { get; set; }
        public string LastLoginDevice { get; set; }
        public string LastLoginDate { get; set; }
        public string DomainName { get; set; }
        public string Salutation { get; set; }
        public string UDID { get; set; }
        public string CreateUser { get; set; }
        public string CreateDate { get; set; }
        public string ChangeUser { get; set; }
        public string ChangeDate { get; set; }
        public string UserPIN { get; set; }

    }
}