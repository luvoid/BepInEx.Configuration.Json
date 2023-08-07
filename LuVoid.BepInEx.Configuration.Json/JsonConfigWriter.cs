using Newtonsoft.Json;
using System;

namespace LuVoid.BepInEx.Configuration.Json
{
	/// <summary>
	/// A JsonWriter that instead of writing JSON will invoke various callbacks.
	/// </summary>
	public class JsonConfigWriter : JsonWriter
	{
		/// <summary>
		///		A callback that is invoked everytime a JsonConfigWriter is instructed to write a value.
		/// </summary>
		/// <remarks>
		///		In most cases, the token will be <see cref="JsonToken.None"/>, unless the token cannot be represented by a type.
		///		If type is null, you likely will want to check the token.
		/// </remarks>
		/// <param name="value"></param>
		/// <param name="type"></param>
		/// <param name="path"></param>
		/// <param name="token">
		/// </param>
		public delegate void WriteValueCallback(object value, Type type, string path, JsonToken token);

		private readonly WriteValueCallback writeValueCallback;

		public JsonConfigWriter(WriteValueCallback writeValueCallback)
		{
			this.writeValueCallback = writeValueCallback;
		}

		private void InvokeWriteValueCallback(object value, Type type)
		{
			writeValueCallback.Invoke(value, type, Path, JsonToken.None);
		}

		private void InvokeWriteValueCallback(object value, JsonToken token)
		{
			writeValueCallback.Invoke(value, null, Path, token);
		}

		private void InvokeWriteValueCallback(JsonToken token)
		{
			writeValueCallback.Invoke(null, null, Path, token);
		}

		/// <summary>
		/// Does nothing.
		/// </summary>
		public override void Flush()
		{ }
		
		//
		// Summary:
		//     Writes a System.Object value. An error will raised if the value cannot be written
		//     as a single JSON token.
		//
		// Parameters:
		//   value:
		//     The System.Object value to write.
		public override void WriteValue(object value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(object));
		}

		//
		// Summary:
		//     Writes a null value.
		public override void WriteNull()
		{
			base.WriteNull();
			InvokeWriteValueCallback(JsonToken.Null);
		}

		//
		// Summary:
		//     Writes an undefined value.
		public override void WriteUndefined()
		{
			base.WriteUndefined();
			InvokeWriteValueCallback(JsonToken.Undefined);
		}

		//
		// Summary:
		//     Writes raw JSON.
		//
		// Parameters:
		//   json:
		//     The raw JSON to write.
		public override void WriteRaw(string json)
		{
			base.WriteRaw(json);
			InvokeWriteValueCallback(JsonToken.Raw);
		}

