using FaturaOnay.Models;
using FaturaOnay.Static_Code;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaturaOnay.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserCheck()
        {
            string username = Request["txtUsername"];
            string password = Request["txtPassword"];

            password = StaticMethods.Sha512Encryption(password);

            if (username != "" && password != "")
            {
                using (var db = new FaturaEntities())
                {
                    try
                    {
                        List<_user> user = (from tbl in db.Users
                                            where tbl.Username == username && tbl.Password == password
                                            select new _user
                                            {
                                                id = tbl.Id,
                                                departmanId=tbl.DepartmentId.Value,
                                                name=tbl.Name,
                                                password=tbl.Password,
                                                type=tbl.Type.Value                                                

                                            }).ToList();
                        if (user != null)
                        {
                            Session["UserInfo"] = user;
                            Session["DepartmanId"] = user.First().departmanId;
                            Session["UserId"] = user.First().id;
                            Session["UserName"] = user.First().name;
                            Session["UserLevel"] = user.First().type;
                            if (user.First().type==2)
                            {
                                var menu = db.Menu.OrderBy(x => x.ORDERNUM).ToList();
                                Session["Menu"] = menu;
                            }
                            else
                            {
                                var menu = db.Menu.Where(x => x.WRITE == false).OrderBy(y => y.ORDERNUM).ToList();
                                Session["Menu"] = menu;
                            }

                            return RedirectToAction("Index");

                        }
                        else
                        {
                            Session["loginError"] = "Check your username or password.";
                            return RedirectToAction("Index", "Login");
                        }
                    }
                    catch (Exception ex)
                    {
                        Session["loginError"] = "Please check username or password.";
                        return RedirectToAction("Index", "Login");
                    }



                }
            }
            else
            {
                Session["loginError"] = "Check your username or password.";
                return RedirectToAction("Index", "Login");
            }
        }
    }
}