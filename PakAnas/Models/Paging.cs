using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CrudSystem.Models
{
    public class Paging
    {
        
        public int CountData { get; set;}
        public int StartData { get; set; }
        public int EndData { get; set; }
        public int PositionPage { get; set; }
        public int DataPerPage { get; set; }

        //add by septareosagita@gmail.com 28 april 2015
        int mxpg = 3;
        public double CountPage { get; set; }
        public int First { get; set; }
        public int Last { get; set; }
        public int Next { get; set; }
        public int Prev { get; set; }
        public int MaxPage { get; set; }
        public int MinPage { get; set; }
        //

        public int Length { get; set; }

        public int MaxRange { get; set; }
        public int MinRange { get; set; }

        public List<int> ListIndex { get; set; }
        
        public Paging(int countdata, int positionpage, int dataperpage)
        {  
            List<int> list=new List<int>();
            EndData = positionpage*dataperpage;
            CountData = countdata;
            PositionPage = positionpage;
            DataPerPage = dataperpage;
            StartData = (positionpage-1)*dataperpage+1;

            CountPage = Math.Ceiling((double)countdata / dataperpage);
            First = 1;
            Last = (int)CountPage;
            Next = positionpage < (int)CountPage ? positionpage + 1 : (int)CountPage;
            Prev = positionpage == 1 ? 1 : positionpage - 1;
            MaxPage = positionpage + mxpg;
            MinPage = positionpage - mxpg;

            MinRange = Math.Max(MinPage, positionpage - 1);
            MaxRange = Math.Min(MaxPage, positionpage + 1);

            Double jml = Math.Ceiling((Double)countdata / (Double)dataperpage);
            for (int i = 1; i <= jml; i++)
            {
                list.Add(i);
            }
            ListIndex = list;

            //add by septareosagita@gmail.com 28 april 2015
            CountPage = Math.Ceiling((double)countdata / dataperpage);
            First = 1;
            Last = (int)CountPage;
            Next = positionpage < (int)CountPage ? positionpage + 1 : (int)CountPage;
            Prev = positionpage == 1 ? 1 : positionpage - 1;
            MaxPage = positionpage + mxpg;
            MinPage = positionpage - mxpg;
            //
        }

        public Paging()
        {
            // TODO: Complete member initialization
        }
    }
}