using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private Crystal _crystalPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _boundaryDistance = 5f;

    public void SpawnCrystal()
    {
        if (_spawnPoints.Length == 0)
            return;
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * _boundaryDistance;
        Vector3 spawnPosition = spawnPoint.position + new Vector3(offset.x, 0f, offset.y);
        Instantiate(_crystalPrefab, spawnPosition, Quaternion.identity, spawnPoint);
    }
}
