using System;
using System.Collections;
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
        CrystalCollected += _crystal.NotifyCollected;
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
        CrystalCollected -= _crystal.NotifyCollected;
        _crystal = null;
    }
}