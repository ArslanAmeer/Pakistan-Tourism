using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPakistanTourClasslibrary.FeedbackManagement
{
    public class FeedbackHandler
    {
        private readonly DbContextClass _db = new DbContextClass();

        public List<Feedback> GetAllFeedbacks()
        {
            using (_db)
            {
                return (from f in _db.Feedbacks
                     .Include(f => f.Company)
                     .Include(f => f.Tour)
                        select f).ToList();
            }
        }

        public Feedback GetFeedbackById(int id)
        {
            using (_db)
            {
                return (from f in _db.Feedbacks
                        .Include(f => f.Company)
                        .Include(f => f.Tour)
                        where f.Id == id
                        select f).FirstOrDefault();
            }
        }

        public List<Feedback> GetAllFeedbacksByCompanyId(int id)
        {
            using (_db)
            {
                return (from f in _db.Feedbacks
                        .Include(f => f.Company)
                        .Include(f => f.Tour)
                        where f.Company.Id == id
                        select f).ToList();
            }
        }

        public List<Feedback> GetAllFeedbackByTourId(int id)
        {
            using (_db)
            {
                return (from f in _db.Feedbacks
                        .Include(f => f.Company)
                        .Include(f => f.Tour)
                        where f.Tour.Id == id
                        select f).ToList();
            }
        }

        public void AddFeedback(Feedback feedback)
        {
            using (_db)
            {
                _db.Entry(feedback.Company).State = EntityState.Unchanged;
                _db.Entry(feedback.Tour).State = EntityState.Unchanged;
                _db.Feedbacks.Add(feedback);
                _db.SaveChanges();
            }
        }

        public void UpdateFeedback(Feedback feedback)
        {
            using (_db)
            {
                _db.Entry(feedback).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void DeleteFeedback(int id)
        {
            using (_db)
            {
                _db.Feedbacks.Remove(GetFeedbackById(id));
                _db.SaveChanges();
            }
        }

    }
}
