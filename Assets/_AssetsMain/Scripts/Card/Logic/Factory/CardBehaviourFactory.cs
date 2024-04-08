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
            
            CardType.JackClub or CardType.JackDiamond or CardType.JackHeart or CardType.JackSpade => new JackCard(),
            CardType.KingClub or CardType.KingDiamond or CardType.KingHeart or CardType.KingSpade => new KingCard(),
            CardType.QueenClub or CardType.QueenDiamond or CardType.QueenHeart or CardType.QueenSpade => new QueenCard(),
            
            _ => null
        };
    }
}