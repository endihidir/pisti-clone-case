using UnityEngine;

[CreateAssetMenu(menuName = "Game/Pisti/CardContainerData")]
public class CardContainerSO : ScriptableObject
{
    public int deckCount = 1;
    
    public CardDefinitionSO[] cardDefinitions;

    public void Initialize()
    {
        
    }
}