using UnityEngine;

public class Flag : MonoBehaviour, IHarvesterTarget
{
    public Vector3 Position => transform.position;

    public float ReachRadius => 0.02f;
}
