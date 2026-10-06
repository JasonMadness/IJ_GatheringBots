using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour, IHarvesterTarget
{
    [SerializeField] private List<Harvester> _harvesters;
    [SerializeField] private float _reachRadius = 20f;
    [SerializeField] private float _unloadTime = 2f;

    private List<Crystal> _freeCrystals = new();
    private List<Crystal> _busyCrystals = new(); 

    public float ReachRadius => _reachRadius;
    public float UnloadTime => _unloadTime;
    public Vector3 Position => transform.position;

    private void Start()
    {
        foreach (var harvester in _harvesters)
        {
            harvester.SetHomeBase(this);
        }

        InvokeRepeating(nameof(Scan), 2f, 2f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _reachRadius);
    }

    public void Scan()
    {
        List<Crystal> crystals = new List<Crystal>(FindObjectsOfType<Crystal>());

        foreach (var crystal in crystals)
        {
            if (_busyCrystals.Contains(crystal) == false && _freeCrystals.Contains(crystal) == false)
                _freeCrystals.Add(crystal);
        }

        if (_freeCrystals.Count > 0)
        {
            SendHarvester();
        }
    }

    public void SendHarvester()
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
