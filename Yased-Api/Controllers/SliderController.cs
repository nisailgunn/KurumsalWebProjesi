using System;
using System.Collections.Generic;
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
    public class SliderController : Controller
    {
        YasedWebDBEntities db = new YasedWebDBEntities();
        
        // GET: Slider
        public ActionResult Index()
        {
            return View(db.Sliders.ToList());
        }

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

        // POST: Slider/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(Slider slider, HttpPostedFileBase ImageURL)
        {
            if (ModelState.IsValid)
            {
                //resmi kontrol et
                if (ImageURL != null)
                {
                    WebImage img = new WebImage(ImageURL.InputStream);
                    FileInfo imgInfo = new FileInfo(ImageURL.FileName);
                    string logoname = StringReplace(ImageURL.FileName);
                    img.Save("~/Uploads/Slider/" + logoname);
                    slider.ImageURL = "/Uploads/Slider/" + logoname;
                }

                db.Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(slider);
        }

        // GET: Slider/Edit/5
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Slider slider = db.Sliders.Where(u => u.Id == id).FirstOrDefault();
            if (slider == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.Sliders, "Id", "Name", slider.CategoryId);
            return View(slider);
        }

        // POST: Slider/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit( Slider slider, HttpPostedFileBase ImageURL)
        {
            if (ModelState.IsValid)
            {

                Slider Slider = db.Sliders.Where(u => u.Id == slider.Id).FirstOrDefault();

                if (ImageURL != null)
                {

                    //resmi klasörde kontrol et
                    if (System.IO.File.Exists(Server.MapPath(Slider.ImageURL)))
                    {
                        //resmi klasörden kaldır
                        System.IO.File.Delete(Server.MapPath(Slider.ImageURL));
                    }

                    WebImage img = new WebImage(ImageURL.InputStream);
                    FileInfo imgInfo = new FileInfo(ImageURL.FileName);
                    string logoname = StringReplace(ImageURL.FileName);
                    img.Save("~/Uploads/Slider/" + logoname);
                    Slider.ImageURL = "/Uploads/Slider/" + logoname;
                }
                else
                {
                    Slider.ImageURL = Slider.ImageURL;
                }

                Slider.Title = slider.Title;
                Slider.Title_EN = slider.Title_EN;
                Slider.Title = slider.Title;
                Slider.Description = slider.Description;
                Slider.Description_EN = slider.Description_EN;
                Slider.Slug = slider.Slug;
                Slider.Slug_EN = slider.Slug_EN;
                Slider.Link = slider.Link;
                Slider.Link_EN = slider.Link_EN;
                Slider.CategoryId = slider.CategoryId;
                Slider.Sort = slider.Sort;

                db.Entry(Slider).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(slider);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();

            }
            var slider = db.Sliders.Find(id);

            if(slider == null)
            {
                return HttpNotFound();
            }

            db.Sliders.Remove(slider);
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        public ActionResult GetSlider()
        {
            var slider_tr = db.Sliders.ToList().OrderBy(x => x.Sort).Take(5);
            var slider_en = db.Sliders.Where(u => u.Title_EN != null).ToList().OrderBy(x => x.Sort).Take(5);

            var sliders = new
            {
                slider_tr = slider_tr,
                slider_en = slider_en,
            };


            return Json(sliders, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetBottomSlider(string login)
        {
            var news = db.News.Where(u => u.cat == 3).Where(x => x.Is_Login != 1).OrderByDescending(x => x.Id).FirstOrDefault();
            var news_en = db.News.Where(u => u.cat == 3).Where(x => x.Is_Login != 1).Where(u => u.Title_EN != null).OrderByDescending(x => x.Id).FirstOrDefault();
            var usernews = db.News.Where(u => u.cat == 2).Where(x => x.Is_Login != 1).OrderByDescending(x => x.Id).FirstOrDefault();
            var usernews_en = db.News.Where(u => u.cat == 2).Where(u => u.Title_EN != null).Where(x => x.Is_Login != 1).OrderByDescending(x => x.Id).FirstOrDefault();
            var works_en = db.News.Where(u => u.cat == 4).Where(u => u.Title_EN != null).Where(x => x.Is_Login != 1).OrderByDescending(x => x.Id).FirstOrDefault();
            var works = db.News.Where(u => u.cat == 4).Where(x => x.Is_Login != 1).OrderByDescending(x => x.Id).FirstOrDefault();
            var academy = db.News.Where(u => u.cat == 6).Where(x => x.Is_Login != 1).OrderByDescending(x => x.Id).FirstOrDefault();
            var academy_en = db.News.Where(u => u.cat == 6).Where(u => u.Title_EN != null).Where(x => x.Is_Login != 1).OrderByDescending(x => x.Id).FirstOrDefault();

            if (login != null)
            {
                MemberUser memberUser = db.MemberUsers.Where(x => x.Email == login).FirstOrDefault();

                if (memberUser.Email == login)
                {
                    news = db.News.Where(u => u.cat == 3).OrderByDescending(x => x.Id).FirstOrDefault();
                    news_en = db.News.Where(u => u.cat == 3).Where(u => u.Title_EN != null).OrderByDescending(x => x.Id).FirstOrDefault();
                    usernews = db.News.Where(u => u.cat == 2).OrderByDescending(x => x.Id).FirstOrDefault();
                    usernews_en = db.News.Where(u => u.cat == 2).Where(u => u.Title_EN != null).OrderByDescending(x => x.Id).FirstOrDefault();
                    works_en = db.News.Where(u => u.cat == 4).Where(u => u.Title_EN != null).OrderByDescending(x => x.Id).FirstOrDefault();
                    works = db.News.Where(u => u.cat == 4).OrderByDescending(x => x.Id).FirstOrDefault();
                    academy = db.News.Where(u => u.cat == 6).OrderByDescending(x => x.Id).FirstOrDefault();
                    academy_en = db.News.Where(u => u.cat == 6).Where(u => u.Title_EN != null).OrderByDescending(x => x.Id).FirstOrDefault();
                }
            }

            var my_jsondata = new
            {
                news = news,
                news_en = news_en,
                usernews = usernews,
                usernews_en = usernews_en,
                member = db.Members.OrderBy(p => Guid.NewGuid()).ToList().Take(4),
                works = works,
                works_en = works_en,
                academy = academy,
                academy_en = academy_en,
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
            Slider slider = db.Sliders.Where(u => u.Slug == slug || u.Slug_EN == slug).FirstOrDefault();
            if (slider == null)
            {
                return HttpNotFound();
            }
            return Json(slider, JsonRequestBehavior.AllowGet);
        }
    }
}