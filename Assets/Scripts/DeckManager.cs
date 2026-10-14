using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [SerializeField] private int maxHandSize = 8;
    [SerializeField] private int maxDiscards = 3;
    [SerializeField] private int maxCardsPerDiscard = 3;
    private int discardsUsed = 0;
    private List<Card> actualDeck = new List<Card>();
    private List<Card> hand = new List<Card>();
    public List<Card> Hand => hand;
    private List<Card> discardPile = new List<Card>();
    public int DiscardsLeft => maxDiscards - discardsUsed;
    public int MaxCardsPerDiscard => maxCardsPerDiscard;
    public void StartDeck(DeckData deckData)
    {
        actualDeck = new List<Card>(deckData.Cards);
        Debug.Log(actualDeck.Count);
    }
    public Card DrawCard()
    {
        if (actualDeck.Count == 0)
        {
            ReshuffleDiscardPile();
        }
        if (actualDeck.Count == 0)
        {
            Debug.Log("No cards left to draw.");
            return null;
        }
        int index = Random.Range(0, actualDeck.Count);
        Card cardStole = actualDeck[index];
        hand.Add(cardStole);
        actualDeck.RemoveAt(index);
        Debug.Log(cardStole);
        return cardStole;
    }
    private void ReshuffleDiscardPile()
    {
        actualDeck.AddRange(discardPile);
        discardPile.Clear();
        Debug.Log("Reshuffled discard pile into the deck.");
    }
    public void DiscardCards(List<Card> cardsToDiscard)
    {
        if (discardsUsed >= maxDiscards)
        {
            Debug.Log("Maximum discards reached for this stage.");
            return;
        }
        if (cardsToDiscard.Count > maxCardsPerDiscard)
        {
            Debug.Log($"You can only discard up to {maxCardsPerDiscard} cards at a time.");
            return;
        }
        foreach (Card card in cardsToDiscard)
        {
            if (hand.Contains(card))
            {
                DiscardCard(card);
                Debug.Log($"Discarded Card: {card.CardName}");
            }
            else
            {
                Debug.Log($"Card {card.CardName} is not in hand and cannot be discarded.");
            }
        }
        discardsUsed++;
        DealCards();
    }
    public void DiscardCard (Card card)
    {

        hand.Remove(card);
        discardPile.Add(card);
        Debug.Log(card);
    }
    public void StageFinish()
    {
        discardsUsed = 0;
        actualDeck.AddRange(hand);
        hand.Clear();
        actualDeck.AddRange(discardPile);
        discardPile.Clear();
    }
    public void DealCards()
    {
        int cardsNeeded = maxHandSize - hand.Count;
        for (int i = 0; i < cardsNeeded; i++)
        {
            if(DrawCard() == null)
            {
                Debug.Log("No more cards to draw.");
                break;
            }
        }
    }
}
