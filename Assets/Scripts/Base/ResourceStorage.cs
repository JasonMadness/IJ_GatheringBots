public class ResourceStorage
{
    private int _crystals;

    public int Crystals => _crystals;

    public void AddCrystal()
    {
        _crystals++;
    }
}