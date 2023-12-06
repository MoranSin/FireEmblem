using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_PlacementManager : MonoBehaviour
{
    [SerializeField] GameObject characterPrefab;
    [SerializeField] GameObject characterModel;
    List<GameObject> characterObjects = new List<GameObject>();

    [SerializeField] SC_SquadData squadData;
    List<SC_PlaceableUnitNode> nodes;
    SC_MouseInput mouseInput;

    private void Awake()
    {
        mouseInput = GetComponent<SC_MouseInput>(); 
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        characterObjects = new List<GameObject>();
        for(int i = 0; i < squadData.charactersInTheMission.Count; i++)
        {
            InitCharacter(squadData.charactersInTheMission[i]);
        }
    }

    private void InitCharacter(SC_CharacterData characterData)
    {
        GameObject newCharacterGameObject = Instantiate(characterPrefab);
        GameObject newCharacterModel = Instantiate(characterModel);
        newCharacterModel.transform.parent = newCharacterGameObject.transform;
        newCharacterGameObject.GetComponent<SC_Character>().SetCharacterData(characterData);
        characterObjects.Add(newCharacterGameObject);
    }

    public void AddMe(SC_PlaceableUnitNode placeableNode)
    {
        if(nodes == null) { nodes = new List<SC_PlaceableUnitNode>(); }
        nodes.Add(placeableNode);
    }

    public void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SC_PlaceableUnitNode placeNode = nodes.Find(x => x.gridObject.positionOnGrid == mouseInput.positionOnGrid);
            if (placeNode != null)
            {
                if(placeNode.character == null)
                {
                    PlaceCharacter(placeNode, characterObjects[0]);
                }
            }
        }
    }

    private void PlaceCharacter(SC_PlaceableUnitNode placeNode, GameObject characterObject)
    {
        characterObject.transform.position = placeNode.transform.position;
        placeNode.character = characterObject.GetComponent<SC_Character>();

        characterObjects.Remove(characterObject);

    }
}
