public interface IBaseCardDeck
{
   public void SelectCard(ICardBehaviour cardBehaviour);
   public void DropCardTo(ICenterCardDeck visitor);
}