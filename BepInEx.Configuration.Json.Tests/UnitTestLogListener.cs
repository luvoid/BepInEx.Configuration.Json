using BepInEx.Logging;
using System;
using UnitTestingLogger = Microsoft.VisualStudio.TestTools.UnitTesting.Logging.Logger;
using BepInExLogger = BepInEx.Logging.Logger;

namespace BepInEx.Logging
{
	public class UnitTestLogListener : ILogListener
	{
		private bool disposedValue;

		public UnitTestLogListener()
		{
			BepInExLogger.Listeners.Add(this);
		}

		public void LogEvent(object sender, LogEventArgs eventArgs)
		{
			string message = eventArgs.ToString();
			message = message.Replace("{", "{{").Replace("}", "}}");
			UnitTestingLogger.LogMessage(message);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					BepInExLogger.Listeners.Remove(this);
				}
				disposedValue = true;
			}
		}
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
	}
}
