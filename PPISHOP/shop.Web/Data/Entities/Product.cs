
namespace shop.Web.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Product : IEntity
    {
		public int Id { get; set; }

		[MaxLength(50, ErrorMessage = "El campo {0} debe ser maximo {1} caracteres")]
		[Required]

		[Display(Name = "Nombre")]
		public string Name { get; set; }

		[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]

		[Display(Name = "Precio")]
		public decimal Price { get; set; }

		[Display(Name = "Imagen")]
		public string ImageUrl { get; set; }

		[Display(Name = "Ultima compra")]
		public DateTime? LastPurchase { get; set; }

		[Display(Name = "Ultima venta")]
		public DateTime? LastSale { get; set; }

		[Display(Name = "Esta disponible?")]
		public bool IsAvailabe { get; set; }

		[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
		public double Stock { get; set; }

		//aqui le estoy relacionando la tabla user con producto
		//ejemplo un usuario matricula muchos productos
		//gracias a .net core.. puedo hacer la relacion solo de un lado..
		public User User { get; set; }

		public string ImageFullPath 
		{ get
			{
				if (string.IsNullOrEmpty(this.ImageUrl))
   				{
					return null;
				}

				//TODO  REVISAR LA DIRECCION CON LA QUE ESTA SALIENDO LAS APIS
				//return $"https://localhost:44332/{this.ImageUrl.Substring(1)}";
				return $"https://postrejuanita.azurewebsites.net/";
				// recordar que $ es para interpolar o es decir concatenar
			} 
		}
	}
}

