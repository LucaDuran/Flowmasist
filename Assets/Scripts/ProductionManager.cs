using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    public static event Action OnProductionFinished;
    private List<ConstructionBehaviour> activeConstructions = new List<ConstructionBehaviour>();
    private int ticksLeft;

    public void RegisterConstruction(ConstructionBehaviour construction)
    {
        if (!activeConstructions.Contains(construction))
        {
            activeConstructions.Add(construction);
        }
    }
    public void StartProduction(int ticks)
    {
        ticksLeft = ticks;
        InvokeRepeating("TickAll", 0.5f, 0.5f);
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
        ticksLeft--;
        if (ticksLeft <= 0)
        {
            CancelInvoke("TickAll");
            OnProductionFinished?.Invoke();
        }
    }
}
