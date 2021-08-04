using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Yased_Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Yased_Api.Controllers
{
    public class HomeController : Controller
    {
        YasedWebDBEntities db = new YasedWebDBEntities();


        // GET: Home
        public ActionResult Index()
        {

            //string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            //var xmlDoc = new XmlDocument();
            //xmlDoc.Load(exchangeRate);

            //string usd = getCurrency("USD");
            //string eur = getCurrency("EUR");
            //string gbp = getCurrency("GBP");

            //string usd_status = getCurrencyStatus("USD");
            //string eur_status = getCurrencyStatus("EUR");
            //string gbp_status = getCurrencyStatus("GBP");

            //var data = new
            //{
            //    usd = new
            //    {
            //        total = usd,
            //        status = usd_status
            //    },
            //    eur = new
            //    {
            //        total = eur,
            //        status = eur_status
            //    },
            //    gbp = new
            //    {
            //        total = gbp,
            //        status = gbp_status
            //    },
            //};

            string url = "https://apilayer.net/api/live?access_key=9e88bbdc35d2412efc6c71e77f1c5318&currencies=TRY&source=USD&format=1";

            var USD = new WebClient().DownloadString(url);

            string url2 = "https://apilayer.net/api/live?access_key=9e88bbdc35d2412efc6c71e77f1c5318&currencies=TRY&source=EUR&format=1";

            var EUR = new WebClient().DownloadString(url2);

            string url3 = "https://apilayer.net/api/live?access_key=9e88bbdc35d2412efc6c71e77f1c5318&currencies=TRY&source=GBP&format=1";

            var GBP = new WebClient().DownloadString(url3);


            var my_jsondata = new
            {
                usd = USD,
                eur =EUR,
                gbp = GBP,
            };

            return Json(my_jsondata, JsonRequestBehavior.AllowGet);
        }


        public string getCurrency(string type)
        {
            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(exchangeRate);
            string returning = xmlDoc.SelectSingleNode("Tarih_Date / Currency[@Kod ='" + type + "'] / BanknoteSelling").InnerXml;


            return returning;
        }

        public string getCurrencyStatus(string type)
        {
            string exchangeRate = "http://www.tcmb.gov.tr/kurlar/today.xml";
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(exchangeRate);
            string usd = xmlDoc.SelectSingleNode("Tarih_Date / Currency[@Kod ='" + type + "'] / BanknoteSelling").InnerXml;

            DateTime day = DateTime.Today;
            string name = day.ToString("dddd"); // As String

            int d = 0;
            if(name == "Pazartesi")
            {
                d = -3;
            }
            else if (name == "Cumartesi")
            {
                d = -1;
            }
            else if (name == "Pazar")
            {
                d = -2;
            }
            else 
            {
                d = -1;
            }

            DateTime today = DateTime.Today.AddDays(d); // As DateTime

            string s_today = today.ToString("yyyyMM"); // As String
            string fullday = today.ToString("ddMMyyyy"); // As String
            string lastRate = "http://www.tcmb.gov.tr/kurlar/" + s_today + "/" + fullday + ".xml";
            var lastDoc = new XmlDocument();
            string prev_usd = "";
            if (lastDoc.DocumentElement != null)
            {
                lastDoc.Load(lastRate);
                prev_usd = xmlDoc.SelectSingleNode("Tarih_Date / Currency[@Kod ='" + type + "'] / BanknoteSelling").InnerXml;
            }
            else
            {
                prev_usd = usd;
            }
            string data = "";
            if (float.Parse(usd) == float.Parse(prev_usd))
            {
                data = "up";

            }
            else if (float.Parse(usd) > float.Parse(prev_usd))
            {
                data = "up";
            }
            else
            {
                data = "down";
            }
            return data;
        }

        [HttpPost]
        public ActionResult Login(MemberUser memberUser)
        {

            var login = db.MemberUsers.Where(x => x.Email == memberUser.Email).SingleOrDefault();

            if (login != null)
            {
                if (login.Email == memberUser.Email && login.Password == memberUser.Password)
                {
                    var success = new
                    {
                        status = "success",
                        user = login
                    };
                    return Json(success, JsonRequestBehavior.AllowGet);
                }
            }

            var error = new
            {
                status = "error"
            };

            return Json(error, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Edit(MemberUser memberUser)
        {
            MemberUser MemberUpdate = db.MemberUsers.Where(u => u.ID == memberUser.ID).FirstOrDefault();

            MemberUpdate.FirstName = memberUser.FirstName;
            MemberUpdate.LastName = memberUser.LastName;
            MemberUpdate.Email = memberUser.Email;
            MemberUpdate.Company = memberUser.Company;
            MemberUpdate.Title = memberUser.Title;
            MemberUpdate.MobilePhone = memberUser.MobilePhone;

            db.Entry(MemberUpdate).State = EntityState.Modified;
            db.SaveChanges();

            var success = new
            {
                status = "success"
            };

            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdatePassword(MemberUser memberUser)
        {
            MemberUser MemberUpdate = db.MemberUsers.Where(u => u.ID == memberUser.ID).Where(u => u.Email == memberUser.Email).Where(u => u.Password == memberUser.oldPassword).FirstOrDefault();

            //if has a member
            if (MemberUpdate == null)
            {
                var error = new
                {
                    status = "error"
                };

                return Json(error, JsonRequestBehavior.AllowGet);
            }
            //if passowrds is correct
            else if (memberUser.newPasswordRepeat != memberUser.newPassword)
            {
                var error = new
                {
                    status = "error"
                };

                return Json(error, JsonRequestBehavior.AllowGet);
            }

            if (memberUser.newPasswordRepeat == memberUser.newPassword)
            {
                MemberUpdate.Password = memberUser.newPassword;
            }

            db.Entry(MemberUpdate).State = EntityState.Modified;
            db.SaveChanges();

            var success = new
            {
                status = "success"
            };

            return Json(success, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult sendMail( MemberUser memberUser)
        {

            //auto reply mail add 
            MailMessage replymsg = new MailMessage();
            replymsg.To.Add(memberUser.Email);

            MailAddress replyaddress = new MailAddress("connect@yased.org.tr");
            replymsg.From = replyaddress;
            if (memberUser.Lang == "tr")
            {
                replymsg.Subject = "YASED | Aydınlatma Metni ";
                replymsg.Body = "<p>YASED Connect – İletişim Formu aracılığıyla https://www.yased.org.tr/ adresine iletmiş olduğunuz mesajınız tarafımıza ulaşmıştır. Uluslararası Yatırımcılar Derneği (YASED) olarak 6698 sayılı Kişisel Verilerin Korunması Kanunu (“Kanun”) kapsamında kişisel verilerin güvenliği hususunda azami hassasiyet göstermekteyiz. Bu bilinçle, iletmiş olduğunuz mesajın içerebileceği her türlü kişisel verilerin Kanun’a uygun olarak işlenerek muhafaza edilmesine büyük önem atfetmekteyiz. Kanun kapsamında, iletilen içeriklerin tümünün sistemlerimize kaydedilmesi ve işleme alınması mümkün olmayacaktır. Bu kapsamda mesajınızın içeriğine göre aşağıda yer alan bilgilendirmeleri dikkate almanızı ve gerekiyorsa diğer yollarla başvurunuzu tekrarlamanızı rica ederiz.</p><br/><p><b>6698 sayılı Kişisel Verilerin Korunması Kanunu kapsamında yapacağınız başvurularınızın usulüne</b> uygun olarak tarafımıza iletilmesi ve cevaplanması adına taleplerinizi, Veri Sorumlusuna Başvuru Usul ve Esasları Hakkında Tebliğ’in 5.maddesinde yer alan yöntemlerle iletebilirsiniz. Aksi halde başvurunuz usulüne uygun bir başvuru olarak nitelendirilmeyecek ve veri sorumlusu olarak cevap verme yükümlülüğümüz doğmayacaktır.</p><br/><p><b>YASED olarak YASED Connect aracılığıyla tarafımıza iletilen talep veya önerilerin</b> tarafımızca değerlendirilebilmesi, sistemlerimizde saklanabilmesi ve tarafınıza geri bildirim yapılabilmesi adına https://www.yased.org.tr/ internet adresinde yer alan form doldurularak, 6698 sayılı Kişisel Verilerin Korunması Kanunu’na uyum kapsamında forma ilişkin Aydınlatma Metni ve Açık Rıza Beyan Formu’nun okunması, onaylanması ve YASED’e iletilmiş olması halinde değerlendirmeye alınarak tarafınıza dönüş sağlanacaktır.</p><br/><p>YASED Connect aracılığıyla https://www.yased.org.tr/ <b>adresine iletmiş olduğunuz özgeçmiş ve iş başvuru taleplerinize ilişkin olarak </b>tarafımıza iletilen özgeçmişleri, 6698 sayılı Kişisel Verilerin Korunması Kanunu’na uyum kapsamında YASED bünyesine dahil etmemekte ve sistemlerimize kaydetmemekteyiz. YASED’e ileteceğiniz özgeçmişlerin, https://www.yased.org.tr internet adresinde yer alan form doldurulmak ve link içerisinde yer alan Adaylara İlişkin Aydınlatma Metni ve Açık Rıza Beyan Formu’nun okunması, onaylanması ve YASED’e gönderilmesi sonucunda değerlendirmeye alınacaktır.</p>";
            }
            else
            {
                replymsg.Subject = "YASED | Clarification Text";
                replymsg.Body = "<p>We have received the message that you have sent to the address https://www.yased.org.tr/ via YASED Connect – Communication Form. At International Investors Association (YASED), we show the utmost sensitivity to the protection of personal data, as per Law no. 6698 on Personal Data Protection (“the Law”). In this spirit, we attach immense importance to processing and storing any personal data in your message in accordance with the Law. Pursuant to the Law, it is not possible to save and process all communicated content in our systems. In this regard, depending on the content of your message, we kindly ask you to consider the information provided below, and if necessary, resubmit your application by other means. </p><br/><p><b>In order to duly submit and receive a response to your application as per Law no. 6698 on Personal Data Protection</b>, you may send your request to a data controller via the methods set forth in Communiqué on Application Procedures and Principles, Article 5.Otherwise, your application will not be considered an application in due form and we will not be under the obligation to respond as a data controller.</p><br/><p><b>In order for us to respond to the demands and suggestions you communicate via YASED Connect</b>, save these in our systems, and offer you feedback, you need to fill out the form at the address https://www.yased.org.tr/, read the relevant Clarification Text and Explicit Consent Form, approve and send these to YASED in accordance with Law no. 6698 on Personal Data Protection.</p><p>As per Law no. 6698 on Personal Data Protection, we do not evaluate and save in our systems <b>the CVs you send via YASED Connect to the address </b> https://www.yased.org.tr/ for job application purposes. The CVs you send to YASED shall be evaluated only if you fill out the form at the address https://www.yased.org.tr and then read, approve and send to YASED the Clarification Text for Candidates and Explicit Consent Form in the link.</ p>";

            }

            SmtpClient replyclient = new SmtpClient("smtp.office365.com");
            replymsg.IsBodyHtml = true;

            replyclient.EnableSsl = true;
            replyclient.UseDefaultCredentials = false;

            NetworkCredential replycredentials = new NetworkCredential("connect@yased.org.tr", "CYased1020_!-");
            replyclient.Credentials = replycredentials;
            replyclient.Send(replymsg);


            MailMessage msg = new MailMessage();
            string konu = "";


            if (memberUser.DocGuid == "1")
            {
                konu = "YASED'e yeni üye kazandırmak istiyorum";
            }
            else if (memberUser.DocGuid == "1")
            {
                konu = "YASED etkinlikleri için öneri getirmek istiyorum";

            }
            else if (memberUser.DocGuid == "2")
            {
                konu = "YASED etkinlikleri için öneri getirmek istiyorum";

            }
            else if (memberUser.DocGuid == "3")
            {
                konu = "YASED gündemi için öneri getirmek istiyorum";

            }
            else if (memberUser.DocGuid == "4")
            {
                konu = "Diğer bir üye ile iletişime geçmek istiyorum";

            }
            else if (memberUser.DocGuid == "5")
            {
                konu = "Mevzuat önerisi iletmek istiyorum";

            }
            else if (memberUser.DocGuid == "6")
            {
                konu = "Diğer";

            }
            else if (memberUser.DocGuid == "99")
            {
                konu = "Üst yönetim ile hemen iletişime geçmek istiyorum";
                msg.To.Add("busra@buproject.net");
                msg.To.Add("serkan.valandova@yased.org.tr");
            }
            else
            {
                konu = "";
            }



            string type = "";


            if (memberUser.ContactType == "1")
            {
                type = "Telefon/SMS";
            }
            else if(memberUser.ContactType == "2")
            {
                type = "E-posta";
            }
            else 
            {
                type = "";
            }

            msg.To.Add("busra@buproject.net");
            msg.To.Add("esra.sirman@yased.org.tr");
            msg.To.Add("elif.kaya@yased.org.tr");

            MailAddress address = new MailAddress("connect@yased.org.tr");
            msg.From = address;
            msg.Subject = "New Yased Connect Request: ";
            msg.Body = "<p>Ad:" +memberUser.FirstName + "</p><p>Soyad:" + memberUser.LastName + "</p><p>Email:" + memberUser.Email + "</p><p>Konu:" + konu + "</p><p>Telefon:" + memberUser.MobilePhone + "</p><p>İletişim Tercigi:" + type + "</p><p>Not:" + memberUser.Model+ "</p>";

            SmtpClient client = new SmtpClient("smtp.office365.com");
            msg.IsBodyHtml = true;

            client.EnableSsl = true;
            client.UseDefaultCredentials = false;

            NetworkCredential credentials = new NetworkCredential("connect@yased.org.tr", "CYased1020_!-");
            client.Credentials = credentials;
            client.Send(msg);

            var success = new
            {
                status = "success"
            };

            return Json(success, JsonRequestBehavior.AllowGet);


        }
    }

   
}