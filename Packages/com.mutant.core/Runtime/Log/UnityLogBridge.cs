using UnityEngine;

namespace Bill.Mutant.Core
{
	public static class UnityLogBridge
	{
		private static bool _installed;
		private static bool _isForwarding;

		public static void Install()
		{
			if (_installed) return;
			Application.logMessageReceived += OnLogMessageReceived;
			_installed = true;
		}

		public static void Uninstall()
		{
			if (!_installed) return;
			Application.logMessageReceived -= OnLogMessageReceived;
			_installed = false;
		}

		private static void OnLogMessageReceived(string condition, string stackTrace, LogType type)
		{
			if (_isForwarding) return;

			_isForwarding = true;
			try
			{
				LogLevel level;
				switch (type)
				{
					case LogType.Warning:
						level = LogLevel.Warning;
						break;
					case LogType.Error:
					case LogType.Assert:
					case LogType.Exception:
						level = LogLevel.Error;
						break;
					default:
						level = LogLevel.Debug;
						break;
				}

				LogManager.Instance.Log(
					new LogEntry(
						level,
						LogCategory.Core,
						condition,
						"Unity",
						stackTrace,
						Time.frameCount
						)
					);
			}
			finally
			{
				_isForwarding = false;
			}
		}
	}
}
