public class CardBehaviourFactory
{
    public ICardBehaviour Create(CardDefinitionSO cardDefinition)
    {
        return cardDefinition.type switch
        {
            CardType.Club => new ClubCard(),
            CardType.Diamond => new DiamondCard(),
            CardType.Heart => new HeartCard(),
            CardType.Spade => new SpadeCard(),
            
            CardType.Jack_Club or CardType.Jack_Diamond or CardType.Jack_Heart or CardType.Jack_Spade => new JackCard(),
            CardType.King_Club or CardType.King_Diamond or CardType.King_Heart or CardType.King_Spade => new KingCard(),
            CardType.Queen_Club or CardType.Queen_Diamond or CardType.Queen_Heart or CardType.Queen_Spade => new QueenCard(),
            
            _ => null
        };
    }
}