using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace FaturaOnay.Static_Code
{
    public static class StaticMethods
    {
        public static string Sha512Encryption(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return HashOlustur(hash);
        }
        private static string HashOlustur(byte[] hash)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                result.Append(hash[i].ToString("X2"));
            }
            return result.ToString();
        }
        //public static string GetSessionUserInf(string param)
        //{
        //    string sonuc = string.Empty;
        //    if (HttpContext.Current.Session["userInformation"] != null)
        //    {
        //        List<USER> userInf = (List<USER>)HttpContext.Current.Session["userInformation"];

        //        switch (param)
        //        {
        //            case "id":
        //                sonuc = userInf.FirstOrDefault().ID.ToString();
        //                break;
        //            case "nameSurname":
        //                sonuc = userInf.FirstOrDefault().NAME + " " + userInf.First().SURNAME.ToString();
        //                break;
        //            case "userName":
        //                sonuc = userInf.FirstOrDefault().USERNAME.ToString();
        //                break;
        //            case "email":
        //                sonuc = userInf.FirstOrDefault().EMAIL.ToString();
        //                break;
        //            case "phone":
        //                sonuc = userInf.FirstOrDefault().PHONE.ToString();
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    return sonuc;
        //}
        //public static bool GetSessionRoleInf(int pageId, int rolType)
        //{
        //    if (HttpContext.Current.Session["userRoleAuthList"] != null)
        //    {
        //        List<_AuthList> roleInf = (List<_AuthList>)HttpContext.Current.Session["userRoleAuthList"];
        //        int roleId = Convert.ToInt16(GetSessionUserInf("role"));
        //        int varmi = 0;

        //        if (rolType == 1)
        //        {
        //            varmi = roleInf.Where(p => p.pageId == pageId && p.roleId == roleId).Select(p => new { p.viewAuth }).FirstOrDefault().viewAuth;
        //        }
        //        else if (rolType == 2)
        //        {
        //            try
        //            {
        //                varmi = roleInf.Where(p => p.pageId == pageId && p.roleId == roleId).Select(p => new { p.addAuth }).FirstOrDefault().addAuth;
        //            }
        //            catch (Exception)
        //            {
                         
        //            }
                    
        //        }
        //        else if (rolType == 3)
        //        {
        //            try
        //            {
        //                varmi = roleInf.Where(p => p.pageId == pageId && p.roleId == roleId).Select(p => new { p.editAuth }).FirstOrDefault().editAuth;
        //            }
        //            catch (Exception)
        //            {
                         
        //            }
                    
        //        }
        //        else if (rolType == 4)
        //        {
        //            try
        //            {
        //                varmi = roleInf.Where(p => p.pageId == pageId && p.roleId == roleId).Select(p => new { p.deleteAuth }).FirstOrDefault().deleteAuth;
        //            }
        //            catch (Exception)
        //            {
                         
        //            }
                    
        //        }
        //        else if (rolType == 5)
        //        {
        //            try
        //            {
        //                varmi = roleInf.Where(p => p.pageId == pageId && p.roleId == roleId).Select(p => new { p.exportAuth }).FirstOrDefault().exportAuth;
        //            }
        //            catch (Exception)
        //            {
                         
        //            }
                    
        //        }
        //        else if (rolType == 6)
        //        {
        //            try
        //            {
        //                varmi = roleInf.Where(p => p.pageId == pageId && p.roleId == roleId).Select(p => new { p.stopAuth }).FirstOrDefault().stopAuth;
        //            }
        //            catch (Exception)
        //            {
                         
        //            }
                   
        //        }

        //        if (varmi > 0)
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        public static string Querystr(string path)
        {
            string ret = "";

            if (path != "")
            {
                try
                {
                    if (path.Substring(0, 1) == "/")
                    {
                        ret = path.Substring(1, path.Length - 1).Split('/')[2];
                    }
                    else
                    {
                        ret = path.Split('/')[2];
                    }
                }
                catch (Exception)
                {

                }

            }

            return ret;
        }
        public static string RandomName()
        {
            Random r = new Random();
            int sb = r.Next(0, 99999999);
            return "RandomFile_" + sb.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + "_";
        }
    }
}