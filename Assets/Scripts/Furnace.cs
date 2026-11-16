using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : ConstructionBehaviour
{
    [SerializeField] private GameObject plateItemPrefab;
    [SerializeField] private int craftTicks = 2;
    private bool hasCoal;
    private bool hasIron;
    private int timer;
    public override bool AcceptItems => true;
    public override bool HasItem => (cell != null && cell.ResourceItem != null) || hasCoal || hasIron || timer > 0;

    public bool TryInsert(ResourceItem item)
    {
        if (item.Type == ItemType.Coal && !hasCoal)
        {
            hasCoal = true;
        }
        else if (item.Type == ItemType.IronOre && !hasIron)
        {
            hasIron = true;
        }
        else
        {
            return false;
        }
        item.MoveTo(cell.transform.position);
        Destroy(item.gameObject, 0.5f);
        return true;
    }
    public override void Tick()
    {
        if (timer > 0)
        {
            timer--;
            if (timer == 0)
            {
                SpawnPlate();
            }
        }
        TryPushToBelt();
        if (timer == 0 && cell.ResourceItem == null && hasCoal && hasIron)
        {
            hasCoal = false;
            hasIron = false;
            timer = craftTicks;
        }
    }
    private void SpawnPlate()
    {
        GameObject plate = Instantiate(plateItemPrefab, cell.transform.position, Quaternion.identity);
        cell.SetResourceItem(plate.GetComponent<ResourceItem>());
    }
}