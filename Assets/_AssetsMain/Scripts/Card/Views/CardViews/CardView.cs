using UnityEngine;
using UnityEngine.UI;

public abstract class CardView : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image _cardImage;

    public RectTransform RectTransform => rectTransform;
    public void SetCardSprite(Sprite sprite) => _cardImage.sprite = sprite;
    public void SetActive(bool enable) => gameObject.SetActive(enable);
}