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

        public int? UserId { get; set; }

        public virtual User User { get; set; }

        public Tour Tour { get; set; }

    }
}
