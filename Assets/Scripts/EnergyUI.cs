using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI costText;
    private int usedEnergy;
    private int maxEnergy;
    private void OnEnable()
    {
        CardPlacementManager.OnEnergyChange += ShowEnergy;
        CardPlacementManager.OnCostPreview += ShowCost;
    }
    private void OnDisable()
    {
        CardPlacementManager.OnEnergyChange -= ShowEnergy;
        CardPlacementManager.OnCostPreview -= ShowCost;
    }
    private void ShowEnergy(int used, int max)
    {
        usedEnergy = used;
        maxEnergy = max;
        energyText.text = "Energy: " + used + " / " + max;
    }
    private void ShowCost(int cost)
    {
        if (cost == -1)
        {
            costText.text = "";
            return;
        }
        costText.text = "(+" + cost + ")";
        if(usedEnergy + cost > maxEnergy)
        {
            costText.color = Color.red;
        }
        else
        {
            costText.color = Color.white;
        }
    }
}
