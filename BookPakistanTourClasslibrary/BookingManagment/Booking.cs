using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookPakistanTourClasslibrary.TourManagement;
using BookPakistanTourClasslibrary.UserManagement;

namespace BookPakistanTourClasslibrary.BookingManagment
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        //[Required]
        //public string Name { get; set; }

        //[Required]
        //public float Phone { get; set; }

        //public string Email { get; set; }

        //[Required]
        //public string Address { get; set; }

        public User User { get; set; }

        public Tour Tour { get; set; }

    }
}
