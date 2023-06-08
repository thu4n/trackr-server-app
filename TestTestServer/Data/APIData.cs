using Microsoft.EntityFrameworkCore;
using TestTestServer.Models;

namespace TestTestServer.Data
{
    public class APIData : DbContext
    {
        public APIData(DbContextOptions options) : base(options) 
        {
        }
        public DbSet<Ad> Admins { get; set; }
        public DbSet<DeliveryMan> DeliveryMan { get; set;}
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Parcel> Parcel { get; set; }
    }
}
