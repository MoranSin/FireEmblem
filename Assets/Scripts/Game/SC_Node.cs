using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Node
{
    public bool passable;
    public float elevation;

    [JsonIgnore]
    public SC_GridObject gridObject;

    //public string ToJson()
    //{
    //    Dictionary<string, object> jsonData = new Dictionary<string, object>
    //    {
    //        { "passable", passable.ToString() },
    //        { "elevation", elevation }
    //    };

    //    //if (gridObject != null)
    //    //{
    //    //    jsonData.Add("gridObject", gridObject.ToJson());
    //    //}
    //    //else
    //    //{
    //    //     jsonData.Add("gridObject", null);
    //    //}

    //    return JsonConvert.SerializeObject(jsonData);
    //}
}
