
public interface IUserBoard
{
   public int UserID { get; }
   public IUserDeck UserDeck { get; }
   public ICollectedCards CollectedCards { get; }
   public void Reset();
}