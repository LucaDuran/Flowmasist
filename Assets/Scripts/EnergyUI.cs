using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    private void OnEnable()
    {
        CardPlacementManager.OnEnergyChange += ShowEnergy;
    }
    private void OnDisable()
    {
        CardPlacementManager.OnEnergyChange -= ShowEnergy;
    }
    private void ShowEnergy(int used, int max)
    {
        energyText.text = "Energy: " + used + " / " + max;
    }
}
