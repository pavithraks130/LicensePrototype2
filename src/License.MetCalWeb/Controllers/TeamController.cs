using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using License.MetCalWeb;
using License.MetCalWeb.Common;
using License.MetCalWeb.Models;
using System.Collections;
using System.Net.Http;
using Newtonsoft.Json;

namespace License.MetCalWeb.Controllers
{
    /// <summary>
    /// Controller responsible for performing action related to Team
    /// </summary>
    [Authorize]
    [SessionExpire]
    public class TeamController : BaseController
    {
        public TeamController()
        {

        }

        /// <summary>
        /// GET action , returns view with the List of team
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Text = "1", Value = "1" });
            items.Add(new SelectListItem() { Text = "2", Value = "2" });
            items.Add(new SelectListItem() { Text = "3", Value = "3" });
            items.Add(new SelectListItem() { Text = "4", Value = "4" });
            items.Add(new SelectListItem() { Text = "5", Value = "5" });
            items.Add(new SelectListItem() { Text = "6", Value = "6" });
            items.Add(new SelectListItem() { Text = "7", Value = "7" });
            items.Add(new SelectListItem() { Text = "8", Value = "8" });
            items.Add(new SelectListItem() { Text = "9", Value = "9" });
            items.Add(new SelectListItem() { Text = "10", Value = "10" });
            ViewBag.UserCountList = items;
            string userId = string.Empty;
            if (!LicenseSessionState.Instance.IsSuperAdmin)
                userId = LicenseSessionState.Instance.User.UserId;
            var teamList = OnPremiseSubscriptionLogic.GetTeamList(userId);
            LicenseSessionState.Instance.TeamList = teamList;
            return View(LicenseSessionState.Instance.TeamList);
        }

        /// <summary>
        /// Get Action , return view for the New Team creation
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateTeam()
        {
            return View("Create");
        }

        /// <summary>
        /// POST Action , triggere when Create form submitted return Jsion response based on the service Call response.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CreateTeam(Team model)
        {
            if (ModelState.IsValid)
            {
                if (LicenseSessionState.Instance.IsSuperAdmin)
                    model.AdminId = LicenseSessionState.Instance.User.UserId;
                else if (LicenseSessionState.Instance.IsAdmin)
                    model.AdminId = LicenseSessionState.Instance.AppTeamContext.AdminId;
                    
                HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
                var response = client.PostAsJsonAsync("api/Team/Create", model).Result;
                if (response.IsSuccessStatusCode)
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<Team>(jsonData);
                    LicenseSessionState.Instance.TeamList.Add(data);
                    return Json(new { success = true, message = "" });
                }
                else
                {
                    var jsonData = response.Content.ReadAsStringAsync().Result;
                    var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                    ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);

                }
            }
            var _message = string.Join(Environment.NewLine, ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage));
            return Json(new { success = false, message = _message });
        }

        /// <summary>
        /// GET Action return view with list of products to map to the team .
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public ActionResult AssignLicense(int teamId)
        {
            TeamMappingDetails teamMappingDetails = new TeamMappingDetails();
            var team = LicenseSessionState.Instance.TeamList.ToList().Where(t => t.Id == teamId).FirstOrDefault();
            teamMappingDetails.TeamID = teamId;
            teamMappingDetails.ConcurrentUserCount = team.ConcurrentUserCount;
            teamMappingDetails.SelectedTeamName = team.Name;
            teamMappingDetails.ProductList = OnPremiseSubscriptionLogic.GetProductsFromSubscription().ToList();
            return View(teamMappingDetails);

        }

        /// <summary>
        /// POST Action triggered when assign License form Subsmitted for mapping the products to team. These are updated to db by making service call . 
        /// These product license will be available to user by default when they login based on the  team context logged in and based on the concurrent user login.
        /// </summary>
        /// <param name="teamMappingDetails"></param>
        /// <param name="selectedSubscription"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignLicense(TeamMappingDetails teamMappingDetails, params string[] selectedProducts)
        {
            var responseData = UpdateLicense(teamMappingDetails, selectedProducts);
            if (!String.IsNullOrEmpty(responseData))
            {
                ModelState.AddModelError("", responseData);
                var team = LicenseSessionState.Instance.TeamList.ToList().Where(t => t.Id == teamMappingDetails.TeamID).FirstOrDefault();
                teamMappingDetails.SelectedTeamName = team.Name;
                return View(teamMappingDetails);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Function to update the product License to team  for the specified number of Concurrent User Count
        /// </summary>
        /// <param name="teamMappingDetails"></param>
        /// <param name="selectedSubscription"></param>
        /// <returns></returns>
        public string UpdateLicense(TeamMappingDetails teamMappingDetails, string[] selectedProducts)
        {

            Team team = new Team();
            team.Id = teamMappingDetails.TeamID;
            team.ConcurrentUserCount = teamMappingDetails.ConcurrentUserCount;
            List<ProductLicense> proLicList = ExtractLicenseData(selectedProducts);
            TeamLicenseDataMapping mapping = new TeamLicenseDataMapping() { LicenseDataList = proLicList, TeamList = new List<Team>() { team } };
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PostAsJsonAsync("api/TeamLicense/Create", mapping).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                return response.ReasonPhrase + " - " + obj.Message;
            }
            return String.Empty;
        }

        /// <summary>
        /// Returns the list of Products based on the product id sent as input parameter
        /// </summary>
        /// <param name="SelectedSubscription"></param>
        /// <returns></returns>
        private static List<ProductLicense> ExtractLicenseData(string[] selectedProducts)
        {
            List<ProductLicense> proList = new List<ProductLicense>();
            foreach (var data in selectedProducts)
            {
                var prodId = data.Split(new char[] { ':' })[1];
                proList.Add(new ProductLicense() { ProductId = Convert.ToInt32(prodId) });
            }
            return proList;
        }

        /// <summary>
        /// Return List of Subscrription  with Products
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IList<Subscription> GetLicenseListBySubscription(string userId)
        {
            TempData["UserId"] = userId;
            //Logic to get the Subscription details Who are Team Member and Role is assigned as admin by the Super admin
            string adminUserId = string.Empty;
            IList<Subscription> licenseMapModelList = OnPremiseSubscriptionLogic.GetSubscriptionForLicenseMap(userId, adminUserId);
            return licenseMapModelList;
        }

        /// <summary>
        /// Get Action Return view for editing the team by displaying the existing data for the team Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult EditTeam(int id)
        {
            var team = LicenseSessionState.Instance.TeamList.FirstOrDefault(t => t.Id == id);
            return View("Edit", team);
        }

        /// <summary>
        /// POSt Action , triggers wen form submitted , update the modified team details to DB by making service Call
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditTeam(int id, Team model)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PutAsJsonAsync("api/team/Update/" + id, model).Result;
            if (response.IsSuccessStatusCode)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var teamObj = JsonConvert.DeserializeObject<Team>(data);
                LicenseSessionState.Instance.TeamList.RemoveAll(f => f.Id == id);
                LicenseSessionState.Instance.TeamList.Add(teamObj);
                return RedirectToAction("Index");
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            var _message = string.Join(Environment.NewLine, ModelState.Values
                                      .SelectMany(x => x.Errors)
                                      .Select(x => x.ErrorMessage));
            return Json(new { success = false, message = _message });
        }

        /// <summary>
        /// Get Action , to delete the selected team 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DeleteTeam(int id)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.DeleteAsync("api/team/Delete/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                LicenseSessionState.Instance.TeamList.RemoveAll(t => t.Id == id);
                if (LicenseSessionState.Instance.SelectedTeam != null && LicenseSessionState.Instance.SelectedTeam.Id == id)
                    LicenseSessionState.Instance.SelectedTeam = LicenseSessionState.Instance.TeamList.FirstOrDefault(s => s.IsDefaultTeam = true);
                return RedirectToAction("Index");
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                ModelState.AddModelError("", response.ReasonPhrase + " - " + obj.Message);
            }
            return View();
        }

        /// <summary>
        /// Get Action , return list of assigned product for the selected team 
        /// </summary>
        /// <param name="teamId"></param>
        /// <returns></returns>
        public ActionResult RevokeLicense(int teamId)
        {
            TeamDetails teamDetails = new TeamDetails()
            {
                Team = LicenseSessionState.Instance.TeamList.ToList().Where(t => t.Id == teamId).FirstOrDefault(),
                ProductList = OnPremiseSubscriptionLogic.GetTeamLicenseDetails(teamId)
            };
            return View(teamDetails);
        }


        /// <summary>
        /// POST Action trigered when the Revoke License submitted for the Product license Remova. Selected Product License will be removed  for the selected team
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="SelectedSubscription"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RevokeLicense(int teamId, params string[] selectedProducts)
        {
            TeamLicenseDataMapping teamLicDataMapping = new TeamLicenseDataMapping()
            {
                TeamList = new List<Team> { new Team() { Id = teamId } },
                LicenseDataList = ExtractLicenseData(selectedProducts)
            };

            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            var response = client.PostAsJsonAsync("api/TeamLicense/Revoke", teamLicDataMapping).Result;
            if (!response.IsSuccessStatusCode)
            {
                var jsondata = response.Content.ReadAsStringAsync().Result;
                var responsefailure = JsonConvert.DeserializeObject<ResponseFailure>(jsondata);
                return Json(new { success = false, message = responsefailure.Message });
            }
            return Json(new { success = true, message = "" });

        }

        /// <summary>
        /// GET Action , this action is called using Ajax  call when the concurrent user count is Updated by incresing or decreesing the count
        /// </summary>
        /// <param name="teamId"></param>
        /// <param name="noOfUser"></param>
        /// <returns></returns>
        public ActionResult UpdateConcurentUser(int teamId, int noOfUser)
        {
            HttpClient client = WebApiServiceLogic.CreateClient(ServiceType.OnPremiseWebApi);
            Team team = new Team() { Id = teamId, ConcurrentUserCount = noOfUser };
            var response = client.PostAsJsonAsync("api/team/UpdateConcurentUser", team).Result;
            if (response.IsSuccessStatusCode)
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var responsedata = JsonConvert.DeserializeObject<TeamConcurrentUserResponse>(jsonData);
                if (responsedata.UserUpdateStatus)
                {
                    team = LicenseSessionState.Instance.TeamList.FirstOrDefault(t => t.Id == teamId);
                    if (team != null)
                        team.ConcurrentUserCount = noOfUser;
                    return Json(new { success = true, message = "", OldUserCount = team.ConcurrentUserCount }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { success = false, message = responsedata.ErrorMessage, OldUserCount = responsedata.OldUserCount }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var jsonData = response.Content.ReadAsStringAsync().Result;
                var failureData = JsonConvert.DeserializeObject<ResponseFailure>(jsonData);
                team = LicenseSessionState.Instance.TeamList.FirstOrDefault(t => t.Id == teamId);
                return Json(new { success = false, message = failureData.Message, OldUserCount = team != null ? team.ConcurrentUserCount : 0 }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}