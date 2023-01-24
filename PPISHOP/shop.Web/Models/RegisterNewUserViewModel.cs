using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Models
{
    public class RegisterNewUserViewModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

 
        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        // compare es para hacer igual el password y re confirmar
        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }

}
