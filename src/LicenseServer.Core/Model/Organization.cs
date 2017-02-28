using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace LicenseServer.Core.Model
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
