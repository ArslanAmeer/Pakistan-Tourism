using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookPakistanTourClasslibrary.LocationManagement;
using BookPakistanTourClasslibrary.FeedbackManagement;

namespace BookPakistanTourClasslibrary.CompanyManagement
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public City City { get; set; }

        public string FacebookPageUrl { get; set; }

    }
}
