using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceItem : MonoBehaviour
{
    [SerializeField] ItemType type;
    public ItemType Type => type;
    [SerializeField] private float travelTime = 0.5f;
    private Vector3 targetPosition;
    private float currentSpeed;
    public bool HasArrived => transform.position == targetPosition;
    void Awake()
    {
        targetPosition = transform.position;
    }
    public void MoveTo(Vector3 newTarget)
    {
       this.targetPosition = newTarget;
        float distance = Vector3.Distance(transform.position, targetPosition);
        currentSpeed = distance / travelTime;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
    }
}
