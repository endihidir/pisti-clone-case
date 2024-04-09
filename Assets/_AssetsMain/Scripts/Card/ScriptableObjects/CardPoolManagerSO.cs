using UnityEngine;

[CreateAssetMenu(menuName = "Game/ManagerData/CardPoolManagerData")]
public class CardPoolManagerSO : ScriptableObject
{
    public Transform cardOnScreenParent;
    public void Initialize()
    {
        cardOnScreenParent = FindObjectOfType<Tag_CardDistributionArea>()?.transform;
    }
}