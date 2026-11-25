using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arm : ConstructionBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform hand;
    [SerializeField] private float pickupDelay = 0.1f;
    [SerializeField] private float carryTime = 0.3f;
    private static readonly int WorkTrigger = Animator.StringToHash("Work");
    private void PlayWork()
    {
        if (animator != null)
        {
            animator.SetTrigger(WorkTrigger);
        }
    }
    private void Carry(ResourceItem item, Vector3 target)
    {
        if (hand != null)
        {
            item.MoveToArm(hand.position, target, pickupDelay, carryTime);
        }
        else
        {
            item.MoveTo(target);
        }
    }
   public override void Tick()
    {
        Cell source = GetNeighbor(-GetDirectionVector());
        Cell destination = GetNeighbor(GetDirectionVector());
        if (source == null || destination == null || source.ResourceItem == null)
        {
            return;
        }
        Furnace furnace = GetConstructionAt(GetDirectionVector()) as Furnace;
        if (furnace != null)
        {
            ResourceItem item = source.ResourceItem;
            if (furnace.TryInsert(item))
            {
                source.SetResourceItem(null);
                Carry(item, destination.transform.position);
                PlayWork();
            }
            return;
        }
        if (destination.ResourceItem == null && destination.AcceptsItems)
        {
            destination.SetResourceItem(source.ResourceItem);
            Carry(source.ResourceItem, destination.transform.position);
            source.SetResourceItem(null);
            PlayWork();
        }
    }
}
