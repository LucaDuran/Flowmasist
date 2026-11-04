using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class FaceCamera : MonoBehaviour
{
    private Transform cameraTransform;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    private void LateUpdate()
    {
        transform.rotation = cameraTransform.rotation;
    }
}
