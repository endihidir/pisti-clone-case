using UnityEngine;

public class OpponentBoardView : BoardView
{
    [SerializeField] private int _id;

    public int ID => _id;
}