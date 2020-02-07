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
    public class Branch_ApiController : ApiController
    {


        [HttpGet]
        public List<_branch> BranchList()
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
            return list;
        }
        [HttpPost]
        public _return AddNew(_branch branch)
        {
            _return result = new _return();
            using (var db = new FaturaEntities())
            {
                try
                {
                    Branch rec = new Branch();
                    rec.Name = branch.name;
                    db.Branch.Add(rec);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Şube Ekleme Başarılı.";
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
        public _return Update(_branch rec)
        {
            _return result = new _return();
            using (var db = new FaturaEntities())
                try
                {
                    var list = db.Branch.Where(x => x.Id == rec.id).FirstOrDefault();
                    list.Name = rec.name;
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Şube güncellendi.";
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
        public _branch Detail(int id)
        {
            _return result = new _return();
            _branch list = new _branch();
            using (var db = new FaturaEntities())
            {
                try
                {
                    list = (from tbl in db.Branch
                            where tbl.Id == id
                            select new _branch
                            {
                                id = tbl.Id,
                                name = tbl.Name
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
            Branch branch = new Branch();

            using (var db = new FaturaEntities())
            {
                try
                {
                    branch = db.Branch.Where(x => x.Id == id).FirstOrDefault();
                    db.Branch.Remove(branch);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Şube başarıyla silindi.";
                    }
                    else
                    {
                        result.statusText = "Şube silme esnasında hata oluştu.";
                    }

                }
                catch (Exception ex)
                {
                    result.status = -1;
                    result.statusText = "Şube silme esnasında hata oluştu.";
                }

                return result;

            }
        }
    }
}
