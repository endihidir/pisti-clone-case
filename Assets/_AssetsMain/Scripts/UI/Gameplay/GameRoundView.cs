using TMPro;
using UnityEngine;

public class GameRoundView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _gameRoundTxt;

    public void SetGameRoundTxt(int currentRound, int totalRound)
    {
        _gameRoundTxt.text = "Round " + currentRound + "/" + totalRound;
    }
}