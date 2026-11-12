using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionManager : MonoBehaviour
{
    public static event Action OnProductionFinished;
    [SerializeField] private int maxDrainTicks = 20;
    private List<ConstructionBehaviour> activeConstructions = new List<ConstructionBehaviour>();
    private int ticksLeft;
    private bool isDraining;
    private int drainTicksLeft;
    private string lastSnapshot;
    private int stuckTicks;
    public static event Action OnTickProcessed;
    private bool isRunning;

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
        isDraining = false;
        isRunning = true;
        activeConstructions.RemoveAll(c => c == null);
        int maxSteps = activeConstructions.Count;
        activeConstructions.Sort((a, b) =>
        {
            int result = a.GetDepth(maxSteps).CompareTo(b.GetDepth(maxSteps));
            if (result != 0)
            {
                return result;
            }
            result = a.GridPosition.x.CompareTo(b.GridPosition.x);
            if (result != 0)
            {
                return result;
            }
            return a.GridPosition.y.CompareTo(b.GridPosition.y);
        });
        InvokeRepeating("TickAll", 0.5f, 0.5f);
    }
    private void TickAll()
    {
        if (!isRunning)
        {
            return;
        }
        for (int i = 0; i < activeConstructions.Count; i++)
        {
            if (activeConstructions[i] != null)
            {
                activeConstructions[i].Tick();
            }
        }
        if (isDraining)
        {
            CheckDrain();
            return;
        }
        ticksLeft--;
        if (ticksLeft <= 0)
        {
            StartDraining();
        }
        OnTickProcessed?.Invoke();
    }
    private void StartDraining()
    {
        isDraining = true;
        drainTicksLeft = maxDrainTicks;
        lastSnapshot = "";
        stuckTicks = 0;
        foreach (ConstructionBehaviour construction in activeConstructions)
        {
            if (construction != null)
            {
                construction.StopProduction();
            }
        }
    }
    private void CheckDrain()
    {
        drainTicksLeft--;
        string snapshot = GetItemSnapshot();
        stuckTicks = snapshot == lastSnapshot ? stuckTicks + 1 : 0;
        lastSnapshot = snapshot;
        bool isEmpty = !snapshot.Contains("1");
        if (isEmpty || stuckTicks >= 2 || drainTicksLeft <=0)
        {
            FinishProduction();
        }
    }
    private string GetItemSnapshot()
    {
        string snapshot = "";
        foreach (ConstructionBehaviour construction in activeConstructions)
        {
            snapshot += construction != null && construction.HasItem ? "1" : "0";
        }
        return snapshot;
    }
    public void FinishProduction()
    {
        if (!isRunning)
        {
            return;
        }
        isRunning = false;
        CancelInvoke("TickAll");
        OnProductionFinished?.Invoke();
    }
    public int RemainingPotential()
    {
        int potential = 0;
        foreach (ConstructionBehaviour construction in activeConstructions)
        {
            if (construction == null)
            {
                continue;
            }
            if (construction.HasItem)
            {
                potential++;
            }
            if (!isDraining && construction is Extractor extractor && CanReachChest(extractor))
            {
                potential += extractor.GetPotential(ticksLeft);
            }
        }
            return potential;
    }
    private bool CanReachChest(ConstructionBehaviour start)
    {
        List<ConstructionBehaviour> visited = new List<ConstructionBehaviour>{start};
        for(int i = 0; i < visited.Count; i++)
        {
            ConstructionBehaviour current = visited[i];
            if (current is Chest)
            {
                return true;
            }
            foreach (ConstructionBehaviour next in GetOutputs(current))
            {
                if (!visited.Contains(next))
                {
                    visited.Add(next);
                }
            }
        }
        return false;
    }
    private List<ConstructionBehaviour> GetOutputs(ConstructionBehaviour current)
    {
        List<ConstructionBehaviour> outputs = new List<ConstructionBehaviour>();
        ConstructionBehaviour front = current.GetNextConstruction();
        if (current is Arm)
        {
            if(front != null && front.AcceptItems)
            {
                outputs.Add(front);
            }
            return outputs;
        }
        if (front is ConveyorBelt)
        {
            outputs.Add(front);
        }
        foreach (ConstructionBehaviour construction in activeConstructions)
        {
            if (construction is Arm && construction.GetPreviousConstruction() == current)
            {
                outputs.Add(construction);
            }
        }
        return outputs;
    }
}
