using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using MvcFirmaCagri.Models.Entity;
namespace MvcFirmaCagri.Controllers
{
    [Authorize]
    public class DefaultController : Controller
    {
        IsTakipEntities db = new IsTakipEntities();
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AktifCagrilar()
        {

            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Id).FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(x => x.Durum == true && x.CagriFirma == id).ToList();
            var cagriSayisi = db.TblCagrilar.Where(x => x.Durum == true && x.CagriFirma == id).Count();
            if (cagriSayisi == 0)
            {
                ViewBag.CagriSayisi= "Aktif Çağrınız bulunmamaktadır.";
            }
            return View(cagrilar);

        }

        public ActionResult PasifCagrilar()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Id).FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(x => x.Durum == false && x.CagriFirma == id).ToList();
            return View(cagrilar);
        }

        [HttpGet]
        public ActionResult YeniCagri()
        {
            return View();
        }

        [HttpPost]
        public ActionResult YeniCagri(TblCagrilar p)
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Id).FirstOrDefault();
            p.Durum = true;
            p.Tarih = DateTime.Parse(DateTime.Now.ToShortDateString());
            p.CagriFirma = id;
            db.TblCagrilar.Add(p);
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        public ActionResult CagriDetay(int id)
        {
            var cagri = db.TblCagriDetay.Where(x => x.Cagri == id).ToList();
            return View(cagri);
        }

        public ActionResult CagriGetir(int id)
        {
            var cagri = db.TblCagrilar.Find(id);
            return View("CagriGetir", cagri);
        }

        public ActionResult CagriDuzenle(TblCagrilar p)
        {
            var cagri = db.TblCagrilar.Find(p.Id);
            cagri.Konu = p.Konu;
            cagri.Aciklama = p.Aciklama;
            db.SaveChanges();
            return RedirectToAction("AktifCagrilar");
        }

        [HttpGet]
        public ActionResult ProfilDuzenle()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Id).FirstOrDefault();
            var profil = db.TblFirmalar.Where(x => x.Id == id).FirstOrDefault();
            return View(profil);
        }
        
        public ActionResult Anasayfa()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Id).FirstOrDefault();
            var toplamCagri = db.TblCagrilar.Where(x => x.CagriFirma == id).Count();
            var aktifCagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == true).Count();
            var pasifCagri = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum == false).Count();
            var yetkili = db.TblFirmalar.Where(x => x.Id == id).Select(y => y.Yetkili).FirstOrDefault();
            var sektor = db.TblFirmalar.Where(x => x.Id == id).Select(y => y.Sektor).FirstOrDefault();
            var firmaadi = db.TblFirmalar.Where(x => x.Id == id).Select(y => y.Ad).FirstOrDefault();
            var firmagorsel = db.TblFirmalar.Where(x => x.Id == id).Select(y => y.Gorsel).FirstOrDefault();
            ViewBag.t1 = toplamCagri;
            ViewBag.a1 = aktifCagri;
            ViewBag.p1 = pasifCagri;
            ViewBag.y1 = yetkili;
            ViewBag.s1 = sektor;
            ViewBag.f1 = firmaadi;
            ViewBag.g1 = firmagorsel;


            return View();
        }

        public PartialViewResult Partial1()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Id).FirstOrDefault();
            var mesajlar = db.TblMesajlar.Where(x => x.Alici == id && x.Durum == true).ToList();
            var mesajSayisi = db.TblMesajlar.Where(x => x.Alici == id && x.Durum == true).Count();
            ViewBag.mesaj = mesajSayisi;
            return PartialView(mesajlar);
        }
        public PartialViewResult Partial2()
        {
            var mail = (string)Session["Mail"];
            var id = db.TblFirmalar.Where(x => x.Mail == mail).Select(y => y.Id).FirstOrDefault();
            var cagrilar = db.TblCagrilar.Where(x => x.CagriFirma == id && x.Durum==true).ToList();
            var cagrisayisi = db.TblCagrilar.Where(x=>x.CagriFirma==id && x.Durum == true).Count();
            ViewBag.cagriSayisi = cagrisayisi;
            return PartialView(cagrilar);
        }

        public PartialViewResult Partial3()
        {
            return PartialView();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index","Login");
                
        }
    }
}