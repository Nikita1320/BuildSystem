using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSystem : MonoBehaviour
{
    public delegate void BeginConstruction();
    public BeginConstruction beginConstructionEvent;
    public delegate void CanceledConstruction();
    public CanceledConstruction canceledConstructionEvent;

    [SerializeField] private int maxCountBuilders;
    [SerializeField] private int countFreeBuilders;
    [SerializeField] private GridMap map;
    [SerializeField] private Building currentBuilding;
    private Camera mainCamera;
    [SerializeField] private bool isStartPlacing = false;

    [SerializeField] private Vector3 buildingOldPosition;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Start()
    {
        countFreeBuilders = maxCountBuilders;
    }
    private void Update()
    {
        if (isStartPlacing)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

                if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layerMask: map.LayerMaskBuildingMap))
                {
                    bool territoryAccsess = map.CheckTerritoryForBuild(hit.point, currentBuilding.Size, out Vector3 worldPosition);

                    currentBuilding.transform.position = worldPosition;

                    currentBuilding.SetTransparent(territoryAccsess);

                    currentBuilding.ManagerPanels.ActivatePanel(territoryAccsess);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void StartPlacingBuilding(Building building)
    {
        if (currentBuilding)
        {
            if (currentBuilding.ManagerPanels.CurrentState == StateBuilding.PlacingForBuild)
            {
                Destroy(currentBuilding);
            }
            else
            {
                CancelMoving(currentBuilding);
            }
        }
        currentBuilding = Instantiate(building);
        currentBuilding.Init(this);
        currentBuilding.transform.position = Vector3.zero;


        isStartPlacing = true;
    }
    public void StartMovingBuilding(Building building)
    {
        if (currentBuilding)
        {
            if (currentBuilding.ManagerPanels.CurrentState == StateBuilding.PlacingForBuild)
            {
                Destroy(currentBuilding);
            }
            else
            {
                CancelMoving(currentBuilding);
            }
        }
        currentBuilding = building;

        map.ClearTerritory(currentBuilding);
        buildingOldPosition = currentBuilding.transform.position;
        isStartPlacing = true;
    }
    public bool StartConstruction(Building building)
    {
        if (countFreeBuilders > 0)
        {
            isStartPlacing = false;
            countFreeBuilders--;
            return true;
        }
        return false;
    }
    public void UpdateBuilding(Building building)
    {

    }
    public void CancelPlacingForBuild(Building building)
    {
        isStartPlacing = false;
        currentBuilding = null;
        Destroy(building.gameObject);
    }
    public void CancelMoving(Building building)
    {
        building.transform.position = buildingOldPosition;

        map.AddBuilding(building, buildingOldPosition);
    }
    
    public void CancelUpdateBuilding(Building building)
    {

    }
    public void CancelConstruction(Building building)
    {
        map.ClearTerritory(building);
        isStartPlacing = false;
        countFreeBuilders++;
        Destroy(building.gameObject);
        currentBuilding = null;
    }
    public void EndPlacing(Building building)
    {
        map.AddBuilding(building, currentBuilding.transform.position);

        isStartPlacing = false;
        currentBuilding = null;
    }

    public void EndConstruction(Building building)
    {
        countFreeBuilders++;
    }
    public void EndMoving(Building building)
    {
        map.AddBuilding(building, currentBuilding.transform.position);

        isStartPlacing = false;
        currentBuilding = null;
    }
    public void DestroyBuilding(Building building)
    {
        map.ClearTerritory(building);
    }
}
