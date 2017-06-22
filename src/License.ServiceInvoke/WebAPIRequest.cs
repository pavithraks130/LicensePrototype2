using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.ServiceInvoke
{
    public class WebAPIRequest
    {
        public string Id { get; set; }
        public Method InvokeMethod { get; set; }
        public ServiceType ServiceType { get; set; }
        public Modules ServiceModule { get; set; }
        public string Functionality { get; set; }
        public string UrlObjType { get; set; }
        public string JsonData { get; set; }
        public string AdminId { get; set; }


    }

    public class WebAPIResponse
    {
        public bool Status { get; set; }

        public string ResponseData { get; set; }

        public ResponseFailure Error { get; set; }
    }
}
