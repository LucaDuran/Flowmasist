using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectiveType
{
    DeliveredAtLeast,
    UseEnergyAtMost,
    UseBeltsAtMost
}
[System.Serializable]
public class LevelObjective
{
    [SerializeField] private ObjectiveType type;
    [SerializeField] private int target;
    public ObjectiveType Type => type;
    public int Target => target;
    public bool IsMet(RoundStats stats)
    {
        switch (type)
        {
            case ObjectiveType.DeliveredAtLeast:
                return stats.Delivered >= target;
            case ObjectiveType.UseEnergyAtMost:
                return stats.EnergyUsed <= target;
            case ObjectiveType.UseBeltsAtMost:
                return stats.BeltsUsed <= target;
        }
        return false;
    }
    public string Describe()
    {
        switch (type)
        {
            case ObjectiveType.DeliveredAtLeast:
                return "Deliver " + target + "+";
            case ObjectiveType.UseEnergyAtMost:
                return "Max energy: " + target;
            case ObjectiveType.UseBeltsAtMost:
                return "Max belts: " + target;
        }
        return "";
    }
    public int GetValue(RoundStats stats)
    {
        switch (type)
        {
            case ObjectiveType.DeliveredAtLeast:
                return stats.Delivered;
            case ObjectiveType.UseEnergyAtMost:
                return stats.EnergyUsed;
            case ObjectiveType.UseBeltsAtMost:
                return stats.BeltsUsed;
        }
        return 0;
    }
}
