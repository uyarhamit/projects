using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaturaOnay.Models
{
    public class _invoice_User
    {
        public int id { get; set; }
        public string userId { get; set; }
        public string departmanId { get; set; }
        public int invoiceId { get; set; }
        public int confirm { get; set; }
    }
}