using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelCreator levelCreator;
    [SerializeField] private LevelData currentLevel;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private DeckData deckData;
    [SerializeField] private CardPlacementManager cardPlacementManager;
    [SerializeField] private HandUI handUI;

    private void Start()
    {
        levelCreator.Build(currentLevel);
        deckManager.StartDeck(deckData);
        deckManager.DealCards();
        handUI.RefreshHand();
        deckManager.DiscardCards(new List<Card> { deckManager.Hand[0], deckManager.Hand[1] });
        handUI.RefreshHand();
    }
}
