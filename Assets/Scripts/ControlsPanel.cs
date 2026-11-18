using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPanel : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    private void Start()
    {
        panel.SetActive(false);
    }
    private void Update()
    {
        panel.SetActive(Input.GetKey(KeyCode.Tab));
    }
}
