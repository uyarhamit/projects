using FaturaOnay.Models;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaturaOnay.Controllers
{
    public class BranchController : Controller
    {
        // GET: Branch
        public ActionResult Index()
        {
            List<_branch> list = new List<_branch>();
            using (var db = new FaturaEntities())
            {
                list = (from tbl in db.Branch
                        orderby tbl.Name
                        select new _branch
                        {
                            id = tbl.Id,
                            name = tbl.Name
                        }).ToList();

            }
            return View(list);
        }

        public ActionResult Add()
        {
            return View();
        }
    }
}