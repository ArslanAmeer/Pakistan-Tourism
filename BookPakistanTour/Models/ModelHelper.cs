using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BookPakistanTour.Models;
using BookPakistanTourClasslibrary.TourManagement;

namespace FYProject1.Models
{
    public class ModelHelper
    {

        // taking Dyanimc values and adding them into SelectListItem's List

        public static List<SelectListItem> ToSelectItemList(dynamic values)
        {
            List<SelectListItem> templist = null;

            if (values != null)
            {
                templist = new List<SelectListItem>();

                foreach (var v in values)
                {
                    templist.Add(new SelectListItem { Text = v.Name, Value = Convert.ToString(v.Id) });
                }
                templist.TrimExcess();
            }

            return templist;
        }

        // COnverting Tour model to TourSUmjmary MOdel for smaller or lighter use

        public static TourSummaryModel ToTourSummary(Tour tour)
        {
            return new TourSummaryModel
            {
                Id = tour.Id,
                Title = tour.Title,
                Price = tour.Price,
                Sale = tour.Sale,
                ImageUrl = (tour.TourImages.Count > 0) ? tour.TourImages.First().ImageUrl : null,
                Company = tour.Company.Name,
                Description = tour.Description
            };
        }

        // Convert Tour List Items to Tour Summry List Items

        public static List<TourSummaryModel> ToTourSummaryList(IEnumerable<Tour> tour)
        {
            List<TourSummaryModel> tourList = new List<TourSummaryModel>();
            if (tour != null)
            {
                foreach (var c in tour)
                {
                    tourList.Add(ToTourSummary(c));
                }
                tourList.TrimExcess();
            }

            return tourList;
        }

    }
}