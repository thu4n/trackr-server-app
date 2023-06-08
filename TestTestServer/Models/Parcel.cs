using System.ComponentModel.DataAnnotations;

namespace TestTestServer.Models
{
    public class Parcel
    {
        [Key]
        public int ParID { get; set; }
        public string? ParImage { get; set; }
        public string? ParDescription { get; set; } 
        public string? ParStatus { get; set; }
        public DateTime ParDeliveryDate { get; set; }// ngày mua hàng
        public string? ParRouteLocation { get; set; } // cái này là sẽ lưu full path
        public string? ParLocation { get; set; }// lưu giá trị nơi xuất phát, r từ từ cập nhật
        public string? Realtime { get; set; }// y chang cái trên 
        public string? Note { get; set; }
        public int Price { get; set; }
        public int CusID { get; set; }
        public int ManID { get; set; }
    }
}
