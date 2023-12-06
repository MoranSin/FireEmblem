using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting;

public class SC_GridObject : MonoBehaviour
{
    public SC_Grid targetGrid;
    [JsonIgnore]

    public Vector2Int positionOnGrid;
    public SC_Character character;
    private void Start()
    {
        character = GetComponent<SC_Character>();
        Init();   
    }

    private void Init()
    {
        positionOnGrid =  targetGrid.GetGridPosition(transform.position);
        targetGrid.PlaceObject(positionOnGrid, this);
        Vector3 pos = targetGrid.GetWorldPosition(positionOnGrid.x, positionOnGrid.y, true);
        transform.position = pos;
    }

    [System.Serializable]
    public class SerializableVector2Int
    {
        public int x;
        public int y;

        [JsonIgnore]
        public Vector2Int UnityVector
        {
            get
            {
                return new Vector2Int(x, y);
            }
        }

        public SerializableVector2Int(Vector2Int v)
        {
            x = v.x;
            y = v.y;
        }

        public SerializableVector2Int(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public static List<SerializableVector2Int> GetSerializableList(List<Vector2Int> vList)
        {
            List<SerializableVector2Int> list = new List<SerializableVector2Int>(vList.Count);
            for (int i = 0; i < vList.Count; i++)
            {
                list.Add(new SerializableVector2Int(vList[i]));
            }
            return list;
        }

        public static List<Vector2Int> GetSerializableList(List<SerializableVector2Int> vList)
        {
            List<Vector2Int> list = new List<Vector2Int>(vList.Count);
            for (int i = 0; i < vList.Count; i++)
            {
                list.Add(vList[i].UnityVector);
            }
            return list;
        }
    }

    public string ToJson()
    {
        Dictionary<string, object> jsonData = new Dictionary<string, object>
       {
            { "targetGrid", targetGrid.ToJson() },
           { "positionOnGrid", new List<int> { positionOnGrid.x, positionOnGrid.y } }
       };

        if (character != null)
        {
            jsonData.Add("character", character.ToJson());
        }
    return MiniJSON.Json.Serialize(jsonData);
    }

}
