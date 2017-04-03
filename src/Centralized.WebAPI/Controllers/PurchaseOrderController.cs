using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;
using LicenseServer.DataModel;

namespace Centralized.WebAPI.Controllers
{
    [RoutePrefix("api/purchaseorder")]
    public class PurchaseOrderController : BaseController
    {
        private PurchaseOrderLogic logic = null;
        public PurchaseOrderController()
        {
            logic = new PurchaseOrderLogic();
        }
        
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetPurchaseOrder()
        {
            var orderList = logic.GetAllPendingPurchaseOrder();
            return Ok(orderList);
        }

        [HttpGet]
        [Route("OrderByUser/{userid}")]
        public IHttpActionResult GetPurchaseOrderByUser(string userid)
        {
            var orderList = logic.GetPurchaseOrderByUser(userid);
            return Ok(orderList);
        }

        [HttpGet]
        [Route("OrderById/{id}")]
        public HttpResponseMessage GetOrderDetails(int id)
        {
            var poOrder = logic.GetPurchaseOrderById(id);
            if (poOrder != null)
                return Request.CreateResponse(HttpStatusCode.OK, poOrder);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public HttpResponseMessage DeletePurchaseOrder(int id)
        {
            var status = logic.DeletePurchaseOrder(id);
            if (status)
                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPut]
        [Route("update/{id}")]
        public HttpResponseMessage UpdatePurchaseOrder(int id, PurchaseOrder order)
        {
            var obj = logic.UpdatePurchaseOrder(id, order);
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPut]
        [Route("UpdataMuliplePO")]
        public HttpResponseMessage UpdateMultiplePurchaseOrder(List<PurchaseOrder> orders)
        {
           logic.UpdatePurchaseOrder(orders);
            if (String.IsNullOrEmpty(logic.ErrorMessage))
                return Request.CreateResponse(HttpStatusCode.OK, "success");
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        [HttpPost]
        [Route("create")]
        public HttpResponseMessage CreatePurchaseOrder(PurchaseOrder order)
        {
            var obj = logic.CreatePurchaseOrder(order);
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }
    }
}
