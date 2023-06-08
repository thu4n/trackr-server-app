using TestTestServer.Data;
using Microsoft.AspNetCore.Mvc;
using TestTestServer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Collections;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Diagnostics.Tracing;
using Microsoft.AspNetCore.Authorization;

namespace TestTestServer.Controllers
{
    [ApiController]
    [Route("api/Login")]
    public class LogInController : Controller
    {
        private readonly IConfiguration _configuration;
        public LogInController(IConfiguration _configuration)
        {
            this._configuration = _configuration;
        }
        public int check = 0;
        public int check1 = 0;
        public int check2 = 0;
        [HttpPost]
        public async Task<ActionResult<Login>> Get([FromBody]LoginCheck loginP)
        {
            var admin = await GetAd(loginP);
            if (check != 0)
            {
                check = 0;
                return admin;
            }
            else
            {
                var customer = await GetCus(loginP);
                if (check1 != 0)
                {
                    check1 = 0;
                    return customer;
                }
                else
                {
                    var DeliMan = await GetDeli(loginP);
                    if (check2 != 0)
                    {
                        check2 = 0;
                        return DeliMan;
                    }
                    else
                    {
                        return NotFound();
                    }    
                }
            } 
        }
        private async Task<ActionResult<Login>> GetAd(LoginCheck login)
        {
            var Admins = new Login(); var AdminCheck = new Login();
            await
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
            {
                // SqlParameter ID = new SqlParameter("@id", SqlDbType.Int);
                //  ID.Value = id;
                var sql = "SELECT AdID, AdName, AdAccount, AdPassword FROM Admins Where AdAccount = '" + login.Account.ToString() + "' and AdPassword = '" + login.Password.ToString() + "'";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var admin = new Login()
                    {  //
                        ID = (int)reader["AdID"],
                        Name = reader["AdName"].ToString(),
                        Account = reader["AdAccount"].ToString(),
                        Password = reader["AdPassword"].ToString(),
                        role = "Admin"
                    };
                    Admins = admin;
                    check++;
                }
            }
            if (Admins == AdminCheck)
                check = 0;
            return Ok(Admins);
        }
        private async Task<ActionResult<Login>> GetCus(LoginCheck login)
        {
            var Cuss = new Login(); var CusCheck = new Login();
            await
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
            {
                // SqlParameter ID = new SqlParameter("@id", SqlDbType.Int);
                //  ID.Value = id;
                var sql = "SELECT CusID, CusName, CusAccount, CusPassword FROM Customer Where CusAccount = '" + login.Account.ToString() + "' and CusPassword = '" + login.Password.ToString() + "'";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var customer = new Login()
                    {  
                        ID = (int)reader["CusID"],
                        Name = reader["CusName"].ToString(),
                        Account = reader["CusAccount"].ToString(),
                        Password = reader["CusPassword"].ToString(),
                        role = "Customer"
                    };
                    Cuss = customer;
                    check1++;
                }
            }
            if (Cuss == CusCheck)
                check1 = 0;
            return Ok(Cuss);
        }
        private async Task<ActionResult<Login>> GetDeli(LoginCheck login)
        {
            var deli = new Login(); var delicheck = new Login();
            await
            using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
            {
                // SqlParameter ID = new SqlParameter("@id", SqlDbType.Int);
                //  ID.Value = id;
                var sql = "SELECT ManID, ManName, ManAccount, ManPassword FROM DeliveryMan Where ManAccount = '" + login.Account.ToString() + "' and ManPassword = '" + login.Password.ToString() + "'";
                connection.Open();
                using SqlCommand command = new SqlCommand(sql, connection);
                using SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var deliveryMan = new Login()
                    {  //
                        ID = (int)reader["ManID"],
                        Name = reader["ManName"].ToString(),
                        Account = reader["ManAccount"].ToString(),
                        Password = reader["ManPassword"].ToString(),
                        role = "DeliveryMan"
                    };
                    deli = deliveryMan;
                    check2++;
                }
            }
            if ( deli == delicheck)
                check2 = 0;
            return Ok(deli);
        }
    }
}

