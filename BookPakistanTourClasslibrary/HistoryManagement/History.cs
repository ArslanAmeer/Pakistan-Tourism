using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookPakistanTourClasslibrary.HistoryManagement
{
    public class History
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string ImageUrl { get; set; }

    }
}
