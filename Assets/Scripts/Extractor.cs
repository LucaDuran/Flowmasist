using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : ConstructionBehaviour
{
    private bool canExtract = true;
    public override void Tick()
    {
        TryPushToBelt();
        if (cell.ResourceItem == null && canExtract && cell.TryTakeStock())
        {
            GameObject resourceItem = Instantiate(cell.ItemPrefab, cell.transform.position, Quaternion.identity);
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