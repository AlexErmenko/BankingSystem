using System.Collections.Generic;
using System.Linq;
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
		private readonly ApplicationDbContext _context;
		protected readonly BankingSystemContext Context;

		public EfRepository(BankingSystemContext context, ApplicationDbContext usContext)
		{
			Context = context;
			_context = usContext;
		}

		public async Task<T> GetById(int? id) => await Context.Set<T>().FindAsync(id);
		public async Task<T> GetById(string id) => await Context.Set<T>().FindAsync(id);

		public async Task<List<T>> GetAll() => await Context.Set<T>().ToListAsync();

		public async Task AddAsync(T entity)
		{
			await Context.Set<T>().AddAsync(entity: entity);
			await Context.SaveChangesAsync();
		}

		public async Task UpdateAsync(T entity)
		{
			Context.Entry(entity: entity).State = EntityState.Modified;
			await Context.SaveChangesAsync();
		}

		public async Task UpdateUserAsync(T entity)
		{
			_context.Entry(entity: entity).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}

		public async Task DeleteAsync(T entity)
		{
			Context.Set<T>().Remove(entity: entity);
			await Context.SaveChangesAsync();
		}

		public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec) => await ApplySpecification(spec: spec).ToListAsync();

		private IQueryable<T> ApplySpecification(ISpecification<T> spec) => SpecificationEvaluator<T>.GetQuery(inputQuery: Context.Set<T>().AsQueryable(), specification: spec);

		public async Task<T> GetById(Task<int?> id) => await Context.Set<T>().FindAsync(id);
	}
}
