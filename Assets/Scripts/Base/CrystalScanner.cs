using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScanner : MonoBehaviour
{
    [SerializeField] private float _scanRadius = 200f;
    [SerializeField] private float _scanInterval = 2f;
    [SerializeField] private LayerMask _crystalLayer;

    private readonly Collider[] _buffer = new Collider[64];
    private readonly List<Crystal> _free = new();
    private readonly List<Crystal> _busy = new();

    private Coroutine _scanRoutine;

    public int FreeCount => _free.Count;

    private void OnEnable()
    {
        _scanRoutine = StartCoroutine(ScanRoutine());
    }

    private void OnDisable()
    {
        if (_scanRoutine != null)
        {
            StopCoroutine(_scanRoutine);
            _scanRoutine = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _scanRadius);
    }

    private IEnumerator ScanRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(_scanInterval);
        yield return wait;

        while (true)
        {
            Refresh();
            yield return wait;
        }
    }

    public void Refresh()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position,
            _scanRadius,
            _buffer,
            _crystalLayer
        );

        for (int i = 0; i < count; i++)
        {
            if (_buffer[i].TryGetComponent(out Crystal crystal) == false)
                continue;

            if (_free.Contains(crystal) || _busy.Contains(crystal))
                continue;

            _free.Add(crystal);
        }
    }

    public bool TryGetFree(out Crystal crystal)
    {
        if (_free.Count == 0)
        {
            crystal = null;
            return false;
        }

        crystal = _free[0];
        _free.RemoveAt(0);
        _busy.Add(crystal);
        return true;
    }

    public void RemoveFromBase(Crystal crystal)
    {
        _free.Remove(crystal);
        _busy.Remove(crystal);
    }
}