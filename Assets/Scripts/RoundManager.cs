using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI quotaText;
    private int quota;
    private int delivered;

    private void OnEnable()
    {
        Chest.OnResourceCollected += HandleResourceCollected;
    }
    private void OnDisable()
    {
        Chest.OnResourceCollected -= HandleResourceCollected;
    }
    public void StartRound(LevelData levelData)
    {
        quota = levelData.Quota;
        delivered = 0;
        UpdateText();
    }
    private void HandleResourceCollected()
    {
        ++delivered;
        UpdateText();
        if (delivered == quota)
        {
            Debug.Log("Round completed");
        }
    }
    private void UpdateText()
    {
        quotaText.text = "Recursos: " + delivered + "/" + quota;
    }
}
