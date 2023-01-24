using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace shop.Web.Data
{
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
	using shop.Web.Data.Entities;

    public class GenericRepository<T> : IGenericRepository<T> where T : class, IEntity
{
	private readonly DataContext context;

	public GenericRepository(DataContext context)
	{
    	this.context = context;
	}

	public IQueryable<T> GetAll()
	{
			//aqui le estoy diciendo que me jale los productos
			//y que me setee lo que venga
			// AsNotracking nos funcion el metodo generico
    	return this.context.Set<T>().AsNoTracking();
	}

	public async Task<T> GetByIdAsync(int id)
	{
			// retorne el contexto.. setee lo que venga.. y retorne lo que le trajeron de parametro
    	return await this.context.Set<T>()
        	.AsNoTracking()
        	.FirstOrDefaultAsync(e => e.Id == id);
	}

	public async Task CreateAsync(T entity)
	{
    	await this.context.Set<T>().AddAsync(entity);
    	await SaveAllAsync();
	}

	public async Task UpdateAsync(T entity)
	{
    	this.context.Set<T>().Update(entity);
    	await SaveAllAsync();
	}

	public async Task DeleteAsync(T entity)
	{
    	this.context.Set<T>().Remove(entity);
    	await SaveAllAsync();
	}

	public async Task<bool> ExistAsync(int id)
	{
    	return await this.context.Set<T>().AnyAsync(e => e.Id == id);

	}

	public async Task<bool> SaveAllAsync()
	{
    	return await this.context.SaveChangesAsync() > 0;
	}
}

}
