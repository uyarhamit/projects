using FaturaOnay.Models;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FaturaOnay.Controllers
{
    public class Departman_ApiController : ApiController
    {

        [HttpGet]
        public List<_department> DepartmanList()
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
            return list;
        }

        [HttpPost]
        public _return AddNew(_department departman)
        {
            _return result = new _return();
            using (var db = new FaturaEntities())
            {
                try
                {
                    Departments rec = new Departments();
                    rec.Name = departman.departmenName;
                    db.Departments.Add(rec);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Departman Ekleme Başarılı.";
                    }
                    else
                    {
                        result.status = -1;
                        result.statusText = "Ekleme Sırasında Bir Hata Oluştu.";
                    }

                }
                catch (Exception ex)
                {
                    result.status = -1;
                    result.statusText = "Ekleme Sırasında Bir Hata Oluştu.";
                }

            }
            return result;
        }

        [HttpPost]
        public _return Update(_department rec)
        {
            _return result = new _return();
            using (var db = new FaturaEntities())
                try
                {
                    var list = db.Departments.Where(x => x.Id == rec.id).FirstOrDefault();
                    list.Name = rec.departmenName;
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Departman güncellendi.";
                        result.recordId = rec.id;
                    }
                }
                catch (Exception)
                {
                    result.status = 0;
                    result.statusText = "Güncelleme esnasında bir hata oluştu.";
                }
            return result;
        }

        [HttpGet]
        public _department Detail(int id)
        {
            _return result = new _return();
            _department list = new _department();
            using (var db = new FaturaEntities())
            {
                try
                {
                    list = (from tbl in db.Departments
                            where tbl.Id == id
                            select new _department
                            {
                                id = tbl.Id,
                                departmenName=tbl.Name
                            }).FirstOrDefault();
                }
                catch (Exception ex)
                {
                }

                return list;
            }

        }
        [HttpGet]
        public _return Delete(int id)
        {
            _return result = new _return();
            Departments departman = new Departments();

            using (var db = new FaturaEntities())
            {
                try
                {
                    departman = db.Departments.Where(x => x.Id == id).FirstOrDefault();
                    db.Departments.Remove(departman);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Departman başarıyla silindi.";
                    }
                    else
                    {
                        result.statusText = "Departman silme esnasında hata oluştu.";
                    }

                }
                catch (Exception ex)
                {
                    result.status = -1;
                    result.statusText = "Departman silme esnasında hata oluştu.";
                }

                return result;

            }
        }
        [HttpGet]
        public List<_department> DutyUser(int id)
        {
            List<_department> list = new List<_department>();
            using (var db = new FaturaEntities())
            {
                list = (from tbl in db.Departments
                        join invoice in db.Invoice_User on tbl.Id equals invoice.DepartmentId
                        where invoice.InvoiceId == id
                        orderby tbl.Name
                        select new _department
                        {
                            id = tbl.Id,
                            departmenName = tbl.Name
                        }).ToList();

            }
            return list;
        }
    }
}
