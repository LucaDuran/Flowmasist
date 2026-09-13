using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private string levelName;
    public string LevelName => levelName;
    [SerializeField] private int rows;
    public int Rows => rows;
    [SerializeField] private int columns;
    public int Columns => columns;
    [SerializeField] private int maxMoves;
    public int MaxMoves => maxMoves;
    [SerializeField] private List<Vector2Int> extractZone;
    public List<Vector2Int> ExtractZone => extractZone;
    [SerializeField] private List<Vector2Int> lowEnergyZone;
    public List<Vector2Int> LowEnergyZone => lowEnergyZone;
    [SerializeField] private List<Vector2Int> cantBuild;
    public List<Vector2Int> CantBuild => cantBuild;
    [SerializeField] private bool canRotate;
    public bool CanRotate => canRotate;
}
