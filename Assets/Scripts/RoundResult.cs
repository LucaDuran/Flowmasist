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
    public List<ObjectiveResult> Objectives;
}
