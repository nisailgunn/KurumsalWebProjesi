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
    public class ProfilesController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

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

        // GET: Profiles
        public ActionResult Index()
        {
            return View(db.Profiles.ToList());
        }

        // GET: Profiles/Details/5
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // GET: Profiles/Create 
        public ActionResult Create()
        {
            ViewBag.name_en = new SelectList(db.Contents, "Id", "Title");
            return View();
        }

        // POST: Profiles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,name,name_en,title,title_en,label,label_en,image")] Profile profile)
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
                    image.SaveAs(Server.MapPath("~/Uploads/Profiles/") + fileName);
                    profile.image = "/Uploads/Profiles/" + fileName;
                }

                db.Profiles.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profile);
        }

        // GET: Profiles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            ViewBag.name_en = new SelectList(db.Contents, "Id", "Title", profile.name_en);

            return View(profile);
        }

        // POST: Profiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,name,name_en,title,title_en,label,label_en,image")] Profile profile)
        {
            if (ModelState.IsValid)
            {

                Profile ProfileUpdate = db.Profiles.Where(u => u.Id == profile.Id).FirstOrDefault();
                //resmi kontrol et
                HttpPostedFileBase image = Request.Files[0];
                if (image.ContentLength > 0)
                {
                    int fileSize = image.ContentLength;
                    FileInfo docInfo = new FileInfo(image.FileName);
                    string fileName = StringReplace(image.FileName);
                    string mimeType = image.ContentType;
                    System.IO.Stream fileContent = image.InputStream;
                    image.SaveAs(Server.MapPath("~/Uploads/Profiles/") + fileName);
                    ProfileUpdate.image= "/Uploads/Profiles/" + fileName;
                }
                else
                {
                    ProfileUpdate.image = ProfileUpdate.image;
                }


                ProfileUpdate.name= profile.name;
                ProfileUpdate.name_en= profile.name_en;
                ProfileUpdate.title= profile.title;
                ProfileUpdate.title_en= profile.title_en;
                ProfileUpdate.label = profile.label;
                ProfileUpdate.label_en= profile.label_en;

                db.Entry(ProfileUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(profile);
        }

        // GET: Profiles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Profile profile = db.Profiles.Find(id);
            if (profile == null)
            {
                return HttpNotFound();
            }
            return View(profile);
        }

        // POST: Profiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Profile profile = db.Profiles.Find(id);
            db.Profiles.Remove(profile);
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
