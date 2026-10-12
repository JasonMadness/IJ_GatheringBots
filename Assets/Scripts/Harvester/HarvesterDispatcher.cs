using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterDispatcher : MonoBehaviour
{
    private List<Harvester> _harvesters = new();
    private CrystalScanner _scanner;

    public void Initialize(CrystalScanner scanner)
    {
        _scanner = scanner;
        _scanner.FreeCrystalFound += OnFreeCrystalFound;
    }

    public void RegisterHarvester(Harvester harvester)
    {
        _harvesters.Add(harvester);
        harvester.FreeCrystalObtained += _scanner.OnFreeCrystalTargetObtained;
    }

    public void OnFreeCrystalFound(List<Crystal> crystals)
    {
        Harvester freeHarvester = GetFreeHarvester();

        if (freeHarvester == null)
            return;

        foreach (var crystal in crystals)
        {
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
