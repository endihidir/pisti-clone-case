using UnityEngine;

[CreateAssetMenu(menuName = "Game/Pisti/CardContainerData")]
public class CardContainerSO : ScriptableObject
{
    public int totalDeckCount = 1;
    
    public CardDefinitionSO[] cardDefinitions;

    public void Initialize()
    {
        
    }
}