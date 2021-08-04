using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yased_Api.Models;

namespace Yased_Api.Controllers
{
    public class SearchController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Search
        public ActionResult Index(string searching)
        {
            var contents = db.Contents.Where(x => x.Title.Contains(searching) || x.Description_EN.Contains(searching)).ToList();
            var insights = db.Insights.Where(x => x.Title.Contains(searching) || x.Title_EN.Contains(searching)).ToList();
            var member = db.Members.Where(x => x.Title.Contains(searching) || x.Title_EN.Contains(searching)).ToList();
            var news = db.News.Where(x => x.Title.Contains(searching) || x.Title_EN.Contains(searching)).ToList();
            var uniteds = db.Uniteds.Where(x => x.Title.Contains(searching) || x.Title_EN.Contains(searching)).ToList();


            var my_jsondata = new
            {
                contents = contents,
                insights = insights,
                member = member,
                news = news,
                uniteds = uniteds
            };

            return Json(my_jsondata, JsonRequestBehavior.AllowGet);
        }
    }
}