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
    [SerializeField] private RoundManager roundManager;

    private void Start()
    {
        levelCreator.Build(currentLevel);
        deckManager.StartDeck(deckData);
        cardPlacementManager.StartLevel(currentLevel);
        roundManager.StartRound(currentLevel);
        deckManager.DealCards();
        handUI.RefreshHand();
    }
}
