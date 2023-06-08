using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TestTestServer.Data;
using TestTestServer.Models;

namespace TestTestServer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly APIData dbContext;
        private readonly IConfiguration _configuration;
        private readonly EsistAccountService _esistAccountService;
        public CustomerController(APIData dbContext, IConfiguration _configuration, EsistAccountService esistAccountService)
        {
            this.dbContext = dbContext;
            this._configuration = _configuration;
            _esistAccountService = esistAccountService;
        }
        // get: lấy dữ liệu
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await dbContext.Customer.ToListAsync());
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetOne([FromRoute] int id)
        {
            var customer = await dbContext.Customer.FindAsync(id);
            if (customer == null) { return NotFound(); }
            return Ok(customer);
        }
        // Trả về dữ liệu đơn hàng tương ứng với ID Khách Hàng 
        [HttpGet("Parcel")]
        public async Task<IEnumerable<Parcel>> GetParCel(int id)
        {
            var Parcels = new List<Parcel>();
            await
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
            {
                var sql = "SELECT ParRouteLocation,ParID,ParImage, ParDescription, ParStatus, ParDeliveryDate ,ParLocation ,Realtime ,Note, Price, CusID , ManID FROM Parcel Where CusID = '" + id.ToString() +  "'";
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
        // post: tạo  mới
        [HttpPost]
        public async Task<IActionResult> Add(CustomerRequest Request)
        {
            LoginCheck login = new LoginCheck
            {
                Account = Request.CusAccount,
                Password = Request.CusPassword,
            };

            var check = await _esistAccountService.checkAccount(login); // kiểm tra xem có trùng tên đăng nhập hay không
            if (check.Account != null) { return NotFound(); }

            // THêm 1 Customer mới
            var customer = new Customer()
            {
                CusImage = Request.CusImage,
                CusName = Request.CusName,
                CusAddress = Request.CusAddress,
                CusPhone = Request.CusPhone,
                CusBirth = Request.CusBirth,
                CusDateRegister = Request.CusDateRegister,
                CusAccount = Request.CusAccount,
                CusPassword = Request.CusPassword,
            };
            await dbContext.Customer.AddAsync(customer);// lưu customer
            await dbContext.SaveChangesAsync();  // lưu các thay đổi customer 
            return Ok(customer);
        }
        // Tạo customer với 5 đơn hàng ngẫu nhiên có sẵn 
        [HttpPost("addParcel")]
        public async Task<IActionResult> Add(Customer customer)
        {
            var ParcelRD = new List<ParcelRandom>();
            using (StreamReader r = new StreamReader("DataParcel.json"))
            {
                string json = r.ReadToEnd();
                ParcelRD = JsonSerializer.Deserialize<List<ParcelRandom>>(json); // đọc file json r lưu vào 1 list chứa model tương ứng
            }

            DateTime currentDateTime = DateTime.Now;
            for (int i = 0; i < 5; i++) // Tạo random 5 đơn hàng
            {
                Random rd = new Random();
                int Numrd = rd.Next(0, 19);//biến Numrd sẽ nhận có giá trị ngẫu nhiên trong khoảng 0 đến 19

                var Parcel = new Parcel()
                {
                    // ParID = addd.ParID,
                    ParImage = ParcelRD[Numrd].Picture,
                    ParDescription = ParcelRD[Numrd].Description,
                    ParStatus = ParcelRD[Numrd].Status,
                    ParDeliveryDate = currentDateTime.AddDays(3),
                    ParLocation = ParcelRD[Numrd].Location,
                    Realtime = currentDateTime.ToString(),
                   // Price = ParcelRD[Numrd].Price,
                    CusID = customer.CusID,
                    ManID = -99,
                };
                await dbContext.Parcel.AddAsync(Parcel);
                await dbContext.SaveChangesAsync();
            }
            return Ok("succced");
        }
        // put: chỉnh sửa dữ liệu
        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, CustomerRequest Update)
        {
            var customer = await dbContext.Customer.FindAsync(id);
            if (customer != null)
            {
                customer.CusName = Update.CusName;
                customer.CusAddress = Update.CusAddress;
                customer.CusPhone = Update.CusPhone;
                customer.CusBirth = Update.CusBirth;
                customer.CusDateRegister = Update.CusDateRegister;
                customer.CusAccount = Update.CusAccount;
                customer.CusPassword = Update.CusPassword;
               
                await dbContext.SaveChangesAsync();

                return Ok(customer);
            }
            return NotFound();
        }
        // delete: xóa dữ liệu
        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var customer = await dbContext.Customer.FindAsync(id);
            if (customer != null)
            {
                dbContext.Remove(customer);
                await dbContext.SaveChangesAsync();
                return Ok(customer);
            }
            return NotFound();
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
                // cập nhật Location
                var sqlUpdate = "UPDATE Parcel SET Parlocation  = '" + LocationParcel + "' WHERE ParID = '" + location.ParID + "'";
                using SqlCommand sqlCommand = new SqlCommand(sqlUpdate, connection);
                SqlDataReader sqlReader = sqlCommand.ExecuteReader();
                sqlReader.Close();
                // cập nhật realtime
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
    }
}
