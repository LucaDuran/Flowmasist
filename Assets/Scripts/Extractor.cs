using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : ConstructionBehaviour
{
    [SerializeField] private GameObject resourceItemPrefab;
    public override void Tick()
    {
        Debug.Log("Extractor tick. Cell es null: " + (cell == null) + " | Resource actual: " + (cell != null && cell.ResourceItem != null));
        if (cell.ResourceItem == null)
        {
            GameObject resourceItem = Instantiate(resourceItemPrefab, cell.transform.position, Quaternion.identity);
            cell.SetResourceItem(resourceItem.GetComponent<ResourceItem>());
        }

        if (cell.ResourceItem != null)
        {
            Vector2Int nextCellPosition = cell.GridPosition + GetDirectionVector();
            Cell nextCell = levelCreator.GetCell(nextCellPosition.x, nextCellPosition.y);
            if (nextCell != null && nextCell.ResourceItem == null)
            {
                ResourceItem itemCell = cell.ResourceItem;
                itemCell.transform.position = nextCell.transform.position;
                nextCell.SetResourceItem(cell.ResourceItem);
                cell.SetResourceItem(null);
            }
            return;
        }
    }
    public Vector2Int GetDirectionVector()
    {
        return new Vector2Int(1, 0);
    }
}
