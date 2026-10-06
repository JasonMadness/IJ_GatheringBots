using UnityEngine;

public class Crystal : MonoBehaviour
{
    [SerializeField] private float _collectRadius = 4f;

    public float CollectRadius => _collectRadius;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _collectRadius);
    }
}
