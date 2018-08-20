using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPakistanTourClasslibrary.BookingManagment
{
    public class BookingHandler : IDisposable
    {
        private readonly DbContextClass _db = new DbContextClass();

        public List<Booking> GetAllBookings()
        {
            using (_db)
            {
                return (from b in _db.Bookings
                    .Include(b => b.Tour.Company)
                    .Include(b => b.User.City)
                        select b).ToList();
            }
        }

        public Booking GetBookingById(int? id)
        {
            using (_db)
            {
                return (from b in _db.Bookings
                        .Include(b => b.Tour.Company)
                        .Include(b => b.User.City)
                        where b.Id == id
                        select b).FirstOrDefault();
            }
        }

        public List<Booking> GetBookingsByUserId(int userId)
        {
            using (_db)
            {
                return (from b in _db.Bookings
                        .Include(b => b.Tour.Company)
                        .Include(b => b.User.City)
                        where b.User.Id == userId
                        select b).ToList();
            }
        }

        public List<Booking> GetBookingsByTourId(int tourId)
        {
            using (_db)
            {
                return (from b in _db.Bookings
                        .Include(b => b.Tour.Company)
                        .Include(b => b.User.City)
                        where b.Tour.Id == tourId
                        select b).ToList();
            }
        }

        public void AddBooking(Booking booking)
        {
            using (_db)
            {
                _db.Entry(booking.Tour).State = EntityState.Unchanged;
                //_db.Entry(booking.User).State = EntityState.Unchanged;
                _db.Bookings.Add(booking);
                _db.SaveChanges();
            }
        }

        public int BookingCount()
        {
            return (from b in _db.Bookings select b).Count();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
        }
    }
}
