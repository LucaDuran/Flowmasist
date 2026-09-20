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
    public abstract void Tick();
}
