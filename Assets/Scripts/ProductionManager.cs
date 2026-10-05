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
        for (int i = activeConstructions.Count - 1; i >= 0; i--)
        {
            if (activeConstructions[i] == null)
            {
                activeConstructions.RemoveAt(i);
            }
            else
            {
                activeConstructions[i].Tick();
            }
        }
    }
    public void Start()
    {
        InvokeRepeating("TickAll", 0.5f, 0.5f);
    }
}
