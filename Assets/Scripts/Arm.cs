using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : ConstructionBehaviour
{
    public override void Tick()
    {
        Cell source = GetNeighbor(-GetDirectionVector());
        Cell destination = GetNeighbor(GetDirectionVector());
        if (source == null || destination == null)
        {
            return;
        }
        if (source.ResourceItem != null && destination.ResourceItem == null)
        {
            destination.SetResourceItem(source.ResourceItem);
            source.ResourceItem.MoveTo(destination.transform.position);
            source.SetResourceItem(null);
        }
    }
}
