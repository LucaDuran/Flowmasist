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
    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private CardPlacementManager cardPlacementManager;
    [SerializeField] private Button executeButton;
    [SerializeField] private Button discardButton;
    [SerializeField] private TutorialManager tutorialManager;
    private IReadOnlyList<ItemQuota> quotas;
    private int[] delivered = new int[Enum.GetValues(typeof(ItemType)).Length];
    private int executionTicks;
    private string levelName;
    private RoundPhase phase;
    public bool IsSetup => phase == RoundPhase.Setup;
    public static event Action<RoundResult> OnRoundFinished;
    public static event Action OnRoundStarted;
    private IReadOnlyList<LevelObjective> objectives;
    public static event Action<IReadOnlyList<ItemQuota>> OnQuotasSet;
    public static event Action<IReadOnlyList<LevelObjective>> OnObjectivesSet;
    public static event Action<ItemType, int> OnDeliveredChanged;

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
        quotas = levelData.Quotas;
        executionTicks = levelData.ExecutionTicks;
        levelName = levelData.LevelName;
        delivered = new int[Enum.GetValues(typeof(ItemType)).Length];
        if (quotas.Count == 0)
        {
            Debug.LogError("Level has no quotas: It would be won instantly.");
        }
        objectives = levelData.Objectives;
        OnObjectivesSet?.Invoke(objectives);
        phase = RoundPhase.Setup;
        OnQuotasSet?.Invoke(quotas);
    }
    public void OnExecuteButtonPressed()
    {
        if (tutorialManager != null && !tutorialManager.IsPlayAllowed())
        {
            return;
        }
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
    private void HandleResourceCollected(ItemType type)
    {
        delivered[(int)type]++;
        OnDeliveredChanged?.Invoke(type, delivered[(int)type]);
    }
    private void HandleProductionFinished()
    {
        phase = RoundPhase.Finished;
        bool won = AllQuotasMet();
        RoundStats stats = new RoundStats
        {
            Delivered = TotalDelivered(),
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
            Delivered = TotalDelivered(),
            Quota = TotalQuota(),
            Stars = stars,
            MaxStars = objectives.Count,
            EnergyUsed = stats.EnergyUsed,
            BeltsUsed = stats.BeltsUsed,
            LevelName = levelName,
            Layout = cardPlacementManager.LayoutString
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
    private bool AllQuotasMet()
    {
        foreach (ItemQuota quota in quotas)
        {
            if (delivered[(int)quota.Type] < quota.Amount)
            {
                return false;
            }
        }
        return true;
    }
    private int TotalDelivered()
    {
        int total = 0;
        foreach (int amount in delivered)
        {
            total += amount;
        }
        return total;
    }
    private int TotalQuota()
    {
        int total = 0;
        foreach (ItemQuota quota in quotas)
        {
            total += quota.Amount;
        }
        return total;
    }
}
