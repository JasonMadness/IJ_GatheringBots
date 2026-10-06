using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalCollector : MonoBehaviour
{
    [SerializeField] private float _complitionTime = 3f;

    private Crystal _crystal;

    public event Action<Crystal> CrystalCollected;

    public void Begin()
    {
        StartCoroutine(CollectionCoroutine());
    }

    private IEnumerator CollectionCoroutine()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _complitionTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        CrystalCollected?.Invoke(_crystal);
    }
}
