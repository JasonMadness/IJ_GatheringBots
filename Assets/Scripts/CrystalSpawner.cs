using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [SerializeField] private Crystal _crystalPrefab;
    [SerializeField] private CrystalPool _crystalPool;
    [SerializeField] private Transform[] _spawnPoints;
    [SerializeField] private float _boundaryDistance = 5f;
    [SerializeField] private int _startingCrystalCount = 5;

    private void Start()
    {
        SpawnStartingCrystals();
    }

    public void SpawnCrystalAtRandomPoint()
    {
        if (_spawnPoints.Length == 0)
            return;

        Transform spawnPoint = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
        Vector2 offset = Random.insideUnitCircle * _boundaryDistance;
        Vector3 spawnPosition = spawnPoint.position + new Vector3(offset.x, 0f, offset.y);

        Crystal crystal;

        if (_crystalPool.TryGet(out crystal))
        {
            crystal.transform.SetParent(spawnPoint);
            crystal.transform.SetPositionAndRotation(spawnPosition, Quaternion.identity);
            crystal.gameObject.SetActive(true);
        }
        else
        {
            crystal = Instantiate(_crystalPrefab, spawnPosition, Quaternion.identity, spawnPoint);
        }

        crystal.Collected += OnCrystalCollected;
    }

    private void OnCrystalCollected(Crystal crystal)
    {
        crystal.Collected -= OnCrystalCollected;
        _crystalPool.Release(crystal);
        SpawnCrystalAtRandomPoint();
    }

    private void SpawnStartingCrystals()
    {
        if (_spawnPoints.Length == 0)
            return;

        for (int i = 0; i < _startingCrystalCount; i++)
            SpawnCrystalAtRandomPoint();
    }
}