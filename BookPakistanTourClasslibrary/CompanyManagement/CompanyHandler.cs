using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPakistanTourClasslibrary.CompanyManagement
{
    public class CompanyHandler
    {

        private readonly DbContextClass _db = new DbContextClass();

        public List<Company> GetAllCompanies()
        {
            using (_db)
            {
                return (from c in _db.Companies
                        .Include(c => c.City)
                        select c).ToList();
            }
        }

        public Company GetCompanybyId(int id)
        {
            using (_db)
            {
                return (from c in _db.Companies
                        .Include(c => c.City)
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

        public void DeleteCompany(int id)
        {
            using (_db)
            {
                _db.Companies.Remove(GetCompanybyId(id));
                _db.SaveChanges();
            }
        }

    }
}
