using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using License.Models;
namespace License.MetCalWeb.Models
{
    public class TeamExtended: Team
    {
        public bool IsSelected { get; set; }
    }

}