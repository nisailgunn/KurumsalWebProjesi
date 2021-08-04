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
    public class ContentsController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Contents
        public ActionResult Index()
        {
            return View(db.Contents.ToList());
        }

        // GET: Contents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // GET: Contents/Create
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

        // POST: Contents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Description_EN,Slug,Slug_EN,CategoryId,Image,Type")] Content content)
        {
            if (ModelState.IsValid)
            {

                HttpPostedFileBase image = Request.Files[0];
                if (image.ContentLength > 0)
                {
                    int fileSize = image.ContentLength;

                    FileInfo docInfo = new FileInfo(image.FileName);
                    string fileName = StringReplace(image.FileName);
                    string mimeType = image.ContentType;
                    System.IO.Stream fileContent = image.InputStream;
                    image.SaveAs(Server.MapPath("~/Uploads/Content/") + fileName);
                    content.Image = "/Uploads/Content/" + fileName;
                }

                db.Contents.Add(content);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(content);
        }

        // GET: Contents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Description_EN,Slug,Slug_EN,CategoryId,Image,Type")] Content content)
        {
            if (ModelState.IsValid)
            {
                Content cont = db.Contents.Where(u => u.Id == content.Id).FirstOrDefault();
                HttpPostedFileBase image = Request.Files[0];
                if (image.ContentLength > 0)
                {
                    int fileSize = image.ContentLength;

                    FileInfo docInfo = new FileInfo(image.FileName);
                    string fileName = StringReplace(image.FileName);
                    string mimeType = image.ContentType;
                    System.IO.Stream fileContent = image.InputStream;
                    image.SaveAs(Server.MapPath("~/Uploads/Content/") + fileName);
                    cont.Image = "/Uploads/Content/" + fileName;
                }
                else
                {
                    cont.Image = cont.Image;
                }

                cont.Title = content.Title;
                cont.Description = content.Description;
                cont.Description_EN = content.Description_EN;
                cont.Slug = content.Slug;
                cont.Slug_EN = content.Slug_EN;
                cont.CategoryId = content.CategoryId;
                cont.Type = content.Type;

                db.Entry(cont).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(content);
        }

        // GET: Contents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Find(id);
            if (content == null)
            {
                return HttpNotFound();
            }
            return View(content);
        }

        // POST: Contents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Content content = db.Contents.Find(id);
            db.Contents.Remove(content);
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

        public ActionResult GetRoute()
        {
            var my_jsondata = new
            {
                about = db.Contents.Where(u => u.CategoryId == 1).ToList(),
                workgroup = db.Contents.Where(u => u.CategoryId == 2).ToList(),
            };

            return Json(my_jsondata, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetSubMenu(int? id)
        {

            //var parentMenu = db.Menus.Where(u => u.ParentId == id).Include(p=>p.submenu).ToList();
            var my_jsondata = new
            {
                items = db.Menus.Where(u => u.ParentId == id).OrderBy(z => z.Sort).ToList()
            };

            int counter = 0;
            foreach (var item in my_jsondata.items)
            {
                var submenu = db.Menus.Where(u => u.ParentId == item.Id).OrderBy(z => z.Sort).ToList();
                my_jsondata.items[counter].submenu = submenu;
                ++counter;
            }

            return Json(my_jsondata , JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDetail(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Content content = db.Contents.Where(u => u.Slug == slug).FirstOrDefault();
            var id = content.Id.ToString();
            var my_jsondata = new
            {
                content = content,
                profile = db.Profiles.Where(u => u.name_en == id).FirstOrDefault(),
            };

           
            if (my_jsondata.content== null)
            {
                return HttpNotFound();
            }
            return Json(my_jsondata, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDetailWithNote(string slug)
        {
            if (slug == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           

            Content content = db.Contents.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault();
            if (content != null) { 
                var id = content.Id.ToString();
                var my_jsondata = new
                {
                    data = content,
                    notes = db.Notes.Where(u => u.ParentId == content.Id).ToList().OrderByDescending(x => x.Date),
                    profile = db.Profiles.Where(u => u.name_en == id).FirstOrDefault(),
                };
                return Json(my_jsondata, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return HttpNotFound();

            }

            if (content == null)
            {
                return HttpNotFound();
            }
           
        }
    }
}
