using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour, IHarvesterTarget
{
    [SerializeField] private List<Harvester> _harvesters;
    [SerializeField] private float _reachRadius = 20f;
    [SerializeField] private float _unloadTime = 2f;

    private readonly CrystalScanner _scanner = new();
    private readonly CrystalStorage _storage = new();

    public float ReachRadius => _reachRadius;
    public float UnloadTime => _unloadTime;
    public Vector3 Position => transform.position;

    private void Start()
    {
        foreach (var harvester in _harvesters)
        {
            harvester.SetHomeBase(this);
            harvester.CrystalUnloaded += OnCrystalUnloaded;
        }

        InvokeRepeating(nameof(Scan), 2f, 2f);
    }

    private void OnDisable()
    {
        foreach (var harvester in _harvesters)
        {
            harvester.CrystalUnloaded -= OnCrystalUnloaded;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _reachRadius);
    }

    public void Scan()
    {
        _scanner.Refresh();

        if (_scanner.FreeCount > 0)
            SendHarvester();
    }

    public void SendHarvester()
    {
        foreach (var harvester in _harvesters)
        {
            if (harvester.IsBusy)
                continue;

            if (_scanner.TryGetFree(out Crystal crystal) == false)
                break;

            harvester.Send(crystal);
        }
    }

    public void OnCrystalCollected(Crystal crystal)
    {
        _scanner.RemoveFromBase(crystal);
    }

    public void OnCrystalUnloaded(Harvester harvester)
    {
        _storage.AddCrystal();
    }
}
