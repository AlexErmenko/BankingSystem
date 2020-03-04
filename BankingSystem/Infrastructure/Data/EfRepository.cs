using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
	/// <summary>
	///     Реализация интерфейса
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class EfRepository<T> : IAsyncRepository<T> where T : class
	{
		protected readonly BankingSystemContext Context;

		public EfRepository(BankingSystemContext context)
		{
			Context = context;
			// context.Set<T>().Load();
		}

		public async Task<T> GetById(int id) { return await Context.Set<T>().FindAsync(id); }

		public async Task<List<T>> GetAll() { return await Context.Set<T>().ToListAsync(); }

		public async Task AddAsync(T entity)
		{
			await Context.Set<T>().AddAsync(entity);
			await Context.SaveChangesAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			Context.Entry(entity).State = EntityState.Modified;
			await Context.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			Context.Set<T>().Remove(entity);
			await Context.SaveChangesAsync();
		}
	}
}