using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardConstruction : Card
{
    public enum ConstructionType
    {
        Extractor,
        Chest,
        ConveyorBelt,
        Arm,

    }
    [SerializeField] private ConstructionType constructionType;
    public ConstructionType Type => constructionType;
    [SerializeField] private bool canRotate = false;
    public bool CanRotate => canRotate;
    [SerializeField] private GameObject instancePrefab;
    public GameObject InstancePrefab => instancePrefab;
    [SerializeField] private Vector2Int size = Vector2Int.one;
    public Vector2Int Size => size;
}