using System.Threading;
using System.Threading.Tasks;

namespace Web.Services
{
	public interface IScopedСurrencyService
	{
		Task DoWork(CancellationToken stoppingToken);
	}
}
