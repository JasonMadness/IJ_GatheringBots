using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, IHarvesterTarget
{
    [SerializeField] private List<Harvester> _harvesters;
    [SerializeField] private float _reachRadius = 10f;
    private List<Crystal> _freeCrystals = new();
    private List<Crystal> _busyCrystals = new();

    public float ReachRadius => _reachRadius;
    public Vector3 Position => transform.position;

    public void Scan()
    {
        List<Crystal> crystals = new List<Crystal>(FindObjectsOfType<Crystal>());

        foreach (var crystal in crystals)
        {
            if (_busyCrystals.Contains(crystal) == false)
                _freeCrystals.Add(crystal);
        }
    }

    public void SentHarvester()
    {
        foreach (var harvester in _harvesters)
        {
            if (harvester.IsBusy == false && _freeCrystals.Count > 0)
            {
                Crystal targetCrystal = _freeCrystals[0];
                harvester.Send(targetCrystal);
                OnHarvesterSent(targetCrystal);
            }
        }
    }

    public void OnHarvesterSent(Crystal crystal)
    {
        _freeCrystals.Remove(crystal);
        _busyCrystals.Add(crystal);
    }

    public void OnCrystalCollected(Crystal crystal)
    {
        _busyCrystals.Remove(crystal);
    }
}
