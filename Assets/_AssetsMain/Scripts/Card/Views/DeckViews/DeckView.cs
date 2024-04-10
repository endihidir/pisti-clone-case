using UnityEngine;

public abstract class DeckView : MonoBehaviour
{
    [SerializeField] private Transform[] _slots;

    [SerializeField] private float _slotOffset;

    public Transform[] Slots => _slots;

    private void Awake()
    {
        foreach (var slot in _slots)
        {
            slot.localPosition += slot.up * _slotOffset;
        }
    }
}
