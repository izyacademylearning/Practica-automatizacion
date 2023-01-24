using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Data
{
	using Entities;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;

    // cuando implementamos el product repository 
    // hace una implementacion del generic repository con product
    // y le estamos diciendo que implementa el repository generico y que implementa el Iproducrepository
    public class ProductRepository : GenericRepository<Product>, IProductRepository
	{
		private readonly DataContext context;

		// inyecta al datacontext 
		public ProductRepository(DataContext context) : base(context)
		{
			this.context = context;
		}

		public IQueryable GetAllWithUsers()
		{
			return this.context.Products.Include(p=>p.User);
		}

		//aqui me devuelve una lista
		public IEnumerable<SelectListItem> GetComboProducts()
		{ //seleccione un nuevo objeto por cada producto name
			//value es lo que almacena el combobox
			var list = this.context.Products.Select(p => new SelectListItem
			{
				Text = p.Name,
				Value = p.Id.ToString()
			}).ToList();

			list.Insert(0, new SelectListItem
			{
				Text = "(Selecciona un product...)",
				Value = "0"
			});

			return list;
		}

	}

}
