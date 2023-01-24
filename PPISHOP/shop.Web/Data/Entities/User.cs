using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Data.Entities
{
    public class User : IdentityUser

    {
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name ="Nombre Completo")]
        public string FullName { get { return $"{this.FirstName} {this.LastName }"; } }

    }
}
