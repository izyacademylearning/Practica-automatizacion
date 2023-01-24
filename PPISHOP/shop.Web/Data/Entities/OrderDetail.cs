using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Data.Entities
{
    public class OrderDetail : IEntity
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public Product Product { get; set; }


        [Display(Name ="Precio")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal Price { get; set; }

        [Display(Name = "Cantidad")]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public double Quantity { get; set; }

        [DisplayFormat(DataFormatString = "{0:C2}")]
        [Display(Name = "Valor")]
        public decimal Value { get { return this.Price * (decimal)this.Quantity; } }

       

    }

}
