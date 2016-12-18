using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebDeviceMock.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index(string did)
        {
            if (string.IsNullOrEmpty(did)) did = "d2";
            ViewBag.did = did;
            return View();
        }
    }
}