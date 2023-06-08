namespace TestTestServer.Models
{
    public class UpdateLocation
    {
        public int ParID { get; set; }
        public string? Location { get; set; } // địa điểm mà shipper đang ở
        public string? RealTime { get; set; } // thời gian hiện tại mà shipper
    }
}
