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

    private void Start()
    {
        levelCreator.Build(currentLevel);
        deckManager.StartDeck(deckData);
        cardPlacementManager.StartLevel(currentLevel);
        deckManager.DealCards();
        List<Card> toDiscard = new List<Card> { deckManager.Hand[0], deckManager.Hand[1] };
        cardPlacementManager.OnCardSelected(deckManager.Hand[0]);
        cardPlacementManager.OnCellSelected(levelCreator.GetCell(0, 0));
        deckManager.DiscardCards(toDiscard);
    }
}
