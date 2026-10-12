using UnityEngine;

public class HomeBaseFactory : MonoBehaviour
{
    [SerializeField] private HomeBase _basePrefab;
    [SerializeField] private CrystalScanner _crystalScanner;
    [SerializeField] private HarvesterFactory _harvesterFactory;
    [SerializeField] private Vector3 _defaultBasePosition;

    private int _newBaseCost = 5;
    private int _startingCrystalAmount = 9;

    public HomeBase DefaultBase { get; private set; }

    private void Start()
    {
        BuildDefaultBase();
    }

    public HomeBase Build(Vector3 position, int startingCrystalAmount)
    {
        var homebase = Instantiate(_basePrefab, position, _basePrefab.transform.rotation);
        homebase.Initialize(_crystalScanner, _harvesterFactory, startingCrystalAmount, _newBaseCost);
        return homebase;
    }

    private void BuildDefaultBase()
    {
        DefaultBase = Build(_defaultBasePosition, _startingCrystalAmount);
    }
}