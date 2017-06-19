using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class CSVFile
    {
        public int Id { get; set; }
        public string TestDevice { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}