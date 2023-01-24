using Microsoft.AspNetCore.Http;
using shop.Web.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Models
{
    public class ProducViewModel : Product

    {
        //etiqueta para ver la imagen
        [Display(Name = "Imagen")]

        // esto me almacena temporalmente la imagen median la interfaz iForfie
        public IFormFile ImageFile { get; set; }

    }
}
