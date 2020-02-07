using FaturaOnay.Static_Code;
using System.Web.Mvc;

namespace FaturaOnay.Controllers
{
    public class PagesController : Controller
    {
        [HttpPost]
        public ActionResult InvoiceDetail()//silmeyin
        {
            ViewBag.invoiceId = StaticMethods.Querystr(Request.Path);
            return PartialView("~/Views/Invoice/Detail.cshtml");
        }
        [HttpPost]
        public ActionResult UserDetail()//silmeyin
        {
            ViewBag.userId = StaticMethods.Querystr(Request.Path);
            return PartialView("~/Views/User/Detail.cshtml");
        }
        [HttpPost]
        public ActionResult DepartmentDetail()//silmeyin
        {
            ViewBag.departmentId = StaticMethods.Querystr(Request.Path);
            return PartialView("~/Views/Department/Detail.cshtml");
        }
        [HttpPost]
        public ActionResult BranchDetail()//silmeyin
        {
            ViewBag.branchId = StaticMethods.Querystr(Request.Path);
            return PartialView("~/Views/Branch/Detail.cshtml");
        }
    }
}