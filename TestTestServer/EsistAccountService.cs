using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Security.Certificates;
using TestTestServer.Models;

namespace TestTestServer;

public class EsistAccountService
{
    private readonly IConfiguration _configuration;
    public EsistAccountService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public  async Task<Login?> checkAccount(LoginCheck login)
    {
        var Account = new Login(); 

        await
        using (var connection = new SqlConnection(_configuration.GetConnectionString("ApiDatabase")))
        {
            var sqlCus = "SELECT CusName,CusAccount,CusPassword ,CusID FROM Customer Where CusAccount = '" + login.Account.ToString() + "'";
            connection.Open();
            using SqlCommand command = new SqlCommand(sqlCus, connection);
            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var check = new Login()
                {
                    ID = (int)reader["CusID"],
                    Account = reader["CusAccount"].ToString(),
                    Password = reader["CusPassword"].ToString(),
                    role = "Customer",
                    Name = reader["CusName"].ToString(),
                };
                Account = check;
            }
            reader.Close();
            if (Account.Account == null)
            {
                var sqlAd = "SELECT AdName,AdID,AdAccount,AdPassword FROM Admins Where AdAccount = '" + login.Account.ToString() + "'";
                using SqlCommand commandAd = new SqlCommand(sqlAd, connection);
                using SqlDataReader readerAd = commandAd.ExecuteReader();
                while (readerAd.Read())
                {
                    var check = new Login()
                    {
                        ID = (int)readerAd["AdID"],
                        Account = readerAd["AdAccount"].ToString(),
                        Password = readerAd["AdPassword"].ToString(),
                        Name = readerAd["AdName"].ToString(),
                        role = "Admin",
                    };
                    Account = check;
                }
                readerAd.Close();
                if (Account.Account == null)
                {
                    readerAd.Close();
                    var sqlDeli = "SELECT ManName,ManID,ManAccount,ManPassword FROM DeliveryMan Where ManAccount = '" + login.Account.ToString() + "'";
                    using SqlCommand commandDeli = new SqlCommand(sqlDeli, connection);
                    using SqlDataReader readerDeli = commandDeli.ExecuteReader();
                    while (readerDeli.Read())
                    {
                        var check = new Login()
                        {
                            ID = (int)readerDeli["ManID"],
                            Account = readerDeli["ManAccount"].ToString(),
                            Password = readerDeli["ManPassword"].ToString(),
                            Name = readerDeli["ManName"].ToString(),
                            role = "DeliveryMan",
                        };
                        Account = check;
                    }
                    readerDeli.Close();
                    if (Account.Account == null) { return Account; }
                }
            }
            return Account;
        }   
    }
}
