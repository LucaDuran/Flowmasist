using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI starsText;
    [SerializeField] private TextMeshProUGUI objectivesText;
    [SerializeField] private GameObject nextButton;
    [SerializeField] private LevelList levelList;
    private int usedEnergy;
    private int maxEnergy;

    private void OnEnable()
    {
        RoundManager.OnRoundFinished += ShowResult;
    }
    private void OnDisable()
    {
        RoundManager.OnRoundFinished -= ShowResult;
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
    private void ShowResult(RoundResult result)
    {
        string text = "";
        foreach (ObjectiveResult objective in result.Objectives)
        {
            string mark = objective.Met ? "[X]" : "[ ]";
            text += mark + " " + objective.Description + " (you: " + objective.Value + ")" + "\n";
        }
        objectivesText.text = text;
        titleText.text = result.Won ? "Level completed!" : "Level failed";
        starsText.text = "Stars: " + result.Stars + " / " + result.MaxStars;
        bool hasNext = GameSession.CurrentIndex + 1 < levelList.Count;
        nextButton.SetActive(result.Won && hasNext);
        panel.SetActive(true);
    }
}