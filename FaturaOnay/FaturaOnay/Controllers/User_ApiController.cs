using FaturaOnay.Models;
using FaturaOnay.Static_Code;
using MODEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FaturaOnay.Controllers
{
    public class User_ApiController : ApiController
    {

        [HttpPost]
        public _return AddNew(_user user)
        {
            string psw = StaticMethods.Sha512Encryption(user.password);
            _return result = new _return();
            using (var db = new FaturaEntities())
            {
                try
                {
                    Users rec = new Users();
                    rec.Name = user.name;
                    rec.Type = user.type;
                    rec.DepartmentId =user.departmanId;
                    rec.Username = user.userName;
                    rec.Password = psw;
                    rec.Branch = user.branch;
                    db.Users.Add(rec);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Kullanıcı Ekleme Başarılı.";
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
        public _return Update(_user rec)
        {
            string psw = rec.password;
            if (psw != "" && psw !=null)
            {
                psw = StaticMethods.Sha512Encryption(rec.password);
            }
            _return result = new _return();
            using (var db = new FaturaEntities())
                try
                {
                    var list = db.Users.Where(x => x.Id == rec.id).FirstOrDefault();
                    list.Name = rec.name;
                    list.Username = rec.userName;
                    list.Branch = rec.branch;
                    if (psw != "" && psw != null)
                    {
                        list.Password = psw;
                    }
                    list.Type = rec.type;
                    list.DepartmentId = rec.departmanId;

                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Kullanıcı güncellendi.";
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
        public _user Detail(int id)
        {
            _return result = new _return();
            _user list = new _user();
            using (var db = new FaturaEntities())
            {
                try
                {
                    list = (from tbl in db.Users
                            where tbl.Id == id
                            select new _user
                            {
                                id=tbl.Id,
                                departmanId=tbl.DepartmentId.Value,
                                name=tbl.Name,
                                type=tbl.Type.Value,
                                userName=tbl.Username,
                                branch = tbl.Branch
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
            Users user = new Users();

            using (var db = new FaturaEntities())
            {
                try
                {
                    user = db.Users.Where(x => x.Id == id).FirstOrDefault();
                    db.Users.Remove(user);
                    int sonuc = db.SaveChanges();
                    if (sonuc > 0)
                    {
                        result.status = 1;
                        result.statusText = "Kullanıcı başarıyla silindi.";
                    }
                    else
                    {
                        result.statusText = "Kullanıcı silme esnasında hata oluştu.";
                    }

                }
                catch (Exception)
                {
                    result.statusText = "Kullanıcı silme esnasında hata oluştu.";
                }

                return result;

            }
        }
        [HttpGet]
        public List<_user> UserList()
        {
            List<_user> list = new List<_user>();
            using (var db = new FaturaEntities())
            {
                list = (from tbl in db.Users
                        orderby tbl.Name
                        select new _user
                        {
                            id = tbl.Id,
                            name=tbl.Name
                        }).ToList();

            }
            return list;
        }
        [HttpGet]
        public List<_user> DutyUser(int id)
        {
            List<_user> list = new List<_user>();
            using (var db = new FaturaEntities())
            {
                list = (from tbl in db.Users
                        join invoice in db.Invoice_User on tbl.Id equals invoice.UserId
                        where invoice.InvoiceId==id
                        orderby tbl.Name
                        select new _user
                        {
                            id = tbl.Id,
                            name = tbl.Name
                        }).ToList();

            }
            return list;
        }
        [HttpGet]
        public List<_userType> UserTypeList()
        {
            List<_userType> list = new List<_userType>();
            using (var db = new FaturaEntities())
            {
                list = (from tbl in db.User_Type
                        orderby tbl.Type
                        select new _userType
                        {
                            id = tbl.Id,
                            type = tbl.Type
                        }).ToList();

            }
            return list;
        }
    }
}
