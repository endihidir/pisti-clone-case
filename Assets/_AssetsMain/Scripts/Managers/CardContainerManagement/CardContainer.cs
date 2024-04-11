using System.Collections.Generic;
using UnityBase.Extensions;
using UnityBase.ManagerSO;
using UnityBase.Service;

public class CardContainer : ICardContainer, IGameplayConstructorService
{
    private const int CARD_COUNT = 52;
    
    private readonly int _totalDeckCount;
    private readonly CardContainerSO _cardContainerSo;
    private readonly CardBehaviourFactory _cardBehaviourFactory;
    private readonly IDictionary<int, ICardBehaviour> _cardBehaviours;
    private readonly IDictionary<int, CardViewController> _cardViewControllers;
    private readonly ICardPoolService _cardPoolService;
    private readonly CardDefinitionSO[] _cardDefinitions;
    private Stack<int> _cardIndexes;

    public CardContainer(GameplayDataHolderSO gameplayDataHolderSo, CardBehaviourFactory cardBehaviourFactory, ICardPoolService cardPoolService)
    {
        _cardContainerSo = gameplayDataHolderSo.cardContainerSo;
        _cardBehaviourFactory = cardBehaviourFactory;
        _cardPoolService = cardPoolService;
        
        _cardDefinitions = _cardContainerSo.cardDefinitions;
        _totalDeckCount = _cardContainerSo.totalDeckCount;

        _cardViewControllers = new Dictionary<int, CardViewController>();
        _cardBehaviours = new Dictionary<int, ICardBehaviour>();
        _cardIndexes = new Stack<int>();
    }
    
    public void Initialize()
    {
        CashCardData();
        ShuffleCardIndexData();
    }

    public void Start() { }
    public void Dispose() { }

    private void CashCardData()
    {
        var index = 0;

        for (var i = 0; i < _cardDefinitions.Length; i++)
        {
            var cardDefinition = _cardDefinitions[i];
            
            var cardCount = cardDefinition.count * _totalDeckCount;

            for (int j = 0; j < cardCount; j++)
            {
                _cardBehaviours[index] = _cardBehaviourFactory.Create(cardDefinition);

                var cardNumber = _cardBehaviours[index] is NumberedCard ? (j + 1) : 0;

                _cardBehaviours[index].Initialize(index, cardNumber, cardDefinition.type);

                index++;
            }
        }
    }

    private void ShuffleCardIndexData()
    {
        _cardIndexes.Clear();
        
        for (int i = 0; i < _totalDeckCount * CARD_COUNT; i++) 
            _cardIndexes.Push(i);

        _cardIndexes = _cardIndexes.Shuffle();
    }

    public bool TryGetRandomCard(out ICardBehaviour cardBehaviour)
    {
        cardBehaviour = null;
        
        if (IsAllCardsFinished()) return false;

        var index = _cardIndexes.Pop();

        var cardViewController = _cardPoolService.GetCardView<CardViewController>();
            
        if (_cardBehaviours.TryGetValue(index, out cardBehaviour))
        {
            cardViewController.Initialize(cardBehaviour);
            
            cardViewController.FlipCard(CardFace.Back);

            _cardViewControllers[index] = cardViewController;
            
            return true;
        }

        return false;
    }

    public bool IsAllCardsFinished() => _cardIndexes.Count < 1;
}