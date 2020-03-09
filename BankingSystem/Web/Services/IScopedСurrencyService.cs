using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Web.Services
{
	public interface IScopedСurrencyService
	{
		Task DoWork(CancellationToken stoppingToken);
	}
}