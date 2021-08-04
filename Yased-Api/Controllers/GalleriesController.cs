using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Yased_Api.Models;

namespace Yased_Api.Controllers
{
    public class GalleriesController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Galleries
        public ActionResult Index()
        {
            return View(db.Galleries.ToList());
        }

        // GET: Galleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // GET: Galleries/Create
        public ActionResult Create(int? news_id,int? insight_id, int? united_id)
        {
            ViewBag.news_id = news_id;
            ViewBag.insight_id = insight_id;
            ViewBag.united_id = united_id;

            if(news_id != null)
            {
                ViewBag.gallery = db.Galleries.Where(x => x.news_id == news_id).ToList();
            }

            if (insight_id != null)
            {
                ViewBag.gallery = db.Galleries.Where(x => x.insight_id == insight_id).ToList();
            }

            if (united_id != null)
            {
                ViewBag.gallery = db.Galleries.Where(x => x.news_id == united_id).ToList();
            }


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

        // POST: Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,image,insight_id,news_id,united_id,sort")] Gallery gallery, HttpPostedFileBase[] image)
        {
            if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase file in image)
                {
                    if (file != null)
                    {
                        int fileSize = file.ContentLength;

                        FileInfo docInfo = new FileInfo(file.FileName);
                        string fileName = StringReplace(file.FileName);
                        string mimeType = file.ContentType;
                        System.IO.Stream fileContent = file.InputStream;
                        file.SaveAs(Server.MapPath("~/Uploads/Gallery/") + fileName);
                        gallery.image = "/Uploads/Gallery/" + fileName;
                        db.Galleries.Add(gallery);
                        db.SaveChanges();
                    }
                }
            }

            if( gallery.news_id != null)
            {
                return RedirectToAction("Create", new { news_id = gallery.news_id });

            }
            else if (gallery.united_id != null)
            {
                return RedirectToAction("Create", new { united_id = gallery.united_id });

            }
            else if (gallery.insight_id != null)
            {
                return RedirectToAction("Create", new { insight_id = gallery.insight_id });

            }
            return RedirectToAction("Create");

        }
        // GET: Galleries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,image,insight_id,news_id,united_id,sort")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallery);
        }

        // GET: Galleries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gallery gallery = db.Galleries.Find(id);
            db.Galleries.Remove(gallery);
            db.SaveChanges();

            if (gallery.news_id != null)
            {
                return RedirectToAction("Create", new { news_id = gallery.news_id });

            }
            else if (gallery.united_id != null)
            {
                return RedirectToAction("Create", new { united_id = gallery.united_id });

            }
            else if (gallery.insight_id != null)
            {
                return RedirectToAction("Create", new { insight_id = gallery.insight_id });

            }

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
    }
}
