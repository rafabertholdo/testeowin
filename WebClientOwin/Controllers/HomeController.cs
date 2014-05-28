using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Util;

namespace WebClientOwin.Controllers
{    
    public class HomeController : Controller
    {       
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.Token = ((ClaimsPrincipal)Thread.CurrentPrincipal).Claims.Where(e => e.Type == "urn:thinktecture:token").FirstOrDefault().Value;  //OAuthUtil.GetToken();
            return View();
        }

        

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}