using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    [SerializeField] ItemType type;
    public ItemType Type => type;
    [SerializeField] private float travelTime = 0.5f;
    private Vector3 targetPosition;
    private Vector3 waypoint;
    private bool hasWaypoint;
    private float currentSpeed;
    private float delay;
    public bool HasArrived => transform.position == targetPosition;
    void Awake()
    {
        targetPosition = transform.position;
    }
    public void MoveTo(Vector3 newTarget)
    {
        hasWaypoint = false;
        this.targetPosition = newTarget;
        float distance = Vector3.Distance(transform.position, targetPosition);
        currentSpeed = distance / travelTime;
    }
    public void MoveToArm (Vector3 mta, Vector3 newTarget, float startDelay, float duration)
    {
        waypoint = mta;
        hasWaypoint = true;
        delay = startDelay;
        targetPosition = newTarget;
        float distance = Vector3.Distance(transform.position, mta) + Vector3.Distance(mta, newTarget);
        currentSpeed = distance / duration;
    }
    private void Update()
    {
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
            return;
        }
        Vector3 goal = hasWaypoint ? waypoint : targetPosition;
        transform.position = Vector3.MoveTowards(transform.position, goal, currentSpeed * Time.deltaTime);
        if (hasWaypoint && transform.position == waypoint)
        {
            hasWaypoint = false;
        }
    }
}
