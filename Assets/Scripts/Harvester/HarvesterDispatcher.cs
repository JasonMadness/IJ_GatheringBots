using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterDispatcher : MonoBehaviour
{
    private List<Harvester> _harvesters = new();
    private CrystalScanner _scanner;

    public int Count => _harvesters.Count;

    private void OnDisable()
    {
        if (_scanner != null)
            _scanner.FreeCrystalFound -= OnFreeCrystalFound;
    }

    public void Initialize(CrystalScanner scanner)
    {
        _scanner = scanner;
        _scanner.FreeCrystalFound += OnFreeCrystalFound;
    }

    public void RegisterHarvester(Harvester harvester)
    {
        _harvesters.Add(harvester);
    }

    public void OnFreeCrystalFound()
    {
        while (_scanner.TryGetFree(out Crystal crystal))
        {
            Harvester freeHarvester = GetFreeHarvester();

            if (freeHarvester == null)
            {
                _scanner.ReleaseFreeCrystal(crystal);
                break;
            }

            freeHarvester.Send(crystal);
        }
    }    

    private Harvester GetFreeHarvester()
    {
        foreach (var harvester in _harvesters)
        {
            if (harvester.IsBusy == false)
                return harvester;
        }

        return null;
    }
}
