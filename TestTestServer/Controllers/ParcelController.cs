using TestTestServer.Data;
using Microsoft.AspNetCore.Mvc;
using TestTestServer;
using TestTestServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System;
using System.Diagnostics;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using MimeKit;
using MailKit.Net.Smtp;
namespace TestTestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParcelController : Controller
    {
        private readonly APIData dbContext;
        private  ProcessTree Tree;
        private readonly IConfiguration _configuration;

        public ParcelController(APIData dbContext, ProcessTree tree, IConfiguration configuration)
        {
            this.dbContext = dbContext;
            Tree = tree;
            _configuration = configuration;
        }
        // get: lấy dữ liệu
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await dbContext.Parcel.ToListAsync());
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            var Parcel = await dbContext.Parcel.FindAsync(id);
            if (Parcel == null) { return NotFound(); }
            return Ok(Parcel);
        }
        // post: tạo  mới
        [HttpPost]
        public async Task<IActionResult> Add(ParcelRequest parcelRequest)
        {
            var Parcel = new Parcel()
            {
                ParImage = parcelRequest.ParImage,
                ParDescription = parcelRequest.ParDescription,
                ParStatus = parcelRequest.ParStatus,
                ParDeliveryDate = parcelRequest.ParDeliveryDate,
                ParRouteLocation = parcelRequest.ParRouteLocation,
                ParLocation = parcelRequest.ParLocation,
                Realtime = parcelRequest.Realtime,
                Note = parcelRequest.Note,
                Price = parcelRequest.Price,
                CusID = parcelRequest.CusID,
                ManID = parcelRequest.ManID,
            };
            await dbContext.Parcel.AddAsync(Parcel);
            await dbContext.SaveChangesAsync();
            return Ok(Parcel);
        }
        // put: chỉnh sửa dữ liệu
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, ParcelRequest Update)
        {   
            string? ProcessTree = null;
            // chức năng tạo lộ trình cho parcel 
           // try
          //  {
                if (Update.ParStatus == "PROCESSED") // nếu trạng thái là PROCESSED sẽ chạy thuật toán tìm kiếm đường đi cho parcel
                {
                    var AddressHCM = new List<ProcessTreeAddress>();
                    using (StreamReader r = new StreamReader("DataLocationHCMcity.json"))  // đọc file dữ liệu bản đồ Thành Phố HCM
                    {
                        string json = r.ReadToEnd();
                        AddressHCM = JsonSerializer.Deserialize<List<ProcessTreeAddress>>(json);
                    }
                    await
                   using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
                    {
                        string loca = ""; int check = -1;
                        var sql = "SELECT  CusAddress FROM Customer Where CusID = '" + Update.CusID + "'";
                        connection.Open();
                        using SqlCommand command = new SqlCommand(sql, connection);
                        using SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            loca = reader["CusAddress"].ToString();  // Lấy địa chỉ của khách hàng
                        }
                        reader.Close();
                        string[] EndLoca = loca.Split('*');
                        string Name = EndLoca[1];
                        for (int i = 0; i < 24; i++)
                        {
                            if (Name == AddressHCM[i].name)
                            {
                                check = i;
                            }
                        }
                        Tree.dijkstra(0, check);  // Chạy THuật toán Dijikstra tìm đường đi ngắn nhất 
                        int[] check1 = Tree.Tree();
                        ProcessTree = Tree.distance.ToString() + "@";
                        for (int i = 0; i < Tree.Location(); i++)  // Thêm lộ trình đi
                        {
                            ProcessTree += AddressHCM[check1[i]].nearest_address + "@" + AddressHCM[check1[i]].address + "@";
                        }
                        ProcessTree += loca;
                        Update.Price = Tree.distance*5000; // tính ship cod
                        Update.ParRouteLocation = ProcessTree;
                    }
                }
                if(Update.ParStatus == "COMPLETED")
                {
                    await
                   using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
                    {
                        string ?account = ""; string? name = "";
                        var sql = "SELECT  CusAccount, CusName FROM Customer Where CusID = '" + Update.CusID + "'";
                        connection.Open();
                        using SqlCommand command = new SqlCommand(sql, connection);
                        using SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                        {
                            account = reader["CusAccount"].ToString();  // Lấy địa chỉ của khách hàng
                            name = reader["CusName"].ToString();
                        }
                        reader.Close();
                        var client = new SmtpClient();
                        client.Connect("smtp.gmail.com", 465, true); // smtp host, port, use ssl.
                        client.Authenticate("TrackrService@gmail.com", "dtwvjgfkeyypoliw"); // gmail account, app password
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress("Trackr", "TrackrService@gmail.com"));
                        message.To.Add(new MailboxAddress("", account));
                        message.Subject = "Đơn Hàng #" + id.ToString() +" Đã Giao Thành Công!";
                        string text = "  Xin Chào "+ name +"\n  Đơn Hàng #" + id.ToString() +" của bạn đã được giao thành công \n\n"
                            + "  Mã Đơn Hàng: " + id.ToString() + "\n  Tên Đơn Hàng : "+ Update.ParDescription;
                        message.Body = new TextPart("plain")
                        {
                            Text = text
                        };
                        client.Send(message);
                    }
                }    
          //  }
           // catch { }
            
            
            // chỉnh sửa dữ liệu trong parcel
            var Parcel = await dbContext.Parcel.FindAsync(id);
            if (Parcel != null)
            {
                Parcel.ParImage = Update.ParImage;
                Parcel.ParDescription = Update.ParDescription;
                Parcel.ParStatus = Update.ParStatus;
                Parcel.ParDeliveryDate = Update.ParDeliveryDate;
                Parcel.ParRouteLocation = Update.ParRouteLocation;
                Parcel.ParLocation = Update.ParLocation;
                Parcel.Realtime = Update.Realtime;
                Parcel.Note = Update.Note;
                Parcel.Price = Update.Price;
                Parcel.CusID = Update.CusID;
                Parcel.ManID = Update.ManID;

                await dbContext.SaveChangesAsync();

                return Ok(Parcel);
            }
            return NotFound();
        }
        // delete: xóa dữ liệu
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var Parcel = await dbContext.Parcel.FindAsync(id);
            if (Parcel != null)
            {
                dbContext.Remove(Parcel);
                await dbContext.SaveChangesAsync();
                return Ok(Parcel);
            }
            return NotFound();
        }
    }
}
