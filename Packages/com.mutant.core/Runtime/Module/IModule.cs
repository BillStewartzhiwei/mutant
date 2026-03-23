namespace Bill.Mutant.Core
{
    public interface IModule
    {
        bool IsInitialized { get; }

        void Init();
        void Update(float deltaTime);
        void Dispose();
    }
}
