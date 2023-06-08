namespace TestTestServer.Models
{
    public class ParcelRequest
    {
        public string? ParImage { get; set; }
        public string? ParDescription { get; set; }
        public string? ParStatus { get; set; }
        public string? ParRouteLocation { get; set; }
        public DateTime ParDeliveryDate { get; set; }
        public string? ParLocation { get; set; }
        public string? Realtime { get; set; }
        public string? Note { get; set; }
        public int Price { get; set; }
        public int CusID { get; set; }
        public int ManID { get; set; }
    }
}
