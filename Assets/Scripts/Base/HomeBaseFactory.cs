using UnityEngine;

public class HomeBaseFactory : MonoBehaviour
{
    [SerializeField] private HomeBase _basePrefab;
    [SerializeField] private CrystalScanner _crystalScanner;
    [SerializeField] private Vector3 _defaultBasePosition;

    public HomeBase DefaultBase { get; private set; }

    private void Start()
    {
        BuildDefaultBase();
    }

    public HomeBase Build(Vector3 position)
    {
        var homebase = Instantiate(_basePrefab, position, Quaternion.identity);
        homebase.Initialize(_crystalScanner);
        return homebase;
    }

    private void BuildDefaultBase()
    {
        DefaultBase = Build(_defaultBasePosition);
    }
}