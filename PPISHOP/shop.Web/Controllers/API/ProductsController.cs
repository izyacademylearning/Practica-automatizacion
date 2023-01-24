

namespace shop.Web.Controllers.API
{
	using Data;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

	// aqui es donde le vamos a decir a donde debe acceder a los datos 
	[Route("api/[Controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ProductsController : Controller
	{
		private readonly IProductRepository productRepository;

		// como necesitamos los datos del products... vamos a inyectarle los datos
		public ProductsController(IProductRepository productRepository)
		{
			this.productRepository = productRepository;
		}


		// este va a ser el get del controlador.. con el OK. 
		// esto envuelve el resultado en un JSON y me lo devuelve
		// mandamos esto en un servicio result
		// me obtiene todo
		[HttpGet]
		public IActionResult GetProducts()
		{
			return this.Ok(this.productRepository.GetAllWithUsers());
		}
	}

}
