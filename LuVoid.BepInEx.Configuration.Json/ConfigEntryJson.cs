using BepInEx.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace LuVoid.BepInEx.Configuration.Json
{
	public sealed class ConfigEntryJson<T> : ConfigEntryJsonBase
		where T : struct
	{
		private bool _initialized = false;
		private T _typedValue;
		public T Value
		{
			get => _typedValue;
			set
			{
				//value = ClampValue(value);
				if (!object.Equals(_typedValue, value))
				{
					_typedValue = value;
					OnSettingChanged(this);
				}
			}
		}

		public override object BoxedValue { get => Value; set => Value = (T)value; }

		public event EventHandler SettingChanged;

		private readonly Dictionary<string, ConfigEntryBase> ConfigEntries = new Dictionary<string, ConfigEntryBase>();

		internal ConfigEntryJson(ConfigFile configFile, ConfigDefinition definition, T defaultValue, ConfigDescription configDescription, JsonSerializer jsonSerializer)
			: base(configFile, definition, typeof(T), defaultValue, configDescription, jsonSerializer)
		{
			configFile.SettingChanged += OnConfigFileSettingChanged;

			//BindFromJson(this.GetSerializedValue());
			BindWithJsonConfigWriter(defaultValue);

			// Load the settings from the file back into the value
			string json = ConfigEntriesToJson();
			_typedValue = Serializer.Deserialize<T>(new JsonTextReader(new StringReader(json)));

			_initialized = true;
		}

		protected override void OnSettingChanged(object sender)
		{
			if (!_initialized) return;

			//JsonToConfigEntries(this.GetSerializedValue());
			SetConfigEntriesWithJsonConfigWriter();
		}
		
		private void OnConfigFileSettingChanged(object sender, SettingChangedEventArgs args)
		{
			if (args.ChangedSetting.Definition.Section != Definition.Section) return;
			if (!ConfigEntries.TryGetValue(args.ChangedSetting.Definition.Key, out var changedSetting)) return;

			//var newData = new Dictionary<string, object>();
			//AddEntryToData(args.ChangedSetting, newData);
			//string json = DataToJson(newData);
			//object newValue = _typedValue;
			//Serializer.Populate(new StringReader(json), newValue);
			//_typedValue = (T)newValue;

			// XXX The nuclear approach
			string json = ConfigEntriesToJson();
			//Logger.LogDebug($"OnSettingChanged JSON:\n{json}");
			_typedValue = Serializer.Deserialize<T>(new JsonTextReader(new StringReader(json)));

			var eventArgs = new JsonSettingChangedEventArgs(this, changedSetting);
			this.SettingChanged?.Invoke(sender, eventArgs);
		}

		private void BindWithJsonConfigWriter(T defaultValue)
		{
			string lastComment = Description.Description;
			void OnWriteValue(object value, Type type, string path, JsonToken token)
			{
				//Logger.LogDebug($"-----------------------------------");
				//Logger.LogDebug($"Path     : {path }");
				//Logger.LogDebug($"Token    : {token}");
				//Logger.LogDebug($"ValueType: {type }");
				//Logger.LogDebug($"Value    : {value}");

				ConfigEntryBase entry = null;
				if (type != null)
				{
					if (TomlTypeConverter.CanConvert(type))
					{
						Func<ConfigDefinition, object, ConfigDescription, object> bindObjectMethod = ConfigFile.Bind;
						var generic = bindObjectMethod.Method.GetGenericMethodDefinition();
						var bindMethod = generic.MakeGenericMethod(type);
						
						ConfigDefinition entryDefinition = new ConfigDefinition(Definition.Section, $"{Definition.Key}.{path}");
						ConfigDescription entryDescription = lastComment != null ? new ConfigDescription(lastComment) : null;

						entry = (ConfigEntryBase)bindMethod.Invoke(ConfigFile, new object[] { entryDefinition, value, entryDescription });

					}
					else
					{
						Logger.LogWarning($"Ignoring field {path} in {Definition.Key} {nameof(ConfigEntryJson<T>)}. ({type} is not supported)");
					}

				}
				else if (token == JsonToken.Comment)
				{
					lastComment = (string)value;
				}
				
				if (entry != null)
				{
					ConfigEntries.Add(entry.Definition.Key, entry);
					lastComment = null;
				}
			}
			
			JsonConfigWriter writer = new JsonConfigWriter(OnWriteValue);
			Serializer.Serialize(writer, defaultValue);
		}

		private void AddEntryToData(ConfigEntryBase entry, Dictionary<string, object> data)
		{
			string[] parts = entry.Definition.Key.Split('.');
			Dictionary<string, object> parent = data;

			for (int i = 0; i < parts.Length - 1; i++)
			{
				string part = parts[i];
				if (!parent.TryGetValue(part, out object value))
				{
					var newChild = new Dictionary<string, object>();
					parent[part] = newChild;
					parent = newChild;
				}
				else if (value is Dictionary<string, object> child)
				{
					parent = child;
				}
				else
				{
					throw new FormatException("A field has the same name as a child");
				}
			}

			string fieldName = parts[parts.Length - 1];
			parent[fieldName] = entry.BoxedValue;
		}

		private string ConfigEntriesToJson()
		{
			var data = new Dictionary<string, object>();
			foreach (var entry in ConfigEntries.Values)
			{
				AddEntryToData(entry, data);
			}

			return DataToJson((Dictionary<string, object>)data[Definition.Key]);
		}

		private string DataToJson(Dictionary<string, object> data)
		{
			StringWriter stringWriter = new StringWriter();
			JsonTextWriter jsonWriter = new JsonTextWriter(stringWriter);
			//jsonWriter.Formatting = Formatting.Indented;
			JsonSerializer simpleSerializer = new JsonSerializer();
			simpleSerializer.Serialize(jsonWriter, data);
			return stringWriter.ToString();
		}

		private void SetConfigEntriesWithJsonConfigWriter()
		{

			void OnWriteValue(object value, Type type, string path, JsonToken token)
			{
				if (type == null || !TomlTypeConverter.CanConvert(type))
					return;

				if (!ConfigEntries.TryGetValue($"{Definition.Key}.{path}", out var entry))
				{
					throw new FormatException($"ConfigEntryJson schema mutated. Field {path} was not present at time of bind.");
				}

				entry.BoxedValue = value;
			}

			JsonConfigWriter writer = new JsonConfigWriter(OnWriteValue);
			Serializer.Serialize(writer, _typedValue);
		}
	}
}
