using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CardPlacementManager : MonoBehaviour
{
    [SerializeField] private LevelCreator levelCreator;
    [SerializeField] private DeckManager deckManager;
    [SerializeField] private HandUI handUI;
    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private RoundManager roundManager;
    [SerializeField] private Color rejectColor = new Color(1f, 0.2f, 0.2f, 0.6f);
    [SerializeField] private Material ghostMaterial;
    [SerializeField] private float ghostRejectDuration = 0.3f;
    [SerializeField] private int maxDragLength = 4;
    private List<Cell> dragPath = new List<Cell>();
    private List<GameObject> dragGhosts = new List<GameObject>();
    private bool hasLastFacingOverride;
    private Vector2Int lastFacingOverride;
    private bool isDragging;
    private GameObject ghost;
    private float ghostRejectTime;

    private Card cardSelected;
    private Cell cellSelected;
    private int maxMoves;
    private int movesUsed = 0;
    private Vector2Int pendingFacing = new Vector2Int(1, 0);
    private CardUI selectedCardUI;
    private int lastPreviewCost = -2;
    private int beltsUsed;
    public int EnergyUsed => movesUsed;
    public int BeltsUsed => beltsUsed;

    public void StartLevel(LevelData levelData)
    {
        movesUsed = 0;
        beltsUsed = 0;
        maxMoves = levelData.Energy;
        NotifyEnergy();
    }
    public void OnCardSelected(Card card, CardUI cardUI)
    {
        if (!roundManager.IsSetup)
        {
            Debug.LogWarning("Cannot select card before round setup.");
            return;
        }
        if (deckManager.Hand.Contains(card))
        {
            if (selectedCardUI == cardUI)
            {
                CancelSelection();
                return;
            }
            if (selectedCardUI != null)
            {
                selectedCardUI.SetSelected(false);
            }
            cardSelected = card;
            selectedCardUI = cardUI;
            cardUI.SetSelected(true);
            pendingFacing = new Vector2Int(1, 0);
            CreateGhost(card as CardConstruction);
        }
        else
        {
            Debug.LogWarning("Card not in hand: " + card.CardName);
        }
    }
    public void OnCellSelected(Cell cell)
    {
        if (cardSelected == null)
        {
            Debug.LogWarning("No card selected to place.");
            return;
        }
        if (!roundManager.IsSetup)
        {
            Debug.LogWarning("Cannot select cell before round setup.");
            return;
        }
        if (cell.CanBuild)
        {
            if (IsBeltCard(cardSelected))
            {
                StartDrag(cell);
                return;
            }
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
        CardConstruction construction = cardSelected as CardConstruction;
        if (construction == null)
        {
            RejectPlacement();
            return;
        }
        List<Cell> footprint = GetFootprint(cellSelected, construction, pendingFacing);
        if (footprint == null)
        {
            RejectPlacement();
            return;
        }
        int energyCost = 0;
        foreach (Cell footprintCell in footprint)
        {
            if(!footprintCell.CanBuild || footprintCell.IsBusy)
            {
                RejectPlacement();
                return;
            }
            energyCost += footprintCell.GetEnergyCost(construction.MoveCost);
        }
        if (movesUsed + energyCost > maxMoves)
        {
            RejectPlacement();
            return;
        }
        if (construction.Type == CardConstruction.ConstructionType.Extractor && !cellSelected.IsExtractionZone)
        {
            RejectPlacement();
            return;
        }
        BuildAt(cellSelected, construction, pendingFacing);
        deckManager.DiscardCard(cardSelected);
        deckManager.DealCards();
        handUI.RefreshHand();
        movesUsed += energyCost;
        NotifyEnergy();
        selectedCardUI = null;
        cardSelected = null;
        NotifyCostPreview(-1);
        DestroyGhost();
        cellSelected = null;
    }
    private void BuildAt(Cell cell, CardConstruction construction, Vector2Int facing)
    {
        List<Cell> footprint = GetFootprint(cell, construction, facing);
        if (footprint == null)
        {
            return;
        }
        GameObject placed = Instantiate(construction.InstancePrefab, GetFootprintCenter(footprint), FacingToRotation(facing));
        foreach (Cell footprintCell in footprint)
        {
            footprintCell.SetCurrentConstruction(placed);
        }
        ConstructionBehaviour behaviour = placed.GetComponent<ConstructionBehaviour>();
        behaviour.SetCell(cell);
        behaviour.SetOccupiedCells(footprint);
        behaviour.SetLevelCreator(levelCreator);
        behaviour.SetFacing(facing);
        productionManager.RegisterConstruction(behaviour);
        if (construction.Type == CardConstruction.ConstructionType.ConveyorBelt)
        {
            beltsUsed++;
        }
    }
    private List<Cell> GetFootprint(Cell origin, CardConstruction construction, Vector2Int facing)
    {
        List<Cell> cells = new List<Cell>();
        Vector2Int side = new Vector2Int(-facing.y, facing.x);
        for (int i = 0; i < construction.Size.x; i++)
        {
            for(int j = 0; j < construction.Size.y; j++)
            {
                Vector2Int position = origin.GridPosition + facing * i + side * j;
                Cell found = levelCreator.GetCell(position.x, position.y);
                if (found == null)
                {
                    return null;
                }
                cells.Add(found);
            }
        }
        return cells;
    }
    private bool IsFootprintFree(List<Cell> footprint)
    {
        if (footprint == null)
        {
            return false;
        }
        foreach (Cell footprintCell in footprint)
        {
            if (!footprintCell.CanBuild || footprintCell.IsBusy)
            {
                return false;
            }
        }
        return true;
    }
    private bool CanPlace(Cell origin, CardConstruction construction, List<Cell> footprint)
    {
        if (!IsFootprintFree(footprint))
        {
            return false;
        }
        if (construction.Type == CardConstruction.ConstructionType.Extractor && !origin.IsExtractionZone)
        {
            return false;
        }
        return true;
    }
    private Vector3 GetFootprintCenter(List<Cell> footprint)
    {
        Vector3 center = Vector3.zero;
        foreach (Cell footprintCell in footprint)
        {
            center += footprintCell.transform.position;
        }
        return center / footprint.Count;
    }
    private bool IsBeltCard(Card card)
    {
        CardConstruction construction = card as CardConstruction;
        return construction != null && construction.Type == CardConstruction.ConstructionType.ConveyorBelt;
    }
    private void StartDrag(Cell cell)
    {
        if (cell.IsBusy)
        {
            RejectPlacement();
            return;
        }
        dragPath.Clear();
        dragPath.Add(cell);
        isDragging = true;
        hasLastFacingOverride = false;
        if (ghost != null)
        {
            ghost.SetActive(false);
        }
        RefreshDragPreview();
    }
    private int GetDragLimit()
    {
        int beltsInHand = 0;
        foreach (Card card in deckManager.Hand)
        {
            if (card == cardSelected)
            {
                beltsInHand++;
            }
        }
        return Mathf.Min(beltsInHand, maxDragLength);
    }
    private int GetDragCost(CardConstruction construction)
    {
        int total = 0;
        foreach (Cell cell in dragPath)
        {
            total += cell.GetEnergyCost(construction.MoveCost);
        }
        return total;
    }
    private void ExtendDrag(Cell cell)
    {
        CardConstruction construction = cardSelected as CardConstruction;
        Cell last = dragPath[dragPath.Count - 1];
        if (cell == last)
        {
            return;
        }
        if (dragPath.Count >= 2 && cell == dragPath[dragPath.Count - 2])
        {
            dragPath.RemoveAt(dragPath.Count - 1);
            hasLastFacingOverride = false;
            RefreshDragPreview();
            return;
        }
        if (dragPath.Contains(cell))
        {
            return;
        }
        Vector2Int step = cell.GridPosition - last.GridPosition;
        if (Mathf.Abs(step.x) + Mathf.Abs(step.y) != 1)
        {
            return;
        }
        if (!cell.CanBuild || cell.IsBusy)
        {
            return;
        }
        if (dragPath.Count >= GetDragLimit())
        {
            return;
        }
        if (movesUsed + GetDragCost(construction) + cell.GetEnergyCost(construction.MoveCost) > maxMoves)
        {
            return;
        }
        dragPath.Add(cell);
        hasLastFacingOverride = false;
        RefreshDragPreview();
    }
    private Vector2Int GetDragFacing(int index)
    {
        bool isLast = index == dragPath.Count - 1;
        if (isLast && hasLastFacingOverride)
        {
            return lastFacingOverride;
        }
        if (dragPath.Count == 1)
        {
            return pendingFacing;
        }
        int from = index < dragPath.Count - 1 ? index : index - 1;
        return dragPath[from + 1].GridPosition - dragPath[from].GridPosition;
    }
    private void RefreshDragPreview()
    {
        CardConstruction construction = cardSelected as CardConstruction;
        if (construction == null)
        {
            return;
        }
        while (dragGhosts.Count < dragPath.Count)
        {
            dragGhosts.Add(CreateGhostObject(construction));
        }
        while (dragGhosts.Count > dragPath.Count)
        {
            int last = dragGhosts.Count - 1;
            Destroy(dragGhosts[last]);
            dragGhosts.RemoveAt(last);
        }
        for (int i = 0; i < dragPath.Count; i++)
        {
            dragGhosts[i].transform.position = dragPath[i].transform.position;
            dragGhosts[i].transform.rotation = FacingToRotation(GetDragFacing(i));
            dragGhosts[i].SetActive(true);
        }
        NotifyCostPreview(GetDragCost(construction));
    }
    private void ClearDragGhosts()
    {
        foreach (GameObject dragGhost in dragGhosts)
        {
            Destroy(dragGhost);
        }
        dragGhosts.Clear();
    }
    private void RotateLastDragGhost()
    {
        Vector2Int current = GetDragFacing(dragPath.Count - 1);
        lastFacingOverride = new Vector2Int(-current.y, current.x);
        hasLastFacingOverride = true;
        RefreshDragPreview();
    }
    private void FinishDrag()
    {
        isDragging = false;
        ClearDragGhosts();
        NotifyCostPreview(-1);
        CardConstruction construction = cardSelected as CardConstruction;
        if (construction == null || dragPath.Count == 0)
        {
            dragPath.Clear();
            return;
        }
        int totalCost = GetDragCost(construction);
        if (movesUsed + totalCost > maxMoves)
        {
            RejectPlacement();
            dragPath.Clear();
            return;
        }
        for (int i = 0; i < dragPath.Count; i++)
        {
            BuildAt(dragPath[i], construction, GetDragFacing(i));
            deckManager.DiscardCard(cardSelected);
        }
        deckManager.DealCards();
        handUI.RefreshHand();
        movesUsed += totalCost;
        NotifyEnergy();
        selectedCardUI = null;
        cardSelected = null;
        NotifyCostPreview(-1);
        DestroyGhost();
        dragPath.Clear();
    }
    private Cell GetCellUnderMouse()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return null;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return levelCreator.GetCellFromRay(ray);
    }
    private void UpdateHover(Cell cell)
    {
        if (isDragging)
        {
            return;
        }
        CardConstruction construction = cardSelected as CardConstruction;
        List<Cell> footprint = null;
        if (ghost != null && cell != null && construction != null)
        {
            footprint = GetFootprint(cell, construction, pendingFacing);
        }
       if (footprint == null)
        {
            NotifyCostPreview(-1);
            if (ghost != null)
            {
                ghost.SetActive(false);
            }
            return;
        }
        bool valid = CanPlace(cell, construction, footprint);
        ghost.transform.position = GetFootprintCenter(footprint);
        ghost.SetActive(true);
        if (ghostRejectTime <= 0f)
        {
            SetGhostColor(valid ? ghostMaterial.color : rejectColor);
        }
        if (valid)
        {
            int cost = 0;
            foreach (Cell footprintCell in footprint)
            {
                cost += footprintCell.GetEnergyCost(construction.MoveCost);
            }
            NotifyCostPreview(cost);
        }
        else
        {
            NotifyCostPreview(-1);
        }
    }
    private GameObject CreateGhostObject(CardConstruction construction)
    {
        GameObject instance = Instantiate(construction.InstancePrefab, Vector3.zero, FacingToRotation());
        Destroy(instance.GetComponent<ConstructionBehaviour>());
        foreach (Collider col in instance.GetComponentsInChildren<Collider>())
        {
            Destroy(col);
        }
        foreach (Renderer rend in instance.GetComponentsInChildren<Renderer>())
        {
            rend.material = ghostMaterial;
        }
        instance.SetActive(false);
        return instance;
    }
    private void CreateGhost(CardConstruction construction)
    {
        DestroyGhost();
        ghost = CreateGhostObject(construction);
    }
    private void DestroyGhost()
    {
        if (ghost != null)
        {
            Destroy(ghost);
            ghost = null;
        }
    }
    private void RejectPlacement()
    {
        if (selectedCardUI != null)
        {
            ghostRejectTime = ghostRejectDuration;
            selectedCardUI.Shake();
        }
    }
    private void SetGhostColor(Color color)
    {
        if (ghost == null)
        {
            return;
        }
        foreach (Renderer rend in ghost.GetComponentsInChildren<Renderer>())
        {
            rend.material.color = color;
        }
    }
    private Quaternion FacingToRotation(Vector2Int facing)
    {
        return Quaternion.LookRotation(new Vector3(facing.x, 0, facing.y));
    }
    private Quaternion FacingToRotation()
    {
        return FacingToRotation(pendingFacing);
    }
    public void CancelSelection()
    {
        isDragging = false;
        dragPath.Clear();
        ClearDragGhosts();
        if(selectedCardUI != null)
        {
            selectedCardUI.SetSelected(false);
            selectedCardUI = null;
        }
        cardSelected = null;
        cellSelected = null;
        NotifyCostPreview(-1);
        DestroyGhost();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (isDragging)
            {
                RotateLastDragGhost();
            }
            else
            {
                pendingFacing = new Vector2Int(-pendingFacing.y, pendingFacing.x);
                if (ghost != null)
                {
                    ghost.transform.rotation = FacingToRotation();
                }
            }
        }
        if (ghostRejectTime > 0f)
        {
            ghostRejectTime -= Time.deltaTime;
            float t = Mathf.Max(ghostRejectTime, 0f) / ghostRejectDuration;
            SetGhostColor(Color.Lerp(ghostMaterial.color, rejectColor, t));
        }
        if (Input.GetMouseButtonDown(1))
        {
            CancelSelection();
        }
        Cell hovered = GetCellUnderMouse();
        UpdateHover(hovered);
        if (Input.GetMouseButtonDown(0) && hovered != null)
        {
            OnCellSelected(hovered);
        }
        if (isDragging)
        {
            Cell cell = GetCellUnderMouse();
            if (cell != null)
            {
                ExtendDrag(cell);
            }
            if (Input.GetMouseButtonUp(0))
            {
                FinishDrag();
            }
        }
    }
    public static event Action<int, int> OnEnergyChange;
    private void NotifyEnergy()
    {
        OnEnergyChange?.Invoke(movesUsed, maxMoves);
    }
    public static event Action<int> OnCostPreview;
    private void NotifyCostPreview(int cost)
    {
        if (cost == lastPreviewCost)
        {
            return;
        }
        lastPreviewCost = cost;
        OnCostPreview?.Invoke(cost);
    }
}