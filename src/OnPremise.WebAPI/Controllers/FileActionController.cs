using License.DataModel;
using License.Logic.DataLogic;
using LumenWorks.Framework.IO.Csv;
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

namespace OnPremise.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/FileAction")]
    public class FileActionController : BaseController
    {
        CSVFileLogic _CSVFileLogic = null;
        public FileActionController()
        {
            _CSVFileLogic = new CSVFileLogic();
        }

        /// <summary>
        /// POST Method: Used to Update the products in bulk
        /// </summary>
        /// <param name="CSV data List"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("UploadFile")]
        public HttpResponseMessage UploadFile(URLData data)
        {
            List<CSVFile> csvListData = new List<CSVFile>();
            try
            {

           
            string[] rows = File.ReadAllLines(@data.Url);
            List<CSVFile> listObj = null;
            char charector = Convert.ToChar(data.Delimiter[0]);
            if (rows.Length > 1)
            {
                foreach (string row in rows.Skip(1))
                {
                    //data.Delimiter[0] -it is taking single charector as delimiter
                    String[] rowItems = row.Split(Convert.ToChar(data.Delimiter[0]));
                    CSVFile csvObj = new CSVFile();
                    csvObj.TestDevice = rowItems[0];
                    csvObj.ExpirationDate = DateTime.Parse(rowItems[1], new CultureInfo("en-US", true));
                    csvListData.Add(csvObj);
                }

                 listObj = _CSVFileLogic.CreateCSVData(csvListData);
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
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, _CSVFileLogic.ErrorMessage);
        }

    }
}
