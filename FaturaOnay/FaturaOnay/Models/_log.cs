using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FaturaOnay.Models
{
    public class _log
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string userName { get; set; }
        public DateTime time { get; set; }
        public string action { get; set; }
    }
}