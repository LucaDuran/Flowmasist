using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuotaPanel : MonoBehaviour
{
    [SerializeField] private QuotaRow rowPrefab;
    [SerializeField] private Transform container;
    private Dictionary<ItemType, QuotaRow> rows = new Dictionary<ItemType, QuotaRow>();

    private void OnEnable()
    {
        RoundManager.OnQuotasSet += BuildRows;
        RoundManager.OnDeliveredChanged += UpdateRow;
    }
    private void OnDisable()
    {
        RoundManager.OnQuotasSet -= BuildRows;
        RoundManager.OnDeliveredChanged -= UpdateRow;
    }
    private void BuildRows(IReadOnlyList<ItemQuota> quotas)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        rows.Clear();
        foreach (ItemQuota quota in quotas)
        {
            QuotaRow row = Instantiate(rowPrefab, container);
            row.Setup(quota.Type, quota.Amount);
            rows[quota.Type] = row;
        }
    }
    private void UpdateRow(ItemType type, int total)
    {
        if (rows.TryGetValue(type, out QuotaRow row))
        {
            row.SetDelivered(total);
        }
    }
}
