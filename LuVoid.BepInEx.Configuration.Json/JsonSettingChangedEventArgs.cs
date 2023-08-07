using BepInEx.Configuration;
using System;

namespace LuVoid.BepInEx.Configuration.Json
{
	public sealed class JsonSettingChangedEventArgs : EventArgs
	{
		public readonly ConfigEntryJsonBase ChangedJsonSetting;
		public readonly ConfigEntryBase ChangedSetting;

		public JsonSettingChangedEventArgs(ConfigEntryJsonBase changedJsonSetting, ConfigEntryBase changedSetting)
		{
			ChangedJsonSetting = changedJsonSetting;
			ChangedSetting = changedSetting;
		}

	}
}
