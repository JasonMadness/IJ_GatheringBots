using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPool : MonoBehaviour
{
    private readonly Queue<Crystal> _pool = new();

    public bool TryGet(out Crystal crystal)
    {
        if (_pool.Count > 0)
        {
            crystal = _pool.Dequeue();
            return true;
        }

        crystal = null;
        return false;
    }

    public void Release(Crystal crystal)
    {
        crystal.gameObject.SetActive(false);
        crystal.transform.SetParent(transform);
        _pool.Enqueue(crystal);
    }
}