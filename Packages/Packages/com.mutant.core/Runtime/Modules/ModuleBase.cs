namespace Mutant.Core.Modules
{
	public abstract class ModuleBase : IModule
	{
		public virtual string Name => GetType().Name;
		public virtual int Priority => 0;
		public bool IsInitialized { get; private set; }

		public void Init()
		{
			if (IsInitialized)
				return;

			OnInit();
			IsInitialized = true;
		}

		public void Dispose()
		{
			if (!IsInitialized)
				return;

			OnDispose();
			IsInitialized = false;
		}

		public virtual void Update() { }
		public virtual void LateUpdate() { }
		public virtual void FixedUpdate() { }

		protected virtual void OnInit() { }
		protected virtual void OnDispose() { }
	}
}
