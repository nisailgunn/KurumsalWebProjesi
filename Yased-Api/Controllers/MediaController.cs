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
    public class MediaController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Media
        public ActionResult Index()
        {
            return View(db.Medias.ToList());
        }

        // GET: Media/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Media media = db.Medias.Find(id);
            if (media == null)
            {
                return HttpNotFound();
            }
            return View(media);
        }

        // GET: Media/Create
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
        // POST: Media/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Title_EN,File")] Media media)
        {
            if (ModelState.IsValid)
            {

                //dosyayı kontrol et
                HttpPostedFileBase file = Request.Files[0];
                if (file.FileName != "")
                {
                    int fileSize = file.ContentLength;

                    FileInfo docInfo = new FileInfo(file.FileName);
                    string fileName = StringReplace(file.FileName);

                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(Server.MapPath("~/Uploads/Media/") + fileName);
                    media.File = "/Uploads/Media/" + fileName;
                }
                else
                {
                    media.File = "";
                }

                db.Medias.Add(media);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(media);
        }

        // GET: Media/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Media media = db.Medias.Find(id);
            if (media == null)
            {
                return HttpNotFound();
            }
            return View(media);
        }

        // POST: Media/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Title_EN,File")] Media media)
        {
            if (ModelState.IsValid)
            {

                Media MediaUpdate = db.Medias.Where(u => u.Id == media.Id).FirstOrDefault();

                //dosyayı kontrol et
                HttpPostedFileBase file = Request.Files[0];
                if (file.FileName != "")
                {
                    int fileSize = file.ContentLength;

                    FileInfo docInfo = new FileInfo(file.FileName);
                    string fileName = StringReplace(file.FileName);

                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(Server.MapPath("~/Uploads/Media/") + fileName);
                    MediaUpdate.File = "/Uploads/Media/" + fileName;
                }
                else
                {
                    MediaUpdate.File = MediaUpdate.File;
                }

                MediaUpdate.Title = media.Title;
                MediaUpdate.Title_EN = media.Title_EN;


                db.Entry(MediaUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(media);
        }

        // GET: Media/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Media media = db.Medias.Find(id);
            if (media == null)
            {
                return HttpNotFound();
            }
            return View(media);
        }

        // POST: Media/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Media media = db.Medias.Find(id);
            db.Medias.Remove(media);
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
    }
}
