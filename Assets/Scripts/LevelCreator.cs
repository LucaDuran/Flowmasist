using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private GameObject veinPrefab;
    [SerializeField] private GameObject wallPrefab;
    [SerializeField] private GameObject chestPrefab;
    [SerializeField] private Transform cellFather;
    [SerializeField] private float cellSize;
    [SerializeField] private CardPlacementManager cardPlacementManager;
    [SerializeField] private ProductionManager productionManager;
    [SerializeField] private int stockPerDigit = 5;
    private Cell[,] grid;
    public void Build(LevelData levelData)
    {
        List<string> lines = ParseMap(levelData.Map);
        if (lines.Count == 0)
        {
            Debug.LogError("The map is empty.");
            return;
        }
        int rows = lines.Count;
        int columns = lines[0].Length;
        foreach(string line in lines)
        {
            if (line.Length != columns)
            {
                Debug.LogError("All map lines must have the same length.");
                return;
            }
        }
        grid = new Cell[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                grid[i, j] = CreateCell(i, j, lines[i][j]);
            }
        }
    }
    private List<string> ParseMap(TextAsset map)
    {
        List<string> lines = new List<string>();
        foreach (string rawLine in map.text.Split('\n'))
        {
            string line = rawLine.Trim();
            if (line =="" || line.StartsWith("#"))
            {
                continue;
            }
            lines.Add(line);
        }
        return lines;
    }
    private Cell CreateCell(int row, int column,char symbol)
    {
        GameObject prefab = floorPrefab;
        bool canBuild = true;
        bool isExtractionZone = false;
        int cellStock = -1;
        if (char.IsDigit(symbol))
        {
            prefab = veinPrefab;
            isExtractionZone = true;
            cellStock = (symbol - '0') * stockPerDigit;
        }
        switch (symbol)
        {  
            case '.':
                break;
            case 'V':
                prefab = veinPrefab;
                isExtractionZone = true;
                break;
            case 'X':
                prefab = wallPrefab;
                canBuild = false;
                break;
            case 'M':
                canBuild = false;
                break;
            default:
                Debug.LogWarning($"Unknown map symbol '{symbol}' at ({row}, {column}). Defaulting to floor.");
                break;
        }
        Vector3 worldPos = cellFather.position + new Vector3(row * cellSize, 0, column * cellSize);
        GameObject cellObject = Instantiate(prefab, worldPos, Quaternion.identity, cellFather);
        cellObject.name = $"Cell_{row}_{column}";
        Cell cell = cellObject.GetComponent<Cell>();
        cell.Initialize(new Vector2Int(row, column), canBuild, isExtractionZone, false);
        cell.SetCardPlacementManager(cardPlacementManager);
        if (cellStock > 0)
        {
            cell.SetStock(cellStock);
        }
        if (symbol == 'M')
        {
            PlaceChest(cell, worldPos);
        }
        return cell;
    }
    private void PlaceChest(Cell cell, Vector3 worldPos)
    {
        GameObject chestObject = Instantiate(chestPrefab, worldPos, Quaternion.identity, cellFather);
        cell.SetCurrentConstruction(chestObject);
        ConstructionBehaviour chest = chestObject.GetComponent<ConstructionBehaviour>();
        chest.SetCell(cell);
        chest.SetLevelCreator(this);
        productionManager.RegisterConstruction(chest);
    }
    public Cell GetCell(int row, int column)
    {
        if (row < 0 || row >= grid.GetLength(0) || column < 0 || column >= grid.GetLength(1))
        {
            return null;
        }
        return grid[row, column];
    }
}
