using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] private float padding = 1.5f;
    private Camera cam;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    public void Frame(Vector3 center, Vector2 size)
    {
        transform.position = new Vector3(center.x, transform.position.y, center.z);
        float halfHeight = size.y / 2f;
        float halfWidth = size.x / 2f / cam.aspect;
        cam.orthographicSize = Mathf.Max(halfHeight, halfWidth) + padding;
    }
}
