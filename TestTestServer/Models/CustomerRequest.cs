namespace TestTestServer.Models
{
    public class CustomerRequest
    {
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
