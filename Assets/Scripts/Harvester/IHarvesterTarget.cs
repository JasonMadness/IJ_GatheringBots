using UnityEngine;

public interface IHarvesterTarget
{
	Vector3 Position { get; }
    float ReachRadius { get; }
}