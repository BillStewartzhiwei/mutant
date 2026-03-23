namespace Mutant.Core
{
	using System;

	public abstract class Singleton<T> where T : Singleton<T>, new()
	{
		private static T _instance;
		private static readonly object _lock = new();

		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_lock)
					{
						if (_instance == null)
						{
							_instance = new T();
							_instance.OnInit();
						}
					}
				}
				return _instance;
			}
		}

		/// <summary>
		/// 初始化（子类可重写）
		/// </summary>
		protected virtual void OnInit() { }

		/// <summary>
		/// 释放（可选）
		/// </summary>
		public virtual void Dispose()
		{
			_instance = null;
		}
	}
}