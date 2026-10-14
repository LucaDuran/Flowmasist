using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ConstructionBehaviour
{
    public override bool AcceptItems => true;
    public static event Action OnResourceCollected;
    private int collectedCount = 0;
    public int CollectedCount => collectedCount;

    public override void Tick()
    {
        if (cell.ResourceItem != null && cell.ResourceItem.HasArrived)
        {
            Destroy(cell.ResourceItem.gameObject);
            cell.SetResourceItem(null);
            collectedCount++;
            OnResourceCollected?.Invoke();
            Debug.Log("Chest contains: " + collectedCount + " resources");
        }
    }
}
