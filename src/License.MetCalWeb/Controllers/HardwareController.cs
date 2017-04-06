using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
			
			//TODO: Need to add actual code
			return hm;
		}

        public ActionResult EditHardware(int id)
        {
            
            return PartialView(null);
        }
		public ActionResult AssetConfiguration(int id, string actionType)
		{
			//TeamAssetLogic logic = new TeamAssetLogic();
			//switch (actionType)
			//{
			//	case "Admin":
			//		//logic.SetAsAdmin(id, userId, true);
			//		break;
			//	case "EditAsset":
			//		//logic.SetAsAdmin(id, userId, false);
			//		break;
			//	case "Remove":
			//		//logic.DeleteTeamMember(id);
			//		break;
			//}
			return RedirectToAction("TeamContainer");
		}
	}
}