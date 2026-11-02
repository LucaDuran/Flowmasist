using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI deliveredText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private LevelList levelList;
    private int usedEnergy;
    private int maxEnergy;

    private void OnEnable()
    {
        RoundManager.OnRoundFinished += ShowResult;
        CardPlacementManager.OnEnergyChange += SaveEnergy;
    }
    private void OnDisable()
    {
        RoundManager.OnRoundFinished -= ShowResult;
        CardPlacementManager.OnEnergyChange -= SaveEnergy;
    }
    public void OnNextButtonPressed()
    {
        GameSession.SelectLevel(GameSession.CurrentIndex + 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    private void Start()
    {
        panel.SetActive(false);
    }
    private void SaveEnergy(int used, int max)
    {
        usedEnergy = used;
        maxEnergy = max;
    }
    private void ShowResult(bool won, int delivered, int quota)
    {
        titleText.text = won ? "Level completed!" : "Level failed";
        deliveredText.text = "Delivered: " + delivered + " / " + quota;
        energyText.text = "Energy: " + usedEnergy + " / " + maxEnergy;
        bool hasNext = GameSession.CurrentIndex + 1 < levelList.Count;
        nextButton.SetActive(won && hasNext);
        panel.SetActive(true);
    }
}