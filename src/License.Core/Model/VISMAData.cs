using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Core.Model
{
    public class VISMAData 
    {
        [Key]
        public int Id { get; set; }
        public string TestDevice { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
