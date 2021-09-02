using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class UserInfo : BaseAuditModel
    {
        public string name { get; set; }
        public DateTime birthdate { get; set; }
        public int gender { get; set; }
        public string note { get; set; }
        public string address { get; set; }
        public string imageUrl { get; set; }
        public string phoneNumber { get; set; }
        public string email { get; set; }

        public override string objectCollection() => "UserInfo";
    }
}
