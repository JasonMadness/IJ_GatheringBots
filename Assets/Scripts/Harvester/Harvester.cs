using System;
using UnityEngine;

[RequireComponent(typeof(HarvesterMover))]
[RequireComponent(typeof(CrystalCollector))]
[RequireComponent(typeof(CrystalUnloader))]
public class Harvester : MonoBehaviour
{
    [SerializeField] private GameObject _fullTrunk;

    private HomeBase _homebase;
    private HarvesterMover _mover;
    private CrystalCollector _collector;
    private CrystalUnloader _unloader;
    private IHarvesterTarget _target;
    private bool _isBusy = false;

    public bool IsBusy => _isBusy;

    public event Action<Harvester> CrystalUnloaded;

    private void Awake()
    {
        _collector = GetComponent<CrystalCollector>();
        _unloader = GetComponent<CrystalUnloader>();
        _mover = GetComponent<HarvesterMover>();
        _fullTrunk.SetActive(false);
    }

    public void SetHomeBase(HomeBase homebase)
    {
        _homebase = homebase;
    }

    public void Send(IHarvesterTarget target)
    {
        _isBusy = true;
        _target = target;
        _mover.SetTarget(_target);
        _mover.TargetReached += OnTargetReached;
    }

    private void OnTargetReached()
    {
        _mover.TargetReached -= OnTargetReached;
        _collector.Begin(_target as Crystal);
        _collector.CrystalCollected += OnCrystalCollected;
    }

    private void OnCrystalCollected(Crystal crystal)
    {
        _collector.CrystalCollected -= OnCrystalCollected;
        _homebase.OnCrystalCollected(crystal);
        _target = null;
        _fullTrunk.SetActive(true);
        ReturnToBase();
    }

    private void ReturnToBase()
    {
        _mover.SetTarget(_homebase);
        _mover.TargetReached += OnReturnedToBase;
    }

    private void OnReturnedToBase()
    {
        _mover.TargetReached -= OnReturnedToBase;
        _unloader.Begin(_homebase.UnloadTime);
        _unloader.Unloaded += OnUnloaded;
    }

    private void OnUnloaded()
    {
        _unloader.Unloaded -= OnUnloaded;
        _fullTrunk.SetActive(false);
        _isBusy = false;
        _target = null;
        CrystalUnloaded?.Invoke(this);
    }
}