using UnityEngine;

public class HarvesterFactory : MonoBehaviour
{
    [SerializeField] private Harvester _harvesterPrefab;

    public int HarvesterCost { get; private set; } = 3;

    public Harvester Create(Transform spawnPoint)
    {
        return Instantiate(_harvesterPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
