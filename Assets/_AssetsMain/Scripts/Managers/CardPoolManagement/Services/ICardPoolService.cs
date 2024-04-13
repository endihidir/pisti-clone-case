using System;

public interface ICardPoolService
{
    public T GetCardView<T>(bool show = true, float duration = 0f, float delay = 0f) where T : CardViewController;
    public void HideCardView(CardViewController cardViewController, float duration = 0f, float delay = 0f, Action onComplete = default, bool readLogs = false);
    public void HideAllCardViewOfType<T>(float duration = 0f, float delay = 0f, Action onComplete = default) where T : CardViewController;
    public void HideAllCardView(float duration = 0f, float delay = 0f);
    public void RemoveCardViewPool<T>(bool readLogs = false) where T : CardViewController;
}