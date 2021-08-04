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
    public class NotesController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Notes
        public ActionResult Index()
        {
            return View(db.Notes.ToList());
        }

        // GET: Notes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // GET: Notes/Create
        public ActionResult Create()
        {
            ViewBag.Contents = db.Contents.ToList();
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

        // POST: Notes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Title_EN,File,Slug,Slug_EN,Date,ParentId")] Note note)
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
                    file.SaveAs(Server.MapPath("~/Uploads/Note/") + fileName);
                    note.File = "/Uploads/Note/" + fileName;
                }
                else
                {
                    note.File = "";
                }

                HttpPostedFileBase file2 = Request.Files[1];
                if (file2.FileName != "")
                {
                    int fileSize = file2.ContentLength;

                    FileInfo docInfo = new FileInfo(file2.FileName);
                    string fileName = StringReplace(file2.FileName);

                    string mimeType = file2.ContentType;
                    System.IO.Stream fileContent = file2.InputStream;
                    file2.SaveAs(Server.MapPath("~/Uploads/Note/") + fileName);
                    note.File_EN = "/Uploads/Note/" + fileName;
                }
                else
                {
                    note.File_EN = "";
                }


                db.Notes.Add(note);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(note);
        }

        // GET: Notes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            ViewBag.Contents = db.Contents.ToList();
            return View(note);
        }

        // POST: Notes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Title_EN,File,Slug,Slug_EN,Date,ContentId,ParentId")] Note note)
        {
            if (ModelState.IsValid)
            {
                Note UpdateNote = db.Notes.Where(u => u.Id == note.Id).FirstOrDefault();
                //dosyayı kontrol et
                HttpPostedFileBase file = Request.Files[0];
                if (file.ContentLength > 0)
                {
                    int fileSize = file.ContentLength;

                    FileInfo docInfo = new FileInfo(file.FileName);
                    string fileName = StringReplace(file.FileName);

                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(Server.MapPath("~/Uploads/Note/") + fileName);
                    UpdateNote.File = "/Uploads/Note/" + fileName;
                }
                else
                {
                    UpdateNote.File = UpdateNote.File;
                }

                HttpPostedFileBase file2 = Request.Files[1];
                if (file2.ContentLength > 1)
                {
                    int fileSize = file2.ContentLength;

                    FileInfo docInfo = new FileInfo(file2.FileName);
                    string fileName = StringReplace(file2.FileName);

                    string mimeType = file2.ContentType;
                    System.IO.Stream fileContent = file2.InputStream;
                    file.SaveAs(Server.MapPath("~/Uploads/Note/") + fileName);
                    UpdateNote.File_EN = "/Uploads/Note/" + fileName;
                }
                else
                {
                    UpdateNote.File_EN = UpdateNote.File_EN;

                }

                UpdateNote.Title = note.Title;
                UpdateNote.Title_EN = note.Title_EN;
                UpdateNote.Slug = note.Slug;
                UpdateNote.ParentId = note.ParentId;
                UpdateNote.Slug_EN = note.Slug_EN;
                UpdateNote.Date = note.Date;

                db.Entry(UpdateNote).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(note);
        }

        // GET: Notes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Note note = db.Notes.Find(id);
            if (note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: Notes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = db.Notes.Find(id);
            db.Notes.Remove(note);
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
