
using UnityEngine;

public class GameRoundViewHandler
{
    private readonly GameRoundView _gameRoundView;
    
    private readonly ICardContainer _cardContainer;
    
    private readonly int _opponentCount;

    private readonly int _totalGameRound;

    private int _roundCounter = 0;
    
    public GameRoundViewHandler(GameRoundView gameRoundView, ICardContainer cardContainer, int opponentCount)
    {
        _gameRoundView = gameRoundView;
        _cardContainer = cardContainer;
        _opponentCount = opponentCount;
        _totalGameRound = CalculateTotalRound();
        UpdateView();
    }

    public void UpdateView()
    {
        _roundCounter++;
        _roundCounter = Mathf.Clamp(_roundCounter, 1, _totalGameRound);
        _gameRoundView.SetGameRoundTxt(_roundCounter, _totalGameRound);
    }

    private int CalculateTotalRound()
    {
        var remainingRound = _cardContainer.TotalCardCount - (((_opponentCount + 1) * CardConstants.DEALING_COUNT) + CardConstants.DEALING_COUNT);
        var divider = CardConstants.DEALING_COUNT * (_opponentCount + 1);
        return (remainingRound / divider) + 1;
    }

    public void Reset() => _roundCounter = 0;
}