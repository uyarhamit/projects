using MODEL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FaturaOnay.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public ActionResult UploadFiles(HttpPostedFileBase[] document)
        {
            if (System.Web.HttpContext.Current.Request.Files.Count > 0)
            {
                string docFullName = System.Web.HttpContext.Current.Request.Files.AllKeys[0];
                int index = docFullName.LastIndexOf('.');
            }
            //Ensure model state is valid  
            if (ModelState.IsValid)
            {   //iterating through multiple file collection   
                try
                {
                    foreach (HttpPostedFileBase file in document)
                    {
                        //Checking file is available to save.  
                        if (file != null)
                        {
                            var InputFileName = Path.GetFileName(file.FileName);
                            var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                            //Save file to server folder  
                            if (System.IO.File.Exists(ServerSavePath))
                            {
                                System.IO.File.Delete(ServerSavePath);
                            }
                            file.SaveAs(ServerSavePath);
                            //assigning file uploaded status to ViewBag for showing message to user.  

                        }
                    }
                    Session["dosyaS"] = document.Count().ToString() + "adet dosya başarıyla yüklendi.";
                }
                catch (Exception)
                {
                    Session["dosyaB"] = "Dosya yükleme başarısız.";
                }

            }
            return RedirectToAction("MultipleUpload", "Invoice");
        }
        public ActionResult DeleteFile(int id)
        {
            using (var db=new FaturaEntities())
            {
                var doc = db.Invoices.Where(x => x.Id==id).SingleOrDefault();
                var InputFileName = Path.GetFileName(doc.Invoice);
                var ServerSavePath = Path.Combine(Server.MapPath("~/UploadedFiles/") + InputFileName);
                //Save file to server folder  
                if (System.IO.File.Exists(ServerSavePath))
                {
                    System.IO.File.Delete(ServerSavePath);
                    Session["dosyaS"] = doc.InvoiceNumber+" nolu fatura silindi.";
                }
                else
                {
                    Session["dosyaB"]= doc.InvoiceNumber + " nolu fatura bulunamadı.";
                }

            }
            return RedirectToAction("MultipleUpload", "Invoice");
        }
    }
}