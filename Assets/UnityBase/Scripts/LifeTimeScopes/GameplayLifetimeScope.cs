using UnityBase.Manager;
using UnityBase.ManagerSO;
using UnityBase.Presenter;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace UnityBase.BaseLifetimeScope
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        [SerializeField] private GameplayDataHolderSO _gameplayDataHolderSo;
        
        protected override void Configure(IContainerBuilder builder)
        {
            _gameplayDataHolderSo.Initialize();

            builder.RegisterInstance(_gameplayDataHolderSo);
            
            RegisterEntryPoints(builder);
            
            RegisterSingletonServices(builder);

            RegisterScopedServices(builder);
            
            RegisterTransientServices(builder);
            
            RegisterComponents(builder);
        }
        
        private void RegisterEntryPoints(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameplayManagerPresenter>();
        }
        
        private void RegisterSingletonServices(IContainerBuilder builder)
        {
            builder.Register<GameplayManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CardManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CardPoolManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CardAnimationProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<PlayerDeckController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<OpponentController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CardBehaviourFactory>(Lifetime.Singleton);
        }
        
        private void RegisterScopedServices(IContainerBuilder builder)
        {
            
        }
        
        private void RegisterTransientServices(IContainerBuilder builder)
        {
           
        }

        private void RegisterComponents(IContainerBuilder builder)
        {
           
        }
    }
}