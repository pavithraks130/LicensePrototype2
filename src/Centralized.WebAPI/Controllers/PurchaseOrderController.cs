using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LicenseServer.Logic;
using LicenseServer.DataModel;
using LicenseServer.Logic.BusinessLogic;

namespace Centralized.WebAPI.Controllers
{
    [Authorize]
    [RoutePrefix("api/purchaseorder")]
    public class PurchaseOrderController : BaseController
    {
        private PurchaseOrderLogic logic = null;
        public PurchaseOrderController()
        {
            logic = new PurchaseOrderLogic();
        }


        /// <summary>
        ///Get Method.  Get All the Purchase Order
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("All")]
        public IHttpActionResult GetPurchaseOrder()
        {
            var orderList = logic.GetAllPendingPurchaseOrder();
            return Ok(orderList);
        }

        /// <summary>
        /// GET Method. Get the Purchase Order List based on the UserId
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("OrderByUser/{userid}")]
        public IHttpActionResult GetPurchaseOrderByUser(string userid)
        {
            var orderList = logic.GetPurchaseOrderByUser(userid);
            return Ok(orderList);
        }

        /// <summary>
        /// GET Method. Gets the Purchase Order Details based on the Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Delete Method. Delete Purchase Order Based on ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// PUT Method. Update Purchase Order details  based on id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update/{id}")]
        public HttpResponseMessage UpdatePurchaseOrder(int id, PurchaseOrder order)
        {
            var obj = logic.UpdatePurchaseOrder(id, order);
            if (obj != null)
                return Request.CreateResponse(HttpStatusCode.OK, obj);
            else
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, logic.ErrorMessage);
        }

        /// <summary>
        /// PUT Method. Multiple Purchase Orders can be updated in single Call which is Bulk status Update
        /// </summary>
        /// <param name="orders"></param>
        /// <returns></returns>
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
        /// <summary>
        /// POST Method. Create Purchase Order with Details.
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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

        /// <summary>
        /// POST Method. Once the purchase Order are approved by Global Admin  based on the User Id user Subscription
        /// Records will be created for the purchase Order and Subscription and Product details will be provided
        /// to sync the Data to OnPremise DB.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SyncPO/{userId}")]
        public HttpResponseMessage SyncPurchaseOrder(string userId)
        {
            CartBO cartBOLogic = new CartBO()
            {
                UserManager = UserManager,
                RoleManager = RoleManager
            };
            var obj = cartBOLogic.SyncPurchaseOrder(userId);
            if (obj == null)
                return Request.CreateErrorResponse(HttpStatusCode.ExpectationFailed, cartBOLogic.ErrorMessage);
            else
                return Request.CreateResponse(HttpStatusCode.OK, obj);
        }
    }
}
