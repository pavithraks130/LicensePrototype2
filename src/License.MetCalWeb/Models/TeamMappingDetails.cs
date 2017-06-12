using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.MetCalWeb.Models
{
    public class TeamMappingDetails
    {
        public int TeamID { get; set; }
        public int ConcurrentUserCount { get; set; }
        public string SelectedTeamName { get; set; }

        public List<ProductDetails> ProductList { get; set; }
    }
}