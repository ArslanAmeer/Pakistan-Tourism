using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookPakistanTourClasslibrary.CompanyManagement;
using BookPakistanTourClasslibrary.TourManagement;

namespace BookPakistanTourClasslibrary.FeedbackManagement
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Message { get; set; }

        public Company Company { get; set; }

        public Tour Tour { get; set; }

    }
}
