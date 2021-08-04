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
    public class InsightsController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Insights
        public ActionResult Index()
        {
            return View(db.Insights.ToList());
        }

        // GET: Insights/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insight insight = db.Insights.Find(id);
            if (insight == null)
            {
                return HttpNotFound();
            }
            return View(insight);
        }

        // GET: Insights/Create
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

        // POST: Insights/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Title_EN,Image,Slug,Slug_EN,Date,Description,Description_EN,Docs,Video_EN,Docs_EN,Video,Spot,Spot_EN")] Insight insight)
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
                    image.SaveAs(Server.MapPath("~/Uploads/Insights/") + fileName);
                    insight.Image = "/Uploads/Insights/" + fileName;
                }


                HttpPostedFileBase video = Request.Files[1];
                if (video.ContentLength > 0)
                {
                    int fileSize = video.ContentLength;
                    FileInfo docInfo = new FileInfo(video.FileName);
                    string fileName = StringReplace(video.FileName);
                    string mimeType = video.ContentType;
                    System.IO.Stream fileContent = video.InputStream;
                    video.SaveAs(Server.MapPath("~/Uploads/Insights/") + fileName);
                    insight.Docs = "/Uploads/Insights/" + fileName;
                }

                HttpPostedFileBase docs_en = Request.Files[2];
                if (docs_en.ContentLength > 0)
                {
                    int fileSize = docs_en.ContentLength;
                    FileInfo docInfo = new FileInfo(docs_en.FileName);
                    string fileName = StringReplace(docs_en.FileName);
                    string mimeType = docs_en.ContentType;
                    System.IO.Stream fileContent = docs_en.InputStream;
                    docs_en.SaveAs(Server.MapPath("~/Uploads/Insights/") + fileName);
                    insight.Docs_EN = "/Uploads/Insights/" + fileName;
                }


                db.Insights.Add(insight);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insight);
        }

        // GET: Insights/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insight insight = db.Insights.Find(id);
            if (insight == null)
            {
                return HttpNotFound();
            }
            return View(insight);
        }

        // POST: Insights/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Title_EN,Image,Slug,Slug_EN,Date,Description,Description_EN,Docs,Video,Video_EN,Docs_EN,Spot,Spot_EN")] Insight insight)
        {
            if (ModelState.IsValid)
            {

                Insight InsightUpdate = db.Insights.Where(u => u.Id == insight.Id).FirstOrDefault();

                //resmi kontrol et
                HttpPostedFileBase image = Request.Files[0];
                if (image.ContentLength > 0)
                {
                    int fileSize = image.ContentLength;
                    FileInfo docInfo = new FileInfo(image.FileName);
                    string fileName = StringReplace(image.FileName);
                    string mimeType = image.ContentType;
                    System.IO.Stream fileContent = image.InputStream;
                    image.SaveAs(Server.MapPath("~/Uploads/Insights/") + fileName);
                    InsightUpdate.Image = "/Uploads/Insights/" + fileName;
                }
                else
                {
                    InsightUpdate.Image = InsightUpdate.Image;
                }

                HttpPostedFileBase video = Request.Files[1];
                if (video.ContentLength > 0)
                {
                    int fileSize = video.ContentLength;
                    FileInfo docInfo = new FileInfo(video.FileName);
                    string fileName = StringReplace(video.FileName);
                    string mimeType = video.ContentType;
                    System.IO.Stream fileContent = video.InputStream;
                    video.SaveAs(Server.MapPath("~/Uploads/Insights/") + fileName);
                    InsightUpdate.Docs= "/Uploads/Insights/" + fileName;
                }
                else
                {
                    InsightUpdate.Docs = InsightUpdate.Docs;
                }

                HttpPostedFileBase docs_en = Request.Files[2];
                if (docs_en.ContentLength > 0)
                {
                    int fileSize = docs_en.ContentLength;
                    FileInfo docInfo = new FileInfo(docs_en.FileName);
                    string fileName = StringReplace(docs_en.FileName);
                    string mimeType = docs_en.ContentType;
                    System.IO.Stream fileContent = docs_en.InputStream;
                    docs_en.SaveAs(Server.MapPath("~/Uploads/Insights/") + fileName);
                    InsightUpdate.Docs_EN = "/Uploads/Insights/" + fileName;
                }
                else
                {
                    InsightUpdate.Docs_EN = InsightUpdate.Docs_EN;
                }


                InsightUpdate.Title = insight.Title;
                InsightUpdate.Title_EN = insight.Title_EN;
                InsightUpdate.Description = insight.Description;
                InsightUpdate.Description_EN = insight.Description_EN;
                InsightUpdate.Slug = insight.Slug;
                InsightUpdate.Slug_EN = insight.Slug_EN;
                InsightUpdate.Video = insight.Video;
                InsightUpdate.Video_EN = insight.Video_EN;
                InsightUpdate.Spot = insight.Spot;
                InsightUpdate.Spot_EN = insight.Spot_EN;

                db.Entry(InsightUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insight);
        }

        // GET: Insights/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insight insight = db.Insights.Find(id);
            if (insight == null)
            {
                return HttpNotFound();
            }
            return View(insight);
        }

        // POST: Insights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insight insight = db.Insights.Find(id);
            db.Insights.Remove(insight);
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

        public ActionResult GetInsights()
        {
            var my_jsondata = new
            {
                label = "hepsi",
                value = 1,
                items = db.Insights.ToList().OrderByDescending(x => x.Id)
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
            Insight insight = db.Insights.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault();
            if (insight == null)
            {
                return HttpNotFound();
            }

            return Json(insight, JsonRequestBehavior.AllowGet);
        }

        // GET: Slider/Edit/5
        [HttpGet]
        public ActionResult GetDetailGallery(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insight insight = db.Insights.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault();
            if (insight == null)
            {
                return HttpNotFound();
            }

            var detail = new
            {
                insight = db.Insights.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault(),
                gallery = db.Galleries.Where(u => u.insight_id== insight.Id).ToList().OrderBy(x => x.sort),
            };

            return Json(detail, JsonRequestBehavior.AllowGet);
        }


    }
}
