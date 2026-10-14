using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiscardController : MonoBehaviour
{
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandUI handUI;
    [SerializeField] private CardPlacementManager cardPlacementManager;
    [SerializeField] private TextMeshProUGUI discardText;
    private bool isDiscarding = false;
    public bool IsDiscarding => isDiscarding;
    private List<CardUI> selectedCards = new List<CardUI>();
    private void Start()
    {
        UpdateButtonText();
    }
    public void OnDiscardButtonPressed()
    {
        if (!isDiscarding)
        {
            if(deckManager.DiscardsLeft <= 0)
            {
                Debug.Log("No discards left!");
                return;
            }
            cardPlacementManager.CancelSelection();
            isDiscarding = true;
        }
        else
        {
            if(selectedCards.Count > 0)
            {
                List<Card> cards = new List<Card>();
                foreach (CardUI cardUI in selectedCards)
                {
                    cards.Add(cardUI.Card);
                }
                deckManager.DiscardCards(cards);
            }
            selectedCards.Clear();
            isDiscarding = false;
            handUI.RefreshHand();
        }
        UpdateButtonText();
    }
    public void ToggleCard(CardUI cardUI)
    {
        if (selectedCards.Contains(cardUI))
        {
            selectedCards.Remove(cardUI);
            cardUI.SetSelected(false);
        }
        else if (selectedCards.Count < deckManager.MaxCardsPerDiscard)
        {
            selectedCards.Add(cardUI);
            cardUI.SetSelected(true);
        }
        UpdateButtonText();
    }   
    private void UpdateButtonText()
    {
        if (isDiscarding)
        {
            discardText.text = "Confirmar (" + selectedCards.Count + "/" + deckManager.MaxCardsPerDiscard + ")";
        }
        else
        {
            discardText.text = "Descartar (" + deckManager.DiscardsLeft + ")";
        }
    }
}