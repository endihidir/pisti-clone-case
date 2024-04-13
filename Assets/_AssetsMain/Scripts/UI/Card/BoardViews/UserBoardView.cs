using TMPro;
using UnityEngine;

public class UserBoardView : BoardView
{
    [SerializeField] private Transform _cardCollectingArea;
    
    [SerializeField] private float _slotOffset;

    [SerializeField] private TextMeshProUGUI _scoreTxt;
    
    [SerializeField] private TextMeshProUGUI _collectedCardCountTxt;

    public Transform CardCollectingArea => _cardCollectingArea;
    
    private void Awake()
    {
        foreach (var slot in Slots)
        {
            slot.localPosition += slot.up * _slotOffset;
        }
        
        _collectedCardCountTxt.gameObject.SetActive(false);
    }

    public void SetCollectedCardCount(int count)
    {
        if(!_collectedCardCountTxt.gameObject.activeSelf)
            _collectedCardCountTxt.gameObject.SetActive(true);

        _collectedCardCountTxt.text = count.ToString();
    }

    public void SetScore(int score)
    {
        _scoreTxt.text = "Score : " + score;
    }
}