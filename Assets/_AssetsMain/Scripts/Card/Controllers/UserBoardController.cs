public class UserBoardController : IUserBoard
{
    private readonly int _userID;
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
}