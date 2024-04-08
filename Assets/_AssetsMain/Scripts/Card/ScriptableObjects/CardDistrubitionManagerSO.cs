using UnityEngine;

[CreateAssetMenu(menuName = "Game/ManagerData/CardDistributionManagerData")]
public class CardDistrubitionManagerSO : ScriptableObject
{
    public int deckCount = 1;
    
    public CardDefinitionSO[] cardDefinitions;

    public void Initialize()
    {
        
    }
}