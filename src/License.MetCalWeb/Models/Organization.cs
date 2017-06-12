using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class Organization
    {
        public int Id { get; set; }
        [DisplayName("Organization Name")]
        public string Name { get; set; }

    }
}