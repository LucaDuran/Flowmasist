using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour
{
    private CardPlacementManager cardPlacementManager;
    private ResourceItem resourceItem;
    public ResourceItem ResourceItem => resourceItem;
    private GameObject currentConstruction;
    public GameObject CurrentConstruction => currentConstruction;
    public bool IsBusy => currentConstruction != null;
    private Vector2Int gridPosition;
    public Vector2Int GridPosition => gridPosition;
    private bool canBuild;
    public bool CanBuild => canBuild;
    private bool isLowEnergyZone;
    private bool isExtractionZone;

    public void Initialize(Vector2Int position, bool canBuild, bool isExtractionZone, bool isLowEnergyZone)
    {
        gridPosition = position;
        this.canBuild = canBuild;
        this.isExtractionZone = isExtractionZone;
        this.isLowEnergyZone = isLowEnergyZone;
    }
    public void SetCanBuild(bool canBuild)
    {
        this.canBuild = canBuild;
    }
    public void SetIsExtractionZone(bool isExtractionZone)
    {
        this.isExtractionZone = isExtractionZone;
    }
    public void SetIsLowEnergyZone(bool isLowEnergyZone)
    {
        this.isLowEnergyZone = isLowEnergyZone;
    }
    public void SetCardPlacementManager(CardPlacementManager manager)
    {
        cardPlacementManager = manager;
    }
    public void SetCurrentConstruction(GameObject construction)
    {
        currentConstruction = construction;
    }
    public void SetResourceItem(ResourceItem resource)
    {
        resourceItem = resource;
    }
    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        cardPlacementManager.OnCellSelected(this);
    }

    void Start()
    {
        
    }
}
