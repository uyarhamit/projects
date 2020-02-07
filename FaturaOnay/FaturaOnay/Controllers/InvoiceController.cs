using FaturaOnay.Models;
using MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.OleDb;
using System.Configuration;
using System.Data;
using FaturaOnay.Static_Code;

namespace FaturaOnay.Controllers
{
    public class InvoiceController : Controller
    {
        // GET: Invoice
        [HttpGet]
        public ActionResult Index(_invoice invoice)
        {
            string start = "";
            string end = "";
            string branch = "";
            //if (StaticMethods.Querystr(Request.Params["startDate"]) != "")
            //{
                start = Request.Params["startDate"];
                end = Request.Params["endDate"];
                branch = Request.Params["branch"];
            //}
            List<_invoice> list = new List<_invoice>();
            using (var db = new FaturaEntities())
            {
                if (start == null && end == null && branch == null)
                {
                    list = (from tbl in db.Invoices
                            select new _invoice
                            {
                                id = tbl.Id,
                                comfirmedByAllUser = tbl.ConfirmedByAllUser.Value,
                                invoiceCompany = tbl.InvoiceCompany,
                                invoiceDate = tbl.InvoiceDate.Value,
                                invoiceMonth = tbl.InvoiceMonth,
                                invoiceNumber = tbl.InvoiceNumber,
                                invoiceYear = tbl.InvoiceYear,
                                total = tbl.Total.Value,
                                totalS = tbl.TotalStr + " " + tbl.Unite,
                                document = tbl.Invoice,
                                unite = tbl.Total.Value + " " + tbl.Unite,
                                branch = tbl.Branch,


                            }).ToList();
                }
                else
                {
                    DateTime startDate = new DateTime();
                    DateTime endDate = new DateTime();
                    if (start != null)
                    {
                        startDate = Convert.ToDateTime(start);
                    }
                    if (end != null)
                    {
                        endDate = Convert.ToDateTime(end);
                    }
                    if (branch == null)
                    {

                        list = (from tbl in db.Invoices
                                where tbl.InvoiceDate >= startDate && tbl.InvoiceDate <= endDate
                                select new _invoice
                                {
                                    id = tbl.Id,
                                    comfirmedByAllUser = tbl.ConfirmedByAllUser.Value,
                                    invoiceCompany = tbl.InvoiceCompany,
                                    invoiceDate = tbl.InvoiceDate.Value,
                                    invoiceMonth = tbl.InvoiceMonth,
                                    invoiceNumber = tbl.InvoiceNumber,
                                    invoiceYear = tbl.InvoiceYear,
                                    total = tbl.Total.Value,
                                    totalS = tbl.TotalStr + " " + tbl.Unite,
                                    document = tbl.Invoice,
                                    unite = tbl.Total.Value + " " + tbl.Unite,
                                    branch = tbl.Branch,
                                }).ToList();
                    }
                    else
                    {
                        list = (from tbl in db.Invoices
                                where tbl.InvoiceDate >= startDate && tbl.InvoiceDate <= endDate && tbl.Branch == branch
                                select new _invoice
                                {
                                    id = tbl.Id,
                                    comfirmedByAllUser = tbl.ConfirmedByAllUser.Value,
                                    invoiceCompany = tbl.InvoiceCompany,
                                    invoiceDate = tbl.InvoiceDate.Value,
                                    invoiceMonth = tbl.InvoiceMonth,
                                    invoiceNumber = tbl.InvoiceNumber,
                                    invoiceYear = tbl.InvoiceYear,
                                    total = tbl.Total.Value,
                                    totalS = tbl.TotalStr + " " + tbl.Unite,
                                    document = tbl.Invoice,
                                    unite = tbl.Total.Value + " " + tbl.Unite,
                                    branch = tbl.Branch,
                                }).ToList();
                    }

                }
            }
            return View(list);
        }
        public ActionResult Add()
        {
            return View();
        }
        public ActionResult Confirm()
        {
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            int departmanId = Convert.ToInt32(Session["DepartmanId"].ToString());
            List<_invoice> list = new List<_invoice>();
            using (var db = new FaturaEntities())
            {
                int userLevel = Convert.ToInt32(Session["UserLevel"].ToString());

                if (userLevel == 2)
                {
                    list = (from tbl in db.Invoices
                            join user in db.Invoice_User on tbl.Id equals user.InvoiceId
                            where user.Confirm == false
                            select new _invoice
                            {
                                id = tbl.Id,
                                comfirmedByAllUser = tbl.ConfirmedByAllUser.Value,
                                invoiceCompany = tbl.InvoiceCompany,
                                invoiceDate = tbl.InvoiceDate.Value,
                                invoiceMonth = tbl.InvoiceMonth,
                                invoiceNumber = tbl.InvoiceNumber,
                                invoiceYear = tbl.InvoiceYear,
                                total = tbl.Total.Value,
                                document = tbl.Invoice,
                                userName=user.UserName,
                                userId=user.UserId.Value == null ? 0 : user.UserId.Value,
                                departmanId=user.DepartmentId.Value == null ? 0 : user.DepartmentId.Value,
                                departmanName=user.DepartmanName,
                                branch=tbl.Branch,
                                totalS=tbl.TotalStr+" "+tbl.Unite

                            }).ToList();
                }
                else
                {
                    list = (from tbl in db.Invoices
                            join user in db.Invoice_User on tbl.Id equals user.InvoiceId
                            where user.Confirm == false && (user.DepartmentId == departmanId || user.UserId == userId)
                            select new _invoice
                            {
                                id = tbl.Id,
                                comfirmedByAllUser = tbl.ConfirmedByAllUser.Value,
                                invoiceCompany = tbl.InvoiceCompany,
                                invoiceDate = tbl.InvoiceDate.Value,
                                invoiceMonth = tbl.InvoiceMonth,
                                invoiceNumber = tbl.InvoiceNumber,
                                invoiceYear = tbl.InvoiceYear,
                                total = tbl.Total.Value,
                                document = tbl.Invoice,
                                userId = user.UserId.Value == null ? 0 : user.UserId.Value,
                                departmanId = user.DepartmentId.Value == null ? 0 : user.DepartmentId.Value,
                                totalS = tbl.TotalStr + " " + tbl.Unite

                            }).ToList();
                }
            }
            return View(list);
        }
        public ActionResult Confirmed()
        {
            int userLevel = Convert.ToInt32(Session["UserLevel"].ToString());
            int userId = Convert.ToInt32(Session["UserId"].ToString());
            int departmanId = Convert.ToInt32(Session["DepartmanId"].ToString());
            List<_confirmDetail> list = new List<_confirmDetail>();
            using (var db = new FaturaEntities())
            {
                if (userLevel == 2)
                {
                    list = (from tbl in db.ConfirmDetails
                            join invoce in db.Invoices on tbl.InvoiceId equals invoce.Id
                            orderby tbl.ConfirmingDate,tbl.ConfirmingTime
                            select new _confirmDetail
                            {
                                id = tbl.Id,
                                confirmingDate = tbl.ConfirmingDate.Value,
                                confirmingDetail = tbl.ConfirmingDetail,
                                confirmingTimeStr = tbl.ConfirmingTime.ToString().Substring(0,8),
                                confirmingTime = tbl.ConfirmingTime.Value,
                                userName =tbl.ConfirmingUserName,
                                invoiceNo=invoce.InvoiceNumber,
                                document=invoce.Invoice,
                                invoiceId=invoce.Id,
                                company=invoce.InvoiceCompany,
                                totalS=invoce.TotalStr+" "+invoce.Unite,
                                total=invoce.Total.Value
                                
                            }).ToList();
                }
                else
                {
                    list = (from tbl in db.ConfirmDetails
                            join invoce in db.Invoices on tbl.InvoiceId equals invoce.Id
                            where tbl.ConfirmingUserId == userId
                            orderby tbl.ConfirmingDate, tbl.ConfirmingTime
                            select new _confirmDetail
                            {
                                id = tbl.Id,
                                confirmingDate = tbl.ConfirmingDate.Value,
                                confirmingDetail = tbl.ConfirmingDetail,
                                confirmingTime = tbl.ConfirmingTime.Value,
                                confirmingTimeStr = tbl.ConfirmingTime.ToString().Substring(0, 8),
                                invoiceNo = invoce.InvoiceNumber,
                                invoiceId = invoce.Id,
                                company = invoce.InvoiceCompany,
                                totalS = invoce.TotalStr + " " + invoce.Unite,
                            }).ToList();
                }
            }
            return View(list);
        }
        public ActionResult ImportFile()
        {
            return View();
        }
        public ActionResult MultipleUpload()
        {
            string fullPath = "";
               List <Invoices> list = new List<Invoices>();
               List <Invoices> model = new List<Invoices>();
            using (var db= new FaturaEntities())
            {
                list = db.Invoices.ToList();
            }
            foreach (var item in list)
            {
                fullPath = Request.MapPath("~/UploadedFiles/" + item.TaxNumber + "_" +item.InvoiceNumber + ".pdf");
                if (System.IO.File.Exists(fullPath))
                {
                    model.Add(item);
                }
            }
            return View(model);
        }
    }
}