using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
	/// <typeparam name="T">Entity</typeparam>
	public interface IAsyncRepository<T>
	{
		Task<T>       GetById (int id);
		Task<T>       GetById (string id);
		Task<List<T>> GetAll();
		Task          AddAsync(T    entity);
		Task          UpdateAsync (T entity);
		Task          UpdateUserAsync (T entity);
		Task          DeleteAsync (T entity);
	}
}