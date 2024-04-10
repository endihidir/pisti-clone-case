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
            slot.up += Vector3.up * _slotOffset;
        }
    }
}
