using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour, IHarvesterTarget
{
    [SerializeField] private HarvesterDispatcher _dispatcher;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private GameObject _flag;
    [SerializeField] private float _reachRadius = 20f;
    [SerializeField] private float _unloadTime = 2f;

    private CrystalScanner _scanner;
    private HarvesterFactory _harvesterFactory;
    private readonly CrystalStorage _storage = new();

    private int _harvesterCost;

    public float ReachRadius => _reachRadius;
    public float UnloadTime => _unloadTime;
    public Vector3 Position => transform.position;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _reachRadius);
    }

    public void Initialize(CrystalScanner scanner, HarvesterFactory harvesterFactory, int startingCrystalAmount)
    {
        _scanner = scanner;
        _dispatcher.Initialize(_scanner);
        _harvesterFactory = harvesterFactory;
        _harvesterCost = _harvesterFactory.HarvesterCost;

        for (int i = 0; i < startingCrystalAmount; i++)
            _storage.AddCrystal();

        while (_storage.Crystals >= _harvesterCost)
            TryBuildHarvester();
    }

    public void OnCrystalCollected(Crystal crystal)
    {
        _scanner.RemoveFromBase(crystal);
    }

    public void SetFlag(Vector3 position)
    {
        _flag.transform.position = position;
        _flag.SetActive(true);
    }

    public void ClearFlag()
    {
        _flag.SetActive(false);
    }

    public void OnCrystalUnloaded(Harvester harvester)
    {
        _storage.AddCrystal();
        TryBuildHarvester();
    }

    private void TryBuildHarvester()
    {
        if (_storage.TrySpend(_harvesterCost) == false)
            return;

        Harvester newHarvester = _harvesterFactory.Create(_spawnPoint);
        RegisterHarvester(newHarvester);
    }

    private void RegisterHarvester(Harvester harvester)
    {
        _dispatcher.RegisterHarvester(harvester);
        harvester.SetHomeBase(this);
        harvester.transform.SetParent(transform);
        harvester.CrystalUnloaded += OnCrystalUnloaded;
    }
}
