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
            builder.Register<GameplayStateMachine>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CardContainer>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CardPoolManager>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<DiscardPileController>(Lifetime.Singleton).AsImplementedInterfaces();
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