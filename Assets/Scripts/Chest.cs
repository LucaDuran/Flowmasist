using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : ConstructionBehaviour
{
    public override void Tick()
    {
        if (cell.ResourceItem != null)
        {
            Debug.Log("Chest contains: " + cell.ResourceItem.name);
            Destroy(cell.ResourceItem.gameObject);
            cell.SetResourceItem(null);
        }
    }
}
