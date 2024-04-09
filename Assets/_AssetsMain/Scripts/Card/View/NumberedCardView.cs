using TMPro;
using UnityEngine;

public class NumberedCardView : CardView
{
    [SerializeField] private TextMeshProUGUI _numberTxt;

#if UNITY_EDITOR
    private void OnValidate()
    {
        _numberTxt = GetComponentInChildren<TextMeshProUGUI>(true);
    }
#endif

    public NumberedCardView SetNumber(int number)
    {
        _numberTxt.text = number.ToString("0");
        return this;
    }

    public void EnableText(bool enable) => _numberTxt.gameObject.SetActive(enable);
}