using UnityEngine;

public class OpponentDeckView : DeckView
{
    [SerializeField] private int _id;

    public int ID => _id;
}