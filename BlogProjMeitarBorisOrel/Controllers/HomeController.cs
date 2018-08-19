using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BlogProjMeitarBorisOrel.Models;
using BlogProjMeitarBorisOrel.Data;
using Microsoft.AspNetCore.Authorization;


using System.Web;



using Newtonsoft.Json;

namespace BlogProjMeitarBorisOrel.Controllers
{
    public class HomeController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    //public class HomeController : Controller

    //{

    //    //

    //    // GET: /Home/




    //    public ActionResult Index()

    //    {

    //        return View();

    //    }




    //    private Uri RediredtUri

    //    {

    //        get

    //        {

    //            var uriBuilder = new UriBuilder();

    //            uriBuilder.Query = null;

    //            uriBuilder.Fragment = null;

    //            uriBuilder.Path = Url.Action("FacebookCallback");

    //            return uriBuilder.Uri;

    //        }

    //    }




    //    [AllowAnonymous]

    //    public ActionResult Facebook()

    //    {

    //        var fb = new FacebookClient();

    //        var loginUrl = fb.GetLoginUrl(new

    //        {




    //            client_id = "249043415725802",

    //            client_secret = "6d75928bcda675afbd25a9af20efbe5f",

    //            redirect_uri = RediredtUri.AbsoluteUri,

    //            response_type = "code",

    //            scope = "email"



    //        });

    //        return Redirect(loginUrl.AbsoluteUri);

    //    }




    //    public ActionResult FacebookCallback(string code)

    //    {

    //        var fb = new FacebookClient();

    //        dynamic result = fb.Post("oauth/access_token", new

    //        {

    //            client_id = "Your App ID",

    //            client_secret = "Your App Secret key",

    //            redirect_uri = RediredtUri.AbsoluteUri,

    //            code = code




    //        });

    //        var accessToken = result.access_token;

    //        //Session["AccessToken"] = accessToken;

    //        fb.AccessToken = accessToken;

    //        dynamic me = fb.Get("me?fields=link,first_name,currency,last_name,email,gender,locale,timezone,verified,picture,age_range");

    //        string email = me.email;

    //        TempData["email"] = me.email;

    //        TempData["first_name"] = me.first_name;

    //        TempData["lastname"] = me.last_name;

    //        TempData["picture"] = me.picture.data.url;

    //        //FormsAuthentication.SetAuthCookie(email, false);

    //        return RedirectToAction("Index", "Home");

    //    }




    //}
}
