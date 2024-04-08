namespace UnityBase.Service
{
    public interface IAppConstructorDataService
    {
        public void Initialize();
        public void Start();
        public void Dispose();
    }
}