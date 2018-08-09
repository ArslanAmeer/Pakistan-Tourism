using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPakistanTourClasslibrary.TourManagement
{
    public class TourHandler
    {
        private readonly DbContextClass _db = new DbContextClass();

        public List<Tour> GetAllTours()
        {
            using (_db)
            {
                return (from t in _db.Tours
                        .Include(t => t.Company)
                        select t).ToList();
            }
        }

        public Tour GetTourById(int id)
        {
            using (_db)
            {
                return (from t in _db.Tours
                        .Include(t => t.Company)
                        where t.Id == id
                        select t).FirstOrDefault();
            }
        }

        public void AddTour(Tour tour)
        {
            using (_db)
            {
                _db.Entry(tour.Company).State = EntityState.Unchanged;
                _db.Tours.Add(tour);
                _db.SaveChanges();
            }
        }

        public void UpdateTour(Tour tour)
        {
            using (_db)
            {
                _db.Entry(tour).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void DeleteTour(int id)
        {
            using (_db)
            {
                _db.Tours.Remove(GetTourById(id));
                _db.SaveChanges();
            }
        }

    }
}
