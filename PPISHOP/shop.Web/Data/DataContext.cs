

namespace shop.Web.Data
{
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore;
	using shop.Web.Data.Entities;
	using System.Linq;
	public class DataContext : IdentityDbContext<User>
	{// AQUI LE ESTAMOS DICIENDO QUE ME TRABAJE CON LAS TABLAS DE SEGURIDAD Y CON MI MODELO USER
	 // creamos una propiedad de tipo Dbset para ver que modelo voy a tirar a la base de datos
		public DbSet<Product> Products { get; set; }
		// cada vez que necesite acceder a los productos , voy a accederr a esta propiedad

			//el MOdelo singular y el valor plural
		public DbSet<Order> Orders { get; set; }

		public DbSet<OrderDetail> OrderDetails { get; set; }

		public DbSet<OrderDetailTemp> OrderDetailTemps { get; set; }

		//con este contructor me pego a la base de datos ,recordar el string de conexion esta en appsetting.json
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>()
			.Property(p => p.Price)
			.HasColumnType("decimal(18,2)");
			base.OnModelCreating(modelBuilder);

			var cascadeFKs = modelBuilder.Model
				.G­etEntityTypes()
				.SelectMany(t => t.GetForeignKeys())
				.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Casca­de);
			foreach (var fk in cascadeFKs)
			{
				fk.DeleteBehavior = DeleteBehavior.Restr­ict;
			}

			base.OnModelCreating(modelBuilder);

		}
	}

}
