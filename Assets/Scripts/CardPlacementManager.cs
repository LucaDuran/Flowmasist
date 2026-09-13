using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardPlacementManager : MonoBehaviour
{
    [SerializeField] private LevelCreator levelCreator;
    [SerializeField] private DeckManager deckManager;
    private Card cardSelected;
    private Cell cellSelected;
    private int maxMoves;
    private int movesUsed = 0;
    public void StartLevel(LevelData levelData)
    {
        movesUsed = 0;
        maxMoves = levelData.MaxMoves;
    }
    public void OnCardSelected(Card card)
    {
        if (deckManager.Hand.Contains(card))
        {
            cardSelected = card;
            PlaceCard();
        }
        else
        {
            Debug.LogWarning("Card not in hand: " + card.CardName);
        }
    }
    public void OnCellSelected(Cell cell)
    {
        if (cell.CanBuild)
        {
            cellSelected = cell;
            PlaceCard();
        }
        else
        {
            Debug.LogWarning("Cell is not buildable: " + cell.name);
        }
    }
    public void PlaceCard()
    {
        if (cellSelected == null || cardSelected == null)
        {
            return;
        }
        if (movesUsed + cardSelected.MoveCost > maxMoves)
        {
            Debug.LogWarning("No moves left to place card.");
            return;
        }
        CardConstruction construction = cardSelected as CardConstruction;
        if (construction == null)
        {
            return;
        }
        Instantiate(construction.InstancePrefab, cellSelected.transform.position, Quaternion.identity);
        deckManager.DiscardCard(cardSelected);
        movesUsed += cardSelected.MoveCost;
        cardSelected = null;
        cellSelected = null;
    }
}
