using BepInEx.Configuration;
using BepInEx.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using LuVoid.BepInEx.Logging;


[assembly: ClassCleanupExecution(ClassCleanupBehavior.EndOfAssembly)]

namespace LuVoid.BepInEx.Configuration.Json.Tests
{
	[TestClass]
	public class TestConfigEntryJson
	{
		const string k_ConfigPath = "testconfig.cfg";

		public static ConfigFile Config;
		public static UnitTestLogListener LogListener;
		public static string ConfigSection = "Default";

		[ClassInitialize]
		public static void ClassInitialize(TestContext context)
		{
			Config = new ConfigFile(k_ConfigPath, false);
			Config.SaveOnConfigSet = false;

			LogListener = new UnitTestLogListener();
			ConfigSection = context.TestName;
		}

		[ClassCleanup]
		public static void ClassCleanup()
		{
			Config.Save();
			LogListener.Dispose();
		}

		[TestMethod]
		public void TestBind()
		{
			var entry = Config.BindJson(ConfigSection, nameof(ExampleStruct), new ExampleStruct(1));
			//var entry2 = Config.Bind(ConfigSection, nameof(ExampleStruct), new ExampleStruct(1));
		}

		[TestMethod]
		public void TestValueAccess()
		{
			var entry = Config.BindJson(ConfigSection, nameof(ExampleStruct), new ExampleStruct(1));

			Assert.AreEqual(true              , entry.Value.BoolField  );
			Assert.AreEqual( 1                , entry.Value.IntField   );
			Assert.AreEqual( 1.0              , entry.Value.FloatField );
			Assert.AreEqual("1"               , entry.Value.StringField);
			Assert.AreEqual(ExampleEnum.Value1, entry.Value.EnumField  );

			var value = entry.Value;
			value.IntField = 2;
			entry.Value = value;
		}

		[TestMethod]
		public void TestNoValueWipe()
		{
			var entry = Config.BindJson(ConfigSection, nameof(ExampleStruct), new ExampleStruct(1));
			
			// Sometimes the changed setting callbacks will cause the values to be wiped temporarly
			// It is a race condition, so test many different values in a loop.
			for (int i = 2; i < 100; i++)
			{
				var value = entry.Value;
				value.IntField = i;
				entry.Value = value;

				Assert.AreEqual(i, entry.Value.IntField);

				// This value was not set, and therefore shouldn't change
				Assert.AreEqual(1.0, entry.Value.FloatField);
			}
		}

	}

	/// <summary>
	/// Structs made up of either unmanaged fields, or strings
	/// should also always be safe.
	/// </summary>
	public struct ExampleStruct
	{
		public bool        BoolField;
		public int         IntField;
		public float       FloatField;
		public string      StringField;
		public ExampleEnum EnumField;

		public ExampleStruct(int fillValue)
		{
			BoolField   = fillValue != 0;
			IntField    = fillValue;
			FloatField  = (float)fillValue;
			StringField = fillValue.ToString();
			EnumField   = (ExampleEnum)fillValue;
		}
	}

	public enum ExampleEnum
	{
		Value0,
		Value1, 
		Value2,
	}
}
