using BepInEx.Configuration;
using BepInEx.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using BepInExLogger = BepInEx.Logging.Logger;

namespace BepInEx.Configuration.Json
{
	public abstract class ConfigEntryJsonBase
	{
		internal static ManualLogSource Logger = BepInExLogger.CreateLogSource("BepInEx.Configuration.Json");

		public ConfigFile ConfigFile { get; private set; }

		public ConfigDefinition Definition { get; private set; }

		public ConfigDescription Description { get; private set; }

		public Type SettingType { get; private set; }

		public object DefaultValue { get; private set; }

		public JsonSerializer Serializer { get; private set; }

		public abstract object BoxedValue { get; set; }

		internal ConfigEntryJsonBase(ConfigFile configFile, ConfigDefinition definition, Type settingType, object defaultValue, ConfigDescription configDescription, JsonSerializer jsonSerializer)
		{
			ConfigFile = configFile ?? throw new ArgumentNullException("configFile");
			Definition = definition ?? throw new ArgumentNullException("definition");
			SettingType = settingType ?? throw new ArgumentNullException("settingType");
			Serializer = jsonSerializer ?? throw new ArgumentNullException("jsonSerializer");
			Description = configDescription ?? ConfigDescription.Empty;
			if (Description.AcceptableValues != null && !SettingType.IsAssignableFrom(Description.AcceptableValues.ValueType))
			{
				throw new ArgumentException("configDescription.AcceptableValues is for a different type than the type of this setting");
			}

			DefaultValue = defaultValue;
			BoxedValue = defaultValue;
		}

		public string GetSerializedValue()
		{
			StringWriter writer = new StringWriter();
			Serializer.Serialize(new JsonTextWriter(writer), BoxedValue);
			return writer.ToString();
		}

		public void SetSerializedValue(string value)
		{
			StringReader reader = new StringReader(value);
			JsonTextReader jsonReader = new JsonTextReader(reader);
			try
			{
				BoxedValue = Serializer.Deserialize(jsonReader, SettingType);
			}
			catch (Exception ex)
			{
				Logger.Log(LogLevel.Warning, $"Config value of setting \"{Definition}\" could not be parsed and will be ignored. Reason: {ex.Message}; Value: {value}");
			}
		}

		protected T ClampValue<T>(T value)
		{
			throw new NotImplementedException();
		}

		protected abstract void OnSettingChanged(object sender);

		public void WriteDescription(StreamWriter writer)
		{
			throw new NotImplementedException();
		}
	}
}
