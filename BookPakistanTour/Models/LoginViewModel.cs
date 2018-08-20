using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BookPakistanTour.Models
{
    public class LoginViewModel
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "The Email field is Required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*",
            ErrorMessage = "Email Not Verified !!")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password Is Too Short..!!")]
        public string Password { get; set; }


        [Display(Name = "RememberMe")]
        public bool RememberMe { get; set; }




    }
}