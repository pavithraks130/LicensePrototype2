using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Core.Model
{
    public class CSVFile
    {
        [Key]
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string Description { get; set; }
    }
}
