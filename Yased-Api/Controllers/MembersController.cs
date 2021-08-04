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
    public class MembersController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: Members
        public ActionResult Index()
        {
            return View(db.Members.ToList());
        }

        // GET: Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create([Bind(Include = "Id,Title,Title_EN,Content,Content_EN,Image,Slug,Slug_EN")] Member member, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                //resmi kontrol et
                if (Image != null)
                {
                    WebImage img = new WebImage(Image.InputStream);
                    FileInfo imgInfo = new FileInfo(Image.FileName);
                    string logoname = StringReplace(Image.FileName);
                    img.Save("~/Uploads/Members/" + logoname);
                    member.Image = "/Uploads/Members/" + logoname;
                }

                db.Members.Add(member);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
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

        // GET: Members/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "Id,Title,Title_EN,Content,Content_EN,Image,Slug,Slug_EN")] Member member, HttpPostedFileBase Image)
        {
            if (ModelState.IsValid)
            {
                Member MemberUpdate = db.Members.Where(u => u.Id == member.Id).FirstOrDefault();

                //resmi kontrol et
                if (Image != null)
                {
                    WebImage img = new WebImage(Image.InputStream);
                    FileInfo imgInfo = new FileInfo(Image.FileName);
                    string logoname = StringReplace(Image.FileName);
                    img.Save("~/Uploads/Members/" + logoname);
                    MemberUpdate.Image = "/Uploads/Members/" + logoname;
                } else
                {
                    MemberUpdate.Image = MemberUpdate.Image;
                }

                MemberUpdate.Title = member.Title;
                MemberUpdate.Title_EN = member.Title_EN;
                MemberUpdate.Content = member.Content;
                MemberUpdate.Content_EN = member.Content_EN;
                MemberUpdate.Slug = member.Slug;
                MemberUpdate.Slug_EN = member.Slug_EN;

                db.Entry(MemberUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
            var members = db.Members.ToList().OrderBy(x => x.Title);
            return Json(members, JsonRequestBehavior.AllowGet);
        }
    }
}
