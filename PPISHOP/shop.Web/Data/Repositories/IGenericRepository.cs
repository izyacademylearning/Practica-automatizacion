

namespace shop.Web.Data
{
	using System.Linq;
	using System.Threading.Tasks;

	public interface IGenericRepository<T> where T : class
	{
		IQueryable<T> GetAll();

		// esto es una clase T.. donde T puede ser cualquier clase
		Task<T> GetByIdAsync(int id);

		// aqui le mandamos T..y el me adiciona un T
		Task CreateAsync(T entity);

		Task UpdateAsync(T entity);

		Task DeleteAsync(T entity);

		Task<bool> ExistAsync(int id);
	}

}
