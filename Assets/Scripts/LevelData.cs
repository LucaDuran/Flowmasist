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
    [SerializeField] private DeckData deck;
    public DeckData Deck => deck;
    [SerializeField] private int energy;
    public int Energy => energy;
    [SerializeField] private List<ItemQuota> quotas = new List<ItemQuota>();
    public IReadOnlyList<ItemQuota> Quotas => quotas;
    [SerializeField] private bool canRotate;
    public bool CanRotate => canRotate;
    [SerializeField] private int executionTicks;
    public int ExecutionTicks => executionTicks;
    [SerializeField] private List<LevelObjective> objectives = new List<LevelObjective>();
    public IReadOnlyList<LevelObjective> Objectives => objectives;
}
