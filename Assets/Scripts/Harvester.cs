using System;
using UnityEngine;

[RequireComponent(typeof(HarvesterMover))]
[RequireComponent(typeof(CrystalCollector))]
public class Harvester : MonoBehaviour
{
    [SerializeField] private GameObject _fullTrunk;
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
        _fullTrunk.SetActive(false);
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

    public void OnReturnedToBase()
    {
        _isBusy = false;
        _target = null;
        _fullTrunk.SetActive(false);
        _mover.TargetReached -= OnReturnedToBase;
    }
}
