using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Pisti/GameplayStateMachineData")]
public class GameplayStateMachineSO : ScriptableObject
{
    public BoardView[] boardViews;
    public GameRoundView gameRoundView;

    public void Initialize()
    {
        boardViews = FindObjectsOfType<BoardView>();
        gameRoundView = FindObjectOfType<GameRoundView>();
    }

    public int GetOpponentCount() => boardViews.Count(boardView => boardView is OpponentBoardView);
    public T GetBoardView<T>() where T : BoardView => boardViews.FirstOrDefault(boardView => boardView is T) as T;
    public T[] GetBoardViews<T>() where T : BoardView => boardViews.OfType<T>().ToArray();
    public OpponentBoardView GetOpponentBoardViewBy(int id) => GetBoardViews<OpponentBoardView>().FirstOrDefault(opponentBoardView => opponentBoardView.ID == id);
}