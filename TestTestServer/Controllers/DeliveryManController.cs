using TestTestServer.Data;
using Microsoft.AspNetCore.Mvc;
using TestTestServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace TestTestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryManController : Controller
    {
        private readonly APIData dbContext;
        private readonly IConfiguration _configuration;
        private readonly EsistAccountService _esistAccountService;
       
        public DeliveryManController(APIData dbContext, IConfiguration configuration, EsistAccountService esistAccountService)
        {
            _configuration = configuration;
            this.dbContext = dbContext;
            _esistAccountService = esistAccountService;
        }
        // get: lấy dữ liệu
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await dbContext.DeliveryMan.ToListAsync());
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            var Deliveryman = await dbContext.DeliveryMan.FindAsync(id);
            if (Deliveryman == null) { return NotFound(); }
            return Ok(Deliveryman);
        }
        // post: tạo  mới
        [HttpPost]
        public async Task<IActionResult> Add(DeliveryManRequest request)
        {
            LoginCheck login = new LoginCheck
            {
                Account = request.ManAccount,
                Password = request.ManPassword,
            };

            var check = await _esistAccountService.checkAccount(login);
            if (check.Account != null) { return NotFound(); }

            var deliveryMan = new DeliveryMan()
            {
                ManImage = request.ManImage,
                ManName = request.ManName,
                ManPhone = request.ManPhone,
                ManAccount = request.ManAccount,
                ManPassword = request.ManPassword,
            };
            await dbContext.DeliveryMan.AddAsync(deliveryMan);
            await dbContext.SaveChangesAsync();
            return Ok(deliveryMan);
        }
        // Cập nhật vị trí đơn hàng
        [HttpPost("Location")]
        public async Task<IActionResult> Update(UpdateLocation location)
        {
            var Location1 = new UpdateLocation();
            string LocationParcel = ""; string Realtime = "";
            await
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
            {
                var sql = "SELECT ParLocation , RealTime FROM Parcel where ParID = '" + location.ParID.ToString() + "'";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LocationParcel = reader["ParLocation"].ToString() + "@" + location.Location;
                    Realtime = reader["RealTime"].ToString() + "@" + location.RealTime;
                }
                reader.Close();
                var sqlUpdate = "UPDATE Parcel SET Parlocation  = '" + LocationParcel + "' WHERE ParID = '" + location.ParID +"'" ;
                using SqlCommand sqlCommand = new SqlCommand(sqlUpdate, connection);
                SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                sqlReader.Close();
                var sqlUpdate1 = "UPDATE Parcel SET Realtime  = '" + Realtime + "' WHERE ParID = '" + location.ParID + "'";
                using SqlCommand sqlCommand1 = new SqlCommand(sqlUpdate1, connection);
                SqlDataReader sqlReader1 = sqlCommand1.ExecuteReader();
                sqlReader1.Close();
                Location1.ParID = location.ParID;
                Location1.Location = LocationParcel;
                Location1.RealTime = Realtime;         
            }
            return Ok(Location1);
        }
        // Trả về dữ liệu đơn hàng tương ứng với ID Người Giao Hàng 
        [HttpGet("Parcel")]
        public async Task<IEnumerable<Parcel>> GetParCel(int id)
        {
            var Parcels = new List<Parcel>();
            await
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
            {
                var sql = "SELECT ParRouteLocation,ParID,ParImage, ParDescription, ParStatus, ParDeliveryDate ,ParLocation ,Realtime ,Note, Price, CusID , ManID FROM Parcel Where ManID = '" + id.ToString() + "'";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var parcel = new Parcel()
                    {  //
                        ParID = (int)reader["ParID"],
                        ParImage = reader["ParImage"].ToString(),
                        ParDescription = reader["ParDescription"].ToString(),
                        ParStatus = reader["ParStatus"].ToString(),
                        ParRouteLocation = reader["ParRouteLocation"].ToString(),
                        ParDeliveryDate = (DateTime)reader["ParDeliveryDate"],
                        ParLocation = reader["ParLocation"].ToString(),
                        Realtime = reader["Realtime"].ToString(),
                        Note = reader["Note"].ToString(),
                        Price = (int)reader["Price"],
                        CusID = (int)reader["CusID"],
                        ManID = (int)reader["ManID"],
                    };
                    Parcels.Add(parcel);
                }
            }
            return Parcels;
        }
        // put: chỉnh sửa dữ liệu
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, DeliveryManRequest Update)
        {
            var deliveryMan = await dbContext.DeliveryMan.FindAsync(id);
            if (deliveryMan != null)
            {
                deliveryMan.ManImage = Update.ManImage;
                deliveryMan.ManName = Update.ManName;   
                deliveryMan.ManAccount = Update.ManAccount;
                deliveryMan.ManPassword = Update.ManPassword;

                await dbContext.SaveChangesAsync();

                return Ok(deliveryMan);
            }
            return NotFound();
        }
        // delete: xóa dữ liệu
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var deliveryMan = await dbContext.DeliveryMan.FindAsync(id);
            if (deliveryMan != null)
            {
                dbContext.Remove(deliveryMan);
                await dbContext.SaveChangesAsync();
                return Ok(deliveryMan);
            }
            return NotFound();
        }
    }
}
