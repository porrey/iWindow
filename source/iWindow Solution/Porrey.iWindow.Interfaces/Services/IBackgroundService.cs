using System.Threading.Tasks;

namespace Porrey.iWindow.Interfaces
{
	public interface IBackgroundService
	{
		string Name { get; }
		Task<bool> Start();
		Task<bool> Stop();
	}
}