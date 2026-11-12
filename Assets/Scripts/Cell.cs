using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private TextMeshPro textVein;
    [SerializeField] private int energyModifier = 0;
    private CardPlacementManager cardPlacementManager;
    private ResourceItem resourceItem;
    public ResourceItem ResourceItem => resourceItem;
    private GameObject currentConstruction;
    public GameObject CurrentConstruction => currentConstruction;
    public bool IsBusy => currentConstruction != null;
    public bool HasConveyor => currentConstruction != null && currentConstruction.GetComponent<ConveyorBelt>() != null;
    public bool AcceptsItems => currentConstruction != null && currentConstruction.GetComponent<ConstructionBehaviour>().AcceptItems;
    public bool AcceptsFromBelt => currentConstruction != null && currentConstruction.GetComponent<ConstructionBehaviour>().AcceptsFromBelt;

    private Vector2Int gridPosition;
    public Vector2Int GridPosition => gridPosition;
    private bool canBuild;
    public bool CanBuild => canBuild;
    private bool isLowEnergyZone;
    private bool isExtractionZone;
    public bool IsExtractionZone => isExtractionZone;
    private int stock = -1;
    public bool HasStock => stock != 0;
    public int Stock => stock;
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
    void Start()
    {
        
    }
    public void SetStock(int stock)
    {
        this.stock = stock;
        UpdateStockLabel();
    }
    public bool TryTakeStock()
    {
        if (stock == 0)
        {
            return false;
        }
        if (stock > 0)
        {
            stock--;
            UpdateStockLabel();
        }
        return true;
    }
    private void UpdateStockLabel()
    {
        if (textVein == null)
        {
            return;
        }
        textVein.color = Color.white;
        if (stock == -1)
        {
            textVein.text = "";
            return;
        }
        if (stock == 0)
        {
            textVein.color = Color.red;
        }
        textVein.text = stock.ToString();
    }
    public void SetEnergyModifier(int energyModifier)
    {
        this.energyModifier = energyModifier;
    }
    public int GetEnergyCost(int baseCost)
    {
        return Mathf.Max(0, baseCost + energyModifier);
    }
}
