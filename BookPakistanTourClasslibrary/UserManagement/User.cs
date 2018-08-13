using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookPakistanTourClasslibrary.LocationManagement;

namespace BookPakistanTourClasslibrary.UserManagement
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string FullAddress { get; set; }

        public string ImageUrl { get; set; }

        public City City { get; set; }

        public virtual Role Role { get; set; }

        public bool IsInRole(int id)
        {
            return Role != null && Role.Id == id;
        }

        public String BirthDate { get; set; }

        public bool? IsActive { get; set; }

        [Required]
        public long Phone { get; set; }

        public bool Male { get; set; }

        public bool Female { get; set; }

    }
}
