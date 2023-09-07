using BepInEx.Configuration;
using LuVoid.Collections.Weak;
using Newtonsoft.Json;

namespace BepInEx.Configuration.Json
{
	public static class ConfigFileExtensions
	{
		class WeakConfigEntries : WeakDictionary<ConfigDefinition, ConfigEntryJsonBase> { }
		class WeakConfigFiles : WeakDictionary<ConfigFile, WeakConfigEntries> { }

		private static readonly WeakConfigFiles ConfigFiles = new WeakConfigFiles();

		private static readonly JsonSerializer defaultSerializer;

		static ConfigFileExtensions()
		{
			defaultSerializer = JsonSerializer.CreateDefault();
			defaultSerializer.Converters.Add(
				new Newtonsoft.Json.Converters.StringEnumConverter()
			);
		}

		public static ConfigEntryJson<T> BindJson<T>(this ConfigFile configFile, ConfigDefinition configDefinition, T defaultValue,
			ConfigDescription configDescription = null, JsonSerializer serializer = null)
			where T : struct
		{
			serializer ??= defaultSerializer;

			if (!ConfigFiles.TryGetValue(configFile, out var entries))
			{
				ConfigFiles[configFile] = entries = new WeakConfigEntries();
			}

			if (entries.TryGetValue(configDefinition, out var value))
			{
				return (ConfigEntryJson<T>)value;
			}

			ConfigEntryJson<T> configEntry = new ConfigEntryJson<T>(configFile, configDefinition, defaultValue, configDescription, serializer);
			entries[configDefinition] = configEntry;

			return configEntry;
		}

		public static ConfigEntryJson<T> BindJson<T>(this ConfigFile configFile, string section, string key, T defaultValue,
			ConfigDescription configDescription = null, JsonSerializer serializer = null)
			where T : struct
		{
			return BindJson(configFile, new ConfigDefinition(section, key), defaultValue, configDescription, serializer);
		}

		public static ConfigEntryJson<T> BindJson<T>(this ConfigFile configFile, string section, string key, T defaultValue,
			string description, JsonSerializer serializer = null)
			where T : struct
		{
			return BindJson(configFile, new ConfigDefinition(section, key), defaultValue, new ConfigDescription(description, null), serializer);
		}
	}
}
