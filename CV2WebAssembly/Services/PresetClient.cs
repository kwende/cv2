using Common;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CV2WebAssembly.Services
{
    public class PresetClient
    {
		private HttpClient _client; 

		public PresetClient(HttpClient client)
		{
			_client = client;
		}

		public async Task<Preset> LoadPreset(string presetName)
		{
			var result = await _client.GetFromJsonAsync<Preset>($"presets/{presetName}.json");
			if(result == null)
			{
				throw new HttpRequestException($"Preset '{presetName}' not found.", null, System.Net.HttpStatusCode.NotFound);
			}
			else
			{
				return result;
			}
		}
    }
}
