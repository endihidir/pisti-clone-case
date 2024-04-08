namespace UnityBase.Service
{
    public interface IGameplayConstructorService
    {
        public void Initialize();
        public void Start();
        public void Dispose();
    }
}