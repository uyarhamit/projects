using FaturaOnay.Models;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaturaOnay.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            List<_user> list = new List<_user>();
            using (var db = new FaturaEntities())
            {
                list = (from tbl in db.Users
                        select new _user
                        {
                            id = tbl.Id,
                            name=tbl.Name,
                            departman=tbl.Department,
                            type=tbl.Type.Value,
                            userType=tbl.Type.Value==1? "Süper Admin" : (tbl.Type.Value == 2 ? "Admin":"Kullanıcı"),
                            branch=tbl.Branch

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