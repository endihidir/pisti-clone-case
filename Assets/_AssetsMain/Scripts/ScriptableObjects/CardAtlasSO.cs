using UnityEngine;
using UnityEngine.U2D;

[CreateAssetMenu(fileName = "CardAtlas", menuName = "Game/Pisti/CardAtlas")]
public class CardAtlasSO : ScriptableObject
{
    public SpriteAtlas cardAtlas;
    public Sprite GetSprite(string spriteName) => cardAtlas.GetSprite(spriteName.ToLower());
    public Sprite GetSprite(CardType cardType) => cardAtlas.GetSprite(cardType.ToString().ToLower());
}