using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private Crystal _crystalPrefab;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _boundaryDistance = 5f;
    [SerializeField] private int _startingCrystalCount = 5;

    private void Start()
    {
        SpawnStartingCrystals();
    }
    
    public void SpawnCrystalAtRandomPoint()
    {
        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * _boundaryDistance;
        Vector3 spawnPosition = spawnPoint.position + new Vector3(offset.x, 0f, offset.y);
        Instantiate(_crystalPrefab, spawnPosition, Quaternion.identity, spawnPoint);
    }

    private void SpawnStartingCrystals()
    {
        if (_spawnPoints.Length == 0)
            return;

        for (int i = 0; i < _startingCrystalCount; i++)
            SpawnCrystalAtRandomPoint();
    }
}
