using System;
using UnityEngine;

[RequireComponent(typeof(HarvesterMover))]
[RequireComponent(typeof(CrystalCollector))]
public class Harvester : MonoBehaviour
{
    private Base _homebase;
    private HarvesterMover _mover;
    private CrystalCollector _collector;
    private IHarvesterTarget _target;
    private bool _isBusy = false;

    public bool IsBusy => _isBusy;

    private void Awake()
    {
        _collector = GetComponent<CrystalCollector>();
        _mover = GetComponent<HarvesterMover>();
    }

    public void SetHomeBase(Base homebase)
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
        _collector.Begin();
        _collector.CrystalCollected += OnCrystalCollected;
    }

    private void OnCrystalCollected(Crystal crystal)
    {
        _collector.CrystalCollected -= OnCrystalCollected;
        _homebase.OnCrystalCollected(crystal);
        _target = null;
        ReturnToBase();
    }

    private void ReturnToBase()
    {
        _mover.SetTarget(_homebase);
    }
}
