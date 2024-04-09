using UnityBase.Command;
using UnityBase.Manager;
using UnityBase.Presenter;
using UnityBase.ManagerSO;
using UnityBase.SceneManagement;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UnityBase.BaseLifetimeScope
{
    public class AppLifetimeScope : LifetimeScope
    {
        [SerializeField] private AppDataHolderSO _appDataHolderSo;

        protected override void Configure(IContainerBuilder builder)
        {
            _appDataHolderSo.Initialize();
            
            builder.RegisterInstance(_appDataHolderSo);

            RegisterEntryPoints(builder);

            RegisterSingletonServices(builder);

            RegisterScopedServices(builder);
            
            RegisterTransientServices(builder);
        }
        
        private void RegisterEntryPoints(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<AppManagerPresenter>();
        }
        
        private void RegisterSingletonServices(IContainerBuilder builder)
        {
            builder.Register<GameManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneGroupManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LevelManager>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.Register<PoolManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PopUpManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TutorialManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TutorialMaskManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TutorialStepManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CinemachineManager>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<CommandManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CurrencyManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<TaskManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SwipeInputManager>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterScopedServices(IContainerBuilder builder)
        {
            builder.Register<JsonDataManager>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.Register<CommandRecorder>(Lifetime.Transient).AsImplementedInterfaces();
        }
        
        private void RegisterTransientServices(IContainerBuilder builder)
        {
            
        }
    }   
}