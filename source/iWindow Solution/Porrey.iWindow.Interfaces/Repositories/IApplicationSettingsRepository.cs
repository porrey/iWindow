using System.Threading.Tasks;

namespace Porrey.iWindow.Interfaces
{
	public interface IApplicationSettingsRepository
	{
		string TemperatureUnit { get; set; }

		Task ResetToDefaults();
	}
}