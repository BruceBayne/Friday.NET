
using Friday.Json.Basics.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Friday.Json.Basics
{
	public sealed class JsonRepositorySettings
	{
		public static JsonSerializerSettings Default
		{
			get
			{
				var jsonSettings = new JsonSerializerSettings
				{
					Formatting = Formatting.Indented,
					// ContractResolver = new WritablePropertiesOnlyResolver(),
					NullValueHandling = NullValueHandling.Ignore,
					DefaultValueHandling = DefaultValueHandling.Include,
					TypeNameHandling = TypeNameHandling.Auto,
					PreserveReferencesHandling = PreserveReferencesHandling.Objects,
				};
				jsonSettings.Converters.Add(new StringEnumConverter { });
			    jsonSettings.Converters.Add(new IpAddressConverter());
			    jsonSettings.Converters.Add(new IpEndPointConverter());
                return jsonSettings;
			}
		}
	}
}