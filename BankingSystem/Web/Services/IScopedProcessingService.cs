using System.Threading;
using System.Threading.Tasks;

namespace Web.Services
{
	internal interface IScopedProcessingService
	{
		Task DoWork(CancellationToken stoppingToken);
	}
}