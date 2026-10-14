using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConstructionBehaviour : MonoBehaviour
{
    protected Cell cell;
    protected Vector2Int facing = new Vector2Int(1, 0);
    public virtual bool AcceptItems => false;
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
    public virtual ConstructionBehaviour GetNextConstruction()
    {
        Cell next = GetNeighbor(facing);
        if (next == null || next.CurrentConstruction == null)
        {
            return null;
        }
        return next.CurrentConstruction.GetComponent<ConstructionBehaviour>();
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
    public abstract void Tick();
}
