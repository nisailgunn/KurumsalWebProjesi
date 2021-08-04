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
    public class ReportsController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Reports
        public ActionResult Index()
        {
            return View(db.Reports.ToList());
        }

        // GET: Reports/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // GET: Reports/Create
        public ActionResult Create()
        {
            ViewBag.ParentId = new SelectList(db.ReportCats, "Id", "Name");
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

        // POST: Reports/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Title_EN,Content,Content_EN,Image,File,Date,Url,Slug,Slug_EN,ParentId,File_EN")] Report report)
        {
            if (ModelState.IsValid)
            {
                //resmi kontrol et
                HttpPostedFileBase CategoryImage = Request.Files[0];
                if (CategoryImage.FileName != "")
                {
                    Random random = new Random();
                    int randomNumber = random.Next(0, 1000);
                    WebImage img = new WebImage(CategoryImage.InputStream);
                    FileInfo imgInfo = new FileInfo(CategoryImage.FileName);
                    string logoname = StringReplace(CategoryImage.FileName);
                    img.Save("~/Uploads/Reports/" + logoname);
                    report.Image = "/Uploads/Reports/" + logoname;
                }
                else
                {
                    report.Image = "";
                }

                //dosyayı kontrol et
                HttpPostedFileBase file = Request.Files[1];
                if (file.FileName != "")
                {
                    int fileSize = file.ContentLength;

                    FileInfo docInfo = new FileInfo(file.FileName);
                    string fileName = StringReplace(file.FileName);

                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    file.SaveAs(Server.MapPath("~/Uploads/Reports/") + fileName);
                    report.File = "/Uploads/Reports/" + fileName;
                }
                else
                {
                    report.File = "";
                }


                //dosyayı kontrol et
                HttpPostedFileBase enfile = Request.Files[2];
                if (enfile.FileName != "")
                {
                    int fileSize = enfile.ContentLength;

                    FileInfo docInfo = new FileInfo(enfile.FileName);
                    string fileName = StringReplace(enfile.FileName);

                    string mimeType = enfile.ContentType;
                    System.IO.Stream fileContent = enfile.InputStream;
                    enfile.SaveAs(Server.MapPath("~/Uploads/Reports/") + fileName);
                    report.File_EN = "/Uploads/Reports/" + fileName;
                }
                else
                {
                    report.File_EN = "";
                }

                db.Reports.Add(report);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(report);
        }

        // GET: Reports/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);

            ViewBag.cats = db.ReportCats.ToList();
            
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Title_EN,Content,Content_EN,Image,File,Date,Url,Slug,Slug_EN,ParentId,File_EN")] Report report)
        {
            if (ModelState.IsValid)
            {

                Report ReportUpdate = db.Reports.Where(u => u.Id == report.Id).FirstOrDefault();

                //resmi kontrol et
                HttpPostedFileBase image = Request.Files[0];
                if (image.ContentLength > 0)
                {
                    int fileSize = image.ContentLength;

                    FileInfo docInfo = new FileInfo(image.FileName);
                    string fileName = StringReplace(image.FileName);
                    string mimeType = image.ContentType;
                    System.IO.Stream fileContent = image.InputStream;
                    image.SaveAs(Server.MapPath("~/Uploads/Reports/") + fileName);
                    ReportUpdate.Image = "/Uploads/Reports/" + fileName;
                }
                else
                {
                    ReportUpdate.Image = ReportUpdate.Image;
                }

                HttpPostedFileBase docs = Request.Files[1];
                if (docs.ContentLength > 0)
                {
                    int fileSize = docs.ContentLength;
                    FileInfo docInfo = new FileInfo(docs.FileName);
                    string fileName = StringReplace(docs.FileName);
                    string mimeType = docs.ContentType;
                    System.IO.Stream fileContent = docs.InputStream;
                    docs.SaveAs(Server.MapPath("~/Uploads/Reports/") + fileName);
                    ReportUpdate.File = "/Uploads/Reports/" + fileName;
                }
                else
                {
                    ReportUpdate.File = ReportUpdate.File;
                }

                //dosyayı kontrol et
                HttpPostedFileBase enfile = Request.Files[2];
                if (enfile.FileName != "")
                {
                    int fileSize = enfile.ContentLength;

                    FileInfo docInfo = new FileInfo(enfile.FileName);
                    string fileName = StringReplace(enfile.FileName);

                    string mimeType = enfile.ContentType;
                    System.IO.Stream fileContent = enfile.InputStream;
                    enfile.SaveAs(Server.MapPath("~/Uploads/Reports/") + fileName);
                    ReportUpdate.File_EN = "/Uploads/Reports/" + fileName;
                }
                else
                {
                    ReportUpdate.File_EN = ReportUpdate.File_EN;
                }


                ReportUpdate.Title = report.Title;
                ReportUpdate.Title_EN = report.Title_EN;
                ReportUpdate.ParentId = report.ParentId;
                ReportUpdate.Content = report.Content;
                ReportUpdate.Content_EN = report.Content_EN;
                ReportUpdate.Slug = report.Slug;
                ReportUpdate.Slug_EN = report.Slug_EN;
                ReportUpdate.Date = report.Date;
                ReportUpdate.Url = report.Url;

                db.Entry(ReportUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(report);
        }

        // GET: Reports/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Report report = db.Reports.Find(id);
            if (report == null)
            {
                return HttpNotFound();
            }
            return View(report);
        }

        // POST: Reports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Report report = db.Reports.Find(id);
            db.Reports.Remove(report);
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
                items = db.Reports.ToList().OrderByDescending(x => x.Date)
            };

            return Json(my_jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetlistByCat()
        {
            var cats = db.ReportCats.ToList().OrderByDescending(x => x.Id);
            var reports = db.Reports.ToList().OrderByDescending(x => x.Date);

            var my_jsondata = new
            {
                data = cats,
                notes = reports,
            };

            return Json(my_jsondata, JsonRequestBehavior.AllowGet);
        }


    }
}
