using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelList", menuName = "Level/Level List")]
public class LevelList : ScriptableObject
{
    [SerializeField] private List<LevelData> levels;
    public int Count => levels.Count;
    public LevelData GetLevel(int index)
    {
        return levels[index];
    }
}
