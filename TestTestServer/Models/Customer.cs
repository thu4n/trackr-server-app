using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestTestServer.Models
{
    public class Customer
    {
        [Key]
        public int CusID { get; set; }
        public string? CusImage { get; set; }
        public string? CusName { get; set; }
        public string? CusAddress { get; set; }
        public string? CusPhone { get; set; }
        public DateTime CusBirth { get; set; }
        public DateTime CusDateRegister { get; set; }
        public string? CusAccount { get; set; }
        public string? CusPassword { get; set; }
    }
}
