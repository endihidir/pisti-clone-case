using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Pisti/GameplayStateMachineData")]
public class GameplayStateMachineSO : ScriptableObject
{
    public BoardView[] deckViews;

    public void Initialize()
    {
        deckViews = FindObjectsOfType<BoardView>();
    }

    public int GetOpponentCount() => deckViews.Count(deckView => deckView is OpponentBoardView);
    public T GetDeckView<T>() where T : BoardView => deckViews.FirstOrDefault(deckView => deckView is T) as T;
    public T[] GetDeckViews<T>() where T : BoardView => deckViews.OfType<T>().ToArray();
    public OpponentBoardView GetOpponentDeckViewBy(int id) => GetDeckViews<OpponentBoardView>().FirstOrDefault(deckView => deckView.ID == id);
}