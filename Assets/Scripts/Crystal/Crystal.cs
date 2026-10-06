using System;
using UnityEngine;

public class Crystal : MonoBehaviour, IHarvesterTarget
{
    [SerializeField] private float _collectRadius = 4f;
    [SerializeField] private float _collectTime = 3f;

    public event Action<Crystal> Collected;

    public Vector3 Position => transform.position;
    public float ReachRadius => _collectRadius;
    public float CollectTime => _collectTime;

    public void NotifyCollected(Crystal crystal)
    {
        Collected?.Invoke(crystal);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _collectRadius);
    }
}