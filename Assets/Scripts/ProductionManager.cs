using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    private List<ConstructionBehaviour> activeConstructions = new List<ConstructionBehaviour>();

    public void RegisterConstruction(ConstructionBehaviour construction)
    {
        if (!activeConstructions.Contains(construction))
        {
            activeConstructions.Add(construction);
        }
    }
    private void TickAll()
    {
        Debug.Log("Cantidad de construcciones registradas: " + activeConstructions.Count);
        foreach (var construction in activeConstructions)
        {
            construction.Tick();
        }
    }
    public void Start()
    {
        InvokeRepeating("TickAll", 3f, 3f);
    }
}
