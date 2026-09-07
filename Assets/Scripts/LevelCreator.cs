using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Transform cellFather;
    [SerializeField] private float cellSize;
    private Cell[,] grid;
    public void Build(LevelData levelData)
    {
        GenerateGrid(levelData.Rows, levelData.Columns);
        ApllyLevelRules(levelData);
    }
    private void GenerateGrid(int rows, int columns)
    {
        grid = new Cell[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                Vector2Int gridPos = new Vector2Int(i, j);
                Vector3 worldPos = new Vector3(i * cellSize, 0, j * cellSize);
                GameObject newCellObject = Instantiate(cellPrefab, worldPos, Quaternion.identity, cellFather);
                Cell cell = newCellObject.GetComponent<Cell>();
                cell.Initialize(gridPos, true, false, false);
                grid[i, j] = cell;
            }
        }
    }
    private void ApllyLevelRules(LevelData levelData)
    {
        foreach (Vector2Int pos in levelData.CantBuild)
        {
            grid[pos.x, pos.y].SetCanBuild(false);
        }
        foreach (Vector2Int pos in levelData.ExtractZone)
        {
            grid[pos.x, pos.y].SetIsExtractionZone(true);
        }
        foreach (Vector2Int pos in levelData.LowEnergyZone)
        {
            grid[pos.x, pos.y].SetIsLowEnergyZone(true);
        }
    }

    void Start()
    {
        
    }
}
