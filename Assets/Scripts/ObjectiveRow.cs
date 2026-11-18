using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    public void Setup(string description)
    {
        label.text = "- " + description;
    }
}
