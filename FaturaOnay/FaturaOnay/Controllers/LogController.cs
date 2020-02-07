using FaturaOnay.Models;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaturaOnay.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Index()
        {
            List<_log> list = new List<_log>();
            using (var db = new FaturaEntities())
            {
                list = (from tbl in db.Log
                        orderby tbl.Time
                        select new _log
                        {
                            id = tbl.Id,
                            action=tbl.Action,
                            time=tbl.Time.Value,
                            userName=tbl.UserName
                        }).ToList();

            }
            return View(list);
        }
    }
}