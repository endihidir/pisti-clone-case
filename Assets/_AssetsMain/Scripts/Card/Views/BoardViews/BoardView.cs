using UnityEngine;

public abstract class BoardView : MonoBehaviour
{
    [SerializeField] private Transform[] _slots;

    public Transform[] Slots => _slots;
}