		//
		// Summary:
		//     Writes a System.String value.
		//
		// Parameters:
		//   value:
		//     The System.String value to write.
		public override void WriteValue(string value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(string));
		}

		//
		// Summary:
		//     Writes a System.Int32 value.
		//
		// Parameters:
		//   value:
		//     The System.Int32 value to write.
		public override void WriteValue(int value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(int));
		}

		//
		// Summary:
		//     Writes a System.UInt32 value.
		//
		// Parameters:
		//   value:
		//     The System.UInt32 value to write.
		[CLSCompliant(false)]
		public override void WriteValue(uint value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(uint));
		}

		//
		// Summary:
		//     Writes a System.Int64 value.
		//
		// Parameters:
		//   value:
		//     The System.Int64 value to write.
		public override void WriteValue(long value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(long));
		}

		//
		// Summary:
		//     Writes a System.UInt64 value.
		//
		// Parameters:
		//   value:
		//     The System.UInt64 value to write.
		[CLSCompliant(false)]
		public override void WriteValue(ulong value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(ulong));
		}

		//
		// Summary:
		//     Writes a System.Single value.
		//
		// Parameters:
		//   value:
		//     The System.Single value to write.
		public override void WriteValue(float value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(float));
		}

		//
		// Summary:
		//     Writes a System.Nullable`1 of System.Single value.
		//
		// Parameters:
		//   value:
		//     The System.Nullable`1 of System.Single value to write.
		public override void WriteValue(float? value)
		{
			InvokeWriteValueCallback(value, typeof(float?));
		}

		//
		// Summary:
		//     Writes a System.Double value.
		//
		// Parameters:
		//   value:
		//     The System.Double value to write.
		public override void WriteValue(double value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(double));
		}

		//
		// Summary:
		//     Writes a System.Nullable`1 of System.Double value.
		//
		// Parameters:
		//   value:
		//     The System.Nullable`1 of System.Double value to write.
		public override void WriteValue(double? value)
		{
			InvokeWriteValueCallback(value, typeof(double?));
		}

		//
		// Summary:
		//     Writes a System.Boolean value.
		//
		// Parameters:
		//   value:
		//     The System.Boolean value to write.
		public override void WriteValue(bool value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(bool));
		}

		//
		// Summary:
		//     Writes a System.Int16 value.
		//
		// Parameters:
		//   value:
		//     The System.Int16 value to write.
		public override void WriteValue(short value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(short));
		}

		//
		// Summary:
		//     Writes a System.UInt16 value.
		//
		// Parameters:
		//   value:
		//     The System.UInt16 value to write.
		[CLSCompliant(false)]
		public override void WriteValue(ushort value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(ushort));
		}

		//
		// Summary:
		//     Writes a System.Char value.
		//
		// Parameters:
		//   value:
		//     The System.Char value to write.
		public override void WriteValue(char value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(char));
		}

		//
		// Summary:
		//     Writes a System.Byte value.
		//
		// Parameters:
		//   value:
		//     The System.Byte value to write.
		public override void WriteValue(byte value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(byte));
		}

		//
		// Summary:
		//     Writes a System.SByte value.
		//
		// Parameters:
		//   value:
		//     The System.SByte value to write.
		[CLSCompliant(false)]
		public override void WriteValue(sbyte value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(sbyte));
		}

		//
		// Summary:
		//     Writes a System.Decimal value.
		//
		// Parameters:
		//   value:
		//     The System.Decimal value to write.
		public override void WriteValue(decimal value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(decimal));
		}

		//
		// Summary:
		//     Writes a System.DateTime value.
		//
		// Parameters:
		//   value:
		//     The System.DateTime value to write.
		public override void WriteValue(DateTime value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(DateTime));
		}

		//
		// Summary:
		//     Writes a System.Byte[] value.
		//
		// Parameters:
		//   value:
		//     The System.Byte[] value to write.
		public override void WriteValue(byte[] value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(byte[]));
		}

		//
		// Summary:
		//     Writes a System.DateTimeOffset value.
		//
		// Parameters:
		//   value:
		//     The System.DateTimeOffset value to write.
		public override void WriteValue(DateTimeOffset value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(DateTimeOffset));
		}

		//
		// Summary:
		//     Writes a System.Guid value.
		//
		// Parameters:
		//   value:
		//     The System.Guid value to write.
		public override void WriteValue(Guid value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(Guid));
		}

		//
		// Summary:
		//     Writes a System.TimeSpan value.
		//
		// Parameters:
		//   value:
		//     The System.TimeSpan value to write.
		public override void WriteValue(TimeSpan value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(TimeSpan));
		}

		//
		// Summary:
		//     Writes a System.Uri value.
		//
		// Parameters:
		//   value:
		//     The System.Uri value to write.
		public override void WriteValue(Uri value)
		{
			base.WriteValue(value);
			InvokeWriteValueCallback(value, typeof(Uri));
		}

		//
		// Summary:
		//     Writes a comment /*...*/ containing the specified text.
		//
		// Parameters:
		//   text:
		//     Text to place inside the comment.
		public override void WriteComment(string text)
		{
			base.WriteComment(text);
			InvokeWriteValueCallback(text, JsonToken.Comment);
		}
	}
}
