public class CrystalStorage
{
    private int _crystals;

    public int Crystals => _crystals;

    public void AddCrystal()
    {
        _crystals++;
    }

    public bool TrySpend(int amount)
    {
        if (_crystals < amount)
            return false;

        _crystals -= amount;
        return true;
    }
}