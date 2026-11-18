using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct RoundResult
{
    public bool Won;
    public int Delivered;
    public int Quota;
    public int Stars;
    public int MaxStars;
    public int EnergyUsed;
    public int BeltsUsed;
    public string LevelName;
    public string Layout;
    public List<ObjectiveResult> Objectives;
}
