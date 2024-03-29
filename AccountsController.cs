using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PBL3_2.Models;

namespace PBL3_2.Controllers
{
    public class AccountsController : Controller
    {
        private DBGym db = new DBGym();

        // GET: Accounts
        // view danh sach 
        public ActionResult Index(string strSearchThietBi, string SortOrder, string SortBy, int? page)
        {

            //ViewBag.strSearch = strSearch;

            ViewBag.SortOrder = SortOrder;
            ViewBag.SortBy = SortBy;
            ViewBag.CurrentFilter = strSearchThietBi;

            var obj = db.Accounts.ToList();

            //Tìm kiếm
            if (!String.IsNullOrEmpty(strSearchThietBi))
            {
                obj = obj.Where(p => p.ACCOUNT_NAME != null &&(p.ACCOUNT_NAME.Contains(strSearchThietBi)
                                     || p.ACCOUNT_ROLE.ToString() == strSearchThietBi)).ToList();
            }

            //Sắp xếp
            switch (SortBy)
            {
                case "ACCOUNT_ROLE":
                    {
                        switch (SortOrder)
                        {
                            case "Asc":
                                {
                                    obj = obj.OrderBy(p => p.ACCOUNT_ROLE).ToList();
                                    break;
                                }
                            default:
                                {
                                    obj = obj.OrderByDescending(p => p.ACCOUNT_ROLE).ToList();
                                    break;
                                }
                        }
                        break;
                    }
           

            }
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(obj.ToPagedList(pageNumber, pageSize));

        }

        // GET: Accounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.ACCOUNT_NAME = new SelectList(db.AccountInfos, "ACCOUNT_NAME", "USER_NAME", account.ACCOUNT_NAME);
            return View(account);
        }

        // POST: Accounts/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ACCOUNT_ID,ACCOUNT_NAME,ACCOUNT_PASSWORD,ACCOUNT_ROLE")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ACCOUNT_NAME = new SelectList(db.AccountInfos, "ACCOUNT_NAME", "USER_NAME", account.ACCOUNT_NAME);
            return View(account);
        }


        // GET: Accounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        public ActionResult Create()
        {
            ViewBag.ACCOUNT_NAME = new SelectList(db.AccountInfos, "ACCOUNT_NAME", "USER_NAME");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ACCOUNT_ID,ACCOUNT_NAME,ACCOUNT_PASSWORD,ACCOUNT_ROLE")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ACCOUNT_NAME = new SelectList(db.AccountInfos, "ACCOUNT_NAME", "USER_NAME", account.ACCOUNT_NAME);
            return View(account);
        }


        // GET: Accounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
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
