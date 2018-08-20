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
        public Tour()
        {
            TourImages = new List<TourImages>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        public float Price { get; set; }

        public float Sale { get; set; }

        public ICollection<TourImages> TourImages { get; set; }

        public string DepartureDate { get; set; }

        public Company Company { get; set; }

    }
}
