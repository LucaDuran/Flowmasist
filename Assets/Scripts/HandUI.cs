using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandUI : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private GameObject cardUIPrefab;
    [SerializeField] private Transform handPanel;
    [SerializeField] private CardPlacementManager cardPlacementManager;

    public void RefreshHand()
    {
        Debug.Log("RefreshHand called, hand count: " + deckManager.Hand.Count);
        foreach (Transform child in handPanel)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Prefab is null: " + (cardUIPrefab == null));
        foreach (Card card in deckManager.Hand)
        {
            Debug.Log("Displaying card: " + card.CardName);
            GameObject cardUIObj = Instantiate(cardUIPrefab, handPanel);
            CardUI cardUI = cardUIObj.GetComponent<CardUI>();
            cardUI.ShowCard(card, cardPlacementManager);
        }
        Debug.Log("Prefab is null: " + (cardUIPrefab == null));
    }
}
