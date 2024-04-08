using UnityEngine;

[CreateAssetMenu(fileName = "CardDefinition", menuName = "Game/Pisti/CardDefinition")]
public class CardDefinitionSO : ScriptableObject
{
    public int count = 1;
    
    public CardType type = CardType.None;

    public Sprite cardSprite;
}