using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Yased_Api.Models;

namespace Yased_Api.Controllers
{
    public class ReportCatsController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();

        // GET: ReportCats
        public ActionResult Index()
        {
            return View(db.ReportCats.ToList());
        }

        // GET: ReportCats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCat reportCat = db.ReportCats.Find(id);
            if (reportCat == null)
            {
                return HttpNotFound();
            }
            return View(reportCat);
        }

        // GET: ReportCats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ReportCats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Name_EN,Slug,Slug_EN")] ReportCat reportCat)
        {
            if (ModelState.IsValid)
            {
                db.ReportCats.Add(reportCat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(reportCat);
        }

        // GET: ReportCats/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCat reportCat = db.ReportCats.Find(id);
            if (reportCat == null)
            {
                return HttpNotFound();
            }
            return View(reportCat);
        }

        // POST: ReportCats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Name_EN,Slug,Slug_EN")] ReportCat reportCat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reportCat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reportCat);
        }

        // GET: ReportCats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ReportCat reportCat = db.ReportCats.Find(id);
            if (reportCat == null)
            {
                return HttpNotFound();
            }
            return View(reportCat);
        }

        // POST: ReportCats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ReportCat reportCat = db.ReportCats.Find(id);
            db.ReportCats.Remove(reportCat);
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
