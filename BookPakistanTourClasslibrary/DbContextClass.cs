using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookPakistanTourClasslibrary.CompanyManagement;
using BookPakistanTourClasslibrary.FeedbackManagement;
using BookPakistanTourClasslibrary.LocationManagement;
using BookPakistanTourClasslibrary.TourManagement;
using BookPakistanTourClasslibrary.UserManagement;

namespace BookPakistanTourClasslibrary
{
    public class DbContextClass : DbContext
    {
        public DbContextClass() : base("DBconstr")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Role> Roles { get; set; }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<Country> Countries { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }

        public DbSet<Company> Companies { get; set; }

    }
}
