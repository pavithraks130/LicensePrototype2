using License.Models;
using License.Logic.DataLogic;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
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
        public HttpResponseMessage UploadFile(dynamic urlLink)
        {
            HttpPostedFileBase upload = null;
            object obj = upload;
            if (upload.FileName.EndsWith(".csv"))
            {
                // Service call to save the data in Onpremise DB
                Stream stream = upload.InputStream;
                DataTable csvTable = new DataTable();
                List<CSVFile> listData = new List<CSVFile>();
                CsvReader csvReader = new CsvReader(new StreamReader(stream), true);
                {
                    csvTable.Load(csvReader);
                }
                foreach (DataRow row in csvTable.Rows)
                {
                    CSVFile cSVobj = new CSVFile();
                    cSVobj.Id = 1;
                    cSVobj.SerialNumber = row[csvTable.Columns[0]] as string;
                    cSVobj.Description = row[csvTable.Columns[1]] as string;
                    listData.Add(cSVobj);
                }
                var listObj = _CSVFileLogic.CreateCSVData(listData);
                if (listObj != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "success");
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, _CSVFileLogic.ErrorMessage);
        }
    }
}