using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TutorialTrigger
{
    ExtractorPlaced,
    BeltPlaced,
    ArmPlaced,
    PlayPressed,
    Continue
}
[Serializable]
public class TutorialStep
{
    [TextArea] public string message;
    public TutorialTrigger trigger;
    public bool restrictCard;
    public CardConstruction.ConstructionType allowedCard;
    public bool blockPlay;
}