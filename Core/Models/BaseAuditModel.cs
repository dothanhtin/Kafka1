using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class BaseAuditModel
    {
        public string id { get; set; }
        public string createdBy { get; set; }
        public DateTime createdOn { get; set; }
        public string updatedBy { get; set; }
        public DateTime updatedOn { get; set; }
    }
}
