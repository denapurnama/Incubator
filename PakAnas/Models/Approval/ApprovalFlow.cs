using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PakAnas.Commons.Models;

namespace PakAnas.Models
{
    public class ApprovalFlow : BaseModel
    {
        public long APPROVAL_FLOW_ID { get; set; }
        public long APPROVAL_ID { get; set; }
        public int SEQ { get; set; }
        public string APPROVER_TYPE { get; set; }
        public int ORG_ID { get; set; }
        public string po_no { get; set; }
        public string request_qty { get; set; }
        public string unit_price { get; set; }
        public string item_code { get; set; }
        public string supplier_code { get; set; }
        public string supplier_name { get; set; }
        public int? STD_LT { get; set; }
    }
}