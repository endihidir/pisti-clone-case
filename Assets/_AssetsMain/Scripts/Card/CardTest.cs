using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityBase.Extensions;
using UnityEngine;
using VContainer;

public class CardTest : MonoBehaviour
{
    [Inject]
    private readonly ICardContainer _cardContainer;

    private Stack<CardViewController> _cardViewController = new Stack<CardViewController>();

    private CardFace _cardFace = CardFace.Back;
    
    [Button]
    public void SpawnCardRandomPos()
    {
        var canGet = _cardContainer.TryGetRandomCard(out var cardViewController);

        if (canGet)
        {
            _cardViewController.Push(cardViewController);
        }
    }

    [Button]
    public void MoveRandomPos()
    {
        if (_cardFace == CardFace.Back)
        {
            _cardFace = CardFace.Front;
        }
        else
        {
            _cardFace = CardFace.Back;
        }
        
        var randomXPos = Random.Range(0, Screen.width);
        var randomYPos = Random.Range(0, Screen.height);
        var pos = new Vector3(randomXPos, randomYPos, 0f).SelectSimulationSpace(PositionSpace.ScreenSpace);

        var count = _cardViewController.Count;
        
        for (int i = 0; i < count; i++)
        {
            var cardViewController = _cardViewController.Pop();

            cardViewController.CardBehaviour.CardAnimationService.Move(pos, 0.5f, Ease.InOutQuad, i * 0.1f);

            cardViewController.CardBehaviour.CardAnimationService.Flip(_cardFace, 0.2f, Ease.InOutQuad, i * 0.1f);
        }
    }
}
