using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private Crystal _crystalPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _boundaryDistance;

    public void SpawnCrystal()
    {
        if (_spawnPoints.Length == 0)
            return;
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Vector3 spawnPosition = spawnPoint.position + Random.insideUnitSphere * _boundaryDistance;
        spawnPosition.y = spawnPoint.position.y;
        Instantiate(_crystalPrefab, spawnPosition, Quaternion.identity);
    }
}
