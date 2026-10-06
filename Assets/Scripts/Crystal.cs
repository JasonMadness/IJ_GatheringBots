using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private float _collectRadius;

    public float CollectRadius => _collectRadius;
}
