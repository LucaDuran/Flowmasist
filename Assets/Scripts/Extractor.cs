using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Extractor : ConstructionBehaviour
{
    [SerializeField] private GameObject resourceItemPrefab;
    public override void Tick()
    {
        if (cell.ResourceItem == null)
        {
            GameObject resourceItem = Instantiate(resourceItemPrefab, cell.transform.position, Quaternion.identity);
            cell.SetResourceItem(resourceItem.GetComponent<ResourceItem>());
        }
    }
}