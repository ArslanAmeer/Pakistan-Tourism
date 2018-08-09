using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookPakistanTourClasslibrary.CompanyManagement;
using BookPakistanTourClasslibrary.FeedbackManagement;

namespace BookPakistanTourClasslibrary.TourManagement
{
    public class Tour
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        public DateTime? DepartureDate { get; set; }

        public Company Company { get; set; }

    }
}
