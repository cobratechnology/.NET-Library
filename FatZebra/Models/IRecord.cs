using Newtonsoft.Json;

namespace FatZebra
{
    public interface IRecord
    {
		[JsonProperty("id")] 
		string ID { get; }
		[JsonProperty("successful")]
		bool Successful { get; }
    }
}
