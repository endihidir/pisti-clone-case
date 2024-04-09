using System.Collections.Generic;
using System.Linq;
using UnityBase.Extensions;
using UnityBase.ManagerSO;
using UnityBase.Service;

public class CardManager : ICardService, IGameplayConstructorService
{
    public const int CARD_COUNT = 52;
    
    private readonly int _deckCount;
    private readonly CardDistrubitionManagerSO _cardDistrubitionManagerSo;
    private readonly CardBehaviourFactory _cardBehaviourFactory;
    private readonly IDictionary<int, ICardBehaviour> _cardBehaviours;
    private readonly ICardPoolService _cardPoolService;
    private readonly CardDefinitionSO[] _cardDefinitions;
    private readonly CardType[] _numberedCardTypes = { CardType.Club, CardType.Diamond, CardType.Heart, CardType.Spade };
    private Stack<int> _cardIndexes;

    public CardManager(GameplayDataHolderSO gameplayDataHolderSo, CardBehaviourFactory cardBehaviourFactory, ICardPoolService cardPoolService)
    {
        _cardDistrubitionManagerSo = gameplayDataHolderSo.cardDistrubitionManagerSo;
        _cardBehaviourFactory = cardBehaviourFactory;
        _cardPoolService = cardPoolService;
        
        _cardDefinitions = _cardDistrubitionManagerSo.cardDefinitions;
        _deckCount = _cardDistrubitionManagerSo.deckCount;

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

                _cardBehaviours[index].Initialize(cardNumber, cardDefinition.type, cardDefinition.sprite);

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

    public bool TryGetCardObject(out CardViewController cardViewController)
    {
        cardViewController = null;
        
        if (_cardIndexes.Count < 1) return false;
        
        var index = _cardIndexes.Pop();

        cardViewController = _cardPoolService.GetCardView<CardViewController>();
            
        if (_cardBehaviours.TryGetValue(index, out var cardBehaviour))
        {
            cardViewController.Initialize(cardBehaviour);
            
            cardViewController.FlipCard(FlipSide.Back);
        }

        return true;
    }

    public bool IsNumberedCard(CardType cardType) => _numberedCardTypes.Contains(cardType);
}