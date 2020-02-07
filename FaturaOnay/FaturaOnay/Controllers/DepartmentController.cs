using FaturaOnay.Models;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaturaOnay.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        public ActionResult Index()
        {
            List<_department> list = new List<_department>();
            using (var db = new FaturaEntities())
            {
                list = (from tbl in db.Departments
                        orderby tbl.Name
                        select new _department
                        {
                            id = tbl.Id,
                            departmenName = tbl.Name
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