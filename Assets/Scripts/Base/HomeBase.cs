using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeBase : MonoBehaviour, IHarvesterTarget
{
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _reachRadius = 20f;
    [SerializeField] private float _unloadTime = 2f;

    private CrystalScanner _scanner;
    private List<Harvester> _harvesters = new();
    private HarvesterFactory _harvesterFactory;
    private readonly CrystalStorage _storage = new();
    private Coroutine _scanningCoroutine;

    private int _startingCrystalAmount;
    private int _harvesterCost;

    public float ReachRadius => _reachRadius;
    public float UnloadTime => _unloadTime;
    public Vector3 Position => transform.position;

    private void OnEnable()
    {
        foreach (var harvester in _harvesters)
            harvester.CrystalUnloaded += OnCrystalUnloaded;

        if (_scanner != null)
            _scanningCoroutine = StartCoroutine(StartScanning());
    }

    private void OnDisable()
    {
        foreach (var harvester in _harvesters)
            harvester.CrystalUnloaded -= OnCrystalUnloaded;

        StopCoroutine(_scanningCoroutine);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _reachRadius);
    }

    public void Initialize(CrystalScanner scanner, HarvesterFactory harvesterFactory, int startingCrystalAmount)
    {
        _scanner = scanner;
        _harvesterFactory = harvesterFactory;
        _startingCrystalAmount = startingCrystalAmount;

        if (_scanningCoroutine == null)
            _scanningCoroutine = StartCoroutine(StartScanning());

        _harvesterCost = _harvesterFactory.HarvesterCost;

        for (int i = 0; i < _startingCrystalAmount; i++)
            _storage.AddCrystal();

        while (_storage.Crystals >= _harvesterCost)
            TryBuildHarvester();
    }

    private IEnumerator StartScanning()
    {
        float delay = 2f;
        yield return new WaitForSeconds(delay);

        while (isActiveAndEnabled)
        {
            Scan();
            yield return new WaitForSeconds(delay);
        }
    }

    private void Scan()
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
        _harvesters.Add(harvester);
        harvester.SetHomeBase(this);
        harvester.CrystalUnloaded += OnCrystalUnloaded;
    }
}
