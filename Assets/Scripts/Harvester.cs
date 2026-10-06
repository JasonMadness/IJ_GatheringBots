using UnityEngine;

[RequireComponent(typeof(HarvesterMover))]
[RequireComponent(typeof(CrystalCollector))]
public class Harvester : MonoBehaviour
{
    private Base _homebase;
    private HarvesterMover _mover;
    private CrystalCollector _collector;
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

    public void Sent(Crystal crystal)
    {
        _isBusy = true;
        _mover.SetTarget(crystal);
    }
}
