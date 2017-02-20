using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace License.WebAPI
{
    public class ServiceResponse
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}