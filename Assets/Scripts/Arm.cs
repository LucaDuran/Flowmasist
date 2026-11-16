using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : ConstructionBehaviour
{
   public override void Tick()
    {
        Cell source = GetNeighbor(-GetDirectionVector());
        Cell destination = GetNeighbor(GetDirectionVector());
        if (source == null || destination == null || source.ResourceItem == null)
        {
            return;
        }
        Furnace furnace = GetConstructionAt(GetDirectionVector()) as Furnace;
        if (furnace != null)
        {
            if (furnace.TryInsert(source.ResourceItem))
            {
                source.SetResourceItem(null);
            }
            return;
        }
        if (destination.ResourceItem == null && destination.AcceptsItems)
        {
            destination.SetResourceItem(source.ResourceItem);
            source.ResourceItem.MoveTo(destination.transform.position);
            source.SetResourceItem(null);
        }
    }
}
