using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPakistanTourClasslibrary.UserManagement
{
    public class UserHandler
    {
        private readonly DbContextClass _db = new DbContextClass();

        public List<User> GetAllUsers()
        {
            using (_db)
            {
                return (from u in _db.Users
                        .Include(u => u.Role)
                        .Include(u => u.City)
                        select u).ToList();
            }
        }

        public User GetUserById(int id)
        {
            using (_db)
            {
                return (from u in _db.Users
                        .Include(u => u.Role)
                        .Include(u => u.City)
                        where u.Id == id
                        select u).FirstOrDefault();
            }

        }

        public User GetUser(string email, string password)
        {
            using (_db)
            {
                return (from u in _db.Users
                        .Include(u => u.Role)
                        .Include(u => u.City)
                        where (u.Email == email && u.Password == password)
                        select u).FirstOrDefault();
            }

        }

        public List<Role> GetRoles()
        {
            using (_db)
            {
                return (from r in _db.Roles select r).ToList();
            }
        }

        public Role GetRoleById(int id)
        {
            using (_db)
            {
                return (from r in _db.Roles where r.Id == id select r).FirstOrDefault();
            }
        }

        public void Adduser(User user)
        {
            using (_db)
            {
                _db.Entry(user.Role).State = EntityState.Unchanged;
                _db.Entry(user.City).State = EntityState.Unchanged;
                _db.Users.Add(user);
                _db.SaveChanges();
            }
        }

        public void DeleteUser(int id)
        {
            using (_db)
            {
                //if any error occured just Uncomment this code

                //User u = db.Users.Find(id);
                _db.Users.Remove(GetUserById(id));
                _db.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            using (_db)
            {
                _db.Entry(user).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public int GetUserCount()
        {
            using (_db)
            {
                return (from c in _db.Users select c).Count();
            }
        }

    }
}
