using System;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour, IHarvesterTarget
{
    [SerializeField] private HarvesterDispatcher _dispatcher;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Flag _flag;
    [SerializeField] private float _reachRadius = 20f;
    [SerializeField] private float _unloadTime = 2f;

    private CrystalScanner _scanner;
    private HarvesterFactory _harvesterFactory;
    private readonly CrystalStorage _storage = new();

    private int _harvesterCost;
    private int _baseCost;
    private bool _needToBuildBase;
    private Harvester _baseBuilderHarvester;

    public event Action<HomeBase, Vector3> FlagReached;

    public float ReachRadius => _reachRadius;
    public float UnloadTime => _unloadTime;
    public Vector3 Position => transform.position;

    public void Initialize(CrystalScanner scanner, HarvesterFactory harvesterFactory, int startingCrystalAmount, int baseCost)
    {
        _scanner = scanner;
        _dispatcher.Initialize(_scanner);
        _harvesterFactory = harvesterFactory;
        _harvesterCost = _harvesterFactory.HarvesterCost;
        _baseCost = baseCost;

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
        if (_baseBuilderHarvester != null)
            return;

        _flag.transform.position = position;
        _flag.gameObject.SetActive(true);
        _needToBuildBase = true;
        UpdateTarget();
    }

    public void ClearFlag()
    {
        _flag.gameObject.SetActive(false);
        _needToBuildBase = false;
        UpdateTarget();
    }

    public void OnCrystalUnloaded(Harvester harvester)
    {
        _storage.AddCrystal();
        UpdateTarget();
    }

    public void OnHarvesterReachedFlag(Harvester harvester)
    {
        FlagReached?.Invoke(this, _flag.transform.position);
    }

    public void OnNewBaseBuilt(HomeBase newBase)
    {
        _dispatcher.Unregister(_baseBuilderHarvester);
        _baseBuilderHarvester.CrystalUnloaded -= OnCrystalUnloaded;
        _baseBuilderHarvester.SetIdle();
        newBase.RegisterHarvester(_baseBuilderHarvester);
        _baseBuilderHarvester = null;
        ClearFlag();
    }

    public void RegisterHarvester(Harvester harvester)
    {
        _dispatcher.RegisterHarvester(harvester);
        harvester.SetHomeBase(this);
        harvester.transform.SetParent(transform);
        harvester.CrystalUnloaded += OnCrystalUnloaded;
    }

    private void UpdateTarget()
    {
        if (_needToBuildBase)
            TryBuildBase();
        else
            TryBuildHarvester();
    }

    private void TryBuildHarvester()
    {
        if (_storage.TrySpend(_harvesterCost) == false)
            return;

        Harvester newHarvester = _harvesterFactory.Create(_spawnPoint);
        RegisterHarvester(newHarvester);
    }

    private void TryBuildBase()
    {
        if (_storage.Crystals < _baseCost)
            return;

        if (_dispatcher.Count < 2)
            return;

        Harvester harvester = _dispatcher.GetFreeHarvester();

        if (harvester == null)
            return;

        _storage.TrySpend(_baseCost);
        harvester.Send(_flag);
        _baseBuilderHarvester = harvester;
    }
}
