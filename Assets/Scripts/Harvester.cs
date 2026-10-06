using UnityEngine;

public class Harvester : MonoBehaviour
{
    private Crystal _target;
    private bool _isBusy = false;

    public bool IsBusy => _isBusy;

    public void Sent(Crystal crystal)
    {
        _isBusy = true;
        _target = crystal;
    }
}
