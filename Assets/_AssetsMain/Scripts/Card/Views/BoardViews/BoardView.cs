using UnityEngine;

public abstract class BoardView : MonoBehaviour
{
    [SerializeField] private Transform[] _slots;

    [SerializeField] private Transform _collectedCards;

    [SerializeField] private float _slotOffset;

    public Transform[] Slots => _slots;

    public Transform CollectedCards => _collectedCards;

    private void Awake()
    {
        foreach (var slot in _slots)
        {
            slot.localPosition += slot.up * _slotOffset;
        }
    }
}
