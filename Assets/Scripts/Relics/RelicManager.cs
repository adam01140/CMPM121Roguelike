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
        relicSelection.Add(RelicBuilder.Instance.GenRelic());
        relicSelection.Add(RelicBuilder.Instance.GenRelic());
        relicSelection.Add(RelicBuilder.Instance.GenRelic());

        return relicSelection;
    }

}


