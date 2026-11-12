using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedControl : MonoBehaviour
{
    [SerializeField] private Button speedButton;
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private float[] speeds = { 1f, 2f, 4f };
    private int index;
    private void OnEnable()
    {
        RoundManager.OnRoundStarted += HandleRoundStarted;
        RoundManager.OnRoundFinished += HandleRoundFinished;
    }
    private void OnDisable()
    {
        RoundManager.OnRoundStarted -= HandleRoundStarted;
        RoundManager.OnRoundFinished -= HandleRoundFinished;
        Time.timeScale = 1f;
    }
    private void Start()
    {
        speedButton.gameObject.SetActive(false);
    }
    public void OnSpeedButtonPressed()
    {
        index = (index + 1) % speeds.Length;
        ApplySpeed();
    }
    private void HandleRoundStarted()
    {
        index = 0;
        ApplySpeed();
        speedButton.gameObject.SetActive(true);
    }
    private void HandleRoundFinished(RoundResult result)
    {
        index = 0;
        ApplySpeed();
        speedButton.gameObject.SetActive(false);
    }
    private void ApplySpeed()
    {
        Time.timeScale = speeds[index];
        label.text = "x" + speeds[index];
    }
}
