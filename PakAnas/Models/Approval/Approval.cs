using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PakAnas.Commons.Models;

namespace PakAnas.Models
{
    public class Approval : BaseModel
    {
        // untuk tampung data header
        public string pr_no { get; set; }
        public string po_no { get; set; }
        public DateTime? po_date { get; set; }
        public string supplier_code { get; set; }
        public string supplier_name { get; set; }
        public string req_org { get; set; }
        public DateTime? create_on { get; set; }
        public DateTime? VALID_TO_DT { get; set; }

        public string PO_DATE_STR { get; set; }
        public string VALID_FROM_DT_STR { get; set; }
        public string VALID_TO_DT_STR { get; set; }
        public string FUNCTION_NAME { get; set; }

        // untuk tampung data detail
        public IList<ApprovalFlow> listApprovalFlow { get; set; }
    }

    // untuk tampung data combobox buyer modal add edit
    public class Buyer
    {
        public string po_no { get; set; }
        public string item_code { get; set; }
        public string request_qty { get; set; }
        public string unit_price { get; set; }
    }

    // untuk tampung data combobox function modal add edit
    public class ApprovalSystem
    {
        public string SYSTEM_TYPE { get; set; }
        public string SYSTEM_CD { get; set; }
        public string SYSTEM_TYPE_CD { get; set; }
        public string SYSTEM_VALUE_TXT { get; set; }
        public decimal? SYSTEM_VALUE_NUM { get; set; }
        public DateTime? SYSTEM_VALUE_DT { get; set; }
        public int SEQ { get; set; }
    }

    // untuk tampung data combobox org modal add edit
    public class OrgHierarchy
    {
        public string supplier_code { get; set; }
        public string supplier_name { get; set; }
        public int ORG_ID { get; set; }
        public string ORG_TITLE { get; set; }
    }
}