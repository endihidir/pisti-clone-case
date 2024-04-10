using UnityEngine;

namespace UnityBase.ManagerSO
{
    //[CreateAssetMenu(menuName = "Game/GameData/GameplayManagerDataHolder")]
    public class GameplayDataHolderSO : ScriptableObject
    {
        public CardContainerSO cardContainerSo;

        public CardPoolManagerSO cardPoolManagerSo;

        public GameplayStateMachineSO gameplayStateMachineSo;
        
        public void Initialize()
        {
            cardContainerSo.Initialize();
            
            cardPoolManagerSo.Initialize();
            
            gameplayStateMachineSo.Initialize();
        }
    }
}