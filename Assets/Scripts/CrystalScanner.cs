using System.Collections.Generic;
using UnityEngine;

public class CrystalScanner
{
    private readonly List<Crystal> _free = new();
    private readonly List<Crystal> _busy = new();

    public int FreeCount => _free.Count;

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