using System;

namespace FaturaOnay.Models
{
    public class _confirmDetail
    {
        public int id { get; set; }
        public string company { get; set; }
        public string invoiceNo { get; set; }
        public string document { get; set; }
        public int invoiceId { get; set; }
        public int userId { get; set; }
        public decimal total { get; set; }
        public string totalS { get; set; }
        public string unite { get; set; }
        public int departmanId { get; set; }
        public int confirmingUserId { get; set; }
        public string userName { get; set; }
        public string confirmingDetail { get; set; }
        public DateTime confirmingDate { get; set; }
        public TimeSpan confirmingTime { get; set; }
        public string confirmingTimeStr { get; set; }
    }
}