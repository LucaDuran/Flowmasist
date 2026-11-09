using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    [SerializeField] private float padding = 1.5f;
    [SerializeField] private RectTransform boardArea;
    [SerializeField] private Camera cam;
    [SerializeField] private float baseYaw = 270f;
    [SerializeField] private float isoYawOffsert = 45f;
    [SerializeField] private float topPitch = 90f;
    [SerializeField] private float isoPitch = 50f;
    [SerializeField] private float boardHeight = 1f;
    [SerializeField] private float rotateSpeed = 6f;
    private Vector3[] corners = new Vector3[4];
    private Vector3[] boardCorners = new Vector3[8];
    private Vector3 levelCenter;
    private Vector2 levelSize;
    private bool hasLevel;
    private bool isIso;
    private int quarterTurns;
    private float yaw;
    private float pitch;
    private float TargetYaw => baseYaw + quarterTurns * 90f + (isIso ? isoYawOffsert : 0f);
    private float TargetPitch => isIso ? isoPitch : topPitch;
    private Rect lastArea;
    private Vector2Int lastScreen;
    private void Awake()
    {
        yaw = TargetYaw;
        pitch = TargetPitch;
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
    public void Frame(Vector3 center, Vector2 size)
    {
        levelCenter = center;
        int i = 0;
        for (int sx = -1; sx <= 1; sx += 2)
        {
            for (int sy = -1; sy <= 1; sy += 2)
            {
                for (int sz = -1; sz <= 1; sz += 2)
                {
                    boardCorners[i] = new Vector3(sx * size.x / 2f, sy * boardHeight / 2f, sz * size.y / 2f);
                    i++;
                }
            }
        }
        hasLevel = true;
        Canvas.ForceUpdateCanvases();
    }
    private void LateUpdate()
    {
        float t = 1f - Mathf.Exp(-rotateSpeed * Time.deltaTime);
        yaw = Mathf.LerpAngle(yaw, TargetYaw, t);
        pitch = Mathf.LerpAngle(pitch, TargetPitch, t);
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        if (hasLevel)
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
        if (cam.pixelWidth == 0 || cam.pixelHeight == 0 || area.width <= 0f || area.height <= 0f)
        {
            return;
        }
        float areaWidth = area.width/ cam.pixelWidth;
        float areaHeight = area.height / cam.pixelHeight;
        float areaCenterX = area.center.x / cam.pixelWidth;
        float areaCenterY = area.center.y / cam.pixelHeight;
        float extentX = 0f;
        float extentY = 0f;
        foreach (Vector3 corner in boardCorners)
        {
            extentX = Mathf.Max(extentX, Mathf.Abs(Vector3.Dot(corner, transform.right)));
            extentY = Mathf.Max(extentY, Mathf.Abs(Vector3.Dot(corner, transform.up)));
        }
        float neededHeight = extentY + padding / areaHeight;
        float neededWidth = extentX + padding / cam.aspect * areaWidth;
        cam.orthographicSize = Mathf.Max(neededHeight, neededWidth);
        float offsetX = (areaCenterX - 0.5f) * 2f *cam.orthographicSize * cam.aspect;
        float offsetY = (areaCenterY - 0.5f) * 2f * cam.orthographicSize;
        Vector3 position = levelCenter - transform.right * offsetX - transform.up * offsetY;
        transform.position = levelCenter - transform.right * offsetX - transform.up * offsetY;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleView();
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Rotate(-1);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Rotate(1);
        }
    }
    public void ToggleView()
    {
        isIso = !isIso;
    }
    public void Rotate(int direction)
    {
        quarterTurns += direction;
    }
}
