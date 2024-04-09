using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using VContainer;

public class CardTest : MonoBehaviour
{
    [Inject]
    private readonly ICardService _cardService;

    private CardViewController _cardViewController;
    
    [Button]
    public void SpawnCardRandomPos()
    {
        var canGet = _cardService.TryGetCardObject(out var cardViewController);

        if (canGet)
        {
            _cardViewController = cardViewController;
            /*var randomXPos = Random.Range(0, Screen.width);
            var randomYPos = Random.Range(0, Screen.height);
            var pos = new Vector3(randomXPos, randomYPos, 0f);
            cardViewController.transform.position = pos.SelectSimulationSpace(PositionSpace.ScreenSpace);*/
        }
    }

    [Button]
    public void FlipCard(FlipSide flipSide)
    {
        _cardViewController.CardBehaviour.CardAnimationService.Flip(flipSide, 0.2f, Ease.InOutQuad, default);
    }
}
