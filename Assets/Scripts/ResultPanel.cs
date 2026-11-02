using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI deliveredText;
    [SerializeField] private TextMeshProUGUI energyText;
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
        panel.SetActive(true);
    }
}