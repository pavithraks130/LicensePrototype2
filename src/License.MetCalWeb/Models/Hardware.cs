using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using License.Models;

namespace License.MetCalWeb.Models
{
    public class Hardware
    {
        public List<TeamAsset> Assets { get; set; }

        public Hardware()
        {
            Assets = new List<TeamAsset>();
        }
    }
}
