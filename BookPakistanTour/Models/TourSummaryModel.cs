using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookPakistanTour.Models
{
    public class TourSummaryModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public float Price { get; set; }

        public float Sale { get; set; }

        public string Description { get; set; }

        public string Company { get; set; }

        public string ImageUrl { get; set; }
    }
}