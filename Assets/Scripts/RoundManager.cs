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
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private CardPlacementManager cardPlacementManager;
    [SerializeField] private Button executeButton;
    [SerializeField] private Button discardButton;
    [SerializeField] private Button retryButton;
    private int quota;
    private int delivered;
    private int executionTicks;
    private RoundPhase phase;
    public bool IsSetup => phase == RoundPhase.Setup;

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
        phase = RoundPhase.Setup;
        resultText.text = "";
        retryButton.gameObject.SetActive(false);
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
        resultText.text = delivered >= quota ? "¡Has entregado todos los recursos!" : "No has entregado todos los recursos.";
        retryButton.gameObject.SetActive(true);
    }
    private void UpdateText()
    {
        quotaText.text = "Recursos: " + delivered + "/" + quota;
    }
}
