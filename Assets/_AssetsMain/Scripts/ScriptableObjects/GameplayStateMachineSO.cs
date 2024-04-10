using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Pisti/GameplayStateMachineData")]
public class GameplayStateMachineSO : ScriptableObject
{
    public DeckView[] deckViews;

    public void Initialize()
    {
        deckViews = FindObjectsOfType<DeckView>();
    }

    public T GetDeckView<T>() where T : DeckView => deckViews.FirstOrDefault(deckView => deckView is T) as T;

    public List<T> GetDeckViews<T>() where T : DeckView
    {
        var views = new List<T>();
        
        var array = deckViews.Where(deckView => deckView is T);
        
        foreach (var deck in array)
        {
            views.Add(deck as T);
        }
        
        return views;
    }

    public OpponentDeckView GetOpponentDeckViewBy(int id) => GetDeckViews<OpponentDeckView>().FirstOrDefault(deckView => deckView.ID == id);

    public int GetOpponentCount() => deckViews.Count(deckView => deckView is OpponentDeckView);

}