﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using License.Core.Model;
using License.Logic.ServiceLogic;
using License.Model;
using Microsoft.AspNet.Identity;

namespace License.WebAPIService.Controller
{
    [RoutePrefix("api/User")]
    public class UserAPIController : BaseApiController
    {
        private UserLogic logic = null;

        public UserAPIController()
        {
            logic = new UserLogic();
        }
        [Route("All")]
        [HttpGet]
        public IHttpActionResult GetUsers()
        {
            //if (logic.UserManager == null)
            //    logic.UserManager = UserManager;
            //if (logic.RoleManager == null)
            //    logic.RoleManager = RoleManager;
            return Ok(logic.GetUsers());
        }

        [Route("Create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(Registration user)
        {
            //if (logic.UserManager == null)
            //    logic.UserManager = UserManager;
            //if (logic.RoleManager == null)
            //    logic.RoleManager = RoleManager;
            var result = logic.CreateUser(user);
            if (result)
                return Request.CreateResponse(HttpStatusCode.Created, "Sucess");
            //else
               // return this.GetErrorResult(result);
        }

        [Route("Update/{id}")]
        [HttpPut]
        public HttpResponseMessage UpdateRole(string id, User user)
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            IdentityResult result = logic.UpdateUser(id, user);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.OK, "Updated Successfuly");
            else
                return this.GetErrorResult(result);
        }

        [Route("Get/{id}")]
        [HttpGet]
        public IHttpActionResult GetUserById(string id)
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            var result = logic.GetUserById(id);
            return Ok(result);
        }

        [Route("Delete/{id}")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(string id)
        {
            if (logic.UserManager == null)
                logic.UserManager = UserManager;
            if (logic.RoleManager == null)
                logic.RoleManager = RoleManager;
            IdentityResult result = logic.DeleteUser(id);
            if (result.Succeeded)
                return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfuly");
            else
                return this.GetErrorResult(result);
        }
    }
}