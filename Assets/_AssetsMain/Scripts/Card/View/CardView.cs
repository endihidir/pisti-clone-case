using UnityEngine;
using UnityEngine.UI;

public abstract class CardView : MonoBehaviour
{
    [SerializeField] private Image _cardImage;

    public void SetCardSprite(Sprite sprite)
    {
        _cardImage.sprite = sprite;
    }
}