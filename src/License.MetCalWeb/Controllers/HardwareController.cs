using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using License.Logic.ServiceLogic;
using System.Web.Mvc;
using License.MetCalWeb.Models;

namespace License.MetCalWeb.Controllers
{
    public class HardwareController : BaseController
    {
        // GET: Hardware
        public ActionResult HardwareContainer()
        {
			HardwareModel model = LoadHardware();
			return View(model);
        }

		private HardwareModel LoadHardware()
		{
			var hm = new HardwareModel();
            TeamAssetLogic logic = new TeamAssetLogic();
            hm.Assets = logic.GetAssets();
			return hm;
		}

        public ActionResult EditHardware(int id)
        {
            var obj = new Core.Model.TeamAsset { Name = "FC5222A", SerialNumber = "123", Description = "Calibrator" };
            return PartialView(obj);
        }
		public ActionResult AssetConfiguration(int id, string actionType)
		{
			TeamAssetLogic logic = new TeamAssetLogic();
			switch (actionType)
			{
				case "EditAsset":
					//logic.SetAsAdmin(id, userId, false);
					break;
				case "Remove":
					logic.RemoveAsset(id);
					break;
			}
			return RedirectToAction("HardwareContainer");
		}

        public ActionResult AddHardware()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddEditHardware(HardwareModel assetModel)
        {
            TeamAssetLogic logic = new TeamAssetLogic();
            logic.CreateAsset(assetModel.SelectedAsset);
            return RedirectToAction("HardwareContainer");
        }
	}
}