using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace License.ServiceInvoke
{
    public class WebAPIRequest<T>
    {
        public string Id { get; set; }
        public Method InvokeMethod { get; set; }
        public ServiceType ServiceType { get; set; }
        public Modules ServiceModule { get; set; }
        public Functionality Functionality { get; set; }
        public T ModelObject { get; set; }
        public string AdminId { get; set; }
        public string AccessToken { get; set; }
    }

    public class WebAPIResponse<T>
    {
        public bool Status { get; set; }

        public T ResponseData { get; set; }

        public ResponseFailure Error { get; set; }
    }
}
