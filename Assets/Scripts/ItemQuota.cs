using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemQuota
{
    [SerializeField] private ItemType type;
    [SerializeField] private int amount;
    public ItemType Type => type;
    public int Amount => amount;
}