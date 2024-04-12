using UnityEngine;

public class CardInputDetectionProvider : ICardInputDetectionService
{
    private readonly ICardInputDetector _cardInputDetector;
    private readonly CardView _selectedCardView;
    public CardInputDetectionProvider(ICardInputDetector cardInputDetector)
    {
        _cardInputDetector = cardInputDetector;
        _selectedCardView = _cardInputDetector.SelectedCardView;
    }
    
    public bool IsInRange(Vector2 worldPos)
    {
        var localMousePos = _selectedCardView.RectTransform.InverseTransformPoint(worldPos);
        return _selectedCardView.RectTransform.rect.Contains(localMousePos);
    }
}