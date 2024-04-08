public interface ICardDeck
{
   public void SelectCard(ICardBehaviour cardBehaviour);
   public void DropCardTo(ICenterCardDeck visitor);
}