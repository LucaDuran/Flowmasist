using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private Vector2Int gridPosition;
    private bool canBuild;
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

    void Start()
    {
        
    }
}
