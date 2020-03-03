using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
	public interface IAsyncRepository<T>
	{
		Task<T>       GetById(int id);
		Task<List<T>> GetAll();
		Task       AddAsync(T    entity);
		Task          UpdateAsync(T entity);
		Task          DeleteAsync(T entity);
	}
}