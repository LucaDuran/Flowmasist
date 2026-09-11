using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Deck Data", menuName = "Deck Data")]
public class DeckData : ScriptableObject
{
    [SerializeField] private string deckName;
    public string DeckName => deckName;
    [SerializeField] private List<Card> cards = new List<Card>();
    public List<Card> Cards => cards;

}
