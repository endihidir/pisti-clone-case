using UnityEngine;

namespace UnityBase.ManagerSO
{
    //[CreateAssetMenu(menuName = "Game/GameData/GameplayManagerDataHolder")]
    public class GameplayDataHolderSO : ScriptableObject
    {
        public CardDistrubitionManagerSO cardDistrubitionManagerSo;

        public CardPoolManagerSO cardPoolManagerSo;
        public void Initialize()
        {
            cardDistrubitionManagerSo.Initialize();
            
            cardPoolManagerSo.Initialize();
        }
    }
}