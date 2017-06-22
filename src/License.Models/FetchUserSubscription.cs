using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.Models
{
    public class FetchUserSubscription
    {
        public int TeamId { get; set; }
        public string UserId { get; set; }
        public bool IsFeatureRequired { get; set; }
    }
}
