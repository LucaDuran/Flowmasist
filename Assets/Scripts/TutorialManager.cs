using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public enum TutorialTrigger
{
    ExtractorPlaced,
    BeltPlaced,
    ArmPlaced,
    PlayPressed
}
[Serializable]
public class TutorialStep
{
    [TextArea] public string message;
    public TutorialTrigger trigger;
}
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private TutorialStep[] steps;
    [SerializeField] private int tutorialLevelIndex = 0;
    private HashSet<TutorialTrigger> happened = new HashSet<TutorialTrigger>();
    private int current;

    private void OnEnable()
    {
        CardPlacementManager.OnPiecePlaced += HandlePiecePlaced;
        RoundManager.OnRoundStarted += HandlePlayPressed;
    }
    private void OnDisable()
    {
        CardPlacementManager.OnPiecePlaced -= HandlePiecePlaced;
        RoundManager.OnRoundStarted -= HandlePlayPressed;
    }
    private void Start()
    {
        if (GameSession.CurrentIndex != tutorialLevelIndex)
        {
            panel.SetActive(false);
            return;
        }
        ShowStep(0);
    }
    private void HandlePiecePlaced(ConstructionBehaviour piece)
    {
        if (piece is Extractor)
        {
            Register(TutorialTrigger.ExtractorPlaced);
        }
        else if (piece is ConveyorBelt)
        {
            Register(TutorialTrigger.BeltPlaced);
        }
        else if (piece is Arm)
        {
            Register(TutorialTrigger.ArmPlaced);
        }
    }
    private void HandlePlayPressed()
    {
        Register(TutorialTrigger.PlayPressed);
    }
    private void Register(TutorialTrigger trigger)
    {
        happened.Add(trigger);
        while (current < steps.Length && happened.Contains(steps[current].trigger))
        {
            current++;
        }
        ShowStep(current);
    }
    private void ShowStep(int index)
    {
        current = index;
        if (index >= steps.Length)
        {
            panel.SetActive(false);
            return;
        }
        panel.SetActive(true);
        messageText.text = steps[index].message;
    }
}