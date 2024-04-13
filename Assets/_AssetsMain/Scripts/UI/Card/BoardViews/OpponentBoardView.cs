using UnityEngine;

public class OpponentBoardView : UserBoardView
{
    [SerializeField] private int _id;

    public int ID => _id;
}