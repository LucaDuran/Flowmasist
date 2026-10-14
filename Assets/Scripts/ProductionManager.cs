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
        activeConstructions.RemoveAll(c => c == null);
        int maxSteps = activeConstructions.Count;
        activeConstructions.Sort((a, b) => a.GetDepth(maxSteps).CompareTo(b.GetDepth(maxSteps)));
        InvokeRepeating("TickAll", 0.5f, 0.5f);
    }
    private void TickAll()
    {
        for (int i = 0; i < activeConstructions.Count; i++)
        {
            if (activeConstructions[i] != null)
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
