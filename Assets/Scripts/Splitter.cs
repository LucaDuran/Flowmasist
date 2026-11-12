using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splitter : ConstructionBehaviour
{
    private int nextOutput = 0;
    public override bool AcceptItems => true;
    public override bool AcceptsFromBelt => true;
    public override bool HasItem
    {
        get
        {
            foreach (Cell lane in occupiedCells)
            {
                if (lane.ResourceItem != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
    public override void Tick()
    {
        foreach (Cell lane in occupiedCells)
        {
            if (lane.ResourceItem == null)
            {
                continue;
            }
            Cell output = GetOutputCell(nextOutput);
            if (output == null || output.ResourceItem != null || !output.HasConveyor)
            {
                return;
            }
            ResourceItem item = lane.ResourceItem;
            item.MoveTo(output.transform.position);
            output.SetResourceItem(item);
            lane.SetResourceItem(null);
            nextOutput = (nextOutput + 1) % occupiedCells.Count;
        }
    }
    private Cell GetOutputCell(int index)
    {
        Vector2Int position = occupiedCells[index].GridPosition + facing;
        return levelCreator.GetCell(position.x, position.y);
    }
}
