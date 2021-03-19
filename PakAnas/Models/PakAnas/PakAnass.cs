using PakAnas.Commons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PakAnas.Models.PakAnas
{
    public class PakAnass : BaseModel
    {
        public string org_code { get; set; }
        public string org_name { get; set; }
        public string create_by { get; set; }
        public string create_on { get; set; }
        public string update_by { get; set; }
        public string update_on { get; set; }
    }
}