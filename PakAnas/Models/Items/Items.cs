using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using PakAnas.Commons.Models;
using PakAnas.Commons.Helpers;

namespace PakAnas.Models.Items
{
    public class Items  : BaseModel
    {
        public string ITEM_CODE { get; set; }
        public string ITEM_NAME { get; set; }
        public string ITEM_UM { get; set; }

        public string UPLOAD_FNAME { get; set; }
        public string UPLOAD_PATH { get; set; }
        public string UPLOAD_HALF_PATH { get; set; }
    }

}