using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConstructionBehaviour : MonoBehaviour
{
    protected Cell cell;
    protected Vector2Int facing = new Vector2Int(1, 0);
    public virtual bool AcceptItems => false;
    public virtual bool HasItem => cell != null && cell.ResourceItem != null;
    public virtual bool AcceptsFromBelt => false;
    protected List<Cell> occupiedCells = new List<Cell>();
    public Vector2Int GridPosition => cell.GridPosition;
    public virtual void StopProduction() { }
    public void SetFacing(Vector2Int direction)
    {
        facing = direction;
    }
    public void SetCell(Cell cell)
    {
        this.cell = cell;
    }
    protected LevelCreator levelCreator;
    public void SetLevelCreator(LevelCreator levelCreator)
    {
        this.levelCreator = levelCreator;
    }
    protected Vector2Int GetDirectionVector() 
    {
        return facing;
    }
    protected Cell GetNeighbor(Vector2Int offset)
    {
        Vector2Int neighborPosition = cell.GridPosition + offset;
        return levelCreator.GetCell(neighborPosition.x, neighborPosition.y);
    }
    protected ConstructionBehaviour GetConstructionAt(Vector2Int offset)
    {
        Cell neighbor = GetNeighbor(offset);
        if (neighbor == null || neighbor.CurrentConstruction == null)
        {
            return null;
        }
        return neighbor.CurrentConstruction.GetComponent<ConstructionBehaviour>();
    }
    public virtual ConstructionBehaviour GetNextConstruction()
    {
        return GetConstructionAt(facing);
    }
    public ConstructionBehaviour GetPreviousConstruction()
    {
        return GetConstructionAt(-facing);
    }
    public int GetDepth(int maxSteps)
    {
        int depth = 0;
        ConstructionBehaviour current = this;
        while (depth < maxSteps)
        {
            current = current.GetNextConstruction();
            if (current == null)
            {
                break;
            }
            depth++;
        }
        return depth;
    }
    public void SetOccupiedCells(List<Cell> cells)
    {
        occupiedCells = cells;
    }
    public abstract void Tick();
}
