using System;
using System.Collections;
using UnityEngine;

public class CrystalUnloader : MonoBehaviour
{
    private float _unloadTime;
    public event Action Unloaded;

    public void Begin(float unloadTime)
    {
        _unloadTime = unloadTime;
        StartCoroutine(UnloadCoroutine());
    }

    private IEnumerator UnloadCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < _unloadTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        Unloaded?.Invoke();
    }
}
