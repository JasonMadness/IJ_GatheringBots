using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterDispatcher : MonoBehaviour
{
    private List<Harvester> _harvesters = new();

    public void RegisterHarvester(Harvester harvester)
    {
        _harvesters.Add(harvester);
    }

    public void OnFreeCrystalFound(Crystal crystal)
    {
        foreach (var harvester in _harvesters)
        {
            if (harvester.IsBusy == false)
            {
                harvester.Send(crystal);
                return;
            }
        }
    }
}
