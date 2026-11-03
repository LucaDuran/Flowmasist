using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] private float padding = 1.5f;
    [SerializeField] private RectTransform boardArea;
    private Camera cam;
    private Vector3[] corners = new Vector3[4];
    private Vector3 levelCenter;
    private Vector2 levelSize;
    private bool hasLevel;
    private Rect lastArea;
    private Vector2Int lastScreen;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }
    public void Frame(Vector3 center, Vector2 size)
    {
        levelCenter = center;
        levelSize = size;
        hasLevel = true;
        Canvas.ForceUpdateCanvases();
        Refit();
    }
    private void LateUpdate()
    {
        if (!hasLevel)
        {
            return;
        }
        Vector2Int screen = new Vector2Int(cam.pixelWidth, cam.pixelHeight);
        if (GetAreaInPixels() != lastArea || screen != lastScreen)
        {
            Refit();
        }
    }
    private Rect GetAreaInPixels()
    {
        boardArea.GetWorldCorners(corners);
        return Rect.MinMaxRect(corners[0].x, corners[0].y, corners[2].x, corners[2].y);
    }
    private void Refit()
    {
        Rect area = GetAreaInPixels();
        lastArea = area;
        lastScreen = new Vector2Int(cam.pixelWidth, cam.pixelHeight);
        if (cam.pixelWidth == 0 || cam.pixelHeight == 0 || area.width <= 0f || area.height <= 0f)
        {
            return;
        }
        float areaWidth = area.width/ cam.pixelWidth;
        float areaHeight = area.height / cam.pixelHeight;
        float areaCenterX = area.center.x / cam.pixelWidth;
        float areaCenterY = area.center.y / cam.pixelHeight;
        float neededHeight = (levelSize.y + padding * 2f) / (2f * areaHeight);
        float neededWidth = (levelSize.x + padding * 2f) / (2f * cam.aspect * areaWidth);
        cam.orthographicSize = Mathf.Max(neededHeight, neededWidth);
        float offsetX = (areaCenterX - 0.5f) * 2f *cam.orthographicSize * cam.aspect;
        float offsetY = (areaCenterY - 0.5f) * 2f * cam.orthographicSize;
        Vector3 position = levelCenter - transform.right * offsetX - transform.up * offsetY;
        transform.position = new Vector3(position.x, transform.position.y, position.z);
    }
}
