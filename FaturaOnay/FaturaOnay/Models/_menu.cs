namespace FaturaOnay.Models
{
    public class _menu
    {
        public int id { get; set; }
        public int parentId { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public string target { get; set; }
        public int ordernum { get; set; }
        public bool visible { get; set; }
        public bool write { get; set; }
    }
}