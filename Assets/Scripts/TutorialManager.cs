using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI messageText;
    private IReadOnlyList<TutorialStep> steps;
    private HashSet<TutorialTrigger> happened = new HashSet<TutorialTrigger>();
    private int current;
    public bool IsActive => steps != null && current < steps.Count;

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
    public void StartLevel(LevelData level)
    {
        happened.Clear();
        steps = level.TutorialSteps;
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
        if(steps == null)
        {
            return;
        }
        happened.Add(trigger);
        while (current < steps.Count && steps[current].trigger != TutorialTrigger.Continue && happened.Contains(steps[current].trigger))
        {
            current++;
        }
        ShowStep(current);
    }
    private void ShowStep(int index)
    {
        current = index;
        if (steps == null || index >= steps.Count)
        {
            panel.SetActive(false);
            return;
        }
        panel.SetActive(true);
        messageText.text = steps[index].message;
    }
    public bool IsCardAllowed(Card card)
    {
        if (steps == null || current >= steps.Count)
        {
            return true;
        }
        TutorialStep step = steps[current];
        if (!step.restrictCard)
        {
            return true;
        }
        CardConstruction construction = card as CardConstruction;
        return construction != null && construction.Type == step.allowedCard;
    }
    public bool IsPlayAllowed()
    {
        if (steps == null || current >= steps.Count)
        {
            return true;
        }
        return !steps[current].blockPlay;
    }
    public void OnPanelClicked()
    {
        if (steps == null || current >= steps.Count)
        {
            return;
        }
        if (steps[current].trigger != TutorialTrigger.Continue)
        {
            return;
        }
        current++;
        while(current < steps.Count && steps[current].trigger != TutorialTrigger.Continue && happened.Contains(steps[current].trigger))
        {
            current++;
        }
        ShowStep(current);
    }
}