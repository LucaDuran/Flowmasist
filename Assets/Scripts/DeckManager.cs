using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    private List<Card> actualDeck = new List<Card>();
    private List<Card> hand = new List<Card>();
    private List<Card> discardPile = new List<Card>();
    private void StartDeck(DeckData deckData)
    {
        actualDeck = new List<Card>(deckData.Cards);
    }
    private void DrawCard()
    {
        int index = Random.Range(0, actualDeck.Count);
        hand.Add(actualDeck[index]);
        actualDeck.RemoveAt(index);
    }
    private void DiscardCard (Card card)
    {
        hand.Remove(card);
        discardPile.Add(card);
    }
    private void StageFinish()
    {
        actualDeck.AddRange(hand);
        hand.Clear();
        actualDeck.AddRange(discardPile);
        discardPile.Clear();
    }
}
