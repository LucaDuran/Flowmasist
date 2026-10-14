using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Level/Level Data")]
public class LevelData : ScriptableObject
{
    [SerializeField] private string levelName;
    public string LevelName => levelName;
    [SerializeField] private TextAsset map;
    public TextAsset Map => map;
    [SerializeField] private int maxMoves;
    public int MaxMoves => maxMoves;
    [SerializeField] private int quota;
    public int Quota => quota;
    [SerializeField] private bool canRotate;
    public bool CanRotate => canRotate;
    [SerializeField] private int executionTicks;
    public int ExecutionTicks => executionTicks;
}
