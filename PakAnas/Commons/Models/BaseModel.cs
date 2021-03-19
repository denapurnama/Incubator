using System;

namespace PakAnas.Commons.Models
{
    public class BaseModel
    {
        public string CREATED_BY { get; set; }
        public DateTime? CREATED_DT { get; set; }
        public string CHANGED_BY { get; set; }
        public DateTime? CHANGED_DT { get; set; }

        public string CREATED_DT_STR { get; set; }
        public string CHANGED_DT_STR { get; set; }
        //[DefaultValue(1)]
        public int CurrentPage { get; set; }
        //[DefaultValue(10)]
        public int RowsPerPage { get; set; }
    }
}