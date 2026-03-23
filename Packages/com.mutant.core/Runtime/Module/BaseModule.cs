namespace Bill.Mutant.Core
{
    public abstract class BaseModule : IModule
    {
        public bool IsInitialized { get; private set; }

        public void Init()
        {
            if (IsInitialized) return;
            OnInit();
            IsInitialized = true;
        }

        public void Update(float deltaTime)
        {
            if (!IsInitialized) return;
            OnUpdate(deltaTime);
        }

        public void Dispose()
        {
            if (!IsInitialized) return;
            OnDispose();
            IsInitialized = false;
        }

        protected virtual void OnInit() { }
        protected virtual void OnUpdate(float deltaTime) { }
        protected virtual void OnDispose() { }
    }
}
