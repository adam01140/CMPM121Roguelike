using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

public class RelicManager
{

    public RelicManager()
    {

    }

    public List<Relic> genRelicSelection()
    {
        List<Relic> relicSelection = new List<Relic>();
        List<Relic> allRelics = RelicBuilder.Instance.allRelics;
        List<int> indicies = RelicBuilder.Instance.GenRelicIndices();

        if (allRelics.Count > 1)
        {
            relicSelection.Add(allRelics[indicies[0]]);
            relicSelection.Add(allRelics[indicies[1]]);
        }
        else if (allRelics.Count > 0)
        {
            relicSelection.Add(allRelics[0]);
        }
        return relicSelection;
    }

}


