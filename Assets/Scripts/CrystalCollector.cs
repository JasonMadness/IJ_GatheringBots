using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCollector : MonoBehaviour
{
    private Crystal _crystal;
    private float _collectionTime;

    public event Action<Crystal> CrystalCollected;

    public void Begin(Crystal crystal)
    {
        _crystal = crystal;
        _collectionTime = crystal.CollectTime;
        CrystalCollected += _crystal.OnCollected;
        StartCoroutine(CollectionCoroutine());
    }

    private IEnumerator CollectionCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _collectionTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CrystalCollected?.Invoke(_crystal);
        CrystalCollected -= _crystal.OnCollected;
        _crystal = null;
    }
}
