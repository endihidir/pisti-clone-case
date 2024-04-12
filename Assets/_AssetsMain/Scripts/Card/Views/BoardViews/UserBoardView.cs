using UnityEngine;

public class UserBoardView : BoardView
{
    [SerializeField] private Transform _cardCollectingArea;
    
    [SerializeField] private float _slotOffset;
    public Transform CardCollectingArea => _cardCollectingArea;
    
    private void Awake()
    {
        foreach (var slot in Slots)
        {
            slot.localPosition += slot.up * _slotOffset;
        }
    }
}