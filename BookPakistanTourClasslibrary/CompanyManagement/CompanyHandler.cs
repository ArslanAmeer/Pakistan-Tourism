using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPakistanTourClasslibrary.CompanyManagement
{
    public class CompanyHandler : IDisposable
    {

        private readonly DbContextClass _db = new DbContextClass();

        public List<Company> GetAllCompanies()
        {
            using (_db)
            {
                return (from c in _db.Companies
                        .Include(c => c.City.Country)
                        select c).ToList();
            }
        }

        public Company GetCompanybyId(int id)
        {
            using (_db)
            {
                return (from c in _db.Companies
                        .Include(c => c.City.Country)
                        where c.Id == id
                        select c).FirstOrDefault();
            }
        }

        public void AddCompany(Company company)
        {
            using (_db)
            {
                _db.Entry(company.City).State = EntityState.Unchanged;
                _db.Companies.Add(company);
                _db.SaveChanges();
            }
        }

        public void UpdateCompany(Company company)
        {
            using (_db)
            {
                _db.Entry(company).State = EntityState.Modified;
                _db.SaveChanges();
            }
        }

        public void DeleteCompany(Company company)
        {
            using (_db)
            {
                _db.Entry(company).State = EntityState.Deleted;
                _db.Companies.Remove(company);
                _db.SaveChanges();
            }
        }

        public int GetCompanyCount()
        {
            using (_db)
            {
                return (from c in _db.Companies select c).Count();
            }
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
