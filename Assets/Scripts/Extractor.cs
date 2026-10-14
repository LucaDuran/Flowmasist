using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : ConstructionBehaviour
{
    [SerializeField] private GameObject resourceItemPrefab;
    public override void Tick()
    {
        if (cell.ResourceItem != null)
        {
            Cell nextCell = GetNeighbor(GetDirectionVector());
            if (nextCell != null && nextCell.ResourceItem == null && nextCell.HasConveyor)
            {
                ResourceItem item = cell.ResourceItem;
                item.MoveTo(nextCell.transform.position);
                nextCell.SetResourceItem(item);
                cell.SetResourceItem(null);
            }
        }
        if (cell.ResourceItem == null)
        {
            GameObject resourceItem = Instantiate(resourceItemPrefab, cell.transform.position, Quaternion.identity);
            cell.SetResourceItem(resourceItem.GetComponent<ResourceItem>());
        }
    }
}               