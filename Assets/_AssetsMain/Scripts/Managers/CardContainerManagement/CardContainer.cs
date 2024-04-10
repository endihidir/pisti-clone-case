using System.Collections.Generic;
using System.Linq;
using UnityBase.Extensions;
using UnityBase.ManagerSO;
using UnityBase.Service;

public class CardContainer : ICardContainer, IGameplayConstructorService
{
    public const int CARD_COUNT = 52;
    
    private readonly int _deckCount;
    private readonly CardContainerSO _cardContainerSo;
    private readonly CardBehaviourFactory _cardBehaviourFactory;
    private readonly IDictionary<int, ICardBehaviour> _cardBehaviours;
    private readonly IDictionary<int, CardViewController> _cardViewControllers;
    private readonly ICardPoolService _cardPoolService;
    private readonly CardDefinitionSO[] _cardDefinitions;
    private readonly CardType[] _numberedCardTypes = { CardType.Club, CardType.Diamond, CardType.Heart, CardType.Spade };
    private Stack<int> _cardIndexes;

    public CardContainer(GameplayDataHolderSO gameplayDataHolderSo, CardBehaviourFactory cardBehaviourFactory, ICardPoolService cardPoolService)
    {
        _cardContainerSo = gameplayDataHolderSo.cardContainerSo;
        _cardBehaviourFactory = cardBehaviourFactory;
        _cardPoolService = cardPoolService;
        
        _cardDefinitions = _cardContainerSo.cardDefinitions;
        _deckCount = _cardContainerSo.deckCount;

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

        foreach (var cardDefinition in _cardDefinitions)
        {
            var cardCount = cardDefinition.count * _deckCount;
            
            for (int j = 0; j < cardCount; j++)
            {
                _cardBehaviours[index] = _cardBehaviourFactory.Create(cardDefinition);

                var cardNumber = IsNumberedCard(cardDefinition.type) ? (j + 1) : 0;

                _cardBehaviours[index].Initialize(index, cardNumber, cardDefinition.type, cardDefinition.sprite);

                index++;
            }
        }
    }

    private void ShuffleCardIndexData()
    {
        _cardIndexes.Clear();
        
        for (int i = 0; i < _deckCount * CARD_COUNT; i++) 
            _cardIndexes.Push(i);

        _cardIndexes = _cardIndexes.Shuffle();
    }

    public bool TryGetRandomCard(out CardViewController cardViewController)
    {
        cardViewController = null;
        
        if (_cardIndexes.Count < 1) return false;
        
        var index = _cardIndexes.Pop();

        cardViewController = _cardPoolService.GetCardView<CardViewController>();
            
        if (_cardBehaviours.TryGetValue(index, out var cardBehaviour))
        {
            cardViewController.Initialize(cardBehaviour);
            
            cardViewController.FlipCard(CardFace.Back);

            _cardViewControllers[index] = cardViewController;
        }

        return true;
    }

    public bool TryGetCardBy(int index, out CardViewController cardViewController)
    {
        return _cardViewControllers.TryGetValue(index, out cardViewController);
    }

    public bool IsNumberedCard(CardType cardType) => _numberedCardTypes.Contains(cardType);
}