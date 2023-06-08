using System.ComponentModel.DataAnnotations;

namespace TestTestServer.Models
{
    public class DeliveryMan
    {
        [Key]
        public int ManID { get; set; }
        public string? ManImage { get; set; }
        public string? ManName { get; set; }
        public string? ManPhone { get; set; }
        public string? ManAccount { get; set; }
        public string? ManPassword { get; set; }
    }
}
