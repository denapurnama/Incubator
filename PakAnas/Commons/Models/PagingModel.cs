using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PakAnas.Commons.Models
{
    public class PagingModel
    {
        public const int DEFAULT_RECORD_PER_PAGE = 10;
        public static readonly int[] RECORD_PER_PAGES = new int[] { 5, 10, 25, 50, 100 };

        public long CountData { get; set; }
        public long Start { get; set; }
        public long End { get; set; }
        public int PositionPage { get; set; }
        public int RecordPerPage { get; set; }
        public IList<int> IndexList { get; set; }
        public string IdSuffix { get; set; }

        public PagingModel()
        {
            CountData = 0;
            PositionPage = 0;
            RecordPerPage = DEFAULT_RECORD_PER_PAGE;
            IndexList = new List<int>();
        }

        public PagingModel(long countData, int positionPage)
            : this(countData, positionPage, DEFAULT_RECORD_PER_PAGE, null)
        {

        }

        public PagingModel(long countData, int positionPage, string idSuffix)
            : this(countData, positionPage, DEFAULT_RECORD_PER_PAGE, idSuffix)
        {

        }

        public PagingModel(long countData, int positionPage, int recordPerPage)
            : this(countData, positionPage, recordPerPage, null)
        {

        }

        public PagingModel(long countData, int positionPage, int recordPerPage, string idSuffix)
        {
            List<int> list = new List<int>();
            CountData = countData;
            PositionPage = positionPage;
            RecordPerPage = recordPerPage;
            if (countData == 0)
            {
                Start = 0;
            }
            else
            {
                Start = (positionPage - 1) * recordPerPage + 1;
            }
            //berapa yg ditampilkan di gridnya
            End = positionPage * recordPerPage;
            //brp yg ditampilkan
            Double totalPage = (Math.Floor((Double)countData / (Double)recordPerPage));
            //Double total = 3;
            //Jika jumlah data / 10 > 0
            if (((Double)countData % (Double)recordPerPage > 0))
            {
                totalPage = totalPage + 1;
            }

            for (int i = 1; i <= totalPage; i++)
            {
                list.Add(i);
            }
            IndexList = list;

            //handle case when current page (Start) over then max data (CountData) [start]
            if (Start > CountData)
            {
                if (countData == 0)
                {
                    Start = 0;
                }
                else
                {
                    Start = ((int)totalPage - 1) * recordPerPage + 1;
                }
                End = Start + recordPerPage - 1;
                PositionPage = (int)totalPage;
            }
            //handle case when current page (Start) over then max data (CountData) [end]

            IdSuffix = idSuffix;
        }
    }
}