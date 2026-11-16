using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuotaRow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private float countSpeed = 20f;
    [SerializeField] private float punchScale = 1.3f;
    [SerializeField] private float punchRecovery = 8f;
    [SerializeField] private Color doneColor = Color.green;
    private ItemType type;
    private int amount;
    private int target;
    private float shown;
    private int lastShown = -1;
    private Color normalColor;

    public void Setup(ItemType itemType, int quotaAmount)
    {
        type = itemType;
        amount = quotaAmount;
        target = 0;
        shown = 0f;
        lastShown = -1;
        normalColor = label.color;
        Refresh();
    }
    public void SetDelivered(int value)
    {
        target = value;
        transform.localScale = Vector3.one * punchScale;
    }
    private void Update()
    {
        shown = Mathf.MoveTowards(shown, target, countSpeed * Time.deltaTime);
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, punchRecovery * Time.deltaTime);
        Refresh();
    }
    private void Refresh()
    {
        int whole = Mathf.RoundToInt(shown);
        if (whole == lastShown)
        {
            return;
        }
        lastShown = whole;
        label.text = GetName() + ": " + whole.ToString("00") + "/" + amount.ToString("00");
        label.color = whole >= amount ? doneColor : normalColor;
    }
    private string GetName()
    {
        switch (type)
        {
            case ItemType.Coal:
                return "Coal";
            case ItemType.IronOre:
                return "Iron ore";
            case ItemType.IronPlate:
                return "Iron plate";
        }
        return type.ToString();
    }
}