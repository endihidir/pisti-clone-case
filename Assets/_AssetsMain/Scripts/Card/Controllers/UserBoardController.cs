public class UserBoardController : IUserBoard
{
    private int _userID;
    private readonly IUserDeck _userDeck;
    private readonly ICollectedCards _collectedCards;
    public int UserID => _userID;
    public IUserDeck UserDeck => _userDeck;
    public ICollectedCards CollectedCards => _collectedCards;

    public UserBoardController(int userID, IUserDeck userDeck, ICollectedCards collectedCards)
    {
        _userID = userID;
        _userDeck = userDeck;
        _collectedCards = collectedCards;
    }
    
    public void Reset()
    {
        _userID = 0;
        _userDeck.Reset();
        _collectedCards.Reset();
    }
}