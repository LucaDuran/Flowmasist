using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CardPlacementManager : MonoBehaviour
{
    [SerializeField] private LevelCreator levelCreator;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandUI handUI;
    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private Material ghostMaterial;
    private Card cardSelected;
    private Cell cellSelected;
    private int maxMoves;
    private int movesUsed = 0;
    private Vector2Int pendingFacing = new Vector2Int(1, 0);
    private GameObject ghost;
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
            pendingFacing = new Vector2Int(1, 0);
            CreateGhost(card as CardConstruction);
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
        if (cellSelected.IsBusy)
        {
            Debug.LogWarning("Cell is already occupied.");
            return;
        }
        CardConstruction construction = cardSelected as CardConstruction;
        if (construction == null)
        {
            return;
        }
        GameObject placedConstruction = Instantiate(construction.InstancePrefab, cellSelected.transform.position, FacingToRotation());
        cellSelected.SetCurrentConstruction(placedConstruction);
        ConstructionBehaviour constructionBehaviour = placedConstruction.GetComponent<ConstructionBehaviour>();
        constructionBehaviour.SetCell(cellSelected);
        constructionBehaviour.SetLevelCreator(levelCreator);
        constructionBehaviour.SetFacing(pendingFacing);
        productionManager.RegisterConstruction(constructionBehaviour);
        deckManager.DiscardCard(cardSelected);
        deckManager.DealCards();
        handUI.RefreshHand();
        movesUsed += cardSelected.MoveCost;
        cardSelected = null;
        DestroyGhost();
        cellSelected = null;
    }
    public void OnCellHover(Cell cell)
    {
        if (ghost == null)
        {
            return;
        }
        ghost.transform.position = cell.transform.position;
        ghost.SetActive(true);
    }
    public void OnCellExit(Cell cell)
    {
        if (ghost != null)
        {
            ghost.SetActive(false);
        }
    }
    private void CreateGhost(CardConstruction construction)
    {
        DestroyGhost();
        ghost = Instantiate(construction.InstancePrefab, Vector3.zero, FacingToRotation());
        Destroy(ghost.GetComponent<ConstructionBehaviour>());
        foreach (Collider col in ghost.GetComponentsInChildren<Collider>())
        {
            Destroy(col);
        }
        foreach (Renderer rend in ghost.GetComponentsInChildren<Renderer>())
        {
            rend.material = ghostMaterial;
        }
        ghost.SetActive(false);
    }
    private void DestroyGhost()
    {
        if (ghost != null)
        {
            Destroy(ghost);
            ghost = null;
        }
    }
    private Quaternion FacingToRotation()
    {
        return Quaternion.LookRotation(new Vector3(pendingFacing.x, 0, pendingFacing.y));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            pendingFacing = new Vector2Int(-pendingFacing.y, pendingFacing.x);
            if(ghost != null)
            {
                ghost.transform.rotation = FacingToRotation();
            }
            Debug.Log("Facing rotated to: " + pendingFacing);
        }
    }
}
