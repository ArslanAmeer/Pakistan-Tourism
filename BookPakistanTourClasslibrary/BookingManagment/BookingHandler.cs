using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPakistanTourClasslibrary.BookingManagment
{
    public class BookingHandler
    {
        private readonly DbContextClass _db = new DbContextClass();

        public List<Booking> GetAllBookings()
        {
            using (_db)
            {
                return (from b in _db.Bookings
                    .Include(b => b.Tour)
                        select b).ToList();
            }
        }

        public Booking GetBookingById(int id)
        {
            using (_db)
            {
                return (from b in _db.Bookings
                        .Include(b => b.Tour)
                        where b.Id == id
                        select b).FirstOrDefault();
            }
        }

        public List<Booking> GetBookingsByUserId(int userId)
        {
            using (_db)
            {
                return (from b in _db.Bookings
                        .Include(b => b.Tour)
                        where b.User.Id == userId
                        select b).ToList();
            }
        }

        public List<Booking> GetBookingsByTourId(int tourId)
        {
            using (_db)
            {
                return (from b in _db.Bookings
                        .Include(b => b.Tour)
                        where b.Tour.Id == tourId
                        select b).ToList();
            }
        }
    }
}
