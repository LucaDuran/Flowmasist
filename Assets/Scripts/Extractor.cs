using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : ConstructionBehaviour
{
    [SerializeField] private GameObject resourceItemPrefab;
    private bool canExtract = true;
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
        if (cell.ResourceItem == null && canExtract && cell.TryTakeStock())
        {
            GameObject resourceItem = Instantiate(resourceItemPrefab, cell.transform.position, Quaternion.identity);
            cell.SetResourceItem(resourceItem.GetComponent<ResourceItem>());
        }
    }
    public override void StopProduction()
    {
        canExtract = false;
    }
    public int GetPotential(int ticksLeft)
    {
        if(!canExtract)
        {
            return 0;
        }
        int stock = cell.Stock;
        return stock < 0 ? ticksLeft : Mathf.Min(stock, ticksLeft);
    }
}               