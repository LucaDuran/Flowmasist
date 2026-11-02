using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LevelList levelList;
    [SerializeField] private LevelData debugLevel;
    [SerializeField] private LevelCreator levelCreator;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private DeckData deckData;
    [SerializeField] private CardPlacementManager cardPlacementManager;
    [SerializeField] private HandUI handUI;
    [SerializeField] private RoundManager roundManager;

    private void Start()
    {
        LevelData currentLevel = debugLevel != null ? debugLevel : levelList.GetLevel(GameSession.CurrentIndex);
        levelCreator.Build(currentLevel);
        deckManager.StartDeck(deckData);
        cardPlacementManager.StartLevel(currentLevel);
        roundManager.StartRound(currentLevel);
        deckManager.DealCards();
        handUI.RefreshHand();
    }
}
