using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoundTelemetry
{
    public string token;
    public string session_id;
    public string version;
    public int level;
    public int attempt;
    public bool won;
    public int stars;
    public int delivered;
    public int quota;
    public int energy_used;
    public int energy_max;
    public int belts_used;
    public int planning_sec;
}
