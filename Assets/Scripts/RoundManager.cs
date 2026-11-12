using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum RoundPhase
{
    Setup,
    Executing,
    Finished
}

public class RoundManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI quotaText;
    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private CardPlacementManager cardPlacementManager;
    [SerializeField] private Button executeButton;
    [SerializeField] private Button discardButton;
    private int quota;
    private int delivered;
    private int executionTicks;
    private RoundPhase phase;
    public bool IsSetup => phase == RoundPhase.Setup;
    public static event Action<RoundResult> OnRoundFinished;
    public static event Action OnRoundStarted;
    private IReadOnlyList<LevelObjective> objectives;

    private void OnEnable()
    {
        Chest.OnResourceCollected += HandleResourceCollected;
        ProductionManager.OnProductionFinished += HandleProductionFinished;
    }
    private void OnDisable()
    {
        Chest.OnResourceCollected -= HandleResourceCollected;
        ProductionManager.OnProductionFinished -= HandleProductionFinished;
    }
    public void StartRound(LevelData levelData)
    {
        quota = levelData.Quota;
        executionTicks = levelData.ExecutionTicks;
        delivered = 0;
        objectives = levelData.Objectives;
        phase = RoundPhase.Setup;
        UpdateText();
    }
    public void OnExecuteButtonPressed()
    {
        if (phase !=RoundPhase.Setup)
        {
            return;
        }
        phase = RoundPhase.Executing;
        cardPlacementManager.CancelSelection();
        executeButton.interactable = false;
        discardButton.interactable = false;
        productionManager.StartProduction(executionTicks);
        OnRoundStarted?.Invoke();
    }
    public void OnRetryButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void HandleResourceCollected()
    {
        ++delivered;
        UpdateText();
    }
    private void HandleProductionFinished()
    {
        phase = RoundPhase.Finished;
        bool won = delivered >= quota;
        RoundStats stats = new RoundStats
        {
            Delivered = delivered,
            EnergyUsed = cardPlacementManager.EnergyUsed,
            BeltsUsed = cardPlacementManager.BeltsUsed
        };
        if (won)
        {
            GameSession.CompleteCurrentLevel();
        }
        int stars = CountStars(won, stats);
        List<ObjectiveResult> objectivesResults = new List<ObjectiveResult>();
        foreach (LevelObjective objective in objectives)
        {
            ObjectiveResult entry = new ObjectiveResult
            {
                Description = objective.Describe(),
                Met = objective.IsMet(stats),
                Value = objective.GetValue(stats)
            };
            objectivesResults.Add(entry);
        }
        RoundResult result = new RoundResult
        {
            Won = won,
            Objectives = objectivesResults,
            Delivered = delivered,
            Quota = quota,
            Stars = stars,
            MaxStars = objectives.Count
        };
        OnRoundFinished?.Invoke(result);
    }
    private int CountStars(bool won, RoundStats stats)
    {
        if (!won)
        {
            return 0;
        }
        int stars = 0;
        foreach (LevelObjective objective in objectives)
        {
           if (objective.IsMet(stats))
           {
               stars++;
           }
        }
        return stars;
    }
    private void CheckQuotaReachable()
    {
        if (phase != RoundPhase.Executing)
        {
            return;
        }
        if (delivered + productionManager.RemainingPotential() < quota)
        {
            productionManager.FinishProduction();
        }
    }
    private void UpdateText()
    {
        quotaText.text = "Resources: " + delivered + "/" + quota;
    }
}
