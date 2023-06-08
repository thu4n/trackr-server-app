using System.ComponentModel.DataAnnotations;

namespace TestTestServer.Models
{
    public class Ad
    {
        [Key]
        public int AdID { get; set; }
        public string? AdImage { get; set; }
        public string? AdName { get; set; }
        public string? AdAccount { get; set; }
        public string? AdPassword { get; set; }
    }
}
