using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnergyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private Image pipPrefab;
    [SerializeField] private Transform pipContainer;
    [SerializeField] private int energyPerPip = 5;
    [SerializeField] private Color pipFullColor = Color.green;
    [SerializeField] private Color pipSpentColor = new Color(0.2f, 0.2f, 0.2f, 1f);
    [SerializeField] private Color pipPreviewColor = Color.yellow;
    [SerializeField] private Color pipOverColor = Color.red;
    private List<Image> pips = new List<Image>();
    private int previewCost = -1;
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
        if (pips.Count != max / energyPerPip)
        {
            BuildPips(max);
        }
        RefreshPips();
        RefreshText();
    }
    private void ShowCost(int cost)
    {
        previewCost = cost;
        RefreshPips();
        RefreshText();
    }
    private void BuildPips(int max)
    {
        foreach (Image pip in pips)
        {
            Destroy(pip.gameObject);
        }
        pips.Clear();
        int count = max / energyPerPip;
        for(int i = 0; i < count; i++)
        {
            Image pip = Instantiate(pipPrefab, pipContainer);
            pips.Add(pip);
        }
    }
    private void RefreshPips()
    {
        int usedPips = usedEnergy / energyPerPip;
        int previewPips = previewCost < 0 ? 0 : previewCost / energyPerPip;
        bool tooExpensive = previewCost >= 0 && usedEnergy + previewCost > maxEnergy;
        for (int i = 0; i < pips.Count; i++)
        {
            int position = pips.Count - 1 - i;
            if (position < usedPips)
            {
                pips[i].color = pipSpentColor;
            }
            else if (position < usedPips + previewPips)
            {
                pips[i].color = tooExpensive ? pipOverColor : pipPreviewColor;
            }
            else
            {
                pips[i].color = pipFullColor;
            }
        }
    }
    private void RefreshText()
    {
        string text = (maxEnergy - usedEnergy) + " <size=70%>/" + maxEnergy + "</size>";
        if (previewCost >= 0)
        {
            Color color = usedEnergy + previewCost > maxEnergy ? pipOverColor : pipPreviewColor;
            text += " <color=#" + ColorUtility.ToHtmlStringRGB(color) + ">(-" + previewCost + ")</color>";
        }
        energyText.text = text;
    }
}
