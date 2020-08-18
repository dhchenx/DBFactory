using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DBFactory.Models
{
    // Author: Donghua Chen
    public class CommonPage
    {
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public DataTable DataTable { get; set; }
       
    }

    public class CommonPageWithCat
    {
        public int PageCount { get; set; }
        public int RecordCount { get; set; }
        public DataTable DataTable { get; set; }
        public DataTable CatTable { get; set; }
    }

    public class CommonPage4
    {
  
        public DataTable DataTable1 { get; set; }
        public DataTable DataTable2 { get; set; }
        public DataTable DataTable3 { get; set; }
        public DataTable DataTable4 { get; set; }

    }

}
