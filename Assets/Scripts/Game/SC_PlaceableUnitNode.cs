using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlaceableUnitNode : MonoBehaviour
{
    public SC_Character character;
    SC_PlacementManager manager;
    public SC_GridObject gridObject;

    private void Awake()
    {
        manager = FindObjectOfType<SC_PlacementManager>();
        gridObject = GetComponent<SC_GridObject>();
    }
    void Start()
    {
       manager.AddMe(this);
    }
}
