using UnityEngine;

public class Crystal : MonoBehaviour, IHarvesterTarget
{
    [SerializeField] private float _collectRadius = 4f;

    public Vector3 Position => transform.position;
    public float ReachRadius => _collectRadius;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _collectRadius);
    }
}
