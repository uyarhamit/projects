using FaturaOnay.Models;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FaturaOnay.Controllers
{
    public class Invoice_ApiController : ApiController
    {
        [HttpPost]
        public _return AddWExcel(_invoice rec)
        {
            string action = "";
            _return result = new _return();
            Invoices invoice = new Invoices();
            int row = 0;
            using (var db = new FaturaEntities())
                try
                {
                    for (row = 0; row < rec.invoiceDetail.Length; row++)
                    {

                        invoice.InvoiceNumber = rec.invoiceDetail[row].ToString();
                        row++;
                        invoice.InvoiceDate = Convert.ToDateTime(rec.invoiceDetail[row].ToString());
                        row++;
                        invoice.TaxNumber = rec.invoiceDetail[row].ToString();
                        row++;
                        invoice.InvoiceCompany = rec.invoiceDetail[row].ToString();
                        row++;
                        string str = rec.invoiceDetail[row].Replace('.', ',').ToString();
                        invoice.Total = Convert.ToDecimal(rec.invoiceDetail[row].Replace(',', '.'));
                        invoice.TotalStr = rec.invoiceDetail[row].Replace('.', ',');
                        row++;
                        invoice.Unite = rec.invoiceDetail[row].ToString();
                        row++;
                        invoice.Branch = rec.invoiceDetail[row].ToString();
                        invoice.InvoiceMonth = invoice.InvoiceDate.Value.Month.ToString();
                        invoice.InvoiceYear = invoice.InvoiceDate.Value.Year.ToString();
                        invoice.ConfirmedByAllUser = false;
                        invoice.Invoice = invoice.TaxNumber + "_" + invoice.InvoiceNumber+".pdf";
                        db.Invoices.Add(invoice);
                        db.SaveChanges();
                        action = "Fatura Ekleme";
                        Log(invoice.Id, rec.userId, action);
                    }
                    result.status = 1;
                    result.statusText = "Faturalar ekleme başarılı.";
                }
                catch (Exception ex)
                {
                    result.status = 0;
                    result.statusText = "Ekleme esnasında bir hata oluştu." + rec.invoiceDetail[row].ToString();
                }
            return result;
        }
        [HttpPost]
        public _return AddNew(_invoice rec)
        {
            string action = "";
            _return result = new _return();
            Invoices invoice = new Invoices();
            using (var db = new FaturaEntities())
                try
                {
                    invoice.InvoiceCompany = rec.invoiceCompany;
                    invoice.InvoiceDate = rec.invoiceDate;
                    invoice.Total = rec.total;
                    invoice.TotalStr = rec.total.ToString();
                    invoice.InvoiceNumber = rec.invoiceNumber;
                    invoice.InvoiceYear = rec.invoiceDate.Year.ToString();
                    invoice.InvoiceMonth = rec.invoiceDate.Month.ToString();
                    invoice.TaxNumber = rec.taxNumber.ToString();
                    if (rec.document == null)
                    {
                        invoice.Invoice = invoice.TaxNumber + "_" + invoice.InvoiceNumber + ".pdf";
                    }
                    else
                    {
                        invoice.Invoice = rec.document;
                    }
                    invoice.Branch = rec.branch;
                    db.Invoices.Add(invoice);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        action = "Fatura Ekleme";
                        Log(invoice.Id, rec.userId, action);
                        if (rec.users.Count() > 0)
                        {
                            Invoice_User duty = new Invoice_User();
                            for (int i = 0; i < rec.users.Length; i++)
                            {
                                duty.InvoiceId = invoice.Id;
                                string user = rec.users[i].ToString();
                                duty.UserId = Convert.ToInt32(user);
                                duty.Confirm = false;
                                db.Invoice_User.Add(duty);
                                db.SaveChanges();
                                action = "Fatura Yetkilendirme.";
                                Log(duty.Id, rec.userId, action);
                            }
                        }
                        if (rec.departmans.Count() > 0)
                        {
                            Invoice_User duty = new Invoice_User();
                            for (int i = 0; i < rec.departmans.Length; i++)
                            {
                                duty.InvoiceId = invoice.Id;
                                string department = rec.departmans[i].ToString();
                                duty.DepartmentId = Convert.ToInt32(department);
                                db.Invoice_User.Add(duty);
                                db.SaveChanges();
                                action = "Fatura Yetkilendirme.";
                                Log(duty.Id, rec.userId, action);
                            }
                        }
                        result.status = 1;
                        result.statusText = "Fatura ekleme başarılı.";
                    }
                }
                catch (Exception ex)
                {
                    result.status = 0;
                    result.statusText = "Ekleme esnasında bir hata oluştu.";
                }
            return result;
        }
        [HttpPost]
        public _return assignPersons(_invoice rec)
        {
            _return result = new _return();
            using (var db = new FaturaEntities())
                try
                {
                    foreach (var item in rec.invoiceIds)
                    {
                        var count = db.Invoice_User.Where(x => x.InvoiceId == item && x.Confirm == false && (x.UserId == rec.userId || x.DepartmentId == rec.departmanId)).Count(); 
                        if (count == 0)
                        {
                            Invoice_User duty = new Invoice_User();
                            duty.InvoiceId = item;
                            if (rec.userId>0)
                            {
                                duty.UserId = rec.userId;
                            }
                            if (rec.departmanId>0)
                            {
                                duty.DepartmentId = rec.departmanId;
                            }
                            duty.Confirm = false;
                            db.Invoice_User.Add(duty);
                            db.SaveChanges();
                        }
                        result.status = 1;
                        result.statusText = "Atamalar başarıyla tamamlandı.";
                    }
                   
                }
                catch (Exception)
                {
                    result.status = 0;
                    result.statusText = "Atamalar esnasında bir hata oluştu.";
                }
            return result;
        }
        [HttpPost]
        public _return Update(_invoice rec)
        {
            string action = "";
            _return result = new _return();
            using (var db = new FaturaEntities())
                try
                {
                    var invoice = db.Invoices.Where(x => x.Id == rec.id).FirstOrDefault();
                    invoice.InvoiceCompany = rec.invoiceCompany;
                    invoice.InvoiceDate = rec.invoiceDate;
                    invoice.Total = rec.total;
                    invoice.TotalStr = rec.total.ToString();
                    invoice.InvoiceNumber = rec.invoiceNumber;
                    invoice.InvoiceYear = rec.invoiceDate.Year.ToString();
                    invoice.InvoiceMonth = rec.invoiceDate.Month.ToString();
                    invoice.TaxNumber = rec.taxNumber;
                    if (rec.document == null)
                    {
                        invoice.Invoice = invoice.TaxNumber + "_" + invoice.InvoiceNumber + ".pdf";
                    }
                    else
                    {
                        invoice.Invoice = rec.document;
                    }
                    invoice.Branch = rec.branch;
                    db.SaveChanges();
                    action = "Fatura Güncelleme.";
                    Log(rec.id, rec.userId, action);
                    var duty = db.Invoice_User.Where(x => x.InvoiceId == rec.id && x.Confirm==false);
                    Invoice_User new_duty = new Invoice_User();
                    if (rec.users.Count() > 0)
                    {

                        int row = 0;
                        string user = "";
                        int userid = 0;
                        if (duty.Count()>0)
                        {
                            foreach (var item in duty)
                            {
                                if (rec.users.Count() >= row)
                                {
                                    try
                                    {
                                        user = rec.users[row].ToString();
                                        if (user != "" && user != null)
                                        {
                                            item.UserId = Convert.ToInt32(user);
                                        }
                                        item.DepartmentId = null;
                                        item.Confirm = false;
                                        action = "Fatura Yetkilendirme.";
                                        Log(item.Id, rec.userId, action);
                                        row++;
                                    }
                                    catch
                                    {
                                        var remove = db.Invoice_User.Where(x => x.Id == item.Id).FirstOrDefault();
                                        db.Invoice_User.Remove(remove);
                                    }
                                }
                            }
                            db.SaveChanges();
                            if (rec.users.Count() > duty.Count())
                            {
                                user = rec.users[row].ToString();
                                userid = Convert.ToInt32(user);
                                var varMi = db.Invoice_User.Where(x => x.InvoiceId == rec.id && x.Confirm == true && x.UserId == userid).Count();
                                if (varMi<1)
                                {
                                    Invoice_User yetki = new Invoice_User();
                                    if (user != "" && user != null)
                                    {
                                        yetki.UserId = userid;
                                    }
                                    yetki.DepartmentId = null;
                                    yetki.Confirm = false;
                                    yetki.InvoiceId = rec.id;
                                    db.Invoice_User.Add(yetki);
                                    db.SaveChanges();
                                    action = "Fatura Yetkilendirme.";
                                    Log(rec.id, rec.userId, action);
                                    row++;
                                }
                               
                            }

                        }
                        else
                        {
                            int userId = 0;
                            string new_user = "";
                            for (int i = 0; i < rec.users.Length; i++)
                            {
                                new_user = rec.users[i].ToString();
                                userId = Convert.ToInt32(new_user);
                                var count = db.Invoice_User.Where(x => x.UserId == userId&&x.InvoiceId == rec.id).Count();
                                if (count<1)
                                {
                                    new_duty.UserId = userId;
                                    new_duty.Confirm = false;
                                    new_duty.InvoiceId = rec.id;
                                    db.Invoice_User.Add(new_duty);
                                    db.SaveChanges();
                                    action = "Fatura Yetkilendirme.";
                                    Log(new_duty.Id, rec.userId, action);
                                }
                            }
                        }
                        

                    }
                    if (rec.departmans.Count() > 0)
                    {
                        int row = 0;
                        string departman = "";
                        int departmanid = 0;
                        if (duty.Count()>0)
                        {
                            foreach (var item in duty)
                            {
                                if (rec.departmans.Count() >= row)
                                {
                                    try
                                    {
                                        departman = rec.departmans[row].ToString();
                                        if (departman != "" && departman != null)
                                        {
                                            item.DepartmentId = Convert.ToInt32(departman);
                                        }
                                        item.UserId = null;
                                        item.Confirm = false;
                                        action = "Fatura Yetkilendirme.";
                                        Log(item.Id, rec.userId, action);
                                        row++;
                                    }
                                    catch
                                    {
                                        var remove = db.Invoice_User.Where(x => x.Id == item.Id).FirstOrDefault();
                                        db.Invoice_User.Remove(remove);
                                    }
                                }

                            }
                            db.SaveChanges();
                            if (rec.departmans.Count() > duty.Count())
                            {
                                departman = rec.departmans[row].ToString();
                                departmanid= Convert.ToInt32(departman);
                                var varMi = db.Invoice_User.Where(x => x.InvoiceId == rec.id && x.Confirm == true && x.DepartmentId == departmanid).Count();
                                if (varMi < 1)
                                {
                                    Invoice_User yetki = new Invoice_User();
                                    if (departman != "" && departman != null)
                                    {
                                        yetki.DepartmentId = departmanid;
                                    }
                                    yetki.UserId = null;
                                    yetki.Confirm = false;
                                    yetki.InvoiceId = rec.id;
                                    db.Invoice_User.Add(yetki);
                                    db.SaveChanges();
                                    action = "Fatura Yetkilendirme.";
                                    Log(rec.id, rec.userId, action);
                                    row++;
                                }
                            }
                            
                        }
                        else
                        {
                            int departmentId = 0;
                            string department = "";
                            for (int i = 0; i < rec.departmans.Length; i++)
                            {
                                department = rec.departmans[i].ToString();
                                departmentId= Convert.ToInt32(department);
                                var count = db.Invoice_User.Where(x => x.DepartmentId == departmentId).Count();
                                if (count < 1)
                                {
                                    new_duty.DepartmentId = departmentId;
                                    new_duty.Confirm = false;
                                    new_duty.InvoiceId = rec.id;
                                    db.Invoice_User.Add(new_duty);
                                    db.SaveChanges();
                                    action = "Fatura Yetkilendirme.";
                                    Log(new_duty.Id, rec.userId, action);
                                }
                            }

                        }

                    }
                    result.status = 1;
                    result.statusText = "Fatura ve yetkilendirmeler güncellendi.";
                    result.recordId = rec.id;
                }
                catch (Exception exs)
                {
                    result.status = 0;
                    result.statusText = "Güncelleme esnasında bir hata oluştu.";
                }
            return result;
        }
        [HttpGet]
        public _invoice Detail(int id)
        {
            _invoice list = new _invoice();
            using (var db = new FaturaEntities())
            {
                try
                {
                    list = (from tbl in db.Invoices
                            join user in db.Invoice_User on tbl.Id equals user.InvoiceId
                            where tbl.Id == id
                            select new _invoice
                            {
                                id = tbl.Id,
                                invoiceCompany = tbl.InvoiceCompany,
                                invoiceDate = tbl.InvoiceDate.Value,
                                invoiceNumber = tbl.InvoiceNumber,
                                total = tbl.Total.Value,
                                comfirmedByAllUser = tbl.ConfirmedByAllUser,
                                document = tbl.Invoice,
                                branch=tbl.Branch,
                                taxNumber=tbl.TaxNumber,
                                totalS=tbl.TotalStr

                            }).FirstOrDefault();
                    if (list == null)
                    {
                        list = (from tbl in db.Invoices
                                where tbl.Id == id
                                select new _invoice
                                {
                                    id = tbl.Id,
                                    invoiceCompany = tbl.InvoiceCompany,
                                    invoiceDate = tbl.InvoiceDate.Value,
                                    invoiceNumber = tbl.InvoiceNumber,
                                    total = tbl.Total.Value,
                                    comfirmedByAllUser = tbl.ConfirmedByAllUser,
                                    document = tbl.Invoice,
                                    branch = tbl.Branch,
                                    taxNumber=tbl.TaxNumber,
                                    totalS = tbl.TotalStr

                                }).FirstOrDefault();
                    }
                }
                catch (Exception ex)
                {
                }

                return list;
            }

        }
        [HttpPost]
        public _return Delete(_invoice rec)
        {
            string action = "";
            _return result = new _return();
            Invoices invoice = new Invoices();
            List<ConfirmDetails> confirm = new List<ConfirmDetails>();
            List<Invoice_User> duty = new List<Invoice_User>();

            using (var db = new FaturaEntities())
            {
                try
                {
                    invoice = db.Invoices.Where(x => x.Id ==rec.id).FirstOrDefault();
                    db.Invoices.Remove(invoice);
                    int sonuc = db.SaveChanges();

                    confirm = db.ConfirmDetails.Where(x => x.InvoiceId == rec.id).ToList();
                    if (confirm != null)
                    {
                        foreach (var item in confirm)
                        {
                            db.ConfirmDetails.Remove(item);
                            db.SaveChanges();
                        }
                    }
                    duty = db.Invoice_User.Where(x => x.InvoiceId == rec.id).ToList();
                    if (duty != null)
                    {
                        foreach (var item in duty)
                        {
                            db.Invoice_User.Remove(item);
                            db.SaveChanges();
                        }
                    }
                        result.status = 1;
                        result.statusText = "Fatura başarıyla silindi.";
                    action = "Fatura Silme";
                    Log(rec.id,rec.userId,action);
                }
                catch (Exception)
                {
                    result.status = -1;
                    result.statusText = "Fatura silme esnasında hata oluştu.";
                }

                return result;

            }
        }
        [HttpPost]
        public _return ConfirmInvoice(_confirmDetail rec)
        {
            string action = "";
            _return result = new _return();
            using (var db = new FaturaEntities())
                try
                {
                    if (rec.id > 0)
                    {
                        var invoice = db.ConfirmDetails.Where(x => x.ConfirmingUserId == rec.confirmingUserId && x.InvoiceId == rec.invoiceId).FirstOrDefault();
                        invoice.ConfirmingDetail = rec.confirmingDetail;
                        db.SaveChanges();
                        action = "Fatura Detay Ekleme";
                        Log(rec.id, rec.userId, action);
                        result.status = 1;
                        result.statusText = "Fatura onay açıklaması güncellendi.";
                        var onay = db.Invoice_User.Where(x => x.InvoiceId == rec.invoiceId && x.UserId == rec.confirmingUserId).FirstOrDefault();
                        onay.Confirm = true;
                        db.SaveChanges();
                        action = "Fatura Onaylama";
                        Log(onay.Id, rec.userId, action);
                    }
                    else
                    {
                        var onay = db.Invoice_User.Where(x => x.InvoiceId == rec.invoiceId && x.UserId == rec.confirmingUserId || x.DepartmentId == rec.departmanId).FirstOrDefault();
                        onay.Confirm = true;
                        int sonuc = db.SaveChanges();
                        action = "Fatura Onaylama";
                        Log(onay.Id, rec.userId, action);
                        if (sonuc > 0)
                        {
                            ConfirmDetails invoice = new ConfirmDetails();
                            invoice.ConfirmingUserId = rec.confirmingUserId;
                            invoice.ConfirmingDetail = rec.confirmingDetail;
                            invoice.InvoiceId = rec.invoiceId;
                            invoice.ConfirmingDate = DateTime.Now;
                            invoice.ConfirmingTime = DateTime.Now.TimeOfDay;
                            db.ConfirmDetails.Add(invoice);
                            db.SaveChanges();
                            action = "Fatura Detay Ekleme";
                            Log(onay.Id, rec.userId, action);
                            result.status = 1;
                            result.statusText = "Fatura onaylandı.";
                        }
                    }

                }
                catch (Exception ex)
                {
                    result.status = 0;
                    result.statusText = "Fatura onaylama esnasında bir hata oluştu.";
                }
            return result;
        }
        [HttpGet]
        public _confirmDetail DescriptionDetail(int invoiceId, int userId)
        {
            _confirmDetail list = new _confirmDetail();
            using (var db = new FaturaEntities())
            {
                try
                {
                    list = (from tbl in db.ConfirmDetails
                            where tbl.ConfirmingUserId == userId && tbl.InvoiceId == invoiceId
                            select new _confirmDetail
                            {
                                id = tbl.Id,
                                confirmingDetail = tbl.ConfirmingDetail

                            }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                }

                return list;
            }

        }
        [HttpGet]
        public string Documents(int id)
        {
            string list = "";
            using (var db = new FaturaEntities())
            {
                try
                {
                    list = db.Invoices.Where(x => x.Id == id).Select(y=>y.Invoice).SingleOrDefault();
                }
                catch (Exception ex)
                {
                }

                return list;
            }

        }
        public void Log(int actionId, int user, string action)
        {
            using (var db = new FaturaEntities())
                try
                {
                    Log log = new Log();
                    log.ActionId = actionId;
                    log.UserId = user;
                    log.Time = DateTime.Now;
                    log.Action = action;
                    db.Log.Add(log);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                }
        }

    }
}
