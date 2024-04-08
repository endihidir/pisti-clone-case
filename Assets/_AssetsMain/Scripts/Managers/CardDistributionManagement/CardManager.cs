using System.Collections.Generic;
using System.Linq;
using UnityBase.Extensions;
using UnityBase.ManagerSO;
using UnityBase.Service;

public class CardManager : ICardDataService, IGameplayConstructorService
{
    public const int CARD_COUNT = 52;
    
    private readonly int _deckCount;
    private readonly CardDistrubitionManagerSO _cardDistrubitionManagerSo;
    private readonly CardBehaviourFactory _cardBehaviourFactory;
    private readonly IDictionary<int, ICardBehaviour> _cardBehaviours;
    private readonly ICardPoolDataService _cardPoolDataService;
    private readonly CardDefinitionSO[] _cardDefinitions;
    
    private HashSet<int> _cardIndexes; // Maybe you can use Stack

    public CardManager(GameplayDataHolderSO gameplayDataHolderSo, CardBehaviourFactory cardBehaviourFactory, ICardPoolDataService cardPoolDataService)
    {
        _cardDistrubitionManagerSo = gameplayDataHolderSo.cardDistrubitionManagerSo;
        _cardBehaviourFactory = cardBehaviourFactory;
        _cardPoolDataService = cardPoolDataService;
        
        _cardDefinitions = _cardDistrubitionManagerSo.cardDefinitions;
        _deckCount = _cardDistrubitionManagerSo.deckCount;

        _cardBehaviours = new Dictionary<int, ICardBehaviour>();
        _cardIndexes = new HashSet<int>(_deckCount * CARD_COUNT);
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

                // NOTE: If you seperate NumberedCardBehaviour and SpecialCardBehaviour you could shoten below statement think about it!!!
                
                var cardNumber = 0;
                
                if (cardDefinition.type is CardType.Club or CardType.Diamond or CardType.Heart or CardType.Spade)
                {
                    cardNumber = j + 1;
                }

                _cardBehaviours[index].Initialize(cardNumber, cardDefinition.type, cardDefinition.cardSprite);

                index++;
            }
        }
    }

    private void ShuffleCardIndexData()
    {
        _cardIndexes.Clear();
        
        for (int i = 0; i < _cardIndexes.Count; i++) 
            _cardIndexes.Add(i);

        _cardIndexes = _cardIndexes.Shuffle().ToHashSet();
    }

    public bool TryGetCardObject(out CardViewController cardViewController)
    {
        cardViewController = null;
        
        if (_cardIndexes.Count < 1) return false;
        
        var index = _cardIndexes.FirstOrDefault();

        cardViewController = _cardPoolDataService.GetCardView<CardViewController>();
            
        if (_cardBehaviours.TryGetValue(index, out var cardBehaviour))
        {
            cardViewController.Initialize(cardBehaviour);
        }
            
        _cardIndexes.Remove(0);

        return true;
    }
}