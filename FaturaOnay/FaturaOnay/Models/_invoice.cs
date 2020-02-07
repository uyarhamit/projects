using System;

namespace FaturaOnay.Models
{
    public class _invoice
    {
        public int id { get; set; }
        public int[] invoiceIds { get; set; }
        public DateTime invoiceDate { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
        public string unite { get; set; }
        public string taxNumber { get; set; }
        public string invoiceYear { get; set; }
        public string invoiceMonth { get; set; }
        public string invoiceNumber { get; set; }
        public string invoiceCompany { get; set; }
        public decimal? total { get; set; }
        public string totalS { get; set; }
        public bool? comfirmedByAllUser { get; set; }
        public string document { get; set; }
        public string[] users { get; set; }
        public string[] departmans { get; set; }
        public int? departmanId { get; set; }
        public string departmanName { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public string branch { get; set; }
        public string[] invoiceDetail { get; set; }
    }
}