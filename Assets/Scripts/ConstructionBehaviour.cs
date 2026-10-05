using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConstructionBehaviour : MonoBehaviour
{
    protected Cell cell;
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
        return new Vector2Int(1, 0);
    }
    protected Cell GetNeighbor(Vector2Int offset)
    {
        Vector2Int neighborPosition = cell.GridPosition + offset;
        return levelCreator.GetCell(neighborPosition.x, neighborPosition.y);
    }
    public abstract void Tick();
}
