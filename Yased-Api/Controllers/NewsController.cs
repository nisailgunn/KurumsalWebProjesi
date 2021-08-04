using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Yased_Api.Models;

namespace Yased_Api.Controllers
{
    public class NewsController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: News
        public ActionResult Index()
        {
            return View(db.News.ToList());
        }

        // GET: News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: News/Create
        public ActionResult Create()
        {
            return View();
        }

        public string StringReplace(string text)
        {
            text = text.Replace("İ", "I");
            text = text.Replace("ı", "i");
            text = text.Replace("Ğ", "G");
            text = text.Replace("ğ", "g");
            text = text.Replace("Ö", "O");
            text = text.Replace("ö", "o");
            text = text.Replace("Ü", "U");
            text = text.Replace("ü", "u");
            text = text.Replace("Ş", "S");
            text = text.Replace("ş", "s");
            text = text.Replace("Ç", "C");
            text = text.Replace("ç", "c");
            text = text.Replace(" ", "-");
            text = text.Replace("!", "-");
            text = text.Replace("%", "-");
            text = text.Replace("&", "-");
            text = text.Replace("?", "-");
            text = text.Replace("*", "-");
            text = text.Replace("+", "-");
            return text;
        }

        // POST: News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Title_EN,Content,Content_EN,Image,Date,Url,Slug,Slug_EN,cat,Video,Video_EN,Docs,Docs_EN,Spot,Spot_EN,Is_Login")] News news, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {

                //resmi kontrol et
                HttpPostedFileBase image = Request.Files[0];
                if (image != null)
                {
                    int fileSize = image.ContentLength;

                    FileInfo docInfo = new FileInfo(image.FileName);
                    string fileName = StringReplace(image.FileName);
                    string mimeType = image.ContentType;
                    System.IO.Stream fileContent = image.InputStream;
                    image.SaveAs(Server.MapPath("~/Uploads/News/") + fileName);
                    news.Image = "/Uploads/News/" + fileName;
                }


                HttpPostedFileBase video = Request.Files[1];
                if (video.ContentLength > 0)
                {
                    int fileSize = video.ContentLength;
                    FileInfo docInfo = new FileInfo(video.FileName);
                    string fileName = StringReplace(video.FileName);
                    string mimeType = video.ContentType;
                    System.IO.Stream fileContent = video.InputStream;
                    video.SaveAs(Server.MapPath("~/Uploads/News/") + fileName);
                    news.Docs = "/Uploads/News/" + fileName;
                }

                HttpPostedFileBase docs_en = Request.Files[2];
                if (docs_en.ContentLength > 0)
                {
                    int fileSize = docs_en.ContentLength;
                    FileInfo docInfo = new FileInfo(docs_en.FileName);
                    string fileName = StringReplace(docs_en.FileName);
                    string mimeType = docs_en.ContentType;
                    System.IO.Stream fileContent = docs_en.InputStream;
                    docs_en.SaveAs(Server.MapPath("~/Uploads/News/") + fileName);
                    news.Docs_EN = "/Uploads/News/" + fileName;
                }


                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: News/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Title_EN,Content,Content_EN,Image,Date,Url,Slug,Slug_EN,cat,Video,Video_EN,Docs,Docs_EN,Spot,Spot_EN,Is_Login")] News news)
        {
            if (ModelState.IsValid)
            {

                News NewsUpdate = db.News.Where(u => u.Id == news.Id).FirstOrDefault();

                //resmi kontrol et
                HttpPostedFileBase image = Request.Files[0];
                if (image.ContentLength > 0)
                {
                    int fileSize = image.ContentLength;

                    FileInfo docInfo = new FileInfo(image.FileName);
                    string fileName = StringReplace(image.FileName);
                    string mimeType = image.ContentType;
                    System.IO.Stream fileContent = image.InputStream;
                    image.SaveAs(Server.MapPath("~/Uploads/News/") + fileName);
                    NewsUpdate.Image = "/Uploads/News/" + fileName;
                }
                else
                {
                    NewsUpdate.Image = NewsUpdate.Image;
                }


                HttpPostedFileBase video = Request.Files[1];
                if (video.ContentLength > 0)
                {
                    int fileSize = video.ContentLength;
                    FileInfo docInfo = new FileInfo(video.FileName);
                    string fileName = StringReplace(video.FileName);
                    string mimeType = video.ContentType;
                    System.IO.Stream fileContent = video.InputStream;
                    video.SaveAs(Server.MapPath("~/Uploads/News/") + fileName);
                    NewsUpdate.Docs = "/Uploads/News/" + fileName;
                }
                else
                {
                    NewsUpdate.Docs = NewsUpdate.Docs;
                }

                HttpPostedFileBase docs_en = Request.Files[2];
                if (docs_en.ContentLength > 0)
                {
                    int fileSize = docs_en.ContentLength;
                    FileInfo docInfo = new FileInfo(docs_en.FileName);
                    string fileName = StringReplace(docs_en.FileName);
                    string mimeType = docs_en.ContentType;
                    System.IO.Stream fileContent = docs_en.InputStream;
                    docs_en.SaveAs(Server.MapPath("~/Uploads/News/") + fileName);
                    NewsUpdate.Docs_EN = "/Uploads/News/" + fileName;
                }
                else
                {
                    NewsUpdate.Docs_EN = NewsUpdate.Docs_EN;
                }

                NewsUpdate.Video = news.Video;
                NewsUpdate.Video_EN = news.Video_EN;
                NewsUpdate.Title = news.Title;
                NewsUpdate.Title_EN = news.Title_EN;
                NewsUpdate.Content = news.Content;
                NewsUpdate.Content_EN = news.Content_EN;
                NewsUpdate.Spot = news.Spot;
                NewsUpdate.Spot_EN = news.Spot_EN;
                NewsUpdate.Slug = news.Slug;
                NewsUpdate.Slug_EN = news.Slug_EN;
                NewsUpdate.Date = news.Date;
                NewsUpdate.Url = news.Url;
                NewsUpdate.cat = news.cat;
                NewsUpdate.Is_Login = news.Is_Login;

                db.Entry(NewsUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult GetList(int? id, string login)
        {
            var items = db.News.Where(u => u.cat == id).Where(x => x.Is_Login != 1).ToList().OrderByDescending(x => x.Date);

            if (login != null)
            {
                    MemberUser memberUser = db.MemberUsers.Where(x => x.Email == login).FirstOrDefault();

                if (memberUser.Email == login)
                {
                    items = db.News.Where(u => u.cat == id).ToList().OrderByDescending(x => x.Date);
                }
            }


            var my_jsondata = new
            {
                label = "hepsi",
                value = 1,
                items = items
            };


            return Json(my_jsondata, JsonRequestBehavior.AllowGet);
        }

        // GET: Slider/Edit/5
        [HttpGet]
        public ActionResult GetDetail(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            News news = db.News.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault();


            if (news == null)
            {
                return HttpNotFound();
            }

            var detail = new
            {
                news = db.News.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault(),
                gallery = db.Galleries.Where(u => u.news_id == news.Id).ToList().OrderBy(x => x.sort),
            };

            return Json(detail, JsonRequestBehavior.AllowGet);
        }
    }
}
