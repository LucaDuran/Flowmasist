using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : ConstructionBehaviour
{
    public override bool AcceptItems => true;
    public override void Tick()
    {
        if (cell.ResourceItem != null)
        {
            Vector2Int nextCellPosition = cell.GridPosition + GetDirectionVector();
            Cell nextCell = GetNeighbor(GetDirectionVector());
            if (nextCell != null && nextCell.ResourceItem == null && nextCell.CurrentConstruction != null && nextCell.CurrentConstruction.GetComponent<ConveyorBelt>() != null)
            {
                ResourceItem itemCell = cell.ResourceItem;
                itemCell.MoveTo(nextCell.transform.position);
                nextCell.SetResourceItem(cell.ResourceItem);
                cell.SetResourceItem(null);
            }
            return;
        }

    }
}
