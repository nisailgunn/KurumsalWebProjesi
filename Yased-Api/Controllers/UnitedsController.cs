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
    public class UnitedsController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Uniteds
        public ActionResult Index()
        {
            return View(db.Uniteds.ToList());
        }

        // GET: Uniteds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            United united = db.Uniteds.Find(id);
            if (united == null)
            {
                return HttpNotFound();
            }
            return View(united);
        }

        // GET: Uniteds/Create
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

        // POST: Uniteds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Title_EN,Image,Description,Description_EN,Slug,Slug_EN,Date,Video,Docs,Video_EN,Docs_EN,Spot,Spot_EN")] United united )
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
                    united.Image = "/Uploads/Insights/" + fileName;
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
                    united.Docs = "/Uploads/Insights/" + fileName;
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
                    united.Docs_EN = "/Uploads/Insights/" + fileName;
                }


                db.Uniteds.Add(united);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(united);
        }

        // GET: Uniteds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            United united = db.Uniteds.Find(id);
            if (united == null)
            {
                return HttpNotFound();
            }
            return View(united);
        }

        // POST: Uniteds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Title_EN,Image,Description,Description_EN,Slug,Slug_EN,Date,Video,Docs,Video_EN,Docs_EN,Spot,Spot_EN")] United united)
        {
            if (ModelState.IsValid)
            {

                United UnitedUpdate = db.Uniteds.Where(u => u.Id == united.Id).FirstOrDefault();

                HttpPostedFileBase image = Request.Files[0];
                if (image.ContentLength > 0)
                {
                    int fileSize = image.ContentLength;

                    FileInfo docInfo = new FileInfo(image.FileName);
                    string fileName = StringReplace(image.FileName);
                    string mimeType = image.ContentType;
                    System.IO.Stream fileContent = image.InputStream;
                    image.SaveAs(Server.MapPath("~/Uploads/Insights/") + fileName);
                    UnitedUpdate.Image = "/Uploads/Insights/" + fileName;
                }
                else
                {
                    UnitedUpdate.Image = UnitedUpdate.Image;
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
                    UnitedUpdate.Docs = "/Uploads/Insights/" + fileName;
                }
                else
                {
                    UnitedUpdate.Docs = UnitedUpdate.Docs;
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
                    UnitedUpdate.Docs_EN = "/Uploads/Insights/" + fileName;
                }
                else
                {
                    UnitedUpdate.Docs_EN = UnitedUpdate.Docs_EN;
                }




                UnitedUpdate.Title = united.Title;
                UnitedUpdate.Title_EN = united.Title_EN;
                UnitedUpdate.Description = united.Description;
                UnitedUpdate.Description_EN = united.Description_EN;
                UnitedUpdate.Spot = united.Spot;
                UnitedUpdate.Spot_EN = united.Spot_EN;
                UnitedUpdate.Video = united.Video;
                UnitedUpdate.Video_EN = united.Video_EN;
                UnitedUpdate.Slug = united.Slug;
                UnitedUpdate.Slug_EN = united.Slug_EN;
                UnitedUpdate.Date = united.Date;


                db.Entry(UnitedUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(united);
        }

        // GET: Uniteds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            United united = db.Uniteds.Find(id);
            if (united == null)
            {
                return HttpNotFound();
            }
            return View(united);
        }

        // POST: Uniteds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            United united = db.Uniteds.Find(id);
            db.Uniteds.Remove(united);
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

        public ActionResult GetList()
        {
            var my_jsondata = new
            {
                label = "hepsi",
                value = 1,
                items = db.Uniteds.ToList().OrderByDescending(x => x.Id)
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
            United united = db.Uniteds.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault();
            if (united == null)
            {
                return HttpNotFound();
            }
            return Json(united, JsonRequestBehavior.AllowGet);
        }

        // GET: Slider/Edit/5
        [HttpGet]
        public ActionResult GetDetailGallery(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            United united = db.Uniteds.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault();
            if (united == null)
            {
                return HttpNotFound();
            }

            var detail = new
            {
                united = db.Uniteds.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault(),
                gallery = db.Galleries.Where(u => u.united_id == united.Id).ToList().OrderBy(x => x.sort),
            };

            return Json(detail, JsonRequestBehavior.AllowGet);
        }

    }
}
