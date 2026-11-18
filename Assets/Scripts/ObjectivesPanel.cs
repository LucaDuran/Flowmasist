using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ObjectivesPanel : MonoBehaviour
{
    [SerializeField] private ObjectiveRow rowPrefab;
    [SerializeField] private Transform container;
    private void OnEnable()
    {
        RoundManager.OnObjectivesSet += BuildRows;
    }
    private void OnDisable()
    {
        RoundManager.OnObjectivesSet -= BuildRows;
    }
    private void BuildRows(IReadOnlyList<LevelObjective> objectives)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        foreach (LevelObjective objective in objectives)
        {
            ObjectiveRow row = Instantiate(rowPrefab, container);
            row.Setup(objective.Describe());
        }
    }
}