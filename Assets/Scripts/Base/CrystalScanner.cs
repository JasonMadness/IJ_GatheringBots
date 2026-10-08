using System.Collections.Generic;
using UnityEngine;

public class CrystalScanner : MonoBehaviour
{
    private List<Crystal> _free;
    private List<Crystal> _busy;

    public int FreeCount => _free.Count;

    private void Awake()
    {
        _free = new();
        _busy = new();
    }

    private void Start()
    {
        InvokeRepeating(nameof(Refresh), 2f, 2f);
    }

    public void Refresh()
    {
        Crystal[] found = Object.FindObjectsOfType<Crystal>();

        foreach (var crystal in found)
        {
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