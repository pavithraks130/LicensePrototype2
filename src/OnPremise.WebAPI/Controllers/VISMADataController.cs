using License.Logic.DataLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Models;

namespace OnPremise.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/VISMAData")]
    public class VISMADataController : BaseController
    {
        VISMADataLogic _VISMADataLogic = null;
        public VISMADataController()
        {
            _VISMADataLogic = new VISMADataLogic();
        }

        /// <summary>
        /// POST Method: Used to Update the products in bulk
        /// </summary>
        /// <param name="VISMA data List"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFile")]
        public HttpResponseMessage UploadFile(URLData data)
        {
            List<VISMAData> _VISMAData = new List<VISMAData>();
            try
            {           
            string[] rows = File.ReadAllLines(@data.Url);
            List<VISMAData> listObj = null;
            char charector = Convert.ToChar(data.Delimiter[0]);
            if (rows.Length > 1)
            {
                foreach (string row in rows.Skip(1))
                {
                    //data.Delimiter[0] -it is taking single charector as delimiter
                    String[] rowItems = row.Split(Convert.ToChar(data.Delimiter[0]));
                    VISMAData _VISMAObj = new VISMAData();
                    _VISMAObj.TestDevice = rowItems[0];
                    _VISMAObj.ExpirationDate = DateTime.Parse(rowItems[1], new CultureInfo("en-US", true));
                    _VISMAData.Add(_VISMAObj);
                }

                 listObj = _VISMADataLogic.CreateVISMAData(_VISMAData);
            }
            if (listObj != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            }
            }
            catch (Exception)
            {
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Error in Input file...");
            }
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, _VISMADataLogic.ErrorMessage);
        }

        [HttpGet]
        [Route("GetAllVISMAData")]
        public HttpResponseMessage GetVISMAData()
        {
            List<VISMAData> _VISMAData = new List<VISMAData>();
            _VISMAData = _VISMADataLogic.GetAllVISMAData();
            if (_VISMAData.Count >= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _VISMAData);
            }
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Error in retrieve data");
        }

        [HttpGet]
        [Route("GetVISMADataByTestDevice/{testDevice}")]
        public HttpResponseMessage GetVISMADataByTestDevice(string testDevice)
        {
            List<VISMAData> _VISMAData = new List<VISMAData>();
            _VISMAData = _VISMADataLogic.GetVISMADataByTestDevice(testDevice);
            if (_VISMAData.Count >= 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, _VISMAData);
            }
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, "Error in retrieve data");
        }
    }
}
