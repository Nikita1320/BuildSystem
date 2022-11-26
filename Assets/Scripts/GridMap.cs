using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GridMap : MonoBehaviour
{
    [SerializeField] private Vector2 startCoordinateGrid;
    [SerializeField] private Vector2 endCoordinateGrid;
    [SerializeField] private int cellSize;
    [SerializeField] private GameObject cell;
    [SerializeField] private LayerMask layerMaskBuildingMap = 7;
    private Camera mainCamera;
    [SerializeField] private Building selectedBuilding;

    private Dictionary<Vector2, bool> grid = new Dictionary<Vector2, bool>();
    private Dictionary<Vector2, Building> buildingsPosition = new Dictionary<Vector2, Building>();
    private Dictionary<Vector2, Building> buildingsTerritory = new Dictionary<Vector2, Building>();

    private Dictionary<Vector2, GameObject> cellArray = new Dictionary<Vector2, GameObject>();

    public LayerMask LayerMaskBuildingMap => layerMaskBuildingMap;
    public Dictionary<Vector2, Building> Buildings => buildingsPosition;
    public Dictionary<Vector2, bool> Grid => grid;

    private void Start()
    {
        InitGrid();
        RenderCellsGrid();
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //Debug.Log("PressMouseInGrid");
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMaskBuildingMap))
            {
                if (selectedBuilding != null)
                {
                    selectedBuilding.ManagerPanels.DeActivatePanel();
                }

                selectedBuilding = CheckBuildingOnGrid(hit.point);

                if (selectedBuilding)
                {
                    selectedBuilding.ManagerPanels.ActivatePanel(true);
                }
            }
        }
    }
    private void InitGrid()
    {
        for (float x = startCoordinateGrid.x; x < endCoordinateGrid.x; x += cellSize)
        {
            for (float y = startCoordinateGrid.y; y < endCoordinateGrid.y; y += cellSize)
            {
                grid.Add(new Vector2(x, y), true);
            }
        }
        for (float x = startCoordinateGrid.x; x < endCoordinateGrid.x; x += cellSize)
        {
            for (float y = startCoordinateGrid.y; y < endCoordinateGrid.y; y += cellSize)
            {
                buildingsPosition.Add(new Vector2(x, y), null);
            }
        }
        for (float x = startCoordinateGrid.x; x < endCoordinateGrid.x; x += cellSize)
        {
            for (float y = startCoordinateGrid.y; y < endCoordinateGrid.y; y += cellSize)
            {
                buildingsTerritory.Add(new Vector2(x, y), null);
            }
        }
        foreach (var item in grid)
        {
            GameObject go = Instantiate(cell);
            go.transform.position = new Vector3(item.Key.x, 0, item.Key.y);
            cellArray.Add(item.Key, go);
        }
    }
    public bool CheckTerritoryForBuild(Vector3 pointRay, Vector2 size, out Vector3 positionOnGrid)
    {
        int x = Mathf.RoundToInt(pointRay.x);
        int y = Mathf.RoundToInt(pointRay.z);
       
        positionOnGrid = new Vector3(x, 0, y);
        return CheckTerritory(new Vector2(x, y), size);
    }
    public Building CheckBuildingOnGrid(Vector3 pointRay)
    {
        int x = Mathf.RoundToInt(pointRay.x);
        int y = Mathf.RoundToInt(pointRay.z);

        Vector2 gridPosition = new Vector2(x, y);

        if (buildingsTerritory.ContainsKey(gridPosition) == true)
        {
            Debug.Log("!!!!!!!!!!!!!!!Check" + buildingsTerritory[gridPosition]);
            return buildingsTerritory[gridPosition];
        }
        else
        {
            return null;
        }
    }
    public bool CheckTerritory(Vector2 pointRay, Vector2 size)
    {
        for (int x = 0; x < size.x; x += cellSize)
        {
            for (int y = 0; y < size.y; y += cellSize)
            {
                if (grid.ContainsKey(new Vector2(pointRay.x + x, pointRay.y + y)) == false || grid[new Vector2(pointRay.x + x, pointRay.y + y)] == false)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public void AddBuilding(Building building, Vector3 position)
    {
        Vector2 coordinate = new Vector2(position.x, position.z);

        buildingsPosition[coordinate] = building;
        for (int x = 0; x < building.Size.x; x += cellSize)
        {
            for (int y = 0; y < building.Size.y; y += cellSize)
            {
                grid[new Vector2(position.x + x, position.z + y)] = false;
                buildingsTerritory[new Vector2(coordinate.x + x, coordinate.y + y)] = building;
            }
        }
        RenderCellsGrid();
    }
    public void ClearTerritory(Building building)
    {
        Vector2 coordinate = new Vector2(building.transform.position.x, building.transform.position.z);

        buildingsPosition.Remove(coordinate);
        for (int x = 0; x < building.Size.x; x += cellSize)
        {
            for (int y = 0; y < building.Size.y; y += cellSize)
            {
                grid[new Vector2(coordinate.x + x, coordinate.y + y)] = true;
            }
        }
        RenderCellsGrid();
    }
    private void RenderCellsGrid()
    {
        foreach (var item in grid)
        {
            SetColorCell(cellArray[item.Key], item.Value);
        }
    }
    private void SetColorCell(GameObject cell, bool isEmpty)
    {
        if (isEmpty)
        {
            cell.GetComponentInChildren<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            cell.GetComponentInChildren<MeshRenderer>().material.color = Color.red;
        }
    }
}
