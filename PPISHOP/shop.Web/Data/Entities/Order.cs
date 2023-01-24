using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Data.Entities
{
	public class Order : IEntity
	{
		public int Id { get; set; }

		[Required]
		[Display(Name = "Fecha Orden")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
		public DateTime OrderDate { get; set; }

		[Display(Name = "Fecha de Envio")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
		public DateTime? DeliveryDate { get; set; }

		//un usuario puede estar asignados a muchos productos
		[Required]
		public User User { get; set; }

		// una orden puede tener muchos productos
		public IEnumerable<OrderDetail> Items { get; set; }


		[DisplayFormat(DataFormatString = "{0:N0}")]
		[Display(Name = "Tipos de productos")]
		public int Lines { get { return this.Items == null ? 0 : this.Items.Count(); } }


		// suma de la cantidad de todos mis productos
		[DisplayFormat(DataFormatString = "{0:N2}")]
		[Display(Name = "Cantidad")]
		public double Quantity { get { return this.Items == null ? 0 : this.Items.Sum(i => i.Quantity); } }

		
		[DisplayFormat(DataFormatString = "{0:C2}")]
		[Display(Name = "Valor")]
		public decimal Value { get { return this.Items == null ? 0 : this.Items.Sum(i => i.Value); } }

		//este metodo es para organizar la hora.. para que salga con la hora local y no la de londres
		[Display(Name = "Fecha Orden")]
		[DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm tt}", ApplyFormatInEditMode = false)]
		public DateTime? OrderDateLocal
		{
			get 
			{ 
			if(this.OrderDate==null)
				{
					return null;
				}
				return this.OrderDate.ToLocalTime();
			} 
		}
	}

}
