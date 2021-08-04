using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using Yased_Api.Models;

namespace Yased_Api.Controllers
{
    public class MemberUsersController : Controller
    {
        private YasedWebDBEntities db = new YasedWebDBEntities();


        // GET: MemberUsers
        public ActionResult Index()
        {

            var data = db.MemberUsers.ToList();

            return View(data);
        }

        // GET: MemberUsers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberUser memberUser = db.MemberUsers.Find(id);
            if (memberUser == null)
            {
                return HttpNotFound();
            }
            return View(memberUser);
        }

        // GET: MemberUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MemberUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,FK_MemberStatusID,FK_CompanyID,FK_MemberRolesID,FirstName,LastName,FullName,Email,Password,PhotoPath,Title,MobilePhone,Birthdate,Gender,LoginKey,CreatedDate,LastLoginDate,DeviceToken,IsIOS,Version,Model,Uuid,TokenUpdateDate,ResetGuid,ResetCount,DocGuid")] MemberUser memberUser)
        {
            if (ModelState.IsValid)
            {
                db.MemberUsers.Add(memberUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(memberUser);
        }

        // GET: MemberUsers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberUser memberUser = db.MemberUsers.Find(id);
            if (memberUser == null)
            {
                return HttpNotFound();
            }
            return View(memberUser);
        }

        // POST: MemberUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,FK_MemberStatusID,FK_CompanyID,FK_MemberRolesID,FirstName,LastName,FullName,Email,Password,PhotoPath,Title,MobilePhone,Birthdate,Gender,LoginKey,CreatedDate,LastLoginDate,DeviceToken,IsIOS,Version,Model,Uuid,TokenUpdateDate,ResetGuid,ResetCount,DocGuid")] MemberUser memberUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(memberUser);
        }

        // GET: MemberUsers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberUser memberUser = db.MemberUsers.Find(id);
            if (memberUser == null)
            {
                return HttpNotFound();
            }
            return View(memberUser);
        }

        // POST: MemberUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MemberUser memberUser = db.MemberUsers.Find(id);
            db.MemberUsers.Remove(memberUser);
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

        public static string ClearRandomText(string value)
        {
            value = value.Substring(2, value.Length - 3);
            string result = Reverse((Reverse(value)).Substring(1, value.Length - 1));
            return result;
        }

        public static string Reverse(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            char[] array = text.ToCharArray();
            Array.Reverse(array);
            return new String(array);
        }

    }
}
