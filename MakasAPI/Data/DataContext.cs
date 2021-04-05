using MakasAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MakasAPI.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Saloon> Saloons { get; set; }
        public DbSet<Worker> Workers { get; set; }






    }
}
